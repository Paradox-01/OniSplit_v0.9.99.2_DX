using System.Xml;
using Oni.Metadata;
using Oni.Objects;
using Oni.Xml;

namespace Oni.Physics
{
	internal class ObjectXmlReader
	{
		public static ObjectParticle ReadParticle(XmlReader xml)
		{
			xml.ReadStartElement("Particle");
			ObjectParticle result = new ObjectParticle
			{
				ParticleClass = xml.ReadElementContentAsString("Class", ""),
				Tag = xml.ReadElementContentAsString("Tag", ""),
				Matrix = xml.ReadElementContentAsMatrix43("Transform"),
				DecalScale = xml.ReadElementContentAsVector2("DecalScale"),
				Flags = (xml.ReadElementContentAsEnum<ParticleFlags>("Flags") & ParticleFlags.NotInitiallyCreated)
			};
			xml.ReadEndElement();
			return result;
		}
	}
}
