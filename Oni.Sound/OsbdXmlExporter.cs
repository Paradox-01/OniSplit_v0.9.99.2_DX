using System;
using System.Xml;
using Oni.Metadata;
using Oni.Xml;

namespace Oni.Sound
{
	internal sealed class OsbdXmlExporter : RawXmlExporter
	{
		private OsbdXmlExporter(BinaryReader reader, XmlWriter xml)
			: base(reader, xml)
		{
		}

		public static void Export(BinaryReader reader, XmlWriter xml)
		{
			OsbdXmlExporter osbdXmlExporter = new OsbdXmlExporter(reader, xml);
			osbdXmlExporter.Export();
		}

		private void Export()
		{
			int num = base.Reader.ReadInt32();
			int num2 = base.Reader.ReadInt32();
			int num3 = base.Reader.ReadInt32();
			if (num3 > 6)
			{
				throw new NotSupportedException("Sound version {0} is not supported");
			}
			switch (num)
			{
			case 1330856301:
				base.Xml.WriteStartElement("AmbientSound");
				ExportAmbient(num3);
				break;
			case 1330857842:
				base.Xml.WriteStartElement("SoundGroup");
				ExportGroup(num3);
				break;
			case 1330858349:
				base.Xml.WriteStartElement("ImpulseSound");
				ExportImpulse(num3);
				break;
			default:
				throw new NotSupportedException(string.Format("Unknown sound binary data tag {0:X}", num));
			}
			base.Xml.WriteEndElement();
		}

		private void ExportGroup(int version)
		{
			if (version < 6)
			{
				float num = 1f;
				float num2 = 1f;
				SoundMetadata.OSGrFlags oSGrFlags = SoundMetadata.OSGrFlags.None;
				if (version >= 2)
				{
					num = base.Reader.ReadSingle();
				}
				if (version >= 3)
				{
					num2 = base.Reader.ReadSingle();
				}
				int num3 = base.Reader.ReadInt32();
				int num4 = base.Reader.ReadInt32();
				if (num4 >= 4)
				{
					oSGrFlags |= SoundMetadata.OSGrFlags.PreventRepeat;
				}
				base.Xml.WriteElementString("Volume", XmlConvert.ToString(num));
				base.Xml.WriteElementString("Pitch", XmlConvert.ToString(num2));
				base.Xml.WriteStartElement("Flags");
				base.Xml.WriteString(MetaEnum.ToString(oSGrFlags));
				base.Xml.WriteEndElement();
				base.Xml.WriteElementString("NumberOfChannels", XmlConvert.ToString(num3));
				base.Xml.WriteStartElement("Permutations");
				for (int i = 0; i < num4; i++)
				{
					base.Xml.WriteStartElement("Permutation");
					SoundMetadata.osgrPermutation.Accept(this);
					base.Xml.WriteEndElement();
				}
				base.Xml.WriteEndElement();
			}
			else
			{
				SoundMetadata.osgr6.Accept(this);
			}
		}

		private void ExportAmbient(int version)
		{
			if (version <= 4)
			{
				SoundMetadata.osam4.Accept(this);
				base.Xml.WriteElementString("Treshold", "3");
				base.Xml.WriteElementString("MinOcclusion", "0");
			}
			else if (version <= 5)
			{
				SoundMetadata.osam5.Accept(this);
				base.Xml.WriteElementString("MinOcclusion", "0");
			}
			else
			{
				SoundMetadata.osam6.Accept(this);
			}
		}

		private void ExportImpulse(int version)
		{
			if (version <= 3)
			{
				SoundMetadata.osim3.Accept(this);
				base.Xml.WriteStartElement("AlternateImpulse");
				base.Xml.WriteElementString("Treshold", "0");
				base.Xml.WriteStartElement("Impulse");
				base.Xml.WriteString("");
				base.Xml.WriteEndElement();
				base.Xml.WriteEndElement();
				base.Xml.WriteElementString("ImpactVelocity", "0");
				base.Xml.WriteElementString("MinOcclusion", "0");
			}
			else if (version <= 4)
			{
				SoundMetadata.osim4.Accept(this);
				base.Xml.WriteElementString("ImpactVelocity", "0");
				base.Xml.WriteElementString("MinOcclusion", "0");
			}
			else if (version <= 5)
			{
				SoundMetadata.osim5.Accept(this);
				base.Xml.WriteElementString("MinOcclusion", "0");
			}
			else
			{
				SoundMetadata.osim6.Accept(this);
			}
		}
	}
}
