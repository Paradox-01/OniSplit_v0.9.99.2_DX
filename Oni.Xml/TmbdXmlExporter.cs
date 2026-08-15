using System.Collections.Generic;
using System.Xml;

namespace Oni.Xml
{
	internal class TmbdXmlExporter : RawXmlExporter
	{
		private TmbdXmlExporter(BinaryReader reader, XmlWriter writer)
			: base(reader, writer)
		{
		}

		public static void Export(BinaryReader reader, XmlWriter writer)
		{
			TmbdXmlExporter tmbdXmlExporter = new TmbdXmlExporter(reader, writer);
			tmbdXmlExporter.Export();
		}

		private void Export()
		{
			int num = base.Reader.ReadInt32();
			int num2 = base.Reader.ReadInt32();
			int num3 = base.Reader.ReadInt32();
			Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>(num3);
			for (int i = 0; i < num3; i++)
			{
				string key = base.Reader.ReadString(32);
				string item = base.Reader.ReadString(32);
				List<string> value;
				if (!dictionary.TryGetValue(key, out value))
				{
					value = new List<string>();
					dictionary.Add(key, value);
				}
				value.Add(item);
			}
			base.Xml.WriteStartElement("TextureMaterials");
			foreach (KeyValuePair<string, List<string>> item2 in dictionary)
			{
				base.Xml.WriteStartElement("Material");
				base.Xml.WriteAttributeString("Name", item2.Key);
				foreach (string item3 in item2.Value)
				{
					base.Xml.WriteElementString("Texture", item3);
				}
				base.Xml.WriteEndElement();
			}
			base.Xml.WriteEndElement();
		}
	}
}
