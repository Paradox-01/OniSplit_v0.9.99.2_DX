using System.IO;

namespace Oni.Sound
{
	internal class AifFile
	{
		private const int fcc_FORM = 1179603533;

		private const int fcc_AIFC = 1095321155;

		private const int fcc_COMM = 1129270605;

		private const int fcc_SSND = 1397968452;

		private int channelCount;

		private int numSampleFrames;

		private int sampleSize;

		private byte[] sampleRate;

		private int format;

		private byte[] soundData;

		public int ChannelCount
		{
			get
			{
				return channelCount;
			}
		}

		public int SampleFrames
		{
			get
			{
				return numSampleFrames;
			}
		}

		public int SampleSize
		{
			get
			{
				return sampleSize;
			}
		}

		public byte[] SampleRate
		{
			get
			{
				return sampleRate;
			}
		}

		public int Format
		{
			get
			{
				return format;
			}
		}

		public byte[] SoundData
		{
			get
			{
				return soundData;
			}
		}

		public static AifFile FromFile(string filePath)
		{
			using (BinaryReader binaryReader = new BinaryReader(filePath, true))
			{
				AifFile aifFile = new AifFile();
				if (binaryReader.ReadInt32() != 1179603533)
				{
					throw new InvalidDataException("Not an AIF file");
				}
				int num = binaryReader.ReadInt32();
				if (binaryReader.ReadInt32() != 1095321155)
				{
					throw new InvalidDataException("Not a compressed AIF file");
				}
				while (binaryReader.Position < num)
				{
					int num2 = binaryReader.ReadInt32();
					int num3 = binaryReader.ReadInt32();
					int position = binaryReader.Position;
					switch (num2)
					{
					case 1129270605:
						aifFile.ReadFormatChunk(binaryReader, num3);
						break;
					case 1397968452:
						aifFile.ReadDataChunk(binaryReader, num3);
						break;
					}
					binaryReader.Position = position + num3;
				}
				return aifFile;
			}
		}

		private void ReadFormatChunk(BinaryReader reader, int chunkSize)
		{
			channelCount = reader.ReadInt16();
			numSampleFrames = reader.ReadInt32();
			sampleSize = reader.ReadInt16();
			sampleRate = reader.ReadBytes(10);
			format = reader.ReadInt32();
		}

		private void ReadDataChunk(BinaryReader reader, int chunkSize)
		{
			reader.Position += 8;
			soundData = reader.ReadBytes(chunkSize - 8);
		}
	}
}
