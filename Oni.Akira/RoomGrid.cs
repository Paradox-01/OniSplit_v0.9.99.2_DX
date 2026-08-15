using System;
using System.Collections.Generic;
using System.IO;
using Oni.Imaging;

namespace Oni.Akira
{
	internal class RoomGrid
	{
		private static readonly Color[] gridColors = new Color[10]
		{
			new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue),
			new Color(144, 238, 144),
			new Color(173, 216, 230),
			new Color(135, 206, 250),
			new Color(0, byte.MaxValue, 0),
			new Color(0, 0, byte.MaxValue),
			new Color(0, 0, 128),
			new Color(0, 128, 0),
			new Color(byte.MaxValue, 165, 0),
			new Color(byte.MaxValue, 0, 0)
		};

		private const int origin = -2;

		private const float tileSize = 4f;

		private readonly int xOrigin = -2;

		private readonly int xTiles;

		private readonly int zOrigin = -2;

		private readonly int zTiles;

		private readonly byte[] data;

		private readonly byte[] debugData;

		public int XTiles
		{
			get
			{
				return xTiles;
			}
		}

		public int ZTiles
		{
			get
			{
				return zTiles;
			}
		}

		public float TileSize
		{
			get
			{
				return 4f;
			}
		}

		public int XOrigin
		{
			get
			{
				return xOrigin;
			}
		}

		public int ZOrigin
		{
			get
			{
				return zOrigin;
			}
		}

		public byte[] DebugData
		{
			get
			{
				return debugData;
			}
		}

		public RoomGrid(int xTiles, int zTiles, byte[] data, byte[] debugData)
		{
			this.xTiles = xTiles;
			this.zTiles = zTiles;
			this.data = data;
			this.debugData = debugData;
		}

		public static RoomGrid FromImage(Surface image)
		{
			byte[] array = new byte[image.Width * image.Height];
			for (int i = 0; i < image.Height; i++)
			{
				for (int j = 0; j < image.Width; j++)
				{
					int num = Array.IndexOf(gridColors, image[j, i]);
					if (num == -1)
					{
						throw new InvalidDataException(string.Format("Color '{0}' does not match a valid tile type", image[j, i]));
					}
					array[i * image.Width + j] = (byte)num;
				}
			}
			return new RoomGrid(image.Width, image.Height, array, null);
		}

		public static RoomGrid FromCompressedData(int xTiles, int zTiles, byte[] compressedData)
		{
			byte[] array = new byte[xTiles * zTiles];
			if (compressedData != null)
			{
				int num = 0;
				int num2 = 0;
				while (num2 < compressedData.Length)
				{
					byte b = compressedData[num2++];
					byte b2 = (byte)(b & 0xF);
					byte b3 = (byte)(b >> 4);
					if (b3 == 0)
					{
						b3 = compressedData[num2++];
					}
					for (int i = 0; i < b3; i++)
					{
						array[num++] = b2;
					}
				}
			}
			return new RoomGrid(xTiles, zTiles, array, null);
		}

		public byte[] Compress()
		{
			List<byte> list = new List<byte>(data.Length);
			int j;
			for (int i = 0; i < data.Length; i += j)
			{
				byte b = data[i];
				for (j = 1; j < 255 && i + j < data.Length && data[i + j] == b; j++)
				{
				}
				if (j < 16)
				{
					list.Add((byte)((j << 4) | b));
					continue;
				}
				list.Add(b);
				list.Add((byte)j);
			}
			return list.ToArray();
		}

		public Surface ToImage()
		{
			Surface surface = new Surface(xTiles, zTiles, SurfaceFormat.BGRX);
			for (int i = 0; i < zTiles; i++)
			{
				for (int j = 0; j < xTiles; j++)
				{
					surface[j, i] = gridColors[data[i * xTiles + j]];
				}
			}
			return surface;
		}
	}
}
