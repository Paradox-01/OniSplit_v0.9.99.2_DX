using System;
using System.Collections.Generic;

namespace Oni.Totoro
{
	internal class AnimationDatReader
	{
		private class DatExtent
		{
			public int Frame;

			public readonly AttackExtent Extent = new AttackExtent();
		}

		private class DatExtentInfo
		{
			public float MaxHorizontal;

			public float MinY = 1E+09f;

			public float MaxY = -1E+09f;

			public readonly DatExtentInfoFrame FirstExtent = new DatExtentInfoFrame();

			public readonly DatExtentInfoFrame MaxExtent = new DatExtentInfoFrame();
		}

		private class DatExtentInfoFrame
		{
			public int Frame = -1;

			public int Attack;

			public int AttackOffset;

			public Vector2 Location;

			public float Height;

			public float Length;

			public float MinY;

			public float MaxY;

			public float Angle;
		}

		private readonly Animation animation = new Animation();

		private readonly InstanceDescriptor tram;

		private readonly BinaryReader dat;

		private AnimationDatReader(InstanceDescriptor tram, BinaryReader dat)
		{
			this.tram = tram;
			this.dat = dat;
		}

		public static Animation Read(InstanceDescriptor tram)
		{
			using (BinaryReader binaryReader = tram.OpenRead())
			{
				AnimationDatReader animationDatReader = new AnimationDatReader(tram, binaryReader);
				animationDatReader.ReadAnimation();
				return animationDatReader.animation;
			}
		}

		private void ReadAnimation()
		{
			dat.Skip(4);
			int offset = dat.ReadInt32();
			int offset2 = dat.ReadInt32();
			int offset3 = dat.ReadInt32();
			int offset4 = dat.ReadInt32();
			int offset5 = dat.ReadInt32();
			int offset6 = dat.ReadInt32();
			ReadOptionalThrowInfo();
			int offset7 = dat.ReadInt32();
			int offset8 = dat.ReadInt32();
			int offset9 = dat.ReadInt32();
			int offset10 = dat.ReadInt32();
			int offset11 = dat.ReadInt32();
			animation.Flags = (AnimationFlags)dat.ReadInt32();
			InstanceDescriptor[] array = dat.ReadLinkArray(2);
			for (int i = 0; i < array.Length; i++)
			{
				animation.DirectAnimations[i] = ((array[i] != null) ? array[i].FullName : null);
			}
			animation.OverlayUsedBones = (BoneMask)dat.ReadInt32();
			animation.OverlayReplacedBones = (BoneMask)dat.ReadInt32();
			animation.FinalRotation = dat.ReadSingle();
			animation.Direction = (Direction)dat.ReadUInt16();
			animation.Vocalization = dat.ReadUInt16();
			List<DatExtent> source = ReadExtentInfo();
			animation.Impact = dat.ReadString(16);
			animation.HardPause = dat.ReadUInt16();
			animation.SoftPause = dat.ReadUInt16();
			int count = dat.ReadInt32();
			dat.Skip(6);
			int num = dat.ReadUInt16();
			animation.FrameSize = dat.ReadUInt16();
			animation.Type = (AnimationType)dat.ReadUInt16();
			animation.AimingType = (AnimationType)dat.ReadUInt16();
			animation.FromState = (AnimationState)dat.ReadUInt16();
			animation.ToState = (AnimationState)dat.ReadUInt16();
			int boneCount = dat.ReadUInt16();
			int num2 = dat.ReadUInt16();
			int num3 = dat.ReadInt16();
			animation.Varient = (AnimationVarient)dat.ReadUInt16();
			dat.Skip(2);
			animation.AtomicStart = dat.ReadUInt16();
			animation.AtomicEnd = dat.ReadUInt16();
			animation.InterpolationEnd = dat.ReadUInt16();
			animation.InterpolationMax = dat.ReadUInt16();
			animation.ActionFrame = dat.ReadUInt16();
			animation.FirstLevelAvailable = dat.ReadUInt16();
			animation.InvulnerableStart = dat.ReadByte();
			animation.InvulnerableEnd = dat.ReadByte();
			int count2 = dat.ReadByte();
			int count3 = dat.ReadByte();
			int count4 = dat.ReadByte();
			int count5 = dat.ReadByte();
			int count6 = dat.ReadByte();
			int count7 = dat.ReadByte();
			ReadRawArray(offset, num2, animation.Heights, (BinaryReader r) => r.ReadSingle());
			ReadRawArray(offset2, num2, animation.Velocities, (BinaryReader r) => r.ReadVector2());
			ReadRotations(offset10, boneCount, num2);
			ReadRawArray(offset9, num2, animation.Positions, ReadPosition);
			ReadRawArray(offset6, count5, animation.Shortcuts, ReadShortcut);
			ReadRawArray(offset4, count3, animation.SelfDamage, ReadDamage);
			ReadRawArray(offset8, count7, animation.Particles, ReadParticle);
			ReadRawArray(offset7, count6, animation.Footsteps, ReadFootstep);
			ReadRawArray(offset11, count, animation.Sounds, ReadSound);
			ReadRawArray(offset5, count4, animation.MotionBlur, ReadMotionBlur);
			ReadRawArray(offset3, count2, animation.Attacks, ReadAttack);
			foreach (Attack attack in animation.Attacks)
			{
				int i2;
				for (i2 = attack.Start; i2 <= attack.End; i2++)
				{
					DatExtent datExtent = source.FirstOrDefault((DatExtent e) => e.Frame == i2);
					if (datExtent != null)
					{
						attack.Extents.Add(datExtent.Extent);
					}
				}
			}
		}

		private void ReadRotations(int offset, int boneCount, int frameCount)
		{
			using (BinaryReader binaryReader = tram.GetRawReader(offset))
			{
				int position = binaryReader.Position;
				ushort[] array = binaryReader.ReadUInt16Array(boneCount);
				ushort[] array2 = array;
				foreach (int num in array2)
				{
					binaryReader.Position = position + num;
					List<KeyFrame> list = new List<KeyFrame>();
					int num2 = 0;
					do
					{
						KeyFrame keyFrame = new KeyFrame();
						if (animation.FrameSize == 6)
						{
							keyFrame.Rotation.X = (float)binaryReader.ReadInt16() * 180f / 32767.5f;
							keyFrame.Rotation.Y = (float)binaryReader.ReadInt16() * 180f / 32767.5f;
							keyFrame.Rotation.Z = (float)binaryReader.ReadInt16() * 180f / 32767.5f;
						}
						else if (animation.FrameSize == 16)
						{
							keyFrame.Rotation = binaryReader.ReadQuaternion().ToVector4();
						}
						if (num2 == frameCount - 1)
						{
							keyFrame.Duration = 1;
						}
						else
						{
							keyFrame.Duration = binaryReader.ReadByte();
						}
						num2 += keyFrame.Duration;
						list.Add(keyFrame);
					}
					while (num2 < frameCount);
					animation.Rotations.Add(list);
				}
			}
		}

		private List<DatExtent> ReadExtentInfo()
		{
			DatExtentInfo datExtentInfo = new DatExtentInfo
			{
				MaxHorizontal = dat.ReadSingle(),
				MinY = dat.ReadSingle(),
				MaxY = dat.ReadSingle()
			};
			for (int i = 0; i < animation.AttackRing.Length; i++)
			{
				animation.AttackRing[i] = dat.ReadSingle();
			}
			datExtentInfo.FirstExtent.Frame = dat.ReadUInt16();
			datExtentInfo.FirstExtent.Attack = dat.ReadByte();
			datExtentInfo.FirstExtent.AttackOffset = dat.ReadByte();
			datExtentInfo.FirstExtent.Location = dat.ReadVector2();
			datExtentInfo.FirstExtent.Height = dat.ReadSingle();
			datExtentInfo.FirstExtent.Length = dat.ReadSingle();
			datExtentInfo.FirstExtent.MinY = dat.ReadSingle();
			datExtentInfo.FirstExtent.MaxY = dat.ReadSingle();
			datExtentInfo.FirstExtent.Angle = dat.ReadSingle();
			datExtentInfo.MaxExtent.Frame = dat.ReadUInt16();
			datExtentInfo.MaxExtent.Attack = dat.ReadByte();
			datExtentInfo.MaxExtent.AttackOffset = dat.ReadByte();
			datExtentInfo.MaxExtent.Location = dat.ReadVector2();
			datExtentInfo.MaxExtent.Height = dat.ReadSingle();
			datExtentInfo.MaxExtent.Length = dat.ReadSingle();
			datExtentInfo.MaxExtent.MinY = dat.ReadSingle();
			datExtentInfo.MaxExtent.MaxY = dat.ReadSingle();
			datExtentInfo.MaxExtent.Angle = dat.ReadSingle();
			dat.Skip(4);
			int count = dat.ReadInt32();
			int offset = dat.ReadInt32();
			List<DatExtent> list = new List<DatExtent>();
			ReadRawArray(offset, count, list, ReadExtent);
			foreach (DatExtent item in list)
			{
				AttackExtent extent = item.Extent;
				if (item.Frame == datExtentInfo.FirstExtent.Frame)
				{
					extent.Angle = MathHelper.ToDegrees(datExtentInfo.FirstExtent.Angle);
					extent.Length = datExtentInfo.FirstExtent.Length;
					extent.MinY = datExtentInfo.FirstExtent.MinY;
					extent.MaxY = datExtentInfo.FirstExtent.MaxY;
				}
				else if (item.Frame == datExtentInfo.MaxExtent.Frame)
				{
					extent.Angle = MathHelper.ToDegrees(datExtentInfo.MaxExtent.Angle);
					extent.Length = datExtentInfo.MaxExtent.Length;
					extent.MinY = datExtentInfo.MaxExtent.MinY;
					extent.MaxY = datExtentInfo.MaxExtent.MaxY;
				}
				if (Math.Abs(extent.MinY - datExtentInfo.MinY) < 0.01f)
				{
					extent.MinY = datExtentInfo.MinY;
				}
				if (Math.Abs(extent.MaxY - datExtentInfo.MaxY) < 0.01f)
				{
					extent.MaxY = datExtentInfo.MaxY;
				}
			}
			return list;
		}

		private void ReadOptionalThrowInfo()
		{
			int num = dat.ReadInt32();
			if (num != 0)
			{
				using (BinaryReader raw = tram.GetRawReader(num))
				{
					animation.ThrowSource = ReadThrowInfo(raw);
				}
			}
		}

		private ThrowInfo ReadThrowInfo(BinaryReader raw)
		{
			return new ThrowInfo
			{
				Position = raw.ReadVector3(),
				Angle = raw.ReadSingle(),
				Distance = raw.ReadSingle(),
				Type = (AnimationType)raw.ReadUInt16()
			};
		}

		private Shortcut ReadShortcut(BinaryReader raw)
		{
			return new Shortcut
			{
				FromState = (AnimationState)raw.ReadUInt16(),
				Length = raw.ReadUInt16(),
				ReplaceAtomic = (raw.ReadInt32() != 0)
			};
		}

		private Footstep ReadFootstep(BinaryReader raw)
		{
			return new Footstep
			{
				Frame = raw.ReadUInt16(),
				Type = (FootstepType)raw.ReadUInt16()
			};
		}

		private Sound ReadSound(BinaryReader raw)
		{
			return new Sound
			{
				Name = raw.ReadString(32),
				Start = raw.ReadUInt16()
			};
		}

		private MotionBlur ReadMotionBlur(BinaryReader raw)
		{
			MotionBlur result = new MotionBlur
			{
				Bones = (BoneMask)raw.ReadInt32(),
				Start = raw.ReadUInt16(),
				End = raw.ReadUInt16(),
				Lifetime = raw.ReadByte(),
				Alpha = raw.ReadByte(),
				Interval = raw.ReadByte()
			};
			raw.Skip(1);
			return result;
		}

		private Particle ReadParticle(BinaryReader raw)
		{
			return new Particle
			{
				Start = raw.ReadUInt16(),
				End = raw.ReadUInt16(),
				Bone = (Bone)raw.ReadInt32(),
				Name = raw.ReadString(16)
			};
		}

		private Damage ReadDamage(BinaryReader raw)
		{
			return new Damage
			{
				Points = raw.ReadUInt16(),
				Frame = raw.ReadUInt16()
			};
		}

		private Position ReadPosition(BinaryReader raw)
		{
			return new Position
			{
				X = (float)raw.ReadInt16() * 0.01f,
				Z = (float)raw.ReadInt16() * 0.01f,
				Height = (float)(int)raw.ReadUInt16() * 0.01f,
				YOffset = (float)raw.ReadInt16() * 0.01f
			};
		}

		private Attack ReadAttack(BinaryReader raw)
		{
			Attack result = new Attack
			{
				Bones = (BoneMask)raw.ReadInt32(),
				Knockback = raw.ReadSingle(),
				Flags = (AttackFlags)raw.ReadInt32(),
				HitPoints = raw.ReadInt16(),
				Start = raw.ReadUInt16(),
				End = raw.ReadUInt16(),
				HitType = (AnimationType)raw.ReadUInt16(),
				HitLength = raw.ReadUInt16(),
				StunLength = raw.ReadUInt16(),
				StaggerLength = raw.ReadUInt16()
			};
			raw.Skip(6);
			return result;
		}

		private DatExtent ReadExtent(BinaryReader raw)
		{
			DatExtent datExtent = new DatExtent();
			datExtent.Frame = raw.ReadInt16();
			datExtent.Extent.Angle = (float)(int)raw.ReadUInt16() * 360f / 65535f;
			datExtent.Extent.Length = (float)(raw.ReadUInt32() & 0xFFFF) * 0.01f;
			datExtent.Extent.MinY = (float)raw.ReadInt16() * 0.01f;
			datExtent.Extent.MaxY = (float)raw.ReadInt16() * 0.01f;
			return datExtent;
		}

		private void ReadRawArray<T>(int offset, int count, List<T> list, Func<BinaryReader, T> readElement)
		{
			if (offset == 0 || count == 0)
			{
				return;
			}
			using (BinaryReader arg = tram.GetRawReader(offset))
			{
				for (int i = 0; i < count; i++)
				{
					list.Add(readElement(arg));
				}
			}
		}
	}
}
