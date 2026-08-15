using System.Xml;
using Oni.Metadata;
using Oni.Xml;

namespace Oni.Sound
{
	internal sealed class OsbdXmlImporter : RawXmlImporter
	{
		public OsbdXmlImporter(XmlReader reader, BinaryWriter writer)
			: base(reader, writer)
		{
		}

		public void Import()
		{
			switch (base.Xml.LocalName)
			{
			case "AmbientSound":
				Import(1330856301, SoundMetadata.osam6);
				break;
			case "ImpulseSound":
				Import(1330858349, SoundMetadata.osim6);
				break;
			case "SoundGroup":
				Import(1330857842, SoundMetadata.osgr6);
				break;
			}
		}

		private void Import(int tag, MetaStruct type)
		{
			base.Xml.ReadStartElement();
			BeginStruct(0);
			base.Writer.Write(tag);
			base.Writer.Write(0);
			base.Writer.Write(6);
			ReadStruct(type);
		}
	}
}
