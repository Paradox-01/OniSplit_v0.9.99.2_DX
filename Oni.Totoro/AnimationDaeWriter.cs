using System;
using System.Collections.Generic;
using Oni.Dae;

namespace Oni.Totoro
{
	internal static class AnimationDaeWriter
	{
		public static void AppendFrames(Animation anim1, Animation anim2)
		{
			if ((anim2.Flags & AnimationFlags.Overlay) != 0)
			{
				Console.Error.WriteLine("Cannot merge {0} because it's an overlay animation", anim2.Name);
				return;
			}
			if (anim1.FrameSize == 0)
			{
				anim1.FrameSize = anim2.FrameSize;
			}
			else if (anim1.FrameSize != anim2.FrameSize)
			{
				Console.Error.WriteLine("Cannot merge {0} because its frame size doesn't match the frame size of the previous animation", anim2.Name);
				return;
			}
			anim1.Velocities.AddRange(anim2.Velocities);
			anim1.Heights.AddRange(anim2.Heights);
			if (anim1.Rotations.Count == 0)
			{
				anim1.Rotations.AddRange(anim2.Rotations);
				return;
			}
			for (int i = 0; i < anim1.Rotations.Count; i++)
			{
				anim1.Rotations[i].AddRange(anim2.Rotations[i]);
			}
		}

		public static void Write(Node root, Animation animation, int startFrame = 0, bool convertSceneZUP = false, bool convertEulerXYZ = false)
		{
			List<Vector2> velocities = animation.Velocities;
			List<float> heights = animation.Heights;
			List<List<KeyFrame>> rotations = animation.Rotations;
			bool flag = animation.FrameSize == 6;
			bool flag2 = (animation.Flags & AnimationFlags.Overlay) != 0;
			bool flag3 = (animation.Flags & AnimationFlags.RealWorld) != 0;
			uint num = (uint)(animation.OverlayUsedBones | animation.OverlayReplacedBones);
			List<Node> list = FindNodes(root);
			if (!flag2 && !flag3)
			{
				Node targetNode = list[0];
				Vector2[] array = new Vector2[velocities.Count + 1];
				for (int i = 1; i < array.Length; i++)
				{
					array[i] = array[i - 1] + velocities[i - 1];
				}
				CreateAnimationCurve(startFrame, array.Select((Vector2 p) => p.X).ToList(), targetNode, "pos", "X");
				if (convertSceneZUP)
				{
					CreateAnimationCurve(startFrame, array.Select((Vector2 p) => 0f - p.Y).ToList(), targetNode, "pos", "Y");
				}
				else
				{
					CreateAnimationCurve(startFrame, array.Select((Vector2 p) => p.Y).ToList(), targetNode, "pos", "Z");
				}
				if (convertSceneZUP)
				{
					CreateAnimationCurve(startFrame, heights.ToList(), targetNode, "pos", "Z");
				}
				else
				{
					CreateAnimationCurve(startFrame, heights.ToList(), targetNode, "pos", "Y");
				}
			}
			bool flag4 = true;
			for (int num2 = 0; num2 < rotations.Count; num2++)
			{
				if (flag2 && (num & (uint)(1 << num2)) == 0)
				{
					continue;
				}
				Node targetNode2 = list[num2];
				List<KeyFrame> list2 = rotations[num2];
				int num3 = ((!flag4) ? list2.Count : list2.Sum((KeyFrame k) => k.Duration));
				float[] array2 = new float[num3];
				float[] array3 = new float[num3];
				float[] array4 = new float[num3];
				float[] array5 = new float[num3];
				if (flag4)
				{
					Quaternion[] array6 = new Quaternion[list2.Count];
					for (int num4 = 0; num4 < list2.Count; num4++)
					{
						KeyFrame keyFrame = list2[num4];
						if (flag)
						{
							array6[num4] = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(keyFrame.Rotation.X)) * Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(keyFrame.Rotation.Y)) * Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(keyFrame.Rotation.Z));
						}
						else
						{
							array6[num4] = new Quaternion(keyFrame.Rotation);
						}
					}
					int num5 = 0;
					for (int num6 = 0; num6 < list2.Count; num6++)
					{
						int duration = list2[num6].Duration;
						Quaternion q = array6[num6];
						Quaternion q2 = ((num6 == list2.Count - 1) ? array6[num6] : array6[num6 + 1]);
						for (int num7 = 0; num7 < duration; num7++)
						{
							Quaternion quaternion = Quaternion.Lerp(q, q2, (float)num7 / (float)duration);
							if ((num2 == 0) & convertSceneZUP)
							{
								Quaternion quaternion2 = quaternion;
								quaternion = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90f)) * quaternion2;
							}
							Vector3 vector = ((!convertEulerXYZ) ? quaternion.ToEulerXYZ() : quaternion.ToEulerRevXYZ());
							array2[num5] = (float)(num5 + startFrame) * (1f / 60f);
							array3[num5] = vector.X;
							array4[num5] = vector.Y;
							array5[num5] = vector.Z;
							num5++;
						}
					}
					MakeRotationCurveContinuous(array3);
					MakeRotationCurveContinuous(array4);
					MakeRotationCurveContinuous(array5);
				}
				else
				{
					int num8 = 0;
					for (int num9 = 0; num9 < list2.Count; num9++)
					{
						KeyFrame keyFrame2 = list2[num9];
						array2[num9] = (float)(num8 + startFrame) * (1f / 60f);
						num8 += keyFrame2.Duration;
						if (flag)
						{
							array3[num9] = keyFrame2.Rotation.X;
							array4[num9] = keyFrame2.Rotation.Y;
							array5[num9] = keyFrame2.Rotation.Z;
						}
						else
						{
							Vector3 vector2 = new Quaternion(keyFrame2.Rotation).ToEulerXYZ();
							array3[num9] = vector2.X;
							array4[num9] = vector2.Y;
							array5[num9] = vector2.Z;
						}
					}
				}
				CreateAnimationCurve(array2, array3, targetNode2, "rotX", "ANGLE");
				CreateAnimationCurve(array2, array4, targetNode2, "rotY", "ANGLE");
				CreateAnimationCurve(array2, array5, targetNode2, "rotZ", "ANGLE");
			}
		}

		private static void MakeRotationCurveContinuous(float[] curve)
		{
			for (int i = 1; i < curve.Length; i++)
			{
				float num = curve[i - 1];
				float num2 = curve[i];
				if (Math.Abs(num2 - num) > 180f)
				{
					num2 = ((!(num2 > num)) ? (num2 + 360f) : (num2 - 360f));
					curve[i] = num2;
				}
			}
		}

		private static void CreateAnimationCurve(int startFrame, IList<float> values, Node targetNode, string targetSid, string targetValue)
		{
			if (values.Count != 0)
			{
				float[] array = new float[values.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = (float)(i + startFrame) * (1f / 60f);
				}
				CreateAnimationCurve(array, values, targetNode, targetSid, targetValue);
			}
		}

		private static void CreateAnimationCurve(IList<float> times, IList<float> values, Node targetNode, string targetSid, string targetValue)
		{
			string[] array = new string[times.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = "LINEAR";
			}
			Transform transform = targetNode.Transforms.Find((Transform x) => x.Sid == targetSid);
			transform.BindAnimation(targetValue, new Sampler
			{
				Inputs = 
				{
					new Input(Semantic.Input, new Source(times, 1)),
					new Input(Semantic.Output, new Source(values, 1)),
					new Input(Semantic.Interpolation, new Source(array, 1))
				}
			});
		}

		private static List<Node> FindNodes(Node root)
		{
			List<Node> result = new List<Node>(19);
			FindNodesRecursive(root, result);
			return result;
		}

		private static void FindNodesRecursive(Node node, List<Node> result)
		{
			result.Add(node);
			foreach (Node node2 in node.Nodes)
			{
				FindNodesRecursive(node2, result);
			}
		}
	}
}
