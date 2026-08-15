namespace Oni.Imaging
{
	internal static class Dxt1
	{
		public static Surface Decompress(Surface src, SurfaceFormat dstFormat)
		{
			Surface surface = new Surface(src.Width, src.Height, dstFormat);
			Color[] array = new Color[4];
			int num = 0;
			for (int i = 0; i < surface.Height; i += 4)
			{
				for (int j = 0; j < surface.Width; j += 4)
				{
					array[0] = Color.ReadBgr565(src.Data, num);
					num += 2;
					array[1] = Color.ReadBgr565(src.Data, num);
					num += 2;
					if (array[0].ToBgr565() > array[1].ToBgr565())
					{
						array[2] = Color.Lerp(array[0], array[1], 1f / 3f);
						array[3] = Color.Lerp(array[0], array[1], 2f / 3f);
					}
					else
					{
						array[2] = Color.Lerp(array[0], array[1], 0.5f);
						array[3] = Color.Transparent;
					}
					for (int k = 0; k < 4; k++)
					{
						int num2 = src.Data[num++];
						for (int l = 0; l < 4; l++)
						{
							surface[j + l, i + k] = array[num2 & 3];
							num2 >>= 2;
						}
					}
				}
			}
			return surface;
		}

		public static Surface Compress(Surface src)
		{
			Surface surface = new Surface(Utils.Align4(src.Width), Utils.Align4(src.Height), SurfaceFormat.DXT1);
			Vector3[] array = new Vector3[16];
			Vector3[] array2 = new Vector3[4];
			int[] array3 = new int[16];
			int num = 0;
			int height = surface.Height;
			int width = surface.Width;
			for (int i = 0; i < height; i += 4)
			{
				for (int j = 0; j < width; j += 4)
				{
					for (int k = 0; k < 4; k++)
					{
						for (int l = 0; l < 4; l++)
						{
							array[k * 4 + l] = src[j + l, i + k].ToVector3();
						}
					}
					CompressBlock(array, array3, array2);
					Color.WriteBgr565(new Color(array2[0]), surface.Data, num);
					num += 2;
					Color.WriteBgr565(new Color(array2[1]), surface.Data, num);
					num += 2;
					for (int m = 0; m < 4; m++)
					{
						int num2 = 0;
						for (int num3 = 3; num3 >= 0; num3--)
						{
							num2 = (num2 << 2) | array3[m * 4 + num3];
						}
						surface.Data[num++] = (byte)num2;
					}
				}
			}
			return surface;
		}

		private static void CompressBlock(Vector3[] block, int[] lookup, Vector3[] colors)
		{
			colors[0] = block[0];
			colors[1] = block[0];
			for (int i = 1; i < block.Length; i++)
			{
				colors[0] = Vector3.Min(colors[0], block[i]);
				colors[1] = Vector3.Max(colors[1], block[i]);
			}
			int maxColor;
			if (new Color(colors[0]).ToBgr565() > new Color(colors[1]).ToBgr565())
			{
				colors[2] = Vector3.Lerp(colors[0], colors[1], 1f / 3f);
				colors[3] = Vector3.Lerp(colors[0], colors[1], 2f / 3f);
				maxColor = 4;
			}
			else
			{
				colors[2] = Vector3.Lerp(colors[0], colors[1], 0.5f);
				maxColor = 3;
			}
			for (int j = 0; j < block.Length; j++)
			{
				lookup[j] = LookupNearest(colors, block[j], maxColor);
			}
		}

		private static int LookupNearest(Vector3[] colors, Vector3 pixel, int maxColor)
		{
			int result = 0;
			float num = Vector3.DistanceSquared(pixel, colors[0]);
			for (int i = 1; i < maxColor; i++)
			{
				float num2 = Vector3.DistanceSquared(pixel, colors[i]);
				if (num2 < num)
				{
					num = num2;
					result = i;
				}
			}
			return result;
		}
	}
}
