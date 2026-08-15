using System;
using System.Collections.Generic;
using Oni.Akira;
using Oni.Dae;
using Oni.Motoko;

namespace Oni.Physics
{
	internal class ObjectDaeImporter
	{
		private readonly TextureImporter3 textureImporter;

		private readonly Dictionary<string, AkiraDaeNodeProperties> properties;

		private readonly List<ObjectNode> nodes = new List<ObjectNode>();

		public List<ObjectNode> Nodes
		{
			get
			{
				return nodes;
			}
		}

		public ObjectDaeImporter(TextureImporter3 textureImporter, Dictionary<string, AkiraDaeNodeProperties> properties)
		{
			this.textureImporter = textureImporter;
			this.properties = properties;
		}

		public void Import(Scene scene)
		{
			ImportNode(scene, null, GetNodeProperties(scene));
		}

		private void ImportNode(Node node, List<ObjectAnimationKey> parentAnimation, ObjectDaeNodeProperties parentNodeProperties)
		{
			Console.WriteLine("\t{0}", node.Id);
			List<ObjectAnimationKey> list = ImportNodeAnimation(node, parentAnimation);
			ObjectDaeNodeProperties objectDaeNodeProperties = GetNodeProperties(node);
			if (objectDaeNodeProperties == null && parentNodeProperties != null)
			{
				objectDaeNodeProperties = new ObjectDaeNodeProperties
				{
					HasPhysics = parentNodeProperties.HasPhysics,
					ScriptId = parentNodeProperties.ScriptId,
					ObjectFlags = parentNodeProperties.ObjectFlags
				};
				objectDaeNodeProperties.Animations.AddRange(parentNodeProperties.Animations.Select((ObjectAnimationClip a) => new ObjectAnimationClip
				{
					Name = node.Name + "_anim",
					Start = a.Start,
					Stop = a.Stop,
					End = a.End,
					Flags = a.Flags
				}));
			}
			if (objectDaeNodeProperties != null && objectDaeNodeProperties.HasPhysics)
			{
				List<Oni.Motoko.Geometry> list2 = GeometryDaeReader.Read(node, textureImporter).ToList();
				if (list.Count > 0 || list2.Count > 0)
				{
					nodes.Add(new ObjectNode(list2.Select((Oni.Motoko.Geometry g) => new ObjectGeometry(g)))
					{
						Name = node.Name,
						FileName = node.FileName,
						Animations = CreateAnimations(list, objectDaeNodeProperties),
						ScriptId = objectDaeNodeProperties.ScriptId,
						Flags = objectDaeNodeProperties.ObjectFlags
					});
				}
			}
			foreach (Node node2 in node.Nodes)
			{
				ImportNode(node2, list, objectDaeNodeProperties);
			}
		}

		private List<ObjectAnimationKey> ImportNodeAnimation(Node node, List<ObjectAnimationKey> parentAnimation)
		{
			Vector3 one = Vector3.One;
			TransformScale transformScale = node.Transforms.OfType<TransformScale>().FirstOrDefault();
			if (transformScale != null)
			{
				one.X = transformScale.Values[0];
				one.Y = transformScale.Values[1];
				one.Z = transformScale.Values[2];
			}
			if (parentAnimation != null && parentAnimation.Count > 0)
			{
				one *= parentAnimation[0].Scale;
			}
			List<TransformRotate> list = new List<TransformRotate>();
			List<float[]> list2 = new List<float[]>();
			foreach (TransformRotate item in node.Transforms.OfType<TransformRotate>())
			{
				list.Add(item);
				Sampler angleAnimation = item.AngleAnimation;
				if (angleAnimation != null)
				{
					list2.Add(angleAnimation.Sample());
					continue;
				}
				list2.Add(new float[1] { item.Angle });
			}
			TransformTranslate transformTranslate = node.Transforms.OfType<TransformTranslate>().FirstOrDefault();
			List<float[]> list3 = new List<float[]>();
			if (transformTranslate != null)
			{
				for (int i = 0; i < 3; i++)
				{
					Sampler sampler = transformTranslate.Animations[i];
					if (sampler != null)
					{
						list3.Add(sampler.Sample());
						continue;
					}
					list3.Add(new float[1] { transformTranslate.Translation[i] });
				}
			}
			List<ObjectAnimationKey> list4 = new List<ObjectAnimationKey>();
			int num = Math.Max(list2.Max((float[] a) => a.Length), list3.Max((float[] a) => a.Length));
			for (int num2 = 0; num2 < num; num2++)
			{
				Quaternion quaternion = Quaternion.Identity;
				for (int num3 = 0; num3 < list.Count; num3++)
				{
					float[] array = list2[num3];
					quaternion *= Quaternion.CreateFromAxisAngle(angle: MathHelper.ToRadians((num2 < array.Length) ? array[num2] : array.Last()), axis: list[num3].Axis);
				}
				Vector3 vector = Vector3.Zero;
				if (transformTranslate != null)
				{
					vector.X = list3[0][MathHelper.Clamp(num2, 0, list3[0].Length - 1)];
					vector.Y = list3[1][MathHelper.Clamp(num2, 0, list3[1].Length - 1)];
					vector.Z = list3[2][MathHelper.Clamp(num2, 0, list3[2].Length - 1)];
				}
				if (parentAnimation != null)
				{
					ObjectAnimationKey objectAnimationKey = ((num2 < parentAnimation.Count) ? parentAnimation[num2] : parentAnimation.LastOrDefault());
					if (objectAnimationKey != null)
					{
						quaternion = objectAnimationKey.Rotation * quaternion;
						vector = objectAnimationKey.Translation + Vector3.Transform(vector * objectAnimationKey.Scale, objectAnimationKey.Rotation);
					}
				}
				list4.Add(new ObjectAnimationKey
				{
					Time = num2,
					Scale = one,
					Rotation = quaternion,
					Translation = vector
				});
			}
			return list4;
		}

		private ObjectAnimation[] CreateAnimations(List<ObjectAnimationKey> allFrames, ObjectDaeNodeProperties props)
		{
			List<ObjectAnimation> list = new List<ObjectAnimation>();
			foreach (ObjectAnimationClip animation in props.Animations)
			{
				int start = animation.Start;
				int end = ((animation.End != int.MaxValue) ? animation.End : allFrames.Last().Time);
				ObjectAnimationKey[] array = (from f in allFrames
					where start <= f.Time && f.Time <= end
					select new ObjectAnimationKey
					{
						Time = f.Time - start,
						Scale = f.Scale,
						Rotation = f.Rotation,
						Translation = f.Translation
					}).ToArray();
				if (array.Length != 0)
				{
					list.Add(new ObjectAnimation
					{
						Name = animation.Name,
						Flags = animation.Flags,
						Stop = animation.Stop,
						Length = end - start + 1,
						Keys = array
					});
				}
			}
			return list.ToArray();
		}

		private ObjectDaeNodeProperties GetNodeProperties(Node node)
		{
			AkiraDaeNodeProperties value;
			if (properties == null || !properties.TryGetValue(node.Id, out value))
			{
				return null;
			}
			return value as ObjectDaeNodeProperties;
		}
	}
}
