using System;
using System.Xml;
using Oni.Imaging;
using Oni.Metadata;
using Oni.Xml;

namespace Oni.Objects
{
	internal class Trigger : GunkObject
	{
		public int ScriptId;

		public TriggerFlags Flags;

		public Color LaserColor;

		public float StartPosition;

		public float Speed;

		public int EmitterCount;

		public int TimeOn;

		public int TimeOff;

		public ObjectEvent[] Events;

		public Trigger()
		{
			base.TypeId = ObjectType.Trigger;
		}

		protected override void WriteOsd(BinaryWriter writer)
		{
			writer.Write(base.ClassName, 63);
			writer.WriteUInt16(ScriptId);
			writer.WriteUInt16((int)Flags);
			writer.Write(LaserColor);
			writer.Write(StartPosition);
			writer.Write(Speed);
			writer.WriteUInt16(EmitterCount);
			writer.WriteUInt16(TimeOn);
			writer.WriteUInt16(TimeOff);
			ObjectEvent.WriteEventList(writer, Events);
		}

		protected override void ReadOsd(BinaryReader reader)
		{
			base.ClassName = reader.ReadString(63);
			ScriptId = reader.ReadUInt16();
			Flags = (TriggerFlags)(reader.ReadUInt16() & -133);
			LaserColor = reader.ReadColor();
			StartPosition = reader.ReadSingle();
			Speed = reader.ReadSingle();
			EmitterCount = reader.ReadUInt16();
			TimeOn = reader.ReadUInt16();
			TimeOff = reader.ReadUInt16();
			Events = ObjectEvent.ReadEventList(reader);
		}

		protected override void WriteOsd(XmlWriter xml)
		{
			throw new NotImplementedException();
		}

		protected override void ReadOsd(XmlReader xml, ObjectLoadContext context)
		{
			string name = xml.ReadElementContentAsString("Class", "");
			ScriptId = xml.ReadElementContentAsInt("TriggerId", "");
			Flags = xml.ReadElementContentAsEnum<TriggerFlags>("Flags");
			byte[] array = xml.ReadElementContentAsArray(XmlConvert.ToByte, "LaserColor");
			if (array.Length > 3)
			{
				LaserColor = new Color(array[0], array[1], array[2], array[3]);
			}
			else
			{
				LaserColor = new Color(array[0], array[1], array[2]);
			}
			StartPosition = xml.ReadElementContentAsFloat("StartPosition", "");
			Speed = xml.ReadElementContentAsFloat("Speed", "");
			EmitterCount = xml.ReadElementContentAsInt("EmitterCount", "");
			if (xml.IsStartElement("Offset_0075"))
			{
				TimeOn = xml.ReadElementContentAsInt();
			}
			else
			{
				TimeOn = xml.ReadElementContentAsInt("TimeOn", "");
			}
			if (xml.IsStartElement("Offset_0077"))
			{
				TimeOff = xml.ReadElementContentAsInt();
			}
			else
			{
				TimeOff = xml.ReadElementContentAsInt("TimeOff", "");
			}
			Events = ObjectEvent.ReadEventList(xml);
			base.GunkClass = context.GetClass(TemplateTag.TRIG, name, TriggerClass.Read);
		}
	}
}
