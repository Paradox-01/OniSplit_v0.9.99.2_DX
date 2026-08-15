using System.Xml;

namespace Oni.Metadata
{
	internal static class XmlReaderExtensions
	{
		public static T ReadElementContentAsEnum<T>(this XmlReader xml) where T : struct
		{
			return MetaEnum.Parse<T>(xml.ReadElementContentAsString());
		}

		public static T ReadElementContentAsEnum<T>(this XmlReader xml, string name) where T : struct
		{
			return MetaEnum.Parse<T>(xml.ReadElementContentAsString(name, ""));
		}
	}
}
