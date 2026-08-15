using System;
using System.Xml;
using Oni.Metadata;

namespace Oni.Objects
{
	internal class Console : GunkObject
	{
		public int ScriptId;

		public ConsoleFlags Flags;

		public string InactiveTexture;

		public string ActiveTexture;

		public string TriggeredTexture;

		public ObjectEvent[] Events;

		public Console()
		{
			base.TypeId = ObjectType.Console;
		}

		protected override void WriteOsd(BinaryWriter writer)
		{
			writer.Write(base.ClassName, 63);
			writer.WriteUInt16(ScriptId);
			writer.WriteUInt16((int)Flags);
			writer.Write(InactiveTexture, 63);
			writer.Write(ActiveTexture, 63);
			writer.Write(TriggeredTexture, 63);
			ObjectEvent.WriteEventList(writer, Events);
		}

		protected override void ReadOsd(BinaryReader reader)
		{
			base.ClassName = reader.ReadString(63);
			ScriptId = reader.ReadUInt16();
			Flags = (ConsoleFlags)reader.ReadInt16();
			InactiveTexture = reader.ReadString(63);
			ActiveTexture = reader.ReadString(63);
			TriggeredTexture = reader.ReadString(63);
			Events = ObjectEvent.ReadEventList(reader);
		}

		protected override void WriteOsd(XmlWriter xml)
		{
			throw new NotImplementedException();
		}

		protected override void ReadOsd(XmlReader xml, ObjectLoadContext context)
		{
			string name = null;
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "Class":
					name = xml.ReadElementContentAsString();
					break;
				case "ConsoleId":
					ScriptId = xml.ReadElementContentAsInt();
					break;
				case "Flags":
					Flags = xml.ReadElementContentAsEnum<ConsoleFlags>();
					break;
				case "DisabledTexture":
				case "InactiveTexture":
					InactiveTexture = xml.ReadElementContentAsString();
					break;
				case "EnabledTexture":
				case "ActiveTexture":
					ActiveTexture = xml.ReadElementContentAsString();
					break;
				case "UsedTexture":
				case "TrigerredTexture":
					TriggeredTexture = xml.ReadElementContentAsString();
					break;
				case "Events":
					Events = ObjectEvent.ReadEventList(xml);
					break;
				default:
					xml.Skip();
					break;
				}
			}
			base.GunkClass = context.GetClass(TemplateTag.CONS, name, ConsoleClass.Read);
		}
	}
}
