using System;
using System.Collections.Generic;
using System.IO;

namespace Oni.Imaging
{
	internal static class TgaWriter
	{
		public static void Write(Surface surface, string filePath)
		{
			surface = surface.Convert(SurfaceFormat.BGRA);
			TgaImageType imageType = TgaImageType.TrueColor;
			byte[] array = null;
			if (surface.Width > 2 && surface.Height > 2)
			{
				array = Rle32Compress(surface.Width, surface.Height, surface.Data);
				if (array.Length > surface.Data.Length)
				{
					array = null;
				}
				else
				{
					imageType = TgaImageType.RleTrueColor;
				}
			}
			TgaHeader tgaHeader = TgaHeader.Create(surface.Width, surface.Height, imageType);
			Directory.CreateDirectory(Path.GetDirectoryName(filePath));
			using (FileStream stream = File.Create(filePath))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(stream))
				{
					tgaHeader.Write(binaryWriter);
					if (array != null)
					{
						binaryWriter.Write(array, 0, array.Length);
					}
					else
					{
						binaryWriter.Write(surface.Data, 0, surface.Data.Length);
					}
				}
			}
		}

		private static byte[] Rle32Compress(int width, int height, byte[] sourceData)
		{
			List<byte> list = new List<byte>();
			for (int num = height - 1; num >= 0; num--)
			{
				int num2 = num * width * 4;
				int num3 = BitConverter.ToInt32(sourceData, num * width * 4);
				int xStart = 0;
				byte b = 64;
				for (int i = 1; i < width; i++)
				{
					int num4 = BitConverter.ToInt32(sourceData, i * 4 + num2);
					if (num4 == num3)
					{
						if (b == 0)
						{
							Rle32WritePackets(list, b, sourceData, xStart, i - 1, num2);
							xStart = i - 1;
						}
						b = 128;
					}
					else
					{
						if (b == 128)
						{
							Rle32WritePackets(list, b, sourceData, xStart, i, num2);
							xStart = i;
						}
						b = 0;
					}
					num3 = num4;
					if (i == width - 1)
					{
						Rle32WritePackets(list, b, sourceData, xStart, width, num2);
					}
				}
			}
			return list.ToArray();
		}

		private static void Rle32WritePackets(List<byte> result, byte packetType, byte[] sourceData, int xStart, int xStop, int lineOffset)
		{
			int num = xStop - xStart;
			if (num == 0)
			{
				return;
			}
			int num2 = xStart * 4 + lineOffset;
			if (packetType == 128)
			{
				while (num > 128)
				{
					result.Add((byte)(packetType | 0x7F));
					for (int i = 0; i < 4; i++)
					{
						result.Add(sourceData[num2 + i]);
					}
					num -= 128;
				}
				result.Add((byte)(packetType | (num - 1)));
				for (int j = 0; j < 4; j++)
				{
					result.Add(sourceData[num2 + j]);
				}
				return;
			}
			while (num > 128)
			{
				result.Add((byte)(packetType | 0x7F));
				for (int k = 0; k < 512; k++)
				{
					result.Add(sourceData[num2 + k]);
				}
				num2 += 512;
				num -= 128;
			}
			result.Add((byte)(packetType | (num - 1)));
			for (int l = 0; l < 4 * num; l++)
			{
				result.Add(sourceData[num2 + l]);
			}
		}
	}
}
