using System;
using System.Collections.Generic;
using System.Xml;

namespace Oni.Xml
{
	internal static class XmlReaderExtensions
	{
		private static readonly char[] emptyChars = new char[0];

		[ThreadStatic]
		private static char[] charBuffer;

		private static char[] CharBuffer
		{
			get
			{
				if (charBuffer == null)
				{
					charBuffer = new char[16384];
				}
				return charBuffer;
			}
		}

		public static IEnumerable<string> ReadElementContentAsList(this XmlReader xml)
		{
			if (xml.SkipEmpty())
			{
				yield break;
			}
			xml.ReadStartElement();
			if (xml.NodeType == XmlNodeType.EndElement)
			{
				xml.ReadEndElement();
				yield break;
			}
			string buffer = xml.ReadContentAsString();
			for (int i = 0; i < buffer.Length; i++)
			{
				if (!char.IsWhiteSpace(buffer[i]))
				{
					int num = i;
					do
					{
						i++;
					}
					while (i < buffer.Length && !char.IsWhiteSpace(buffer[i]));
					yield return buffer.Substring(num, i - num);
				}
			}
			xml.ReadEndElement();
		}

		private static void ReadArrayCore<T>(XmlReader xml, Func<string, T> parser, List<T> list)
		{
			foreach (string item in xml.ReadElementContentAsList())
			{
				list.Add(parser(item));
			}
		}

		public static T[] ReadElementContentAsArray<T>(this XmlReader xml, Func<string, T> parser)
		{
			List<T> list = new List<T>();
			ReadArrayCore(xml, parser, list);
			return list.ToArray();
		}

		public static T[] ReadElementContentAsArray<T>(this XmlReader xml, Func<string, T> converter, int count)
		{
			List<T> list = new List<T>(count);
			ReadArrayCore(xml, converter, list);
			T[] array = new T[count];
			list.CopyTo(0, array, 0, count);
			return array;
		}

		public static void ReadElementContentAsArray<T>(this XmlReader xml, Func<string, T> parser, T[] array)
		{
			string text = xml.ReadElementContentAsString();
			string[] array2 = text.Split(emptyChars, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				if (i < array2.Length)
				{
					array[i] = parser(array2[i]);
				}
				else
				{
					array[i] = default(T);
				}
			}
		}

		public static T[] ReadElementContentAsArray<T>(this XmlReader xml, Func<string, T> parser, string name)
		{
			string text = ((name != null) ? xml.ReadElementContentAsString(name, string.Empty) : xml.ReadElementContentAsString());
			string[] input = text.Split(emptyChars, StringSplitOptions.RemoveEmptyEntries);
			return input.ConvertAll(parser);
		}

		private static T[] ReadArray<T>(this XmlReader xml, Func<string, T> parser, int count)
		{
			string text = xml.ReadString();
			string[] input = text.Split(emptyChars, StringSplitOptions.RemoveEmptyEntries);
			T[] array = input.ConvertAll(parser);
			Array.Resize(ref array, count);
			return array;
		}

		public static T[] ReadElementContentAsArray<T>(this XmlReader xml, Func<string, T> converter, int count, string name)
		{
			T[] array = xml.ReadElementContentAsArray(converter, name);
			Array.Resize(ref array, count);
			return array;
		}

		public static Vector2 ReadElementContentAsVector2(this XmlReader xml, string name = null)
		{
			float[] array = xml.ReadElementContentAsArray(XmlConvert.ToSingle, 2, name);
			return new Vector2(array[0], array[1]);
		}

		public static Vector3 ReadElementContentAsVector3(this XmlReader xml, string name = null)
		{
			float[] array = xml.ReadElementContentAsArray(XmlConvert.ToSingle, 3);
			return new Vector3(array[0], array[1], array[2]);
		}

		public static Vector4 ReadElementContentAsVector4(this XmlReader xml)
		{
			float[] array = xml.ReadElementContentAsArray(XmlConvert.ToSingle, 4);
			return new Vector4(array[0], array[1], array[2], array[3]);
		}

		public static Quaternion ReadElementContentAsEulerXYZ(this XmlReader xml)
		{
			float[] array = xml.ReadElementContentAsArray(XmlConvert.ToSingle, 3);
			return Quaternion.CreateFromEulerXYZ(array[0], array[1], array[2]);
		}

		public static Quaternion ReadElementContentAsQuaternion(this XmlReader xml, string name = null)
		{
			Quaternion result = Quaternion.Identity;
			if (xml.IsEmptyElement)
			{
				xml.Skip();
			}
			else
			{
				if (name == null)
				{
					xml.ReadStartElement();
				}
				else
				{
					xml.ReadStartElement(name);
				}
				if (xml.NodeType == XmlNodeType.Text)
				{
					float[] array = xml.ReadArray(XmlConvert.ToSingle, 4);
					result = new Quaternion(array[0], array[1], array[2], 0f - array[3]);
				}
				else
				{
					List<Quaternion> list = new List<Quaternion>();
					while (xml.IsStartElement())
					{
						switch (xml.LocalName)
						{
						case "rotate":
						{
							Vector4 vector = xml.ReadElementContentAsVector4();
							list.Add(Quaternion.CreateFromAxisAngle(vector.XYZ, MathHelper.ToRadians(vector.W)));
							break;
						}
						case "euler":
							list.Add(xml.ReadElementContentAsEulerXYZ());
							break;
						default:
							throw new XmlException(string.Format("Unknown element {0}", xml.LocalName));
						}
					}
					foreach (Quaternion item in Utils.Reverse(list))
					{
						result *= item;
					}
				}
				xml.ReadEndElement();
			}
			return result;
		}

		public static Matrix ReadElementContentAsMatrix43(this XmlReader xml, string name = null)
		{
			Matrix result = Matrix.Identity;
			if (xml.IsEmptyElement)
			{
				xml.Skip();
			}
			else
			{
				if (name == null)
				{
					xml.ReadStartElement();
				}
				else
				{
					xml.ReadStartElement(name);
				}
				if (xml.NodeType == XmlNodeType.Text)
				{
					float[] array = xml.ReadArray(XmlConvert.ToSingle, 12);
					result = new Matrix(array[0], array[1], array[2], 0f, array[3], array[4], array[5], 0f, array[6], array[7], array[8], 0f, array[9], array[10], array[11], 1f);
				}
				else
				{
					List<Matrix> list = new List<Matrix>();
					while (xml.IsStartElement())
					{
						switch (xml.LocalName)
						{
						case "translate":
							list.Add(Matrix.CreateTranslation(xml.ReadElementContentAsVector3()));
							break;
						case "rotate":
						{
							Vector4 vector = xml.ReadElementContentAsVector4();
							list.Add(Matrix.CreateFromAxisAngle(vector.XYZ, MathHelper.ToRadians(vector.W)));
							break;
						}
						case "euler":
							list.Add(xml.ReadElementContentAsEulerXYZ().ToMatrix());
							break;
						case "scale":
							list.Add(Matrix.CreateScale(xml.ReadElementContentAsVector3()));
							break;
						default:
							throw new XmlException(string.Format("Unknown element {0}", xml.LocalName));
						}
					}
					foreach (Matrix item in Utils.Reverse(list))
					{
						result *= item;
					}
				}
				xml.ReadEndElement();
			}
			return result;
		}
	}
}
