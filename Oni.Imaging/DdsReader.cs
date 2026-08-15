using System;
using System.Collections.Generic;

namespace Oni.Imaging
{
	internal static class DdsReader
	{
		public static List<Surface> Read(string filePath, bool noMipMaps)
		{
			List<Surface> list = new List<Surface>();
			using (BinaryReader binaryReader = new BinaryReader(filePath))
			{
				DdsHeader ddsHeader = DdsHeader.Read(binaryReader);
				SurfaceFormat surfaceFormat = ddsHeader.GetSurfaceFormat();
				for (int i = 0; i < ddsHeader.MipmapCount; i++)
				{
					int num = Math.Max(ddsHeader.Width >> i, 1);
					int num2 = Math.Max(ddsHeader.Height >> i, 1);
					if (surfaceFormat == SurfaceFormat.DXT1)
					{
						num = Math.Max(num, 4);
						num2 = Math.Max(num2, 4);
					}
					Surface surface = new Surface(num, num2, surfaceFormat);
					binaryReader.Read(surface.Data, 0, surface.Data.Length);
					list.Add(surface);
					if (noMipMaps)
					{
						break;
					}
				}
			}
			return list;
		}
	}
}
