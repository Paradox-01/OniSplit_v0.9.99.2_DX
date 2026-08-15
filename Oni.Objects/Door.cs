using System;
using System.Xml;
using Oni.Metadata;
using Oni.Xml;

namespace Oni.Objects
{
	internal class Door : GunkObject
	{
		public DoorClass Class;

		public int ScriptId;

		public int KeyId;

		public DoorFlags Flags;

		public Vector3 Center;

		public float ActivationRadius = 30f;

		public readonly string[] Textures = new string[2];

		public ObjectEvent[] Events;

		public Door()
		{
			base.TypeId = ObjectType.Door;
		}

		protected override void WriteOsd(BinaryWriter writer)
		{
			writer.Write(base.ClassName, 63);
			writer.WriteUInt16(ScriptId);
			writer.WriteUInt16(KeyId);
			writer.WriteUInt16((int)Flags);
			writer.Write(Center);
			writer.Write(ActivationRadius * ActivationRadius);
			writer.Write(Textures[0], 63);
			writer.Write(Textures[1], 63);
			ObjectEvent.WriteEventList(writer, Events);
		}

		protected override void ReadOsd(BinaryReader reader)
		{
			base.ClassName = reader.ReadString(63);
			ScriptId = reader.ReadUInt16();
			KeyId = reader.ReadUInt16();
			Flags = (DoorFlags)reader.ReadInt16();
			Center = reader.ReadVector3();
			ActivationRadius = FMath.Sqrt(reader.ReadSingle());
			Textures[0] = reader.ReadString(63).ToUpperInvariant();
			Textures[1] = reader.ReadString(63).ToUpperInvariant();
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
				case "DoorId":
					ScriptId = xml.ReadElementContentAsInt();
					break;
				case "KeyId":
					KeyId = xml.ReadElementContentAsInt();
					break;
				case "Flags":
					Flags = xml.ReadElementContentAsEnum<DoorFlags>();
					break;
				case "Center":
					Center = xml.ReadElementContentAsVector3();
					break;
				case "SquaredActivationRadius":
					ActivationRadius = FMath.Sqrt(xml.ReadElementContentAsFloat());
					break;
				case "ActivationRadius":
					ActivationRadius = xml.ReadElementContentAsFloat();
					break;
				case "Texture":
				case "Texture1":
					Textures[0] = xml.ReadElementContentAsString().ToUpperInvariant();
					break;
				case "Texture2":
					Textures[1] = xml.ReadElementContentAsString().ToUpperInvariant();
					break;
				case "Events":
					Events = ObjectEvent.ReadEventList(xml);
					break;
				default:
					xml.Skip();
					break;
				}
			}
			Class = context.GetClass(TemplateTag.DOOR, name, DoorClass.Read);
			base.GunkClass = Class;
		}
	}
}
