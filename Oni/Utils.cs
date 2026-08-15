using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Oni
{
	internal static class Utils
	{
		private static string version;

		private static readonly char[] wildcards = new char[3] { '*', '?', '.' };

		private static byte[] buffer1;

		private static byte[] buffer2;

		public static string Version
		{
			get
			{
				if (version == null)
				{
					version = typeof(Utils).Assembly.GetName().Version.ToString();
				}
				return version;
			}
		}

		public static string TagToString(int tag)
		{
			return new string(new char[4]
			{
				(char)(tag & 0xFF),
				(char)((tag >> 8) & 0xFF),
				(char)((tag >> 16) & 0xFF),
				(char)((tag >> 24) & 0xFF)
			});
		}

		public static int Align4(int value)
		{
			return (value + 3) & -4;
		}

		public static int Align32(int value)
		{
			return (value + 31) & -32;
		}

		public static short ByteSwap(short value)
		{
			return (short)((value >> 8) | (value << 8));
		}

		public static int ByteSwap(int value)
		{
			value = (value >> 16) | (value << 16);
			return ((value >> 8) & 0xFF00FF) | ((value & 0xFF00FF) << 8);
		}

		public static bool ArrayEquals<T>(T[] a1, T[] a2)
		{
			if (a1 == a2)
			{
				return true;
			}
			if (a1 == null || a2 == null)
			{
				return false;
			}
			if (a1.Length != a2.Length)
			{
				return false;
			}
			EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
			for (int i = 0; i < a1.Length; i++)
			{
				if (!equalityComparer.Equals(a1[i], a2[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static string CleanupTextureName(string name)
		{
			name = name.Replace('/', '_');
			if (name == "<none>")
			{
				name = "none";
			}
			return name;
		}

		private static void WildcardToRegex(string wexp, StringBuilder regexp)
		{
			int num = 0;
			while (num < wexp.Length)
			{
				int num2 = wexp.IndexOfAny(wildcards, num);
				if (num2 == -1)
				{
					regexp.Append(wexp, num, wexp.Length - num);
					break;
				}
				regexp.Append(wexp, num, num2 - num);
				if (wexp[num2] == '.')
				{
					regexp.Append("\\.");
				}
				if (wexp[num2] == '*')
				{
					regexp.Append(".*");
				}
				else if (wexp[num2] == '?')
				{
					regexp.Append('.');
				}
				num = num2 + 1;
			}
		}

		public static Regex WildcardToRegex(string wexp)
		{
			if (string.IsNullOrEmpty(wexp))
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			WildcardToRegex(wexp, stringBuilder);
			return new Regex(stringBuilder.ToString(), RegexOptions.Singleline);
		}

		public static Regex WildcardToRegex(List<string> wexps)
		{
			if (wexps.Count == 0)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string wexp in wexps)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append('|');
				}
				stringBuilder.Append('(');
				WildcardToRegex(wexp, stringBuilder);
				stringBuilder.Append(')');
			}
			Console.WriteLine(stringBuilder.ToString());
			return new Regex(stringBuilder.ToString(), RegexOptions.Singleline);
		}

		public static bool AreFilesEqual(string filePath1, string filePath2)
		{
			if (buffer1 == null)
			{
				buffer1 = new byte[32768];
				buffer2 = new byte[32768];
			}
			using (FileStream fileStream = File.OpenRead(filePath1))
			{
				using (FileStream fileStream2 = File.OpenRead(filePath2))
				{
					if (fileStream.Length != fileStream2.Length)
					{
						return false;
					}
					while (true)
					{
						int num = fileStream.Read(buffer1, 0, buffer1.Length);
						int num2 = fileStream2.Read(buffer2, 0, buffer2.Length);
						if (num != num2)
						{
							return false;
						}
						if (num == 0)
						{
							break;
						}
						for (int i = 0; i < num; i++)
						{
							if (buffer1[i] != buffer2[i])
							{
								return false;
							}
						}
					}
					return true;
				}
			}
		}

		public static bool IsFlagsEnum(Type enumType)
		{
			return enumType.GetCustomAttributes(typeof(FlagsAttribute), false).Length != 0;
		}

		public static void WriteEnum(Type enumType)
		{
			bool flag = IsFlagsEnum(enumType);
			Type underlyingType = Enum.GetUnderlyingType(enumType);
			if (flag)
			{
				Console.WriteLine("flags {0}", enumType.Name);
			}
			else
			{
				Console.WriteLine("enum {0}", enumType.Name);
			}
			string[] names = Enum.GetNames(enumType);
			foreach (string text in names)
			{
				object value = Enum.Parse(enumType, text);
				if (flag)
				{
					if (underlyingType == typeof(ulong))
					{
						Console.WriteLine("\t{0} = 0x{1:X16}", text, Convert.ToUInt64(value));
					}
					else
					{
						Console.WriteLine("\t{0} = 0x{1:X8}", text, Convert.ToUInt32(value));
					}
				}
				else if (underlyingType == typeof(ulong))
				{
					Console.WriteLine("\t{0} = {1}", text, Convert.ToUInt64(value));
				}
				else
				{
					Console.WriteLine("\t{0} = {1}", text, Convert.ToInt32(value));
				}
			}
			Console.WriteLine();
		}

		public static IEnumerable<T> Reverse<T>(this IList<T> list)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				yield return list[i];
			}
		}

		public static T First<T>(this IList<T> list)
		{
			return list[0];
		}

		public static T Last<T>(this IList<T> list)
		{
			return list[list.Count - 1];
		}

		public static T LastOrDefault<T>(this List<T> list)
		{
			if (list.Count == 0)
			{
				return default(T);
			}
			return list[list.Count - 1];
		}

		public static float[] Negate(this float[] values)
		{
			float[] array = new float[values.Length];
			for (int i = 0; i < values.Length; i++)
			{
				array[i] = 0f - values[i];
			}
			return array;
		}

		public static string CommonPrefix(List<string> strings)
		{
			string text = strings[0];
			for (int i = 0; i < text.Length; i++)
			{
				for (int j = 1; j < strings.Count; j++)
				{
					string text2 = strings[j];
					if (i >= text2.Length || text[i] != text2[i])
					{
						return text.Substring(0, i);
					}
				}
			}
			return text;
		}

		public static void SkipSequence(this XmlReader xml, string name)
		{
			while (xml.IsStartElement(name))
			{
				xml.Skip();
			}
		}

		public static bool SkipEmpty(this XmlReader xml)
		{
			if (!xml.IsEmptyElement)
			{
				return false;
			}
			xml.Skip();
			return true;
		}
	}
}
