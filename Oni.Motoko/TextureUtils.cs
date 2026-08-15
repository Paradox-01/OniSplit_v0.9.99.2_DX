using System;
using System.Collections.Generic;
using System.IO;
using Oni.Imaging;

namespace Oni.Motoko
{
	internal static class TextureUtils
	{
		public static SurfaceFormat ToSurfaceFormat(this TextureFormat format)
		{
			switch (format)
			{
			case TextureFormat.BGRA4444:
				return SurfaceFormat.BGRA4444;
			case TextureFormat.BGR555:
				return SurfaceFormat.BGRX5551;
			case TextureFormat.BGRA5551:
				return SurfaceFormat.BGRA5551;
			case TextureFormat.RGBA:
				return SurfaceFormat.RGBA;
			case TextureFormat.BGR:
				return SurfaceFormat.BGRX;
			case TextureFormat.DXT1:
				return SurfaceFormat.DXT1;
			default:
				throw new NotSupportedException(string.Format("Texture format {0} is not supported", format));
			}
		}

		public static Surface LoadImage(string filePath)
		{
			List<Surface> list = new List<Surface>();
			string text = Path.GetExtension(filePath).ToLowerInvariant();
			if (text != null && text == ".tga")
			{
				list.Add(TgaReader.Read(filePath));
			}
			else
			{
				list.Add(SysReader.Read(filePath));
			}
			if (list.Count == 0)
			{
				throw new InvalidDataException(string.Format("Could not load image '{0}'", filePath));
			}
			return list[0];
		}

		public static int RoundToPowerOf2(int value)
		{
			if (value <= 2)
			{
				return value;
			}
			int num = 0;
			int num2 = value;
			while (num2 > 1)
			{
				num2 >>= 1;
				num++;
			}
			return 1 << num;
		}
	}
}
