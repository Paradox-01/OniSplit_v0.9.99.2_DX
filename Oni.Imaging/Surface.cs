using System;

namespace Oni.Imaging
{
	internal class Surface
	{
		private int width;

		private int height;

		private int stride;

		private int pixelSize;

		private SurfaceFormat format;

		private byte[] data;

		public int Width
		{
			get
			{
				return width;
			}
		}

		public int Height
		{
			get
			{
				return height;
			}
		}

		public SurfaceFormat Format
		{
			get
			{
				return format;
			}
		}

		public byte[] Data
		{
			get
			{
				return data;
			}
		}

		public bool HasAlpha
		{
			get
			{
				switch (format)
				{
				case SurfaceFormat.BGRA4444:
				case SurfaceFormat.BGRA5551:
				case SurfaceFormat.BGRA:
				case SurfaceFormat.RGBA:
					return true;
				default:
					return false;
				}
			}
		}

		public Color this[int x, int y]
		{
			get
			{
				if (x < 0 || width <= x || y < 0 || height <= y)
				{
					return Color.Black;
				}
				return GetPixel(x, y);
			}
			set
			{
				if (x >= 0 && width > x && y >= 0 && height > y)
				{
					SetPixel(x, y, value);
				}
			}
		}

		public Surface(int width, int height)
			: this(width, height, SurfaceFormat.RGBA)
		{
		}

		public Surface(int width, int height, SurfaceFormat format)
		{
			if (format == SurfaceFormat.DXT1)
			{
				width = Math.Max(width, 4);
				height = Math.Max(height, 4);
			}
			this.width = width;
			this.height = height;
			this.format = format;
			pixelSize = GetPixelSize(format);
			stride = pixelSize * width;
			data = new byte[GetDataSize(width, height, format)];
		}

		public Surface(int width, int height, SurfaceFormat format, byte[] data)
		{
			if (format == SurfaceFormat.DXT1)
			{
				width = Math.Max(width, 4);
				height = Math.Max(height, 4);
			}
			this.width = width;
			this.height = height;
			this.format = format;
			this.data = data;
			pixelSize = GetPixelSize(format);
			stride = pixelSize * width;
		}

		private static int GetDataSize(int width, int height, SurfaceFormat format)
		{
			switch (format)
			{
			case SurfaceFormat.BGRA4444:
			case SurfaceFormat.BGRX5551:
			case SurfaceFormat.BGRA5551:
				return width * height * 2;
			case SurfaceFormat.BGRX:
			case SurfaceFormat.BGRA:
			case SurfaceFormat.RGBX:
			case SurfaceFormat.RGBA:
				return width * height * 4;
			case SurfaceFormat.DXT1:
				return width * height / 2;
			default:
				throw new NotSupportedException(string.Format("Unsupported texture format {0}", format));
			}
		}

		private static int GetPixelSize(SurfaceFormat format)
		{
			switch (format)
			{
			case SurfaceFormat.BGRA4444:
			case SurfaceFormat.BGRX5551:
			case SurfaceFormat.BGRA5551:
				return 2;
			case SurfaceFormat.BGRX:
			case SurfaceFormat.BGRA:
			case SurfaceFormat.RGBX:
			case SurfaceFormat.RGBA:
				return 4;
			case SurfaceFormat.DXT1:
				return 2;
			default:
				throw new NotSupportedException(string.Format("Unsupported texture format {0}", format));
			}
		}

		public void CleanupAlpha()
		{
			if ((format == SurfaceFormat.BGRA5551 || format == SurfaceFormat.RGBA || format == SurfaceFormat.BGRA) && !HasTransparentPixels())
			{
				switch (format)
				{
				case SurfaceFormat.BGRA5551:
					format = SurfaceFormat.BGRX5551;
					break;
				case SurfaceFormat.BGRA:
					format = SurfaceFormat.BGRX;
					break;
				case SurfaceFormat.RGBA:
					format = SurfaceFormat.RGBX;
					break;
				}
			}
		}

		public bool HasTransparentPixels()
		{
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					if (GetPixel(j, i).A != byte.MaxValue)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void FlipVertical()
		{
			byte[] array = new byte[stride];
			for (int i = 0; i < height / 2; i++)
			{
				int num = height - i - 1;
				Array.Copy(data, i * stride, array, 0, stride);
				Array.Copy(data, num * stride, data, i * stride, stride);
				Array.Copy(array, 0, data, num * stride, stride);
			}
		}

		public void FlipHorizontal()
		{
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width / 2; j++)
				{
					int x = width - j - 1;
					Color pixel = GetPixel(j, i);
					Color pixel2 = GetPixel(x, i);
					SetPixel(j, i, pixel2);
					SetPixel(x, i, pixel);
				}
			}
		}

		public void Rotate90()
		{
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					if (i > j)
					{
						Color pixel = GetPixel(i, j);
						Color pixel2 = GetPixel(j, i);
						SetPixel(i, j, pixel2);
						SetPixel(j, i, pixel);
					}
				}
			}
		}

		public Surface Convert(SurfaceFormat dstFormat)
		{
			Surface surface;
			if (format == dstFormat)
			{
				surface = new Surface(width, height, dstFormat, (byte[])data.Clone());
			}
			else if (dstFormat == SurfaceFormat.DXT1)
			{
				surface = Dxt1.Compress(this);
			}
			else if (format == SurfaceFormat.DXT1)
			{
				surface = Dxt1.Decompress(this, dstFormat);
			}
			else
			{
				surface = new Surface(width, height, dstFormat);
				for (int i = 0; i < height; i++)
				{
					for (int j = 0; j < width; j++)
					{
						surface.SetPixel(j, i, GetPixel(j, i));
					}
				}
			}
			return surface;
		}

		public Surface Resize(int newWidth, int newHeight)
		{
			if (newWidth > width || newHeight > height)
			{
				throw new NotImplementedException();
			}
			Surface surface = new Surface(newWidth, newHeight, format);
			if (newWidth * 2 == width && newHeight * 2 == height)
			{
				Halfsize(surface);
				return surface;
			}
			float num = (float)width / (float)surface.width;
			float num2 = (float)height / (float)surface.height;
			for (int i = 0; i < surface.height; i++)
			{
				float num3 = (float)i * num2;
				float num4 = num3 + num2;
				int num5 = (int)num3;
				int num6 = (int)(num4 - 0.001f);
				float num7 = 1f - (num3 - (float)num5);
				float num8 = num4 - (float)num6;
				for (int j = 0; j < surface.width; j++)
				{
					float num9 = (float)j * num;
					float num10 = num9 + num;
					int num11 = (int)num9;
					int num12 = (int)(num10 - 0.001f);
					float num13 = 1f - (num9 - (float)num11);
					float num14 = num10 - (float)num12;
					Vector4 vector = GetVector4(num11, num5) * (num13 * num7);
					vector += GetVector4(num12, num5) * (num14 * num7);
					vector += GetVector4(num11, num6) * (num13 * num8);
					vector += GetVector4(num12, num6) * (num14 * num8);
					for (int k = num5 + 1; k < num6; k++)
					{
						vector += GetVector4(num11, k) * num13;
						vector += GetVector4(num12, k) * num14;
					}
					for (int l = num11 + 1; l < num12; l++)
					{
						vector += GetVector4(l, num5) * num7;
						vector += GetVector4(l, num6) * num8;
					}
					for (int m = num5 + 1; m < num6; m++)
					{
						for (int n = num11 + 1; n < num12; n++)
						{
							vector += GetVector4(n, m);
						}
					}
					float num15 = (num10 - num9) * (num4 - num3);
					surface.SetPixel(j, i, new Color(vector / num15));
				}
			}
			return surface;
		}

		private void Halfsize(Surface dst)
		{
			int num = dst.width;
			int num2 = dst.height;
			for (int i = 0; i < num2; i++)
			{
				int num3 = i * 2;
				int y = num3 + 1;
				for (int j = 0; j < num; j++)
				{
					int num4 = j * 2;
					int x = num4 + 1;
					Vector4 vector = GetVector4(num4, num3);
					vector += GetVector4(x, num3);
					vector += GetVector4(num4, y);
					vector += GetVector4(x, y);
					dst.SetPixel(j, i, new Color(vector / 4f));
				}
			}
		}

		private Vector4 GetVector4(int x, int y)
		{
			return GetPixel(x, y).ToVector4();
		}

		private Color GetPixel(int x, int y)
		{
			int index = x * pixelSize + y * stride;
			switch (format)
			{
			case SurfaceFormat.BGRA4444:
				return Color.ReadBgra4444(data, index);
			case SurfaceFormat.BGRX5551:
				return Color.ReadBgrx5551(data, index);
			case SurfaceFormat.BGRA5551:
				return Color.ReadBgra5551(data, index);
			case SurfaceFormat.BGR565:
				return Color.ReadBgr565(data, index);
			case SurfaceFormat.BGRX:
				return Color.ReadBgrx(data, index);
			case SurfaceFormat.BGRA:
				return Color.ReadBgra(data, index);
			case SurfaceFormat.RGBX:
				return Color.ReadRgbx(data, index);
			case SurfaceFormat.RGBA:
				return Color.ReadRgba(data, index);
			default:
				throw new NotSupportedException(string.Format("Unsupported texture format {0}", format));
			}
		}

		private void SetPixel(int x, int y, Color color)
		{
			int index = x * pixelSize + y * stride;
			switch (format)
			{
			case SurfaceFormat.BGRA4444:
				Color.WriteBgra4444(color, data, index);
				break;
			case SurfaceFormat.BGRX5551:
				Color.WriteBgrx5551(color, data, index);
				break;
			case SurfaceFormat.BGRA5551:
				Color.WriteBgra5551(color, data, index);
				break;
			case SurfaceFormat.BGR565:
				Color.WriteBgr565(color, data, index);
				break;
			case SurfaceFormat.BGRX:
				Color.WriteBgrx(color, data, index);
				break;
			case SurfaceFormat.BGRA:
				Color.WriteBgra(color, data, index);
				break;
			case SurfaceFormat.RGBX:
				Color.WriteRgbx(color, data, index);
				break;
			case SurfaceFormat.RGBA:
				Color.WriteRgba(color, data, index);
				break;
			default:
				throw new NotSupportedException(string.Format("Unsupported texture format {0}", format));
			}
		}

		public void Fill(int x, int y, int width, int height, Color color)
		{
			for (int i = x; i < x + width; i++)
			{
				for (int j = y; j < y + height; j++)
				{
					SetPixel(i, j, color);
				}
			}
		}
	}
}
