using System;
using System.Collections.Generic;
using System.IO;

namespace Oni.Imaging
{
	internal class DdsHeader
	{
		private enum FOURCC
		{
			FOURCC_NONE = 0,
			FOURCC_DXT1 = 827611204
		}

		[Flags]
		private enum DDS_FLAGS
		{
			DDSD_CAPS = 1,
			DDSD_HEIGHT = 2,
			DDSD_WIDTH = 4,
			DDSD_PITCH = 8,
			DDSD_PIXELFORMAT = 0x1000,
			DDSD_MIPMAPCOUNT = 0x20000,
			DDSD_LINEARSIZE = 0x80000,
			DDSD_DEPTH = 0x800000
		}

		[Flags]
		private enum DDP_FLAGS
		{
			DDPF_RGB = 0x40,
			DDPF_FOURCC = 4,
			DDPF_ALPHAPIXELS = 1
		}

		[Flags]
		private enum DDS_CAPS
		{
			DDSCAPS_TEXTURE = 0x1000,
			DDSCAPS_MIPMAP = 0x400000,
			DDSCAPS_COMPLEX = 8
		}

		[Flags]
		private enum DDS_CAPS2
		{
			DDSCAPS2_CUBEMAP = 0x200,
			DDSCAPS2_VOLUME = 0x200000
		}

		private const int DDS_MAGIC = 542327876;

		private DDS_FLAGS flags;

		private int height;

		private int width;

		private int linearSize;

		private int depth;

		private int mipmapCount;

		private DDP_FLAGS formatFlags;

		private FOURCC fourCC;

		private int rgbBitCount;

		private uint rBitMask;

		private uint gBitMask;

		private uint bBitMask;

		private uint aBitMask;

		private DDS_CAPS caps;

		private DDS_CAPS2 caps2;

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

		public int MipmapCount
		{
			get
			{
				return mipmapCount;
			}
		}

		public SurfaceFormat GetSurfaceFormat()
		{
			if (fourCC == FOURCC.FOURCC_DXT1)
			{
				return SurfaceFormat.DXT1;
			}
			if (rgbBitCount == 32)
			{
				if (rBitMask == 16711680 && gBitMask == 65280 && bBitMask == 255)
				{
					if ((formatFlags & DDP_FLAGS.DDPF_ALPHAPIXELS) == 0)
					{
						return SurfaceFormat.BGRX;
					}
					if (aBitMask == 4278190080u)
					{
						return SurfaceFormat.BGRA;
					}
				}
			}
			else if (rgbBitCount == 16)
			{
				if (rBitMask == 31744 && gBitMask == 992 && bBitMask == 31)
				{
					if ((formatFlags & DDP_FLAGS.DDPF_ALPHAPIXELS) == 0)
					{
						return SurfaceFormat.BGRX5551;
					}
					if (aBitMask == 32768)
					{
						return SurfaceFormat.BGRA5551;
					}
				}
				else if (rBitMask == 3840 && gBitMask == 240 && bBitMask == 15 && (formatFlags & DDP_FLAGS.DDPF_ALPHAPIXELS) != 0)
				{
					return SurfaceFormat.BGRA4444;
				}
			}
			throw new NotSupportedException(string.Format("Unsupported pixel format {0} {1} {2} {3} {4} {5} {6}", formatFlags, fourCC, rgbBitCount, rBitMask, gBitMask, bBitMask, aBitMask));
		}

		public static DdsHeader Read(BinaryReader reader)
		{
			if (reader.ReadInt32() != 542327876)
			{
				throw new InvalidDataException("Not a DDS file");
			}
			DdsHeader ddsHeader = new DdsHeader();
			if (reader.ReadInt32() != 124)
			{
				throw new InvalidDataException("Invalid DDS header size");
			}
			ddsHeader.flags = (DDS_FLAGS)reader.ReadInt32();
			DDS_FLAGS dDS_FLAGS = DDS_FLAGS.DDSD_CAPS | DDS_FLAGS.DDSD_HEIGHT | DDS_FLAGS.DDSD_WIDTH | DDS_FLAGS.DDSD_PIXELFORMAT;
			if ((ddsHeader.flags & dDS_FLAGS) != dDS_FLAGS)
			{
				throw new InvalidDataException(string.Format("Invalid DDS header flags ({0})", ddsHeader.flags));
			}
			ddsHeader.height = reader.ReadInt32();
			ddsHeader.width = reader.ReadInt32();
			if (ddsHeader.width == 0 || ddsHeader.height == 0)
			{
				throw new InvalidDataException("DDS file has 0 width or height");
			}
			ddsHeader.linearSize = reader.ReadInt32();
			ddsHeader.depth = reader.ReadInt32();
			if ((ddsHeader.flags & DDS_FLAGS.DDSD_MIPMAPCOUNT) != 0)
			{
				ddsHeader.mipmapCount = reader.ReadInt32();
			}
			else
			{
				reader.ReadInt32();
				ddsHeader.mipmapCount = 1;
			}
			reader.Position += 44;
			if (reader.ReadInt32() != 32)
			{
				throw new InvalidDataException("Invalid DDS pixel format size");
			}
			ddsHeader.formatFlags = (DDP_FLAGS)reader.ReadInt32();
			if ((ddsHeader.formatFlags & DDP_FLAGS.DDPF_FOURCC) != 0)
			{
				ddsHeader.fourCC = (FOURCC)reader.ReadInt32();
			}
			else
			{
				reader.ReadInt32();
				ddsHeader.fourCC = FOURCC.FOURCC_NONE;
			}
			ddsHeader.rgbBitCount = reader.ReadInt32();
			ddsHeader.rBitMask = reader.ReadUInt32();
			ddsHeader.gBitMask = reader.ReadUInt32();
			ddsHeader.bBitMask = reader.ReadUInt32();
			ddsHeader.aBitMask = reader.ReadUInt32();
			ddsHeader.caps = (DDS_CAPS)reader.ReadInt32();
			ddsHeader.caps2 = (DDS_CAPS2)reader.ReadInt32();
			reader.Position += 12;
			if (ddsHeader.fourCC == FOURCC.FOURCC_NONE)
			{
				if (ddsHeader.rgbBitCount != 16 && ddsHeader.rgbBitCount != 32)
				{
					throw new NotSupportedException(string.Format("Unsupported RGB bit count {0}", ddsHeader.rgbBitCount));
				}
			}
			else if (ddsHeader.fourCC != FOURCC.FOURCC_DXT1)
			{
				throw new NotSupportedException(string.Format("Unsupported FOURCC {0}", ddsHeader.fourCC));
			}
			return ddsHeader;
		}

		public static DdsHeader Create(IList<Surface> surfaces)
		{
			DdsHeader ddsHeader = new DdsHeader();
			int num = surfaces[0].Width;
			int num2 = surfaces[0].Height;
			SurfaceFormat format = surfaces[0].Format;
			ddsHeader.flags = DDS_FLAGS.DDSD_CAPS | DDS_FLAGS.DDSD_HEIGHT | DDS_FLAGS.DDSD_WIDTH | DDS_FLAGS.DDSD_PIXELFORMAT;
			ddsHeader.width = num;
			ddsHeader.height = num2;
			ddsHeader.caps = DDS_CAPS.DDSCAPS_TEXTURE;
			switch (format)
			{
			case SurfaceFormat.BGRA4444:
				ddsHeader.formatFlags = DDP_FLAGS.DDPF_RGB | DDP_FLAGS.DDPF_ALPHAPIXELS;
				ddsHeader.rgbBitCount = 16;
				ddsHeader.aBitMask = 61440u;
				ddsHeader.rBitMask = 3840u;
				ddsHeader.gBitMask = 240u;
				ddsHeader.bBitMask = 15u;
				break;
			case SurfaceFormat.BGRX5551:
			case SurfaceFormat.BGRA5551:
				ddsHeader.formatFlags = DDP_FLAGS.DDPF_RGB | DDP_FLAGS.DDPF_ALPHAPIXELS;
				ddsHeader.rgbBitCount = 16;
				ddsHeader.aBitMask = 32768u;
				ddsHeader.rBitMask = 31744u;
				ddsHeader.gBitMask = 992u;
				ddsHeader.bBitMask = 31u;
				break;
			case SurfaceFormat.BGRA:
				ddsHeader.formatFlags = DDP_FLAGS.DDPF_RGB | DDP_FLAGS.DDPF_ALPHAPIXELS;
				ddsHeader.rgbBitCount = 32;
				ddsHeader.aBitMask = 4278190080u;
				ddsHeader.rBitMask = 16711680u;
				ddsHeader.gBitMask = 65280u;
				ddsHeader.bBitMask = 255u;
				break;
			case SurfaceFormat.BGRX:
				ddsHeader.formatFlags = DDP_FLAGS.DDPF_RGB;
				ddsHeader.rgbBitCount = 32;
				ddsHeader.rBitMask = 16711680u;
				ddsHeader.gBitMask = 65280u;
				ddsHeader.bBitMask = 255u;
				break;
			case SurfaceFormat.RGBA:
				ddsHeader.formatFlags = DDP_FLAGS.DDPF_RGB | DDP_FLAGS.DDPF_ALPHAPIXELS;
				ddsHeader.rgbBitCount = 32;
				ddsHeader.aBitMask = 255u;
				ddsHeader.rBitMask = 65280u;
				ddsHeader.gBitMask = 16711680u;
				ddsHeader.bBitMask = 4278190080u;
				break;
			case SurfaceFormat.RGBX:
				ddsHeader.formatFlags = DDP_FLAGS.DDPF_RGB;
				ddsHeader.rgbBitCount = 32;
				ddsHeader.rBitMask = 65280u;
				ddsHeader.gBitMask = 16711680u;
				ddsHeader.bBitMask = 4278190080u;
				break;
			case SurfaceFormat.DXT1:
				ddsHeader.formatFlags = DDP_FLAGS.DDPF_FOURCC;
				ddsHeader.fourCC = FOURCC.FOURCC_DXT1;
				break;
			}
			switch (format)
			{
			case SurfaceFormat.BGRA4444:
			case SurfaceFormat.BGRX5551:
			case SurfaceFormat.BGRA5551:
				ddsHeader.flags |= DDS_FLAGS.DDSD_PITCH;
				ddsHeader.linearSize = num * 2;
				break;
			case SurfaceFormat.BGRX:
			case SurfaceFormat.BGRA:
			case SurfaceFormat.RGBX:
			case SurfaceFormat.RGBA:
				ddsHeader.flags |= DDS_FLAGS.DDSD_PITCH;
				ddsHeader.linearSize = num * 4;
				break;
			case SurfaceFormat.DXT1:
				ddsHeader.flags |= DDS_FLAGS.DDSD_LINEARSIZE;
				ddsHeader.linearSize = Math.Max(1, num / 4) * Math.Max(1, num2 / 4) * 8;
				break;
			}
			if (surfaces.Count > 1)
			{
				ddsHeader.flags |= DDS_FLAGS.DDSD_MIPMAPCOUNT;
				ddsHeader.mipmapCount = surfaces.Count;
				ddsHeader.caps |= DDS_CAPS.DDSCAPS_MIPMAP | DDS_CAPS.DDSCAPS_COMPLEX;
			}
			return ddsHeader;
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write(542327876);
			writer.Write(124);
			writer.Write((int)flags);
			writer.Write(height);
			writer.Write(width);
			writer.Write(linearSize);
			writer.Write(depth);
			writer.Write(mipmapCount);
			writer.BaseStream.Seek(44L, SeekOrigin.Current);
			writer.Write(32);
			writer.Write((int)formatFlags);
			writer.Write((int)fourCC);
			writer.Write(rgbBitCount);
			writer.Write(rBitMask);
			writer.Write(gBitMask);
			writer.Write(bBitMask);
			writer.Write(aBitMask);
			writer.Write((int)caps);
			writer.Write((int)caps2);
			writer.BaseStream.Seek(12L, SeekOrigin.Current);
		}
	}
}
