using System;
using System.Collections.Generic;
using System.IO;
using Oni.Physics;

namespace Oni.Totoro
{
	internal class Animation
	{
		public string Name;

		public AnimationFlags Flags;

		public readonly string[] DirectAnimations = new string[2];

		public float FinalRotation;

		public Direction Direction = Direction.Forward;

		public int Vocalization = 65535;

		public string Impact;

		public int HardPause;

		public int SoftPause;

		public AnimationType Type;

		public AnimationType AimingType;

		public AnimationState FromState;

		public AnimationState ToState;

		public AnimationVarient Varient;

		public int ActionFrame = 65535;

		public int FirstLevelAvailable;

		public BoneMask OverlayUsedBones;

		public BoneMask OverlayReplacedBones;

		public int AtomicStart;

		public int AtomicEnd;

		public int InvulnerableStart;

		public int InvulnerableEnd;

		public int InterpolationMax;

		public int InterpolationEnd;

		public int FrameSize;

		public readonly List<float> Heights = new List<float>();

		public readonly List<Vector2> Velocities = new List<Vector2>();

		public readonly List<List<KeyFrame>> Rotations = new List<List<KeyFrame>>();

		public readonly List<Shortcut> Shortcuts = new List<Shortcut>();

		public readonly List<Position> Positions = new List<Position>();

		public readonly List<Damage> SelfDamage = new List<Damage>();

		public ThrowInfo ThrowSource;

		public readonly List<Sound> Sounds = new List<Sound>();

		public readonly List<Footstep> Footsteps = new List<Footstep>();

		public readonly List<Particle> Particles = new List<Particle>();

		public readonly List<MotionBlur> MotionBlur = new List<MotionBlur>();

		public readonly List<Attack> Attacks = new List<Attack>();

		public readonly float[] AttackRing = new float[36];

		public readonly List<List<Vector3>> AllPoints = new List<List<Vector3>>();

		public void ValidateFrames()
		{
			TextWriter error = Console.Error;
			int frameCount = Heights.Count;
			foreach (Sound item in Sounds.FindAll((Sound s) => s.Start >= frameCount))
			{
				error.WriteLine("Warning: sound start {0} is beyond the last animation frame", item.Start);
				Sounds.Remove(item);
			}
			foreach (Footstep item2 in Footsteps.FindAll((Footstep f) => f.Frame >= frameCount))
			{
				error.WriteLine("Warning: footstep frame {0} is beyond the last animation frame", item2.Frame);
				Footsteps.Remove(item2);
			}
			foreach (Damage item3 in SelfDamage.FindAll((Damage d) => d.Frame > frameCount))
			{
				error.WriteLine("Warning: damage frame {0} is beyond the last animation frame", item3.Frame);
				SelfDamage.Remove(item3);
			}
			foreach (Attack item4 in Attacks.FindAll((Attack a) => a.Start >= frameCount))
			{
				error.WriteLine("Warning: attack start frame {0} is beyond the last animation frame", item4.Start);
				Attacks.Remove(item4);
			}
			foreach (Particle item5 in Particles.FindAll((Particle p) => p.Start >= frameCount))
			{
				error.WriteLine("Warning: particle start frame {0} is beyond the last animation frame", item5.Start);
				Particles.Remove(item5);
			}
		}

		public void ComputeExtents(Body body)
		{
			Positions.Clear();
			AllPoints.Clear();
			int count = Heights.Count;
			int count2 = Rotations.Count;
			Quaternion[,] array = new Quaternion[count, count2];
			for (int i = 0; i < count2; i++)
			{
				List<KeyFrame> list = Rotations[i];
				for (int j = 0; j < list.Count; j++)
				{
					array[j, i] = new Quaternion(list[j].Rotation);
				}
			}
			Matrix[] array2 = new Matrix[count2];
			Vector2 zero = Vector2.Zero;
			for (int k = 0; k < count; k++)
			{
				for (int l = 0; l < count2; l++)
				{
					array2[l] = Matrix.CreateFromQuaternion(array[k, l]);
					array2[l].Translation = body.Nodes[l].Translation;
				}
				PropagateTransforms(body.Root, array2);
				for (int m = 0; m < count2; m++)
				{
					array2[m] *= Matrix.CreateTranslation(zero.X, Heights[k], zero.Y);
				}
				float num = 1E+09f;
				float num2 = -1E+09f;
				List<Vector3> list2 = new List<Vector3>(8 * count2);
				for (int n = 0; n < count2; n++)
				{
					Vector3[] points = body.Nodes[n].Geometry.Points;
					BoundingBox bbox = BoundingBox.CreateFromPoints(points);
					BoundingSphere boundingSphere = BoundingSphere.CreateFromBoundingBox(bbox);
					Vector3[] collection = Vector3.Transform(bbox.GetCorners(), ref array2[n]);
					Vector3 vector = Vector3.Transform(boundingSphere.Center, ref array2[n]);
					num = Math.Min(num, vector.Y - boundingSphere.Radius);
					num2 = Math.Max(num2, vector.Y + boundingSphere.Radius);
					list2.AddRange(collection);
				}
				Positions.Add(new Position
				{
					Height = num2 - num,
					YOffset = num,
					X = zero.X,
					Z = zero.Y
				});
				AllPoints.Add(list2);
				zero += Velocities[k];
			}
		}

		private static void PropagateTransforms(BodyNode bodyNode, Matrix[] transforms)
		{
			foreach (BodyNode node in bodyNode.Nodes)
			{
				transforms[node.Index] *= transforms[bodyNode.Index];
				PropagateTransforms(node, transforms);
			}
		}

		public ObjectAnimation[] ToObjectAnimation(Body body)
		{
			ObjectAnimation[] array = new ObjectAnimation[body.Nodes.Count];
			foreach (BodyNode node in body.Nodes)
			{
				array[node.Index] = new ObjectAnimation
				{
					Name = Name + "_" + node.Name,
					Length = Heights.Count
				};
			}
			FillObjectAnimationFrames(array, body.Root, null);
			return array;
		}

		private void FillObjectAnimationFrames(ObjectAnimation[] anims, BodyNode node, BodyNode parentNode)
		{
			ObjectAnimationKey[] array = new ObjectAnimationKey[Velocities.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new ObjectAnimationKey
				{
					Time = i,
					Scale = Vector3.One
				};
			}
			List<KeyFrame> list = Rotations[node.Index];
			Quaternion[] array2 = new Quaternion[list.Count];
			bool flag = FrameSize == 6;
			for (int j = 0; j < list.Count; j++)
			{
				KeyFrame keyFrame = list[j];
				if (flag)
				{
					array2[j] = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(keyFrame.Rotation.X)) * Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(keyFrame.Rotation.Y)) * Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(keyFrame.Rotation.Z));
				}
				else
				{
					array2[j] = new Quaternion(keyFrame.Rotation);
				}
			}
			int num = 0;
			for (int k = 0; k < list.Count; k++)
			{
				int duration = list[k].Duration;
				Quaternion q = array2[k];
				Quaternion q2 = ((k == list.Count - 1) ? array2[k] : array2[k + 1]);
				for (int l = 0; l < duration; l++)
				{
					array[num++].Rotation = Quaternion.Lerp(q, q2, (float)l / (float)duration);
				}
			}
			if (parentNode == null)
			{
				Vector2 zero = Vector2.Zero;
				for (int m = 0; m < array.Length; m++)
				{
					zero += Velocities[m];
				}
			}
			else
			{
				for (int n = 0; n < array.Length; n++)
				{
					array[n].Translation = node.Translation;
				}
				ObjectAnimationKey[] keys = anims[parentNode.Index].Keys;
				for (int num2 = 0; num2 < array.Length; num2++)
				{
					array[num2].Rotation = keys[num2].Rotation * array[num2].Rotation;
					array[num2].Translation = keys[num2].Translation + Vector3.Transform(array[num2].Translation, keys[num2].Rotation);
				}
			}
			anims[node.Index].Keys = array;
			foreach (BodyNode node2 in node.Nodes)
			{
				FillObjectAnimationFrames(anims, node2, node);
			}
		}
	}
}
