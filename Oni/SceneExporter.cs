using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Oni.Dae;
using Oni.Motoko;
using Oni.Physics;
using Oni.Totoro;

namespace Oni
{
	internal class SceneExporter
	{
		private class SceneNode
		{
			public string Name;

			public readonly List<Oni.Motoko.Geometry> Geometries = new List<Oni.Motoko.Geometry>();

			public readonly List<SceneNodeAnimation> Animations = new List<SceneNodeAnimation>();

			public readonly List<SceneNode> Nodes = new List<SceneNode>();

			public Body Body;

			public bool IsCamera;
		}

		private class SceneNodeAnimation
		{
			public int Start;

			public ObjectAnimation ObjectAnimation;
		}

		private readonly InstanceFileManager fileManager;

		private readonly string outputDirPath;

		private readonly TextureDaeWriter textureWriter;

		private readonly GeometryDaeWriter geometryWriter;

		private readonly BodyDaeWriter bodyWriter;

		private string basePath;

		public SceneExporter(InstanceFileManager fileManager, string outputDirPath)
		{
			this.fileManager = fileManager;
			this.outputDirPath = outputDirPath;
			textureWriter = new TextureDaeWriter(outputDirPath);
			geometryWriter = new GeometryDaeWriter(textureWriter);
			bodyWriter = new BodyDaeWriter(geometryWriter);
		}

		public void ExportScene(string sourceFilePath)
		{
			basePath = Path.GetDirectoryName(sourceFilePath);
			Scene scene = new Scene();
			XmlReaderSettings settings = new XmlReaderSettings
			{
				IgnoreWhitespace = true,
				IgnoreProcessingInstructions = true,
				IgnoreComments = true
			};
			List<SceneNode> list = new List<SceneNode>();
			using (XmlReader xmlReader = XmlReader.Create(sourceFilePath, settings))
			{
				scene.Name = xmlReader.GetAttribute("Name");
				xmlReader.ReadStartElement("Scene");
				while (xmlReader.IsStartElement())
				{
					list.Add(ReadNode(xmlReader));
				}
				xmlReader.ReadEndElement();
			}
			foreach (SceneNode item in list)
			{
				scene.Nodes.Add(WriteNode(item, null));
			}
			Writer.WriteFile(Path.Combine(outputDirPath, Path.GetFileNameWithoutExtension(sourceFilePath)) + ".dae", scene);
		}

		private string ResolvePath(string path)
		{
			return Path.Combine(basePath, path);
		}

		private SceneNode ReadNode(XmlReader xml)
		{
			SceneNode sceneNode = new SceneNode
			{
				Name = xml.GetAttribute("Name")
			};
			xml.ReadStartElement("Node");
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "Geometry":
					ReadGeometry(xml, sceneNode);
					break;
				case "Body":
					ReadBody(xml, sceneNode);
					break;
				case "Camera":
					ReadCamera(xml, sceneNode);
					break;
				case "Animation":
					ReadAnimation(xml, sceneNode);
					break;
				case "Node":
					sceneNode.Nodes.Add(ReadNode(xml));
					break;
				default:
					Console.WriteLine("Unknown element name {0}", xml.LocalName);
					xml.Skip();
					break;
				}
			}
			xml.ReadEndElement();
			return sceneNode;
		}

		private void ReadGeometry(XmlReader xml, SceneNode node)
		{
			InstanceFile instanceFile = fileManager.OpenFile(ResolvePath(xml.ReadElementContentAsString()));
			Oni.Motoko.Geometry item = GeometryDatReader.Read(instanceFile.Descriptors[0]);
			node.Geometries.Add(item);
		}

		private void ReadBody(XmlReader xml, SceneNode node)
		{
			InstanceFile instanceFile = fileManager.OpenFile(ResolvePath(xml.ReadElementContentAsString()));
			ReadBodyNode(node, (node.Body = BodyDatReader.Read(instanceFile.Descriptors[0])).Root);
		}

		private static void ReadBodyNode(SceneNode node, BodyNode bodyNode)
		{
			node.Name = bodyNode.Name;
			node.Geometries.Add(bodyNode.Geometry);
			foreach (BodyNode node2 in bodyNode.Nodes)
			{
				SceneNode sceneNode = new SceneNode();
				node.Nodes.Add(sceneNode);
				ReadBodyNode(sceneNode, node2);
			}
		}

		private void ReadAnimation(XmlReader xml, SceneNode node)
		{
			string attribute = xml.GetAttribute("Start");
			bool flag = xml.GetAttribute("Type") == "Max";
			bool flag2 = xml.GetAttribute("NoRotation") == "true";
			string path = xml.ReadElementContentAsString();
			int start = ((!string.IsNullOrEmpty(attribute)) ? int.Parse(attribute) : 0);
			InstanceFile instanceFile = fileManager.OpenFile(ResolvePath(path));
			if (node.Body != null)
			{
				ObjectAnimation[] animations = AnimationDatReader.Read(instanceFile.Descriptors[0]).ToObjectAnimation(node.Body);
				ReadBodyAnimation(start, node, node.Body.Root, animations);
				return;
			}
			node.Animations.Add(new SceneNodeAnimation
			{
				Start = start,
				ObjectAnimation = ObjectDatReader.ReadAnimation(instanceFile.Descriptors[0])
			});
			if (flag2)
			{
				ObjectAnimationKey[] keys = node.Animations.Last().ObjectAnimation.Keys;
				foreach (ObjectAnimationKey objectAnimationKey in keys)
				{
					objectAnimationKey.Rotation = Quaternion.Identity;
				}
			}
			else if (flag)
			{
				ObjectAnimationKey[] keys2 = node.Animations.Last().ObjectAnimation.Keys;
				foreach (ObjectAnimationKey objectAnimationKey2 in keys2)
				{
					objectAnimationKey2.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitX, 1.5707965f);
				}
			}
		}

		private void ReadBodyAnimation(int start, SceneNode node, BodyNode bodyNode, ObjectAnimation[] animations)
		{
			node.Animations.Add(new SceneNodeAnimation
			{
				Start = start,
				ObjectAnimation = animations[bodyNode.Index]
			});
			for (int i = 0; i < node.Nodes.Count; i++)
			{
				ReadBodyAnimation(start, node.Nodes[i], bodyNode.Nodes[i], animations);
			}
		}

		private void ReadCamera(XmlReader xml, SceneNode node)
		{
			node.IsCamera = true;
			xml.Skip();
		}

		private Node WriteNode(SceneNode node, List<ObjectAnimationKey> parentFrames)
		{
			Node node2 = new Node
			{
				Name = node.Name
			};
			foreach (Oni.Motoko.Geometry geometry in node.Geometries)
			{
				node2.Instances.Add(geometryWriter.WriteGeometryInstance(geometry, geometry.Name));
			}
			if (node.IsCamera)
			{
				WriteCamera(node2);
			}
			List<ObjectAnimationKey> list = null;
			if (node.Animations.Count > 0)
			{
				list = BuildFrames(node);
				WriteAnimation(node2, BuildLocalFrames((node.Body == null) ? parentFrames : null, list));
			}
			foreach (SceneNode node3 in node.Nodes)
			{
				node2.Nodes.Add(WriteNode(node3, list));
			}
			return node2;
		}

		private static List<ObjectAnimationKey> BuildFrames(SceneNode node)
		{
			List<ObjectAnimationKey> list = new List<ObjectAnimationKey>();
			foreach (SceneNodeAnimation animation in node.Animations)
			{
				List<ObjectAnimationKey> list2 = animation.ObjectAnimation.Interpolate();
				int num = animation.Start;
				if (list.Count > 0)
				{
					num += list.Last().Time + 1;
				}
				foreach (ObjectAnimationKey item in list2)
				{
					item.Time += num;
				}
				if (list.Count > 0)
				{
					while (list.Last().Time >= list2.First().Time)
					{
						list.RemoveAt(list.Count - 1);
					}
					while (list.Last().Time + 1 < list2.First().Time)
					{
						list.Add(new ObjectAnimationKey
						{
							Time = list.Last().Time + 1,
							Rotation = list.Last().Rotation,
							Translation = list.Last().Translation,
							Scale = list.Last().Scale
						});
					}
				}
				list.AddRange(list2);
			}
			return list;
		}

		private static List<ObjectAnimationKey> BuildLocalFrames(List<ObjectAnimationKey> parentFrames, List<ObjectAnimationKey> frames)
		{
			List<ObjectAnimationKey> list = frames;
			if (parentFrames != null)
			{
				list = new List<ObjectAnimationKey>(list.Count);
				for (int i = 0; i < frames.Count; i++)
				{
					ObjectAnimationKey objectAnimationKey = frames[i];
					ObjectAnimationKey objectAnimationKey2 = parentFrames[i];
					list.Add(new ObjectAnimationKey
					{
						Time = objectAnimationKey.Time,
						Scale = objectAnimationKey.Scale / objectAnimationKey2.Scale,
						Rotation = Quaternion.Conjugate(objectAnimationKey2.Rotation) * objectAnimationKey.Rotation,
						Translation = Vector3.Transform(objectAnimationKey.Translation - objectAnimationKey2.Translation, objectAnimationKey2.Rotation.Inverse()) / objectAnimationKey2.Scale
					});
				}
			}
			return list;
		}

		private static void WriteAnimation(Node node, List<ObjectAnimationKey> frames)
		{
			float[] array = new float[frames.Count];
			string[] array2 = new string[array.Length];
			Vector3[] positions = new Vector3[frames.Count];
			Vector3[] angles = new Vector3[frames.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (float)frames[i].Time / 60f;
			}
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = "LINEAR";
			}
			for (int k = 0; k < frames.Count; k++)
			{
				positions[k] = frames[k].Translation;
			}
			for (int l = 0; l < frames.Count; l++)
			{
				angles[l] = frames[l].Rotation.ToEulerXYZ();
			}
			TransformTranslate transform = node.Transforms.Translate("translate", positions[0]);
			TransformRotate transform2 = node.Transforms.Rotate("rotX", Vector3.UnitX, angles[0].X);
			TransformRotate transform3 = node.Transforms.Rotate("rotY", Vector3.UnitY, angles[0].Y);
			TransformRotate transform4 = node.Transforms.Rotate("rotZ", Vector3.UnitZ, angles[0].Z);
			TransformScale transformScale = node.Transforms.Scale("scale", frames[0].Scale);
			WriteSampler(array, array2, (int num) => positions[num].X, transform, "X");
			WriteSampler(array, array2, (int num) => positions[num].Y, transform, "Y");
			WriteSampler(array, array2, (int num) => positions[num].Z, transform, "Z");
			WriteSampler(array, array2, (int num) => angles[num].X, transform2, "ANGLE");
			WriteSampler(array, array2, (int num) => angles[num].Y, transform3, "ANGLE");
			WriteSampler(array, array2, (int num) => angles[num].Z, transform4, "ANGLE");
		}

		private static void WriteSampler(float[] times, string[] interpolations, Func<int, float> getValue, Transform transform, string targetName)
		{
			float[] array = new float[times.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = getValue(i);
			}
			transform.BindAnimation(targetName, new Sampler
			{
				Inputs = 
				{
					new Input(Semantic.Input, new Source(times, 1)),
					new Input(Semantic.Output, new Source(array, 1)),
					new Input(Semantic.Interpolation, new Source(interpolations, 1))
				}
			});
		}

		private static void WriteCamera(Node daeNode)
		{
			daeNode.Instances.Add(new CameraInstance
			{
				Target = new Camera
				{
					XFov = 45f,
					AspectRatio = 1.3333334f,
					ZNear = 1f,
					ZFar = 10000f
				}
			});
		}
	}
}
