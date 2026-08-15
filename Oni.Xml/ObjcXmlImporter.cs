using System;
using System.Collections.Generic;
using System.Xml;
using Oni.Metadata;

namespace Oni.Xml
{
	internal class ObjcXmlImporter : RawXmlImporter
	{
		private struct MeleeMove
		{
			public int Type;

			public float[] Params;
		}

		private readonly Dictionary<ObjectMetadata.TypeTag, Action> typeReaders = new Dictionary<ObjectMetadata.TypeTag, Action>();

		private int nextId;

		private ObjcXmlImporter(XmlReader xml, BinaryWriter writer)
			: base(xml, writer)
		{
			InitTypeReaders(typeReaders);
		}

		public static void Import(XmlReader xml, BinaryWriter writer)
		{
			ObjcXmlImporter objcXmlImporter = new ObjcXmlImporter(xml, writer);
			objcXmlImporter.Import();
		}

		private void Import()
		{
			base.Writer.Write(39);
			nextId = 1;
			while (base.Xml.IsStartElement())
			{
				int position = base.Writer.Position;
				base.Writer.Write(0);
				BeginStruct(position);
				ReadObject();
				base.Writer.Position = Utils.Align4(base.Writer.Position);
				int value = base.Writer.Position - position - 4;
				base.Writer.WriteAt(position, value);
			}
			base.Writer.Write(0);
		}

		private ObjectMetadata.TypeTag ReadObject()
		{
			string attribute = base.Xml.GetAttribute("Id");
			int value = (string.IsNullOrEmpty(attribute) ? nextId++ : XmlConvert.ToInt32(attribute));
			string text = base.Xml.GetAttribute("Type");
			if (text == null)
			{
				text = base.Xml.LocalName;
			}
			ObjectMetadata.TypeTag typeTag = MetaEnum.Parse<ObjectMetadata.TypeTag>(text);
			base.Xml.ReadStartElement();
			base.Xml.MoveToContent();
			base.Writer.Write((int)typeTag);
			base.Writer.Write(value);
			ObjectMetadata.Header.Accept(this);
			typeReaders[typeTag]();
			base.Xml.ReadEndElement();
			return typeTag;
		}

		private void ReadCharacter()
		{
			ObjectMetadata.Character.Accept(this);
		}

		private void ReadCombatProfile()
		{
			ObjectMetadata.CombatProfile.Accept(this);
		}

		private void ReadConsole()
		{
			base.Xml.ReadStartElement();
			base.Xml.MoveToContent();
			ReadStruct(ObjectMetadata.Console);
			ReadEventList();
			base.Xml.ReadEndElement();
		}

		private void ReadDoor()
		{
			base.Xml.ReadStartElement();
			base.Xml.MoveToContent();
			ReadStruct(ObjectMetadata.Door);
			ReadEventList();
			base.Xml.ReadEndElement();
		}

		private void ReadFlag()
		{
			ObjectMetadata.Flag.Accept(this);
		}

		private void ReadFurniture()
		{
			ObjectMetadata.Furniture.Accept(this);
		}

		private void ReadMeleeProfile()
		{
			base.Xml.ReadStartElement();
			base.Xml.MoveToContent();
			ReadStruct(ObjectMetadata.MeleeProfile);
			int position = base.Writer.Position;
			base.Writer.Write(0);
			base.Writer.Write(0);
			base.Writer.Write(0);
			base.Writer.Write(0);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			List<MeleeMove> list = new List<MeleeMove>();
			num = ReadMeleeTechniques("Attacks", list);
			num2 = ReadMeleeTechniques("Evades", list);
			num3 = ReadMeleeTechniques("Maneuvers", list);
			foreach (MeleeMove item in list)
			{
				base.Writer.Write(item.Type);
				base.Writer.Write(item.Params);
			}
			int position2 = base.Writer.Position;
			base.Writer.Position = position;
			base.Writer.Write(num);
			base.Writer.Write(num2);
			base.Writer.Write(num3);
			base.Writer.Write(list.Count);
			base.Writer.Position = position2;
			base.Xml.ReadEndElement();
		}

		private int ReadMeleeTechniques(string xmlName, List<MeleeMove> moves)
		{
			if (base.Xml.IsStartElement(xmlName) && base.Xml.IsEmptyElement)
			{
				base.Xml.Skip();
				return 0;
			}
			base.Xml.ReadStartElement(xmlName);
			base.Xml.MoveToContent();
			int num = 0;
			while (base.Xml.IsStartElement("Technique"))
			{
				base.Xml.ReadStartElement();
				base.Xml.MoveToContent();
				ReadStruct(ObjectMetadata.MeleeTechnique);
				int position = base.Writer.Position;
				base.Writer.Write(0);
				base.Writer.Write(moves.Count);
				int num2 = 0;
				if (base.Xml.IsStartElement("Moves") && base.Xml.IsEmptyElement)
				{
					base.Xml.Skip();
				}
				else
				{
					base.Xml.ReadStartElement("Moves");
					base.Xml.MoveToContent();
					while (base.Xml.IsStartElement())
					{
						ReadMeleeMove(moves);
						num2++;
					}
					base.Xml.ReadEndElement();
				}
				base.Xml.ReadEndElement();
				base.Writer.WriteAt(position, num2);
				num++;
			}
			base.Xml.ReadEndElement();
			return num;
		}

		private void ReadMeleeMove(List<MeleeMove> moves)
		{
			ObjectMetadata.MeleeMoveCategory meleeMoveCategory = (ObjectMetadata.MeleeMoveCategory)Enum.Parse(typeof(ObjectMetadata.MeleeMoveCategory), base.Xml.LocalName);
			string attribute = base.Xml.GetAttribute("Type");
			float[] array = new float[3];
			int num;
			switch (meleeMoveCategory)
			{
			default:
				num = Convert.ToInt32(MetaEnum.Parse<ObjectMetadata.MeleeMoveAttackType>(attribute));
				break;
			case ObjectMetadata.MeleeMoveCategory.Evade:
				num = Convert.ToInt32(MetaEnum.Parse<ObjectMetadata.MeleeMoveEvadeType>(attribute));
				break;
			case ObjectMetadata.MeleeMoveCategory.Throw:
				num = Convert.ToInt32(MetaEnum.Parse<ObjectMetadata.MeleeMoveThrowType>(attribute));
				break;
			case ObjectMetadata.MeleeMoveCategory.Position:
			{
				ObjectMetadata.MeleeMovePositionType meleeMovePositionType = MetaEnum.Parse<ObjectMetadata.MeleeMovePositionType>(attribute);
				if ((ObjectMetadata.MeleeMovePositionType.RunForward <= meleeMovePositionType && meleeMovePositionType <= ObjectMetadata.MeleeMovePositionType.RunBack) || ObjectMetadata.MeleeMovePositionType.CloseForward <= meleeMovePositionType)
				{
					array[0] = XmlConvert.ToSingle(base.Xml.GetAttribute("MinRunInDist"));
					array[1] = XmlConvert.ToSingle(base.Xml.GetAttribute("MaxRunInDist"));
					array[2] = XmlConvert.ToSingle(base.Xml.GetAttribute("ToleranceRange"));
				}
				num = Convert.ToInt32(meleeMovePositionType);
				break;
			}
			case ObjectMetadata.MeleeMoveCategory.Maneuver:
			{
				num = Convert.ToInt32(MetaEnum.Parse<ObjectMetadata.MeleeMoveManeuverType>(attribute));
				ObjectMetadata.MeleeMoveTypeInfo meleeMoveTypeInfo = ObjectMetadata.MeleeMoveManeuverTypeInfo[num];
				for (int i = 0; i < meleeMoveTypeInfo.ParamNames.Length; i++)
				{
					array[i] = XmlConvert.ToSingle(base.Xml.GetAttribute(meleeMoveTypeInfo.ParamNames[i]));
				}
				break;
			}
			}
			moves.Add(new MeleeMove
			{
				Type = (((int)meleeMoveCategory << 24) | (num & 0xFFFFFF)),
				Params = array
			});
			base.Xml.Skip();
		}

		private void ReadNeutralBehavior()
		{
			base.Xml.ReadStartElement();
			base.Xml.MoveToContent();
			ReadStruct(ObjectMetadata.NeutralBehavior);
			int position = base.Writer.Position;
			base.Writer.WriteUInt16(0);
			ReadStruct(ObjectMetadata.NeutralBehaviorParams);
			base.Xml.ReadStartElement("DialogLines");
			short num = 0;
			while (base.Xml.IsStartElement("DialogLine"))
			{
				ObjectMetadata.NeutralBehaviorDialogLine.Accept(this);
				num++;
			}
			base.Xml.ReadEndElement();
			base.Xml.ReadEndElement();
			base.Writer.WriteAt(position, num);
		}

		private void ReadParticle()
		{
			ObjectMetadata.Particle.Accept(this);
		}

		private void ReadPatrolPath()
		{
			base.Xml.ReadStartElement();
			base.Xml.MoveToContent();
			ReadStruct(ObjectMetadata.PatrolPath);
			int position = base.Writer.Position;
			base.Writer.Write(0);
			ReadStruct(ObjectMetadata.PatrolPathInfo);
			int num = 0;
			bool isEmptyElement = base.Xml.IsEmptyElement;
			base.Xml.ReadStartElement("Points");
			if (!isEmptyElement)
			{
				int num2 = -1;
				while (base.Xml.IsStartElement())
				{
					if (ReadPatrolPathPoint())
					{
						num2 = num;
					}
					num++;
				}
				if (num2 != -1)
				{
					if (num2 == 0)
					{
						base.Writer.Write(6);
					}
					else
					{
						base.Writer.Write(16);
						base.Writer.Write(num2);
					}
					if (base.Xml.NodeType == XmlNodeType.EndElement && base.Xml.LocalName == "Loop")
					{
						base.Xml.ReadEndElement();
					}
				}
				base.Xml.ReadEndElement();
			}
			base.Xml.ReadEndElement();
			base.Writer.WriteAt(position, num);
		}

		private bool ReadPatrolPathPoint()
		{
			ObjectMetadata.PatrolPathPointType patrolPathPointType = MetaEnum.Parse<ObjectMetadata.PatrolPathPointType>(base.Xml.LocalName);
			if (patrolPathPointType == ObjectMetadata.PatrolPathPointType.Loop)
			{
				if (base.Xml.IsEmptyElement)
				{
					base.Xml.Skip();
				}
				else
				{
					base.Xml.ReadStartElement();
					base.Xml.MoveToContent();
				}
				return true;
			}
			base.Writer.Write((int)patrolPathPointType);
			switch (patrolPathPointType)
			{
			case ObjectMetadata.PatrolPathPointType.IgnorePlayer:
				base.Writer.WriteByte((base.Xml.GetAttribute("Value") == "Yes") ? 1 : 0);
				break;
			case ObjectMetadata.PatrolPathPointType.CallScript:
			case ObjectMetadata.PatrolPathPointType.ForkScript:
				base.Writer.Write(XmlConvert.ToInt16(base.Xml.GetAttribute("ScriptId")));
				break;
			case ObjectMetadata.PatrolPathPointType.MoveToFlag:
			case ObjectMetadata.PatrolPathPointType.LookAtFlag:
			case ObjectMetadata.PatrolPathPointType.MoveAndFaceFlag:
				base.Writer.Write(XmlConvert.ToInt16(base.Xml.GetAttribute("FlagId")));
				break;
			case ObjectMetadata.PatrolPathPointType.MovementMode:
				base.Writer.Write(Convert.ToInt32(MetaEnum.Parse<ObjectMetadata.PatrolPathMovementMode>(base.Xml.GetAttribute("Mode"))));
				break;
			case ObjectMetadata.PatrolPathPointType.LockFacing:
				base.Writer.Write(Convert.ToInt32(MetaEnum.Parse<ObjectMetadata.PatrolPathFacing>(base.Xml.GetAttribute("Facing"))));
				break;
			case ObjectMetadata.PatrolPathPointType.Pause:
				base.Writer.Write(XmlConvert.ToInt32(base.Xml.GetAttribute("Frames")));
				break;
			case ObjectMetadata.PatrolPathPointType.GlanceAtFlagFor:
				base.Writer.Write(XmlConvert.ToInt16(base.Xml.GetAttribute("FlagId")));
				base.Writer.Write(XmlConvert.ToInt32(base.Xml.GetAttribute("Frames")));
				break;
			case ObjectMetadata.PatrolPathPointType.Scan:
				base.Writer.Write(XmlConvert.ToInt16(base.Xml.GetAttribute("Frames")));
				base.Writer.Write(XmlConvert.ToSingle(base.Xml.GetAttribute("Rotation")));
				break;
			case ObjectMetadata.PatrolPathPointType.MoveThroughFlag:
			case ObjectMetadata.PatrolPathPointType.MoveNearFlag:
				base.Writer.Write(XmlConvert.ToInt16(base.Xml.GetAttribute("FlagId")));
				base.Writer.Write(XmlConvert.ToSingle(base.Xml.GetAttribute("Distance")));
				break;
			case ObjectMetadata.PatrolPathPointType.MoveToFlagLookAndWait:
				base.Writer.Write(XmlConvert.ToInt16(base.Xml.GetAttribute("Frames")));
				base.Writer.Write(XmlConvert.ToInt16(base.Xml.GetAttribute("FlagId")));
				base.Writer.Write(XmlConvert.ToSingle(base.Xml.GetAttribute("Rotation")));
				break;
			case ObjectMetadata.PatrolPathPointType.FaceToFlagAndFire:
				base.Writer.Write(XmlConvert.ToInt16(base.Xml.GetAttribute("FlagId")));
				base.Writer.Write(XmlConvert.ToInt16(base.Xml.GetAttribute("Frames")));
				base.Writer.Write(XmlConvert.ToSingle(base.Xml.GetAttribute("Spread")));
				break;
			case ObjectMetadata.PatrolPathPointType.LookAtPoint:
			case ObjectMetadata.PatrolPathPointType.MoveToPoint:
				base.Writer.Write(XmlConvert.ToSingle(base.Xml.GetAttribute("X")));
				base.Writer.Write(XmlConvert.ToSingle(base.Xml.GetAttribute("Y")));
				base.Writer.Write(XmlConvert.ToSingle(base.Xml.GetAttribute("Z")));
				break;
			case ObjectMetadata.PatrolPathPointType.MoveThroughPoint:
				base.Writer.Write(XmlConvert.ToSingle(base.Xml.GetAttribute("X")));
				base.Writer.Write(XmlConvert.ToSingle(base.Xml.GetAttribute("Y")));
				base.Writer.Write(XmlConvert.ToSingle(base.Xml.GetAttribute("Z")));
				base.Writer.Write(XmlConvert.ToSingle(base.Xml.GetAttribute("Distance")));
				break;
			default:
				throw new NotSupportedException(string.Format("Unsupported path point type {0}", patrolPathPointType));
			case ObjectMetadata.PatrolPathPointType.Stop:
			case ObjectMetadata.PatrolPathPointType.StopLooking:
			case ObjectMetadata.PatrolPathPointType.FreeFacing:
			case ObjectMetadata.PatrolPathPointType.StopScanning:
				break;
			}
			base.Xml.Skip();
			return false;
		}

		private void ReadPowerUp()
		{
			ObjectMetadata.PowerUp.Accept(this);
		}

		private void ReadSound()
		{
			base.Xml.ReadStartElement();
			base.Xml.MoveToContent();
			ReadStruct(ObjectMetadata.Sound);
			ObjectMetadata.SoundVolumeType soundVolumeType = MetaEnum.Parse<ObjectMetadata.SoundVolumeType>(base.Xml.LocalName);
			base.Writer.Write((int)soundVolumeType);
			switch (soundVolumeType)
			{
			case ObjectMetadata.SoundVolumeType.Box:
				MetaType.BoundingBox.Accept(this);
				break;
			case ObjectMetadata.SoundVolumeType.Sphere:
				ObjectMetadata.SoundSphere.Accept(this);
				break;
			}
			ReadStruct(ObjectMetadata.SoundParams);
			if (soundVolumeType == ObjectMetadata.SoundVolumeType.Sphere)
			{
				base.Writer.Skip(16);
			}
			base.Xml.ReadEndElement();
		}

		private void ReadTriggerVolume()
		{
			ObjectMetadata.TriggerVolume.Accept(this);
		}

		private void ReadTrigger()
		{
			base.Xml.ReadStartElement();
			base.Xml.MoveToContent();
			ReadStruct(ObjectMetadata.Trigger);
			ReadEventList();
			base.Xml.ReadEndElement();
		}

		private void ReadTurret()
		{
			ObjectMetadata.Turret.Accept(this);
		}

		private void ReadWeapon()
		{
			ObjectMetadata.Weapon.Accept(this);
		}

		private void ReadEventList()
		{
			int position = base.Writer.Position;
			base.Writer.WriteUInt16(0);
			if (base.Xml.IsStartElement("Events") && base.Xml.IsEmptyElement)
			{
				base.Xml.ReadStartElement();
				return;
			}
			base.Xml.ReadStartElement("Events");
			base.Xml.MoveToContent();
			short num = 0;
			while (base.Xml.IsStartElement())
			{
				ObjectMetadata.EventType eventType = MetaEnum.Parse<ObjectMetadata.EventType>(base.Xml.Name);
				base.Writer.Write((short)eventType);
				switch (eventType)
				{
				case ObjectMetadata.EventType.Script:
					base.Writer.Write(base.Xml.GetAttribute("Function"), 32);
					break;
				default:
					base.Writer.Write(XmlConvert.ToInt16(base.Xml.GetAttribute("TargetId")));
					break;
				case ObjectMetadata.EventType.None:
					break;
				}
				num++;
				base.Xml.Skip();
			}
			base.Writer.WriteAt(position, num);
			base.Xml.ReadEndElement();
		}

		private void InitTypeReaders(Dictionary<ObjectMetadata.TypeTag, Action> typeReaders)
		{
			typeReaders.Add(ObjectMetadata.TypeTag.CHAR, ReadCharacter);
			typeReaders.Add(ObjectMetadata.TypeTag.CMBT, ReadCombatProfile);
			typeReaders.Add(ObjectMetadata.TypeTag.CONS, ReadConsole);
			typeReaders.Add(ObjectMetadata.TypeTag.DOOR, ReadDoor);
			typeReaders.Add(ObjectMetadata.TypeTag.FLAG, ReadFlag);
			typeReaders.Add(ObjectMetadata.TypeTag.FURN, ReadFurniture);
			typeReaders.Add(ObjectMetadata.TypeTag.MELE, ReadMeleeProfile);
			typeReaders.Add(ObjectMetadata.TypeTag.NEUT, ReadNeutralBehavior);
			typeReaders.Add(ObjectMetadata.TypeTag.PART, ReadParticle);
			typeReaders.Add(ObjectMetadata.TypeTag.PATR, ReadPatrolPath);
			typeReaders.Add(ObjectMetadata.TypeTag.PWRU, ReadPowerUp);
			typeReaders.Add(ObjectMetadata.TypeTag.SNDG, ReadSound);
			typeReaders.Add(ObjectMetadata.TypeTag.TRGV, ReadTriggerVolume);
			typeReaders.Add(ObjectMetadata.TypeTag.TRIG, ReadTrigger);
			typeReaders.Add(ObjectMetadata.TypeTag.TURR, ReadTurret);
			typeReaders.Add(ObjectMetadata.TypeTag.WEAP, ReadWeapon);
		}
	}
}
