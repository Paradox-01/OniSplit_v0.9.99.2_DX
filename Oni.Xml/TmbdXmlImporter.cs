using System.Collections.Generic;
using System.Xml;

namespace Oni.Xml
{
	internal class TmbdXmlImporter : RawXmlImporter
	{
		private TmbdXmlImporter(XmlReader reader, BinaryWriter writer)
			: base(reader, writer)
		{
		}

		public static void Import(XmlReader reader, BinaryWriter writer)
		{
			TmbdXmlImporter tmbdXmlImporter = new TmbdXmlImporter(reader, writer);
			tmbdXmlImporter.Import();
		}

		private void Import()
		{
			base.Writer.Write(1);
			int position = base.Writer.Position;
			base.Writer.Write(0);
			Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
			while (base.Xml.IsStartElement("Material"))
			{
				string attribute = base.Xml.GetAttribute("Name");
				base.Xml.ReadStartElement();
				while (base.Xml.IsStartElement("Texture"))
				{
					string item = base.Xml.ReadElementContentAsString();
					List<string> value;
					if (!dictionary.TryGetValue(attribute, out value))
					{
						value = new List<string>();
						dictionary.Add(attribute, value);
					}
					value.Add(item);
				}
				base.Xml.ReadEndElement();
			}
			int num = 0;
			foreach (KeyValuePair<string, List<string>> item2 in dictionary)
			{
				foreach (string item3 in item2.Value)
				{
					base.Writer.Write(item2.Key, 32);
					base.Writer.Write(item3, 32);
					num++;
				}
			}
			base.Writer.WriteAt(position, num);
		}
	}
}
