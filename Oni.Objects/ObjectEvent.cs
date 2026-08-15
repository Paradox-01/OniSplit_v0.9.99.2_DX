using System.Collections.Generic;
using System.Xml;
using Oni.Metadata;

namespace Oni.Objects
{
	internal class ObjectEvent
	{
		private ObjectEventType action;

		private int targetId;

		private string script;

		public ObjectEventType Action
		{
			get
			{
				return action;
			}
		}

		public int TargetId
		{
			get
			{
				return targetId;
			}
		}

		public string Script
		{
			get
			{
				return script;
			}
		}

		public ObjectEvent()
		{
		}

		private ObjectEvent(BinaryReader reader)
		{
			action = (ObjectEventType)reader.ReadInt16();
			if (action == ObjectEventType.Script)
			{
				script = reader.ReadString(32);
			}
			else if (action != ObjectEventType.None)
			{
				targetId = reader.ReadUInt16();
			}
		}

		public static ObjectEvent[] ReadEventList(BinaryReader reader)
		{
			ObjectEvent[] array = new ObjectEvent[reader.ReadUInt16()];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new ObjectEvent(reader);
			}
			return array;
		}

		public static void WriteEventList(BinaryWriter writer, ObjectEvent[] events)
		{
			if (events == null)
			{
				writer.WriteUInt16(0);
				return;
			}
			writer.WriteUInt16(events.Length);
			for (int i = 0; i < events.Length; i++)
			{
				writer.WriteUInt16((ushort)events[i].action);
				if (events[i].action == ObjectEventType.Script)
				{
					writer.Write(events[i].script, 32);
				}
				else if (events[i].action != ObjectEventType.None)
				{
					writer.WriteUInt16(events[i].targetId);
				}
			}
		}

		public static ObjectEvent[] ReadEventList(XmlReader xml)
		{
			List<ObjectEvent> list = new List<ObjectEvent>();
			if (xml.IsStartElement("Events") && xml.IsEmptyElement)
			{
				xml.ReadStartElement();
				return list.ToArray();
			}
			xml.ReadStartElement("Events");
			while (xml.IsStartElement())
			{
				ObjectEvent objectEvent = new ObjectEvent();
				objectEvent.action = MetaEnum.Parse<ObjectEventType>(xml.LocalName);
				switch (objectEvent.action)
				{
				case ObjectEventType.Script:
					objectEvent.script = xml.GetAttribute("Function");
					break;
				default:
					objectEvent.targetId = XmlConvert.ToInt16(xml.GetAttribute("TargetId"));
					break;
				case ObjectEventType.None:
					break;
				}
				list.Add(objectEvent);
				xml.Skip();
			}
			xml.ReadEndElement();
			return list.ToArray();
		}
	}
}
