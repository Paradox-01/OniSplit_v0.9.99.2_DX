using System;

namespace Oni.Imaging
{
	internal static class TgaReader
	{
		public static Surface Read(string filePath)
		{
			using (BinaryReader reader = new BinaryReader(filePath))
			{
				TgaHeader tgaHeader = TgaHeader.Read(reader);
				Surface surface;
				switch (tgaHeader.ImageType)
				{
				case TgaImageType.TrueColor:
					surface = LoadTrueColor(tgaHeader, reader);
					break;
				case TgaImageType.RleTrueColor:
					surface = LoadRleTrueColor(tgaHeader, reader);
					break;
				default:
					throw new NotSupportedException(string.Format("Invalid or unsupported TGA image type {0}", tgaHeader.ImageType));
				}
				if (tgaHeader.XFlip)
				{
					surface.FlipHorizontal();
				}
				if (!tgaHeader.YFlip)
				{
					surface.FlipVertical();
				}
				return surface;
			}
		}

		private static Surface LoadTrueColor(TgaHeader header, BinaryReader reader)
		{
			int pixelSize = header.PixelSize;
			SurfaceFormat surfaceFormat = header.GetSurfaceFormat();
			Surface surface = new Surface(header.Width, header.Height, surfaceFormat);
			byte[] src = reader.ReadBytes(header.Width * header.Height * pixelSize);
			int num = 0;
			for (int i = 0; i < header.Height; i++)
			{
				for (int j = 0; j < header.Width; j++)
				{
					surface[j, i] = header.GetPixel(src, num);
					num += pixelSize;
				}
			}
			return surface;
		}

		private static Surface LoadRleTrueColor(TgaHeader header, BinaryReader reader)
		{
			int pixelSize = header.PixelSize;
			SurfaceFormat surfaceFormat = header.GetSurfaceFormat();
			Surface surface = new Surface(header.Width, header.Height, surfaceFormat);
			byte[] array = reader.ReadBytes(reader.Length - reader.Position);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			Color value = Color.Black;
			while (num2 < header.Height)
			{
				int num4 = array[num++];
				int num5 = (num4 & 0x7F) + 1;
				bool flag = (num4 & 0x80) != 0;
				for (int i = 0; i < num5; i++)
				{
					if (num2 >= header.Height)
					{
						break;
					}
					if (i == 0 || !flag)
					{
						value = header.GetPixel(array, num);
						num += pixelSize;
					}
					surface[num3, num2] = value;
					num3++;
					if (num3 == header.Width)
					{
						num3 = 0;
						num2++;
					}
				}
			}
			return surface;
		}
	}
}
