using System.Xml;
using Oni.Metadata;

namespace Oni.Objects
{
	internal class PowerUp : ObjectBase
	{
		public PowerUpClass Type;

		public PowerUp()
		{
			base.TypeId = ObjectType.PowerUp;
		}

		protected override void WriteOsd(BinaryWriter writer)
		{
			writer.Write((uint)Type);
		}

		protected override void ReadOsd(BinaryReader reader)
		{
			Type = (PowerUpClass)reader.ReadUInt32();
		}

		protected override void WriteOsd(XmlWriter xml)
		{
			xml.WriteElementString("Class", MetaEnum.ToString(Type));
		}

		protected override void ReadOsd(XmlReader xml, ObjectLoadContext context)
		{
			Type = xml.ReadElementContentAsEnum<PowerUpClass>("Class");
		}
	}
}
