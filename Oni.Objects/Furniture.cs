using System.Xml;

namespace Oni.Objects
{
	internal class Furniture : GunkObject
	{
		public FurnitureClass Class;

		public string ParticleTag;

		public Furniture()
		{
			base.TypeId = ObjectType.Furniture;
		}

		protected override void WriteOsd(BinaryWriter writer)
		{
			writer.Write(base.ClassName, 32);
			writer.Write(ParticleTag, 48);
		}

		protected override void ReadOsd(BinaryReader reader)
		{
			base.ClassName = reader.ReadString(32);
			ParticleTag = reader.ReadString(48);
		}

		protected override void WriteOsd(XmlWriter xml)
		{
			xml.WriteElementString("Class", base.ClassName);
			xml.WriteElementString("Particle", ParticleTag);
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
				case "Particle":
					ParticleTag = xml.ReadElementContentAsString();
					break;
				default:
					xml.Skip();
					break;
				}
			}
			Class = context.GetClass(TemplateTag.OFGA, name, FurnitureClass.Read);
			base.GunkClass = Class;
		}
	}
}
