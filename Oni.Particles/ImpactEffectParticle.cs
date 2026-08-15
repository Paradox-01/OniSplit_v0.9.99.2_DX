using System.Xml;

namespace Oni.Particles
{
	internal class ImpactEffectParticle
	{
		private string particleClassName;

		private int orientation;

		private int location;

		private float offset;

		private bool decal1;

		private bool decal2;

		public string ParticleClassName
		{
			get
			{
				return particleClassName;
			}
		}

		public int Orientation
		{
			get
			{
				return orientation;
			}
		}

		public int Location
		{
			get
			{
				return location;
			}
		}

		public float Offset
		{
			get
			{
				return offset;
			}
		}

		public bool Decal1
		{
			get
			{
				return decal1;
			}
		}

		public bool Decal2
		{
			get
			{
				return decal2;
			}
		}

		public ImpactEffectParticle(BinaryReader reader)
		{
			particleClassName = reader.ReadString(64);
			reader.Skip(4);
			orientation = reader.ReadInt32();
			location = reader.ReadInt32();
			switch (location)
			{
			case 1:
				offset = reader.ReadSingle();
				reader.Skip(4);
				break;
			case 4:
				decal1 = reader.ReadByte() != 0;
				decal2 = reader.ReadByte() != 0;
				reader.Skip(6);
				break;
			default:
				reader.Skip(8);
				break;
			}
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write(particleClassName, 64);
			writer.Skip(4);
			writer.Write(orientation);
			writer.Write(location);
			switch (location)
			{
			case 1:
				writer.Write(offset);
				break;
			case 4:
				writer.WriteByte(decal1 ? 1 : 0);
				writer.WriteByte(decal2 ? 1 : 0);
				writer.WriteUInt16(0);
				break;
			default:
				writer.Write(0);
				break;
			}
			writer.Write(0);
		}

		public ImpactEffectParticle(XmlReader xml)
		{
			particleClassName = xml.ReadElementContentAsString("Name", "");
			orientation = XmlConvert.ToInt32(xml.ReadElementContentAsString("Orientation", ""));
			location = XmlConvert.ToInt32(xml.ReadElementContentAsString("Location", ""));
			switch (location)
			{
			case 1:
				offset = XmlConvert.ToSingle(xml.ReadElementContentAsString("Offset", ""));
				break;
			case 4:
				decal1 = bool.Parse(xml.ReadElementContentAsString("Decal1", ""));
				decal2 = bool.Parse(xml.ReadElementContentAsString("Decal2", ""));
				break;
			}
		}

		public void Write(XmlWriter writer)
		{
			writer.WriteElementString("Name", particleClassName);
			writer.WriteElementString("Orientation", XmlConvert.ToString(orientation));
			writer.WriteElementString("Location", XmlConvert.ToString(location));
			switch (location)
			{
			case 1:
				writer.WriteElementString("Offset", XmlConvert.ToString(offset));
				break;
			case 4:
				writer.WriteElementString("Decal1", XmlConvert.ToString(decal1));
				writer.WriteElementString("Decal2", XmlConvert.ToString(decal2));
				break;
			}
		}
	}
}
