using System;
using System.Collections.Generic;
using Oni.Dae;

namespace Oni.Totoro
{
	internal class AnimationDaeReader
	{
		private Animation animation;

		private Scene scene;

		private int startFrame;

		private int endFrame;

		private Body body;

		private int frameCount;

		public Scene Scene
		{
			get
			{
				return scene;
			}
			set
			{
				scene = value;
			}
		}

		public int StartFrame
		{
			get
			{
				return startFrame;
			}
			set
			{
				startFrame = value;
			}
		}

		public int EndFrame
		{
			get
			{
				return endFrame;
			}
			set
			{
				endFrame = value;
			}
		}

		public void Read(Animation targetAnimation)
		{
			animation = targetAnimation;
			body = BodyDaeReader.Read(scene);
			if (scene.CustomAxisConversion)
			{
				Console.WriteLine("AnimationDaeReader: custom axis conversion.");
			}
			ComputeFrameCount();
			ImportTranslation();
			ImportRotations();
			animation.ComputeExtents(body);
		}

		private void ComputeFrameCount()
		{
			float num = float.MinValue;
			IEnumerable<Input> enumerable = from i in (from a in (from t in body.Nodes.SelectMany((BodyNode n) => n.DaeNode.Transforms)
						where t.HasAnimations
						select t).SelectMany((Transform t) => t.Animations)
					where a != null
					select a).SelectMany((Sampler a) => a.Inputs)
				where i.Semantic == Semantic.Input
				select i;
			foreach (Input item in enumerable)
			{
				num = Math.Max(num, item.Source.FloatData.Max());
			}
			float num2 = num * 60f;
			int num3 = ((!((double)num2 - Math.Round(num2) < 0.0005)) ? FMath.TruncateToInt32(num2) : FMath.RoundToInt32(num2));
			if (endFrame == 0)
			{
				endFrame = num3;
			}
			else if (endFrame > num3)
			{
				Console.Error.WriteLine("Warning: the specified animation end frame ({0}) is beyond the last key frame ({1}), using the last frame instead", endFrame, num3);
				endFrame = num3;
			}
			if (startFrame >= num3)
			{
				Console.Error.WriteLine("Warning: the specified animation start frame ({0}) is beyond the last key frame ({1}), using 0 instead", startFrame, num3);
				startFrame = 0;
			}
			frameCount = endFrame - startFrame;
		}

		private void ImportTranslation()
		{
			Node daeNode = body.Nodes[0].DaeNode;
			bool flag = false;
			foreach (Transform transform in daeNode.Transforms)
			{
				TransformTranslate transformTranslate = transform as TransformTranslate;
				if (transformTranslate == null || flag)
				{
					continue;
				}
				flag = true;
				if (scene.CustomAxisConversion && scene.SceneZUP)
				{
					animation.Heights.AddRange(Sample(transformTranslate, 2, endFrame - 1));
				}
				else
				{
					animation.Heights.AddRange(Sample(transformTranslate, 1, endFrame - 1));
				}
				float[] array = Sample(transformTranslate, 0, endFrame);
				float[] array2 = Sample(transformTranslate, 1, endFrame);
				float[] array3 = Sample(transformTranslate, 2, endFrame);
				if (scene.CustomAxisConversion && scene.SceneZUP)
				{
					for (int i = 1; i < array.Length; i++)
					{
						animation.Velocities.Add(new Vector2(array[i] - array[i - 1], array2[i - 1] - array2[i]));
					}
				}
				else
				{
					for (int j = 1; j < array.Length; j++)
					{
						animation.Velocities.Add(new Vector2(array[j] - array[j - 1], array3[j] - array3[j - 1]));
					}
				}
			}
			if (!flag)
			{
				animation.Heights.AddRange(Enumerable.Repeat(0f, frameCount));
				animation.Velocities.AddRange(Enumerable.Repeat(Vector2.Zero, frameCount));
			}
		}

		private void ImportRotations()
		{
			animation.FrameSize = 16;
			foreach (Node item in body.Nodes.Select((BodyNode n) => n.DaeNode))
			{
				List<KeyFrame> list = new List<KeyFrame>();
				animation.Rotations.Add(list);
				List<TransformRotate> list2 = new List<TransformRotate>();
				List<float[]> list3 = new List<float[]>();
				foreach (Transform transform in item.Transforms)
				{
					TransformRotate transformRotate = transform as TransformRotate;
					if (transformRotate != null)
					{
						list2.Add(transformRotate);
						list3.Add(Sample(transformRotate, 3, endFrame - 1));
					}
				}
				for (int num = 0; num < frameCount; num++)
				{
					Quaternion identity = Quaternion.Identity;
					if (scene.CustomAxisConversion && scene.SceneZUP && item.Name.Contains("pelvis"))
					{
						identity *= Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(-90f));
					}
					float degrees = 0f;
					float degrees2 = 0f;
					float degrees3 = 0f;
					for (int num2 = 0; num2 < list2.Count; num2++)
					{
						if (list2[num2].Axis == Vector3.UnitX)
						{
							degrees = list3[num2][num];
							continue;
						}
						if (list2[num2].Axis == -Vector3.UnitX)
						{
							degrees = 0f - list3[num2][num];
							continue;
						}
						if (list2[num2].Axis == Vector3.UnitY)
						{
							degrees2 = list3[num2][num];
							continue;
						}
						if (list2[num2].Axis == -Vector3.UnitY)
						{
							degrees2 = 0f - list3[num2][num];
							continue;
						}
						if (list2[num2].Axis == Vector3.UnitZ)
						{
							degrees3 = list3[num2][num];
							continue;
						}
						if (list2[num2].Axis == -Vector3.UnitZ)
						{
							degrees3 = 0f - list3[num2][num];
							continue;
						}
						Console.WriteLine("Unexpected rotation axis!");
						Console.WriteLine(list2[num2].Axis);
					}
					if (list2.Count < 3)
					{
						Console.WriteLine("Unexpected rotation count!");
						Console.WriteLine(item.Name);
					}
					Quaternion quaternion = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(degrees));
					Quaternion quaternion2 = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(degrees2));
					Quaternion quaternion3 = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(degrees3));
					if (scene.CustomAxisConversion)
					{
						identity *= quaternion3;
						identity *= quaternion2;
						identity *= quaternion;
					}
					else
					{
						identity *= quaternion;
						identity *= quaternion2;
						identity *= quaternion3;
					}
					list.Add(new KeyFrame
					{
						Duration = 1,
						Rotation = identity.ToVector4()
					});
				}
			}
		}

		private float[] Sample(Transform transform, int index, int endFrame)
		{
			Sampler sampler = null;
			if (transform.HasAnimations)
			{
				sampler = transform.Animations[index];
			}
			if (sampler == null)
			{
				float num = transform.Values[index];
				float[] array = new float[endFrame - startFrame + 1];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = num;
				}
				return array;
			}
			return sampler.Sample(startFrame, endFrame);
		}
	}
}
