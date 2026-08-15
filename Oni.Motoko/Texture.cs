using System;
using System.Collections.Generic;
using Oni.Imaging;

namespace Oni.Motoko
{
	internal class Texture
	{
		public readonly List<Surface> Surfaces = new List<Surface>();

		public int Width;

		public int Height;

		public TextureFormat Format;

		public TextureFlags Flags;

		public string Name;

		public Texture EnvMap;

		public bool HasAlpha
		{
			get
			{
				return Surfaces[0].HasAlpha;
			}
		}

		public bool WrapU
		{
			get
			{
				return (Flags & TextureFlags.NoUWrap) == 0;
			}
		}

		public bool WrapV
		{
			get
			{
				return (Flags & TextureFlags.NoVWrap) == 0;
			}
		}

		public void GenerateMipMaps()
		{
			if ((Flags & TextureFlags.HasMipMaps) != TextureFlags.None)
			{
				return;
			}
			Surface surface = Surfaces[0];
			Surfaces.Clear();
			Surfaces.Add(surface);
			if (surface.Format == SurfaceFormat.DXT1)
			{
				surface = surface.Convert(SurfaceFormat.BGRX5551);
			}
			int num = surface.Width;
			int num2 = surface.Height;
			SurfaceFormat surfaceFormat = Format.ToSurfaceFormat();
			while (num > 1 || num2 > 1)
			{
				num = Math.Max(num >> 1, 1);
				num2 = Math.Max(num2 >> 1, 1);
				surface = surface.Resize(num, num2);
				Surfaces.Add(surface);
			}
			if (surface.Format != surfaceFormat)
			{
				for (int i = 1; i < Surfaces.Count; i++)
				{
					Surfaces[i] = Surfaces[i].Convert(surfaceFormat);
				}
			}
			Flags |= TextureFlags.HasMipMaps;
		}
	}
}
