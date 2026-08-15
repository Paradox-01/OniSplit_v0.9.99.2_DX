using System.Xml;
using Oni.Metadata;
using Oni.Xml;

namespace Oni.Objects
{
	internal abstract class ObjectBase
	{
		private ObjectType typeId;

		private int objectId;

		private ObjectFlags objectFlags;

		private Vector3 position;

		private Vector3 rotation;

		private Matrix? transform;

		public ObjectType TypeId
		{
			get
			{
				return typeId;
			}
			protected set
			{
				typeId = value;
			}
		}

		public int ObjectId
		{
			get
			{
				return objectId;
			}
			set
			{
				objectId = value;
			}
		}

		public Matrix Transform
		{
			get
			{
				if (!transform.HasValue)
				{
					transform = Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X)) * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y)) * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z)) * Matrix.CreateTranslation(position);
				}
				return transform.Value;
			}
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write(objectId);
			writer.Write((int)objectFlags);
			writer.Write(position);
			writer.Write(rotation);
			WriteOsd(writer);
		}

		protected abstract void WriteOsd(BinaryWriter writer);

		public void Read(BinaryReader reader)
		{
			objectId = reader.ReadInt32();
			objectFlags = (ObjectFlags)reader.ReadInt32();
			position = reader.ReadVector3();
			rotation = reader.ReadVector3();
			ReadOsd(reader);
		}

		protected abstract void ReadOsd(BinaryReader reader);

		public void Write(XmlWriter xml)
		{
			xml.WriteStartElement("Header");
			xml.WriteEndElement();
			xml.WriteStartElement("OSD");
			xml.WriteEndElement();
			xml.WriteEndElement();
		}

		protected abstract void WriteOsd(XmlWriter xml);

		public void Read(XmlReader xml, ObjectLoadContext context)
		{
			xml.ReadStartElement();
			xml.ReadStartElement("Header");
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "Flags":
					objectFlags = MetaEnum.Parse<ObjectFlags>(xml.ReadElementContentAsString("Flags", ""));
					break;
				case "Position":
					position = xml.ReadElementContentAsVector3("Position");
					break;
				case "Rotation":
					rotation = xml.ReadElementContentAsVector3("Rotation");
					break;
				default:
					xml.Skip();
					break;
				}
			}
			xml.ReadEndElement();
			xml.ReadStartElement("OSD");
			ReadOsd(xml, context);
			xml.ReadEndElement();
			xml.ReadEndElement();
		}

		protected abstract void ReadOsd(XmlReader xml, ObjectLoadContext context);
	}
}
