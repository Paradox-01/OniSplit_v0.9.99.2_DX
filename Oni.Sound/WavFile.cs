using System.IO;

namespace Oni.Sound
{
	internal class WavFile
	{
		private const int fcc_RIFF = 1179011410;

		private const int fcc_WAVE = 1163280727;

		private const int fcc_fmt = 544501094;

		private const int fcc_data = 1635017060;

		private WavFormat format;

		private int channelCount;

		private int sampleRate;

		private int averageBytesPerSecond;

		private int blockAlign;

		private int bitsPerSample;

		private byte[] extraData;

		private byte[] soundData;

		public WavFormat Format
		{
			get
			{
				return format;
			}
		}

		public int ChannelCount
		{
			get
			{
				return channelCount;
			}
		}

		public int SampleRate
		{
			get
			{
				return sampleRate;
			}
		}

		public int AverageBytesPerSecond
		{
			get
			{
				return averageBytesPerSecond;
			}
		}

		public int BlockAlign
		{
			get
			{
				return blockAlign;
			}
		}

		public int BitsPerSample
		{
			get
			{
				return bitsPerSample;
			}
		}

		public byte[] ExtraData
		{
			get
			{
				return extraData;
			}
		}

		public byte[] SoundData
		{
			get
			{
				return soundData;
			}
		}

		public static WavFile FromFile(string filePath)
		{
			using (BinaryReader binaryReader = new BinaryReader(filePath))
			{
				if (binaryReader.ReadInt32() != 1179011410)
				{
					throw new InvalidDataException("Not a WAV file");
				}
				int num = binaryReader.ReadInt32();
				if (binaryReader.ReadInt32() != 1163280727)
				{
					throw new InvalidDataException("Not a WAV file");
				}
				WavFile wavFile = new WavFile();
				while (binaryReader.Position < num)
				{
					int num2 = binaryReader.ReadInt32();
					int num3 = binaryReader.ReadInt32();
					int position = binaryReader.Position;
					switch (num2)
					{
					case 544501094:
						wavFile.ReadFormatChunk(binaryReader, num3);
						break;
					case 1635017060:
						wavFile.ReadDataChunk(binaryReader, num3);
						break;
					}
					binaryReader.Position = position + num3;
				}
				return wavFile;
			}
		}

		private void ReadFormatChunk(BinaryReader reader, int chunkSize)
		{
			format = (WavFormat)reader.ReadInt16();
			channelCount = reader.ReadInt16();
			sampleRate = reader.ReadInt32();
			averageBytesPerSecond = reader.ReadInt32();
			blockAlign = reader.ReadInt16();
			bitsPerSample = reader.ReadInt16();
			if (chunkSize > 16)
			{
				extraData = reader.ReadBytes(reader.ReadInt16());
			}
			else
			{
				extraData = new byte[0];
			}
		}

		private void ReadDataChunk(BinaryReader reader, int chunkSize)
		{
			soundData = reader.ReadBytes(chunkSize);
		}
	}
}
