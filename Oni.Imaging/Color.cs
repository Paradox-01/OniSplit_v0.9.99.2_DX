using System;
using System.Globalization;

namespace Oni.Imaging
{
	internal struct Color : IEquatable<Color>
	{
		private byte b;

		private byte g;

		private byte r;

		private byte a;

		private static readonly Color black = new Color(0, 0, 0, byte.MaxValue);

		private static readonly Color white = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		private static readonly Color transparent = new Color(0, 0, 0, 0);

		public bool IsTransparent
		{
			get
			{
				return a != byte.MaxValue;
			}
		}

		public byte R
		{
			get
			{
				return r;
			}
		}

		public byte G
		{
			get
			{
				return g;
			}
		}

		public byte B
		{
			get
			{
				return b;
			}
		}

		public byte A
		{
			get
			{
				return a;
			}
		}

		public static Color White
		{
			get
			{
				return white;
			}
		}

		public static Color Black
		{
			get
			{
				return black;
			}
		}

		public static Color Transparent
		{
			get
			{
				return transparent;
			}
		}

		public Color(byte r, byte g, byte b)
			: this(r, g, b, byte.MaxValue)
		{
		}

		public Color(byte r, byte g, byte b, byte a)
		{
			this.b = b;
			this.g = g;
			this.r = r;
			this.a = a;
		}

		public Color(Vector3 v)
		{
			r = (byte)(v.X * 255f);
			g = (byte)(v.Y * 255f);
			b = (byte)(v.Z * 255f);
			a = byte.MaxValue;
		}

		public Color(Vector4 v)
		{
			r = (byte)(v.X * 255f);
			g = (byte)(v.Y * 255f);
			b = (byte)(v.Z * 255f);
			a = (byte)(v.W * 255f);
		}

		public int ToBgra32()
		{
			return b | (g << 8) | (r << 16) | (a << 24);
		}

		public int ToBgr565()
		{
			return (b >> 3) | ((g & 0xFC) << 3) | ((r & 0xF8) << 8);
		}

		public Vector3 ToVector3()
		{
			return new Vector3((int)r, (int)g, (int)b) / 255f;
		}

		public Vector4 ToVector4()
		{
			return new Vector4((int)r, (int)g, (int)b, (int)a) / 255f;
		}

		public static bool operator ==(Color a, Color b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Color a, Color b)
		{
			return !a.Equals(b);
		}

		public bool Equals(Color color)
		{
			if (r == color.r && g == color.g && b == color.b)
			{
				return a == color.a;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is Color)
			{
				return Equals((Color)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return r.GetHashCode() ^ g.GetHashCode() ^ b.GetHashCode() ^ a.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{{R:{0} G:{1} B:{2} A:{3}}}", r, g, b, a);
		}

		public static Color ReadBgra4444(byte[] data, int index)
		{
			int num = data[index];
			byte b = (byte)((num << 4) & 0xF0);
			byte b2 = (byte)(num & 0xF0);
			num = data[index + 1];
			byte b3 = (byte)((num << 4) & 0xF0);
			byte b4 = (byte)(num & 0xF0);
			return new Color(b3, b2, b, b4);
		}

		public static void WriteBgra4444(Color color, byte[] data, int index)
		{
			data[index] = (byte)((color.b >> 4) | (color.g & 0xF0));
			data[index + 1] = (byte)((color.r >> 4) | (color.a & 0xF0));
		}

		public static Color ReadBgrx5551(byte[] data, int index)
		{
			int num = data[index] | (data[index + 1] << 8);
			byte b = (byte)((num << 3) & 0xF8);
			byte b2 = (byte)((num >> 2) & 0xF8);
			byte b3 = (byte)((num >> 7) & 0xF8);
			return new Color(b3, b2, b);
		}

		public static void WriteBgrx5551(Color color, byte[] data, int index)
		{
			data[index] = (byte)((color.b >> 3) | ((color.g & 0x38) << 2));
			data[index + 1] = (byte)((color.g >> 6) | ((color.r & 0xF8) >> 1) | 0x80);
		}

		public static Color ReadBgra5551(byte[] data, int index)
		{
			int num = data[index] | (data[index + 1] << 8);
			byte b = (byte)((num << 3) & 0xF8);
			byte b2 = (byte)((num >> 2) & 0xF8);
			byte b3 = (byte)((num >> 7) & 0xF8);
			byte b4 = (byte)((num >> 15) * 255);
			return new Color(b3, b2, b, b4);
		}

		public static void WriteBgra5551(Color color, byte[] data, int index)
		{
			data[index] = (byte)((color.b >> 3) | ((color.g & 0x38) << 2));
			data[index + 1] = (byte)((color.g >> 6) | ((color.r & 0xF8) >> 1) | (color.a & 0x80));
		}

		public static Color ReadBgr565(byte[] data, int index)
		{
			int num = data[index] | (data[index + 1] << 8);
			byte b = (byte)((num << 3) & 0xF8);
			byte b2 = (byte)((num >> 3) & 0xFC);
			byte b3 = (byte)((num >> 8) & 0xF8);
			return new Color(b3, b2, b);
		}

		public static void WriteBgr565(Color color, byte[] data, int index)
		{
			data[index] = (byte)((color.b >> 3) | ((color.g & 0x1C) << 3));
			data[index + 1] = (byte)((color.g >> 5) | (color.r & 0xF8));
		}

		public static Color ReadBgrx(byte[] data, int index)
		{
			byte b = data[index];
			byte b2 = data[index + 1];
			byte b3 = data[index + 2];
			return new Color(b3, b2, b);
		}

		public static void WriteBgrx(Color color, byte[] data, int index)
		{
			data[index] = color.b;
			data[index + 1] = color.g;
			data[index + 2] = color.r;
			data[index + 3] = byte.MaxValue;
		}

		public static Color ReadBgra(byte[] data, int index)
		{
			byte b = data[index];
			byte b2 = data[index + 1];
			byte b3 = data[index + 2];
			byte b4 = data[index + 3];
			return new Color(b3, b2, b, b4);
		}

		public static void WriteBgra(Color color, byte[] data, int index)
		{
			data[index] = color.b;
			data[index + 1] = color.g;
			data[index + 2] = color.r;
			data[index + 3] = color.a;
		}

		public static Color ReadRgbx(byte[] data, int index)
		{
			byte b = data[index];
			byte b2 = data[index + 1];
			byte b3 = data[index + 2];
			return new Color(b, b2, b3);
		}

		public static void WriteRgbx(Color color, byte[] data, int index)
		{
			data[index] = color.r;
			data[index + 1] = color.g;
			data[index + 2] = color.b;
			data[index + 3] = byte.MaxValue;
		}

		public static Color ReadRgba(byte[] data, int index)
		{
			byte b = data[index];
			byte b2 = data[index + 1];
			byte b3 = data[index + 2];
			byte b4 = data[index + 3];
			return new Color(b, b2, b3, b4);
		}

		public static void WriteRgba(Color color, byte[] data, int index)
		{
			data[index] = color.r;
			data[index + 1] = color.g;
			data[index + 2] = color.b;
			data[index + 3] = color.a;
		}

		public static Color Lerp(Color x, Color y, float amount)
		{
			byte b = (byte)MathHelper.Lerp(x.r, y.r, amount);
			byte b2 = (byte)MathHelper.Lerp(x.g, y.g, amount);
			byte b3 = (byte)MathHelper.Lerp(x.b, y.b, amount);
			byte b4 = (byte)MathHelper.Lerp(x.a, y.a, amount);
			return new Color(b, b2, b3, b4);
		}

		public static bool TryParse(string s, out Color color)
		{
			color = default(Color);
			color.a = byte.MaxValue;
			if (string.IsNullOrEmpty(s))
			{
				return false;
			}
			string[] array = s.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
			if (array.Length < 3 || array.Length > 4)
			{
				return false;
			}
			if (!byte.TryParse(array[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out color.r))
			{
				return false;
			}
			if (!byte.TryParse(array[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out color.g))
			{
				return false;
			}
			if (!byte.TryParse(array[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out color.b))
			{
				return false;
			}
			if (array.Length > 3 && !byte.TryParse(array[3], NumberStyles.Integer, CultureInfo.InvariantCulture, out color.a))
			{
				return false;
			}
			return true;
		}

		public static Color Parse(string s)
		{
			Color color;
			if (!TryParse(s, out color))
			{
				throw new FormatException(string.Format("'{0}' is not a color", s));
			}
			return color;
		}
	}
}
