using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Oni.Metadata;

namespace Oni.Xml
{
	internal class ObjcXmlExporter : RawXmlExporter
	{
		private readonly Dictionary<ObjectMetadata.TypeTag, Action> typeWriters = new Dictionary<ObjectMetadata.TypeTag, Action>();

		private int objectEndPosition;

		private ObjcXmlExporter(BinaryReader reader, XmlWriter xml)
			: base(reader, xml)
		{
			InitTypeWriters(typeWriters);
		}

		public static void Export(BinaryReader reader, XmlWriter xml)
		{
			ObjcXmlExporter objcXmlExporter = new ObjcXmlExporter(reader, xml);
			objcXmlExporter.Export();
		}

		private void Export()
		{
			int num = base.Reader.ReadInt32();
			int num2 = base.Reader.ReadInt32();
			base.Xml.WriteStartElement("Objects");
			while (true)
			{
				int num3 = base.Reader.ReadInt32();
				if (num3 == 0)
				{
					break;
				}
				int position = base.Reader.Position;
				objectEndPosition = position + num3;
				BeginStruct(base.Reader.Position);
				ObjectMetadata.TypeTag key = (ObjectMetadata.TypeTag)base.Reader.ReadInt32();
				int num4 = base.Reader.ReadInt32();
				base.Xml.WriteStartElement(key.ToString());
				base.Xml.WriteAttributeString("Id", XmlConvert.ToString(num4));
				base.Xml.WriteStartElement("Header");
				ObjectMetadata.Header.Accept(this);
				base.Xml.WriteEndElement();
				base.Xml.WriteStartElement("OSD");
				typeWriters[key]();
				base.Xml.WriteEndElement();
				base.Xml.WriteEndElement();
				base.Reader.Position = objectEndPosition;
			}
			base.Xml.WriteEndElement();
		}

		private void WriteCharacter()
		{
			ObjectMetadata.Character.Accept(this);
		}

		private void WriteCombatProfile()
		{
			ObjectMetadata.CombatProfile.Accept(this);
		}

		private void WriteConsole()
		{
			ObjectMetadata.Console.Accept(this);
			WriteEventList();
		}

		private void WriteDoor()
		{
			ObjectMetadata.Door.Accept(this);
			WriteEventList();
		}

		private void WriteFlag()
		{
			ObjectMetadata.Flag.Accept(this);
		}

		private void WriteFurniture()
		{
			ObjectMetadata.Furniture.Accept(this);
		}

		private void WriteMeleeProfile()
		{
			ObjectMetadata.MeleeProfile.Accept(this);
			int num = base.Reader.ReadInt32();
			int num2 = base.Reader.ReadInt32();
			int num3 = base.Reader.ReadInt32();
			int num4 = base.Reader.ReadInt32();
			int moveTablePosition = base.Reader.Position + (num + num2 + num3) * 88;
			base.Xml.WriteStartElement("Attacks");
			for (int i = 0; i < num; i++)
			{
				WriteMeleeTechnique(moveTablePosition);
			}
			base.Xml.WriteEndElement();
			base.Xml.WriteStartElement("Evades");
			for (int j = 0; j < num2; j++)
			{
				WriteMeleeTechnique(moveTablePosition);
			}
			base.Xml.WriteEndElement();
			base.Xml.WriteStartElement("Maneuvers");
			for (int k = 0; k < num3; k++)
			{
				WriteMeleeTechnique(moveTablePosition);
			}
			base.Xml.WriteEndElement();
		}

		private void WriteMeleeTechnique(int moveTablePosition)
		{
			base.Xml.WriteStartElement("Technique");
			ObjectMetadata.MeleeTechnique.Accept(this);
			int num = base.Reader.ReadInt32();
			int num2 = base.Reader.ReadInt32();
			int position = base.Reader.Position;
			base.Reader.Position = moveTablePosition + num2 * 16;
			base.Xml.WriteStartElement("Moves");
			for (int i = 0; i < num; i++)
			{
				WriteMeleeMove();
			}
			base.Xml.WriteEndElement();
			base.Xml.WriteEndElement();
			base.Reader.Position = position;
		}

		private void WriteMeleeMove()
		{
			int num = base.Reader.ReadInt32();
			float[] array = base.Reader.ReadSingleArray(3);
			ObjectMetadata.MeleeMoveCategory meleeMoveCategory = (ObjectMetadata.MeleeMoveCategory)(num >> 24);
			base.Xml.WriteStartElement(meleeMoveCategory.ToString());
			switch (meleeMoveCategory)
			{
			default:
				base.Xml.WriteAttributeString("Type", ((ObjectMetadata.MeleeMoveAttackType)(num & 0xFFFFFF)/*cast due to constrained. prefix*/).ToString());
				break;
			case ObjectMetadata.MeleeMoveCategory.Evade:
				base.Xml.WriteAttributeString("Type", ((ObjectMetadata.MeleeMoveEvadeType)(num & 0xFFFFFF)/*cast due to constrained. prefix*/).ToString());
				break;
			case ObjectMetadata.MeleeMoveCategory.Throw:
				base.Xml.WriteAttributeString("Type", ((ObjectMetadata.MeleeMoveThrowType)(num & 0xFFFFFF)/*cast due to constrained. prefix*/).ToString());
				break;
			case ObjectMetadata.MeleeMoveCategory.Maneuver:
			{
				ObjectMetadata.MeleeMoveTypeInfo meleeMoveTypeInfo = ObjectMetadata.MeleeMoveManeuverTypeInfo[num & 0xFFFFFF];
				base.Xml.WriteAttributeString("Type", meleeMoveTypeInfo.Type.ToString());
				for (int i = 0; i < meleeMoveTypeInfo.ParamNames.Length; i++)
				{
					base.Xml.WriteAttributeString(meleeMoveTypeInfo.ParamNames[i], XmlConvert.ToString(array[i]));
				}
				break;
			}
			case ObjectMetadata.MeleeMoveCategory.Position:
			{
				ObjectMetadata.MeleeMovePositionType meleeMovePositionType = (ObjectMetadata.MeleeMovePositionType)(num & 0xFFFFFF);
				base.Xml.WriteAttributeString("Type", meleeMovePositionType.ToString());
				if ((ObjectMetadata.MeleeMovePositionType.RunForward <= meleeMovePositionType && meleeMovePositionType <= ObjectMetadata.MeleeMovePositionType.RunBack) || ObjectMetadata.MeleeMovePositionType.CloseForward <= meleeMovePositionType)
				{
					base.Xml.WriteAttributeString("MinRunInDist", XmlConvert.ToString(array[0]));
					base.Xml.WriteAttributeString("MaxRunInDist", XmlConvert.ToString(array[1]));
					base.Xml.WriteAttributeString("ToleranceRange", XmlConvert.ToString(array[2]));
				}
				break;
			}
			}
			base.Xml.WriteEndElement();
		}

		private void WriteNeutralBehavior()
		{
			ObjectMetadata.NeutralBehavior.Accept(this);
			int length = base.Reader.ReadInt16();
			ObjectMetadata.NeutralBehaviorParams.Accept(this);
			base.Xml.WriteStartElement("DialogLines");
			MetaType.Array(length, ObjectMetadata.NeutralBehaviorDialogLine).Accept(this);
			base.Xml.WriteEndElement();
		}

		private void WriteParticle()
		{
			ObjectMetadata.Particle.Accept(this);
		}

		private void WritePatrolPath()
		{
			ObjectMetadata.PatrolPath.Accept(this);
			int num = base.Reader.ReadInt32();
			ObjectMetadata.PatrolPathInfo.Accept(this);
			int position = base.Reader.Position;
			int num2 = -1;
			ObjectMetadata.PatrolPathPointType patrolPathPointType = (ObjectMetadata.PatrolPathPointType)base.Reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				switch (patrolPathPointType)
				{
				case ObjectMetadata.PatrolPathPointType.Loop:
					num2 = 0;
					break;
				case ObjectMetadata.PatrolPathPointType.LoopFrom:
					num2 = base.Reader.ReadInt32();
					break;
				default:
					base.Reader.Position += ObjectMetadata.GetPatrolPathPointSize(patrolPathPointType);
					break;
				}
				patrolPathPointType = (ObjectMetadata.PatrolPathPointType)base.Reader.ReadInt32();
			}
			base.Reader.Position = position;
			base.Xml.WriteStartElement("Points");
			for (int j = 0; j < num; j++)
			{
				if (num2 == j)
				{
					base.Xml.WriteStartElement("Loop");
				}
				WritePatrolPathPoint();
			}
			if (num2 != -1)
			{
				base.Xml.WriteEndElement();
			}
			base.Xml.WriteEndElement();
		}

		private void WritePatrolPathPoint()
		{
			ObjectMetadata.PatrolPathPointType patrolPathPointType = (ObjectMetadata.PatrolPathPointType)base.Reader.ReadInt32();
			switch (patrolPathPointType)
			{
			case ObjectMetadata.PatrolPathPointType.Loop:
				return;
			case ObjectMetadata.PatrolPathPointType.LoopFrom:
				base.Reader.Skip(4);
				return;
			}
			base.Xml.WriteStartElement(patrolPathPointType.ToString());
			switch (patrolPathPointType)
			{
			case ObjectMetadata.PatrolPathPointType.IgnorePlayer:
				base.Xml.WriteAttributeString("Value", base.Reader.ReadBoolean() ? "Yes" : "No");
				break;
			case ObjectMetadata.PatrolPathPointType.MoveToFlag:
			case ObjectMetadata.PatrolPathPointType.LookAtFlag:
			case ObjectMetadata.PatrolPathPointType.MoveAndFaceFlag:
				base.Xml.WriteAttributeString("FlagId", XmlConvert.ToString(base.Reader.ReadInt16()));
				break;
			case ObjectMetadata.PatrolPathPointType.CallScript:
			case ObjectMetadata.PatrolPathPointType.ForkScript:
				base.Xml.WriteAttributeString("ScriptId", XmlConvert.ToString(base.Reader.ReadInt16()));
				break;
			case ObjectMetadata.PatrolPathPointType.Pause:
				base.Xml.WriteAttributeString("Frames", XmlConvert.ToString(base.Reader.ReadInt32()));
				break;
			case ObjectMetadata.PatrolPathPointType.MovementMode:
				base.Xml.WriteAttributeString("Mode", ((ObjectMetadata.PatrolPathMovementMode)base.Reader.ReadInt32()/*cast due to constrained. prefix*/).ToString());
				break;
			case ObjectMetadata.PatrolPathPointType.LockFacing:
				base.Xml.WriteAttributeString("Facing", ((ObjectMetadata.PatrolPathFacing)base.Reader.ReadInt32()/*cast due to constrained. prefix*/).ToString());
				break;
			case ObjectMetadata.PatrolPathPointType.MoveThroughFlag:
			case ObjectMetadata.PatrolPathPointType.MoveNearFlag:
				base.Xml.WriteAttributeString("FlagId", XmlConvert.ToString(base.Reader.ReadInt16()));
				base.Xml.WriteAttributeString("Distance", XmlConvert.ToString(base.Reader.ReadSingle()));
				break;
			case ObjectMetadata.PatrolPathPointType.GlanceAtFlagFor:
				base.Xml.WriteAttributeString("FlagId", XmlConvert.ToString(base.Reader.ReadInt16()));
				base.Xml.WriteAttributeString("Frames", XmlConvert.ToString(base.Reader.ReadInt32()));
				break;
			case ObjectMetadata.PatrolPathPointType.Scan:
				base.Xml.WriteAttributeString("Frames", XmlConvert.ToString(base.Reader.ReadInt16()));
				base.Xml.WriteAttributeString("Rotation", XmlConvert.ToString(base.Reader.ReadSingle()));
				break;
			case ObjectMetadata.PatrolPathPointType.MoveToFlagLookAndWait:
				base.Xml.WriteAttributeString("Frames", XmlConvert.ToString(base.Reader.ReadInt16()));
				base.Xml.WriteAttributeString("FlagId", XmlConvert.ToString(base.Reader.ReadInt16()));
				base.Xml.WriteAttributeString("Rotation", XmlConvert.ToString(base.Reader.ReadSingle()));
				break;
			case ObjectMetadata.PatrolPathPointType.FaceToFlagAndFire:
				base.Xml.WriteAttributeString("FlagId", XmlConvert.ToString(base.Reader.ReadInt16()));
				base.Xml.WriteAttributeString("Frames", XmlConvert.ToString(base.Reader.ReadInt16()));
				base.Xml.WriteAttributeString("Spread", XmlConvert.ToString(base.Reader.ReadSingle()));
				break;
			case ObjectMetadata.PatrolPathPointType.LookAtPoint:
			case ObjectMetadata.PatrolPathPointType.MoveToPoint:
				base.Xml.WriteAttributeString("X", XmlConvert.ToString(base.Reader.ReadSingle()));
				base.Xml.WriteAttributeString("Y", XmlConvert.ToString(base.Reader.ReadSingle()));
				base.Xml.WriteAttributeString("Z", XmlConvert.ToString(base.Reader.ReadSingle()));
				break;
			case ObjectMetadata.PatrolPathPointType.MoveThroughPoint:
				base.Xml.WriteAttributeString("X", XmlConvert.ToString(base.Reader.ReadSingle()));
				base.Xml.WriteAttributeString("Y", XmlConvert.ToString(base.Reader.ReadSingle()));
				base.Xml.WriteAttributeString("Z", XmlConvert.ToString(base.Reader.ReadSingle()));
				base.Xml.WriteAttributeString("Distance", XmlConvert.ToString(base.Reader.ReadSingle()));
				break;
			default:
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Unsupported path point type {0}", new object[1] { patrolPathPointType }));
			case ObjectMetadata.PatrolPathPointType.Stop:
			case ObjectMetadata.PatrolPathPointType.StopLooking:
			case ObjectMetadata.PatrolPathPointType.FreeFacing:
			case ObjectMetadata.PatrolPathPointType.StopScanning:
				break;
			}
			base.Xml.WriteEndElement();
		}

		private void WritePowerUp()
		{
			ObjectMetadata.PowerUp.Accept(this);
		}

		private void WriteSound()
		{
			ObjectMetadata.Sound.Accept(this);
			switch ((ObjectMetadata.SoundVolumeType)base.Reader.ReadInt32())
			{
			case ObjectMetadata.SoundVolumeType.Box:
				base.Xml.WriteStartElement("Box");
				MetaType.BoundingBox.Accept(this);
				base.Xml.WriteEndElement();
				break;
			case ObjectMetadata.SoundVolumeType.Sphere:
				base.Xml.WriteStartElement("Sphere");
				ObjectMetadata.SoundSphere.Accept(this);
				base.Xml.WriteEndElement();
				break;
			}
			ObjectMetadata.SoundParams.Accept(this);
		}

		private void WriteTriggerVolume()
		{
			ObjectMetadata.TriggerVolume.Accept(this);
		}

		private void WriteTrigger()
		{
			ObjectMetadata.Trigger.Accept(this);
			WriteEventList();
		}

		private void WriteTurret()
		{
			ObjectMetadata.Turret.Accept(this);
		}

		private void WriteWeapon()
		{
			ObjectMetadata.Weapon.Accept(this);
		}

		private void WriteEventList()
		{
			base.Xml.WriteStartElement("Events");
			int num = base.Reader.ReadInt16();
			for (int i = 0; i < num; i++)
			{
				ObjectMetadata.EventType eventType = (ObjectMetadata.EventType)base.Reader.ReadInt16();
				base.Xml.WriteStartElement(eventType.ToString());
				switch (eventType)
				{
				case ObjectMetadata.EventType.Script:
					base.Xml.WriteAttributeString("Function", base.Reader.ReadString(32));
					break;
				default:
					base.Xml.WriteAttributeString("TargetId", XmlConvert.ToString(base.Reader.ReadInt16()));
					break;
				case ObjectMetadata.EventType.None:
					break;
				}
				base.Xml.WriteEndElement();
			}
			base.Xml.WriteEndElement();
		}

		private void InitTypeWriters(Dictionary<ObjectMetadata.TypeTag, Action> typeWriters)
		{
			typeWriters.Add(ObjectMetadata.TypeTag.CHAR, WriteCharacter);
			typeWriters.Add(ObjectMetadata.TypeTag.CMBT, WriteCombatProfile);
			typeWriters.Add(ObjectMetadata.TypeTag.CONS, WriteConsole);
			typeWriters.Add(ObjectMetadata.TypeTag.DOOR, WriteDoor);
			typeWriters.Add(ObjectMetadata.TypeTag.FLAG, WriteFlag);
			typeWriters.Add(ObjectMetadata.TypeTag.FURN, WriteFurniture);
			typeWriters.Add(ObjectMetadata.TypeTag.MELE, WriteMeleeProfile);
			typeWriters.Add(ObjectMetadata.TypeTag.NEUT, WriteNeutralBehavior);
			typeWriters.Add(ObjectMetadata.TypeTag.PART, WriteParticle);
			typeWriters.Add(ObjectMetadata.TypeTag.PATR, WritePatrolPath);
			typeWriters.Add(ObjectMetadata.TypeTag.PWRU, WritePowerUp);
			typeWriters.Add(ObjectMetadata.TypeTag.SNDG, WriteSound);
			typeWriters.Add(ObjectMetadata.TypeTag.TRGV, WriteTriggerVolume);
			typeWriters.Add(ObjectMetadata.TypeTag.TRIG, WriteTrigger);
			typeWriters.Add(ObjectMetadata.TypeTag.TURR, WriteTurret);
			typeWriters.Add(ObjectMetadata.TypeTag.WEAP, WriteWeapon);
		}
	}
}
