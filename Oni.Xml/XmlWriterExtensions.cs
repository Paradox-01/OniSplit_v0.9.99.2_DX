using System.Text;
using System.Xml;

namespace Oni.Xml
{
	internal static class XmlWriterExtensions
	{
		public static void WriteFloatArray(this XmlWriter writer, float[] array)
		{
			writer.WriteArray(array, XmlConvert.ToString);
		}

		public static void WriteArray<T>(this XmlWriter writer, T[] array, Func<T, string> converter)
		{
			StringBuilder stringBuilder = new StringBuilder(array.Length * 8);
			for (int i = 0; i < array.Length; i++)
			{
				if (i != array.Length - 1)
				{
					stringBuilder.AppendFormat("{0} ", converter(array[i]));
				}
				else
				{
					stringBuilder.Append(converter(array[i]));
				}
			}
			writer.WriteValue(stringBuilder.ToString());
		}
	}
}
