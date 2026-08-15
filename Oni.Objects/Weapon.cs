using System.Xml;

namespace Oni.Objects
{
	internal class Weapon : ObjectBase
	{
		public string ClassName;

		public Weapon()
		{
			base.TypeId = ObjectType.Weapon;
		}

		protected override void WriteOsd(BinaryWriter writer)
		{
			writer.Write(ClassName, 32);
		}

		protected override void ReadOsd(BinaryReader reader)
		{
			ClassName = reader.ReadString(32);
		}

		protected override void WriteOsd(XmlWriter xml)
		{
			xml.WriteElementString("Class", ClassName);
		}

		protected override void ReadOsd(XmlReader xml, ObjectLoadContext context)
		{
			ClassName = xml.ReadElementContentAsString("Class", "");
		}
	}
}
