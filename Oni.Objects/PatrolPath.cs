using System;
using System.Collections.Generic;
using System.Xml;
using Oni.Metadata;

namespace Oni.Objects
{
	internal class PatrolPath : ObjectBase
	{
		private string name;

		private PatrolPathPoint[] points;

		private int patrolId;

		private int returnToNearest;

		public PatrolPath()
		{
			base.TypeId = ObjectType.PatrolPath;
		}

		protected override void WriteOsd(BinaryWriter writer)
		{
			writer.Write(name, 32);
			writer.Write(points.Length);
			writer.WriteUInt16(patrolId);
			writer.WriteUInt16(returnToNearest);
			PatrolPathPoint[] array = points;
			foreach (PatrolPathPoint patrolPathPoint in array)
			{
				writer.Write((int)patrolPathPoint.Type);
				switch (patrolPathPoint.Type)
				{
				case PatrolPathPointType.LoopFrom:
					writer.Write((int)patrolPathPoint.Attributes["From"]);
					break;
				case PatrolPathPointType.IgnorePlayer:
					writer.WriteByte((byte)patrolPathPoint.Attributes["Value"]);
					break;
				case PatrolPathPointType.CallScript:
				case PatrolPathPointType.ForkScript:
					writer.Write((short)patrolPathPoint.Attributes["ScriptId"]);
					break;
				case PatrolPathPointType.MoveToFlag:
				case PatrolPathPointType.LookAtFlag:
				case PatrolPathPointType.MoveAndFaceFlag:
					writer.Write((short)patrolPathPoint.Attributes["FlagId"]);
					break;
				case PatrolPathPointType.MovementMode:
					writer.Write((int)(PatrolPathMovementMode)patrolPathPoint.Attributes["Mode"]);
					break;
				case PatrolPathPointType.LockFacing:
					writer.Write((int)(PatrolPathFacing)patrolPathPoint.Attributes["Facing"]);
					break;
				case PatrolPathPointType.Pause:
					writer.Write((int)patrolPathPoint.Attributes["Frames"]);
					break;
				case PatrolPathPointType.GlanceAtFlagFor:
					writer.Write((short)patrolPathPoint.Attributes["FlagId"]);
					writer.Write((int)patrolPathPoint.Attributes["Frames"]);
					break;
				case PatrolPathPointType.Scan:
					writer.Write((short)patrolPathPoint.Attributes["Frames"]);
					writer.Write((float)patrolPathPoint.Attributes["Rotation"]);
					break;
				case PatrolPathPointType.MoveThroughFlag:
				case PatrolPathPointType.MoveNearFlag:
					writer.Write((short)patrolPathPoint.Attributes["FlagId"]);
					writer.Write((float)patrolPathPoint.Attributes["Distance"]);
					break;
				case PatrolPathPointType.MoveToFlagLookAndWait:
					writer.Write((short)patrolPathPoint.Attributes["Frames"]);
					writer.Write((short)patrolPathPoint.Attributes["FlagId"]);
					writer.Write((float)patrolPathPoint.Attributes["Rotation"]);
					break;
				case PatrolPathPointType.FaceToFlagAndFire:
					writer.Write((short)patrolPathPoint.Attributes["FlagId"]);
					writer.Write((short)patrolPathPoint.Attributes["Frames"]);
					writer.Write((float)patrolPathPoint.Attributes["Spread"]);
					break;
				case PatrolPathPointType.LookAtPoint:
				case PatrolPathPointType.MoveToPoint:
					writer.Write((Vector3)patrolPathPoint.Attributes["Point"]);
					break;
				case PatrolPathPointType.MoveThroughPoint:
					writer.Write((Vector3)patrolPathPoint.Attributes["Point"]);
					writer.Write((float)patrolPathPoint.Attributes["Distance"]);
					break;
				default:
					throw new NotSupportedException(string.Format("Unsupported path point type {0}", patrolPathPoint.Type));
				case PatrolPathPointType.Stop:
				case PatrolPathPointType.Loop:
				case PatrolPathPointType.StopLooking:
				case PatrolPathPointType.FreeFacing:
				case PatrolPathPointType.StopScanning:
					break;
				}
			}
		}

		protected override void ReadOsd(BinaryReader reader)
		{
			throw new NotImplementedException();
		}

		protected override void WriteOsd(XmlWriter xml)
		{
			throw new NotImplementedException();
		}

		protected override void ReadOsd(XmlReader xml, ObjectLoadContext context)
		{
			name = xml.ReadElementContentAsString("Name", "");
			patrolId = xml.ReadElementContentAsInt("PatrolId", "");
			returnToNearest = xml.ReadElementContentAsInt("ReturnToNearest", "");
			bool isEmptyElement = xml.IsEmptyElement;
			xml.ReadStartElement("Points");
			if (isEmptyElement)
			{
				points = new PatrolPathPoint[0];
				return;
			}
			List<PatrolPathPoint> list = new List<PatrolPathPoint>();
			int num = -1;
			bool flag = false;
			int num2 = 0;
			while (xml.IsStartElement() | flag)
			{
				if (!xml.IsStartElement())
				{
					xml.ReadEndElement();
					flag = false;
				}
				else if (xml.LocalName == "Loop")
				{
					if (!xml.SkipEmpty())
					{
						flag = true;
						num = num2;
						xml.ReadStartElement();
					}
				}
				else
				{
					PatrolPathPoint patrolPathPoint = new PatrolPathPoint(MetaEnum.Parse<PatrolPathPointType>(xml.LocalName));
					switch (patrolPathPoint.Type)
					{
					case PatrolPathPointType.IgnorePlayer:
						patrolPathPoint.Attributes["Value"] = ((xml.GetAttribute("Value") == "Yes") ? ((byte)1) : ((byte)0));
						break;
					case PatrolPathPointType.CallScript:
					case PatrolPathPointType.ForkScript:
						patrolPathPoint.Attributes["ScriptId"] = XmlConvert.ToInt16(xml.GetAttribute("ScriptId"));
						break;
					case PatrolPathPointType.MoveToFlag:
					case PatrolPathPointType.LookAtFlag:
					case PatrolPathPointType.MoveAndFaceFlag:
						patrolPathPoint.Attributes["FlagId"] = XmlConvert.ToInt16(xml.GetAttribute("FlagId"));
						break;
					case PatrolPathPointType.MovementMode:
						patrolPathPoint.Attributes["Mode"] = Convert.ToInt32(MetaEnum.Parse<PatrolPathMovementMode>(xml.GetAttribute("Mode")));
						break;
					case PatrolPathPointType.LockFacing:
						patrolPathPoint.Attributes["Facing"] = Convert.ToInt32(MetaEnum.Parse<PatrolPathFacing>(xml.GetAttribute("Facing")));
						break;
					case PatrolPathPointType.Pause:
						patrolPathPoint.Attributes["Frames"] = XmlConvert.ToInt32(xml.GetAttribute("Frames"));
						break;
					case PatrolPathPointType.GlanceAtFlagFor:
						patrolPathPoint.Attributes["FlagId"] = XmlConvert.ToInt16(xml.GetAttribute("FlagId"));
						patrolPathPoint.Attributes["Frames"] = XmlConvert.ToInt32(xml.GetAttribute("Frames"));
						break;
					case PatrolPathPointType.Scan:
						patrolPathPoint.Attributes["Frames"] = XmlConvert.ToInt16(xml.GetAttribute("Frames"));
						patrolPathPoint.Attributes["Rotation"] = XmlConvert.ToSingle(xml.GetAttribute("Rotation"));
						break;
					case PatrolPathPointType.MoveThroughFlag:
					case PatrolPathPointType.MoveNearFlag:
						patrolPathPoint.Attributes["FlagId"] = XmlConvert.ToInt16(xml.GetAttribute("FlagId"));
						patrolPathPoint.Attributes["Distance"] = XmlConvert.ToSingle(xml.GetAttribute("Distance"));
						break;
					case PatrolPathPointType.MoveToFlagLookAndWait:
						patrolPathPoint.Attributes["Frames"] = XmlConvert.ToInt16(xml.GetAttribute("Frames"));
						patrolPathPoint.Attributes["FlagId"] = XmlConvert.ToInt16(xml.GetAttribute("FlagId"));
						patrolPathPoint.Attributes["Rotation"] = XmlConvert.ToSingle(xml.GetAttribute("Rotation"));
						break;
					case PatrolPathPointType.FaceToFlagAndFire:
						patrolPathPoint.Attributes["FlagId"] = XmlConvert.ToInt16(xml.GetAttribute("FlagId"));
						patrolPathPoint.Attributes["Frames"] = XmlConvert.ToInt16(xml.GetAttribute("Frames"));
						patrolPathPoint.Attributes["Spread"] = XmlConvert.ToSingle(xml.GetAttribute("Spread"));
						break;
					case PatrolPathPointType.LookAtPoint:
					case PatrolPathPointType.MoveToPoint:
						patrolPathPoint.Attributes["Point"] = new Vector3(XmlConvert.ToSingle(xml.GetAttribute("X")), XmlConvert.ToSingle(xml.GetAttribute("Y")), XmlConvert.ToSingle(xml.GetAttribute("Z")));
						break;
					case PatrolPathPointType.MoveThroughPoint:
						patrolPathPoint.Attributes["Point"] = new Vector3(XmlConvert.ToSingle(xml.GetAttribute("X")), XmlConvert.ToSingle(xml.GetAttribute("Y")), XmlConvert.ToSingle(xml.GetAttribute("Z")));
						patrolPathPoint.Attributes["Distance"] = XmlConvert.ToSingle(xml.GetAttribute("Distance"));
						break;
					default:
						throw new NotSupportedException(string.Format("Unsupported path point type {0}", patrolPathPoint.Type));
					case PatrolPathPointType.Stop:
					case PatrolPathPointType.StopLooking:
					case PatrolPathPointType.FreeFacing:
					case PatrolPathPointType.StopScanning:
						break;
					}
					xml.Skip();
					list.Add(patrolPathPoint);
				}
				num2++;
			}
			xml.ReadEndElement();
			if (num == 0)
			{
				list.Add(new PatrolPathPoint(PatrolPathPointType.Loop));
			}
			else if (num > 0)
			{
				PatrolPathPoint patrolPathPoint2 = new PatrolPathPoint(PatrolPathPointType.LoopFrom);
				patrolPathPoint2.Attributes["From"] = num;
				list.Add(patrolPathPoint2);
			}
			points = list.ToArray();
		}
	}
}
