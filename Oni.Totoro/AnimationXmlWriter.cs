using System.Collections.Generic;
using System.Xml;
using Oni.Metadata;

namespace Oni.Totoro
{
	internal class AnimationXmlWriter
	{
		private Animation animation;

		private XmlWriter xml;

		private string daeFileName;

		private int startFrame;

		private int endFrame;

		private AnimationXmlWriter()
		{
		}

		public static void Write(Animation animation, XmlWriter xml, string daeFileName, int startFrame, int endFrame)
		{
			AnimationXmlWriter animationXmlWriter = new AnimationXmlWriter
			{
				xml = xml,
				animation = animation,
				daeFileName = daeFileName,
				startFrame = startFrame,
				endFrame = endFrame
			};
			animationXmlWriter.Write();
		}

		private void Write()
		{
			xml.WriteStartElement("Animation");
			if (daeFileName != null)
			{
				xml.WriteStartElement("Import");
				xml.WriteAttributeString("Path", daeFileName);
				if (startFrame > 0 || endFrame > 0)
				{
					xml.WriteElementString("Start", XmlConvert.ToString(startFrame));
				}
				if (endFrame > 0)
				{
					xml.WriteElementString("End", XmlConvert.ToString(endFrame));
				}
				xml.WriteEndElement();
			}
			xml.WriteStartElement("Lookup");
			xml.WriteElementString("Type", animation.Type.ToString());
			xml.WriteElementString("AimingType", animation.AimingType.ToString());
			xml.WriteElementString("FromState", animation.FromState.ToString());
			xml.WriteElementString("ToState", animation.ToState.ToString());
			xml.WriteElementString("Varient", MetaEnum.ToString(animation.Varient));
			xml.WriteElementString("FirstLevel", XmlConvert.ToString(animation.FirstLevelAvailable));
			WriteRawArray("Shortcuts", animation.Shortcuts, Write);
			xml.WriteEndElement();
			xml.WriteElementString("Flags", MetaEnum.ToString(animation.Flags));
			xml.WriteStartElement("Atomic");
			xml.WriteElementString("Start", XmlConvert.ToString(animation.AtomicStart));
			xml.WriteElementString("End", XmlConvert.ToString(animation.AtomicEnd));
			xml.WriteEndElement();
			xml.WriteStartElement("Invulnerable");
			xml.WriteElementString("Start", XmlConvert.ToString(animation.InvulnerableStart));
			xml.WriteElementString("End", XmlConvert.ToString(animation.InvulnerableEnd));
			xml.WriteEndElement();
			xml.WriteStartElement("Overlay");
			xml.WriteElementString("UsedBones", MetaEnum.ToString(animation.OverlayUsedBones));
			xml.WriteElementString("ReplacedBones", MetaEnum.ToString(animation.OverlayReplacedBones));
			xml.WriteEndElement();
			xml.WriteStartElement("DirectAnimations");
			xml.WriteElementString("Link", animation.DirectAnimations[0]);
			xml.WriteElementString("Link", animation.DirectAnimations[1]);
			xml.WriteEndElement();
			xml.WriteStartElement("Pause");
			xml.WriteElementString("Hard", XmlConvert.ToString(animation.HardPause));
			xml.WriteElementString("Soft", XmlConvert.ToString(animation.SoftPause));
			xml.WriteEndElement();
			xml.WriteStartElement("Interpolation");
			xml.WriteElementString("End", XmlConvert.ToString(animation.InterpolationEnd));
			xml.WriteElementString("Max", XmlConvert.ToString(animation.InterpolationMax));
			xml.WriteEndElement();
			xml.WriteElementString("FinalRotation", XmlConvert.ToString(MathHelper.ToDegrees(animation.FinalRotation)));
			xml.WriteElementString("Direction", animation.Direction.ToString());
			xml.WriteElementString("Vocalization", XmlConvert.ToString(animation.Vocalization));
			xml.WriteElementString("ActionFrame", XmlConvert.ToString(animation.ActionFrame));
			xml.WriteElementString("Impact", animation.Impact);
			WriteRawArray("Particles", animation.Particles, Write);
			WriteRawArray("MotionBlur", animation.MotionBlur, Write);
			WriteRawArray("Footsteps", animation.Footsteps, Write);
			WriteRawArray("Sounds", animation.Sounds, Write);
			if (daeFileName == null)
			{
				WriteHeights();
				WriteVelocities();
				WriteRotations();
				WritePositions();
			}
			WriteThrowInfo();
			WriteRawArray("SelfDamage", animation.SelfDamage, Write);
			if (animation.Attacks.Count > 0)
			{
				WriteRawArray("Attacks", animation.Attacks, Write);
				if (daeFileName == null)
				{
					WriteAttackRing();
				}
			}
			xml.WriteEndElement();
		}

		private void WriteRotations()
		{
			xml.WriteStartElement("Rotations");
			foreach (List<KeyFrame> rotation in animation.Rotations)
			{
				xml.WriteStartElement("Bone");
				foreach (KeyFrame item in rotation)
				{
					if (animation.FrameSize == 6)
					{
						xml.WriteElementString("EKey", string.Format("{0} {1} {2} {3}", XmlConvert.ToString(item.Duration), XmlConvert.ToString(item.Rotation.X), XmlConvert.ToString(item.Rotation.Y), XmlConvert.ToString(item.Rotation.Z)));
					}
					else if (animation.FrameSize == 16)
					{
						xml.WriteElementString("QKey", string.Format("{0} {1} {2} {3} {4}", XmlConvert.ToString(item.Duration), XmlConvert.ToString(item.Rotation.X), XmlConvert.ToString(item.Rotation.Y), XmlConvert.ToString(item.Rotation.Z), XmlConvert.ToString(0f - item.Rotation.W)));
					}
				}
				xml.WriteEndElement();
			}
			xml.WriteEndElement();
		}

		private void WritePositions()
		{
			List<Position> positions = animation.Positions;
			xml.WriteStartElement("PositionOffset");
			xml.WriteElementString("X", XmlConvert.ToString((positions.Count == 0) ? 0f : positions[0].X));
			xml.WriteElementString("Z", XmlConvert.ToString((positions.Count == 0) ? 0f : positions[0].Z));
			xml.WriteEndElement();
			WriteRawArray("Positions", animation.Positions, Write);
		}

		private void WriteHeights()
		{
			xml.WriteStartElement("Heights");
			foreach (float height in animation.Heights)
			{
				xml.WriteElementString("Height", XmlConvert.ToString(height));
			}
			xml.WriteEndElement();
		}

		private void WriteVelocities()
		{
			xml.WriteStartElement("Velocities");
			foreach (Vector2 velocity in animation.Velocities)
			{
				xml.WriteElementString("Velocity", string.Format("{0} {1}", XmlConvert.ToString(velocity.X), XmlConvert.ToString(velocity.Y)));
			}
			xml.WriteEndElement();
		}

		private void WriteAttackRing()
		{
			xml.WriteStartElement("AttackRing");
			for (int i = 0; i < 36; i++)
			{
				xml.WriteElementString("Length", XmlConvert.ToString(animation.AttackRing[i]));
			}
			xml.WriteEndElement();
		}

		private void WriteThrowInfo()
		{
			xml.WriteStartElement("ThrowSource");
			ThrowInfo throwSource = animation.ThrowSource;
			if (throwSource != null)
			{
				xml.WriteStartElement("TargetAdjustment");
				xml.WriteElementString("Position", string.Format("{0} {1} {2}", XmlConvert.ToString(throwSource.Position.X), XmlConvert.ToString(throwSource.Position.Y), XmlConvert.ToString(throwSource.Position.Z)));
				xml.WriteElementString("Angle", XmlConvert.ToString(throwSource.Angle));
				xml.WriteEndElement();
				xml.WriteElementString("Distance", XmlConvert.ToString(throwSource.Distance));
				xml.WriteElementString("TargetType", throwSource.Type.ToString());
			}
			xml.WriteEndElement();
		}

		private void Write(Position position)
		{
			xml.WriteStartElement("Position");
			xml.WriteElementString("Height", XmlConvert.ToString(position.Height));
			xml.WriteElementString("YOffset", XmlConvert.ToString(position.YOffset));
			xml.WriteEndElement();
		}

		private void Write(AttackExtent extent)
		{
			xml.WriteStartElement("Extent");
			xml.WriteElementString("Angle", XmlConvert.ToString(extent.Angle));
			xml.WriteElementString("Length", XmlConvert.ToString(extent.Length));
			xml.WriteElementString("MinY", XmlConvert.ToString(extent.MinY));
			xml.WriteElementString("MaxY", XmlConvert.ToString(extent.MaxY));
			xml.WriteEndElement();
		}

		private void Write(Attack attack)
		{
			xml.WriteStartElement("Attack");
			xml.WriteElementString("Start", XmlConvert.ToString(attack.Start));
			xml.WriteElementString("End", XmlConvert.ToString(attack.End));
			xml.WriteElementString("Bones", MetaEnum.ToString(attack.Bones));
			xml.WriteElementString("Flags", MetaEnum.ToString(attack.Flags));
			xml.WriteElementString("Knockback", XmlConvert.ToString(attack.Knockback));
			xml.WriteElementString("HitPoints", XmlConvert.ToString(attack.HitPoints));
			xml.WriteElementString("HitType", attack.HitType.ToString());
			xml.WriteElementString("HitLength", XmlConvert.ToString(attack.HitLength));
			xml.WriteElementString("StunLength", XmlConvert.ToString(attack.StunLength));
			xml.WriteElementString("StaggerLength", XmlConvert.ToString(attack.StaggerLength));
			if (daeFileName == null)
			{
				WriteRawArray("Extents", attack.Extents, Write);
			}
			xml.WriteEndElement();
		}

		private void Write(Particle particle)
		{
			xml.WriteStartElement("Particle");
			xml.WriteElementString("Start", XmlConvert.ToString(particle.Start));
			xml.WriteElementString("End", XmlConvert.ToString(particle.End));
			xml.WriteElementString("Bone", particle.Bone.ToString());
			xml.WriteElementString("Name", particle.Name);
			xml.WriteEndElement();
		}

		private void Write(Damage damage)
		{
			xml.WriteStartElement("Damage");
			xml.WriteElementString("Points", XmlConvert.ToString(damage.Points));
			xml.WriteElementString("Frame", XmlConvert.ToString(damage.Frame));
			xml.WriteEndElement();
		}

		private void Write(Sound sound)
		{
			xml.WriteStartElement("Sound");
			xml.WriteElementString("Name", sound.Name);
			xml.WriteElementString("Start", XmlConvert.ToString(sound.Start));
			xml.WriteEndElement();
		}

		private void Write(Footstep footstep)
		{
			xml.WriteStartElement("Footstep");
			xml.WriteElementString("Frame", XmlConvert.ToString(footstep.Frame));
			xml.WriteElementString("Type", MetaEnum.ToString(footstep.Type));
			xml.WriteEndElement();
		}

		private void Write(Shortcut shortcut)
		{
			xml.WriteStartElement("Shortcut");
			xml.WriteElementString("FromState", shortcut.FromState.ToString());
			xml.WriteElementString("Length", XmlConvert.ToString(shortcut.Length));
			xml.WriteElementString("ReplaceAtomic", shortcut.ReplaceAtomic ? "yes" : "no");
			xml.WriteEndElement();
		}

		private void Write(MotionBlur motionBlur)
		{
			xml.WriteStartElement("MotionBlur");
			xml.WriteElementString("Bones", MetaEnum.ToString(motionBlur.Bones));
			xml.WriteElementString("Start", XmlConvert.ToString(motionBlur.Start));
			xml.WriteElementString("End", XmlConvert.ToString(motionBlur.End));
			xml.WriteElementString("Lifetime", XmlConvert.ToString(motionBlur.Lifetime));
			xml.WriteElementString("Alpha", XmlConvert.ToString(motionBlur.Alpha));
			xml.WriteElementString("Interval", XmlConvert.ToString(motionBlur.Interval));
			xml.WriteEndElement();
		}

		private void WriteRawArray<T>(string name, List<T> list, Action<T> elementWriter)
		{
			xml.WriteStartElement(name);
			foreach (T item in list)
			{
				elementWriter(item);
			}
			xml.WriteEndElement();
		}
	}
}
