using System.Xml;
using Oni.Metadata;
using Oni.Xml;

namespace Oni.Objects
{
	internal class Particle : ObjectBase
	{
		public string ClassName;

		public string Tag;

		public ParticleFlags Flags;

		public Vector2 DecalScale = new Vector2(1f, 1f);

		public Particle()
		{
			base.TypeId = ObjectType.Particle;
		}

		protected override void WriteOsd(BinaryWriter writer)
		{
			writer.Write(ClassName, 64);
			writer.Write(Tag, 48);
			writer.Write((ushort)Flags);
			writer.Write(DecalScale);
		}

		protected override void ReadOsd(BinaryReader reader)
		{
			ClassName = reader.ReadString(64);
			Tag = reader.ReadString(48);
			Flags = (ParticleFlags)reader.ReadUInt16();
			DecalScale = reader.ReadVector2();
		}

		protected override void WriteOsd(XmlWriter xml)
		{
			xml.WriteElementString("Class", ClassName);
			xml.WriteElementString("Tag", Tag);
			xml.WriteElementString("Flags", MetaEnum.ToString(Flags));
			xml.WriteElementString("DecalScale", XmlConvert.ToString(DecalScale.X) + " " + XmlConvert.ToString(DecalScale.Y));
		}

		protected override void ReadOsd(XmlReader xml, ObjectLoadContext context)
		{
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "Class":
					ClassName = xml.ReadElementContentAsString();
					break;
				case "Tag":
					Tag = xml.ReadElementContentAsString();
					break;
				case "Flags":
					Flags = xml.ReadElementContentAsEnum<ParticleFlags>() & ParticleFlags.NotInitiallyCreated;
					break;
				case "DecalScale":
					DecalScale = xml.ReadElementContentAsVector2();
					break;
				default:
					xml.Skip();
					break;
				}
			}
		}
	}
}
