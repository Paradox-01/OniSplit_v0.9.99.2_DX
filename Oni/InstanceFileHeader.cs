using System.IO;

namespace Oni
{
	internal sealed class InstanceFileHeader
	{
		public const long OniPCTemplateChecksum = 1052091763926815L;

		public const long OniMacTemplateChecksum = 1052091493724257L;

		public const int Version31 = 1448227633;

		public const int Version32 = 1448227634;

		public const long Signature = 2251868534472768L;

		private long templateChecksum;

		private int version;

		private long signature;

		private int instanceCount;

		private int nameCount;

		private int templateCount;

		private int dataTableOffset;

		private int dataTableSize;

		private int nameTableOffset;

		private int nameTableSize;

		private int rawTableOffset;

		private int rawTableSize;

		public long TemplateChecksum
		{
			get
			{
				return templateChecksum;
			}
		}

		public int Version
		{
			get
			{
				return version;
			}
		}

		public int InstanceCount
		{
			get
			{
				return instanceCount;
			}
		}

		public int NameCount
		{
			get
			{
				return nameCount;
			}
		}

		public int TemplateCoun
		{
			get
			{
				return templateCount;
			}
		}

		public int DataTableOffset
		{
			get
			{
				return dataTableOffset;
			}
		}

		public int DataTableSize
		{
			get
			{
				return dataTableSize;
			}
		}

		public int NameTableOffset
		{
			get
			{
				return nameTableOffset;
			}
		}

		public int NameTableSize
		{
			get
			{
				return nameTableSize;
			}
		}

		public int RawTableOffset
		{
			get
			{
				return rawTableOffset;
			}
		}

		public int RawTableSize
		{
			get
			{
				return rawTableSize;
			}
		}

		internal static InstanceFileHeader Read(BinaryReader reader)
		{
			InstanceFileHeader instanceFileHeader = new InstanceFileHeader
			{
				templateChecksum = reader.ReadInt64(),
				version = reader.ReadInt32(),
				signature = reader.ReadInt64()
			};
			ValidateHeader(instanceFileHeader);
			instanceFileHeader.instanceCount = reader.ReadInt32();
			instanceFileHeader.nameCount = reader.ReadInt32();
			instanceFileHeader.templateCount = reader.ReadInt32();
			instanceFileHeader.dataTableOffset = reader.ReadInt32();
			instanceFileHeader.dataTableSize = reader.ReadInt32();
			instanceFileHeader.nameTableOffset = reader.ReadInt32();
			instanceFileHeader.nameTableSize = reader.ReadInt32();
			if (instanceFileHeader.version == 1448227634)
			{
				instanceFileHeader.rawTableOffset = reader.ReadInt32();
				instanceFileHeader.rawTableSize = reader.ReadInt32();
				reader.Skip(8);
			}
			else
			{
				reader.Skip(16);
			}
			return instanceFileHeader;
		}

		private static void ValidateHeader(InstanceFileHeader header)
		{
			if (header.templateChecksum != 1052091763926815L && header.templateChecksum != 1052091493724257L)
			{
				header.templateChecksum = 1052091493724257L;
			}
			if (header.version != 1448227633 && header.version != 1448227634)
			{
				throw new InvalidDataException("Unknown file version");
			}
			if (header.version == 1448227633 && header.signature != 2251868534472768L)
			{
				throw new InvalidDataException("Invalid file signature");
			}
		}
	}
}
