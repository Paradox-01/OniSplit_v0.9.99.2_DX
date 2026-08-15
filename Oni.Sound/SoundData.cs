using System;

namespace Oni.Sound
{
	internal class SoundData
	{
		public int SampleRate;

		public int ChannelCount;

		public byte[] Data;

		public static SoundData Read(InstanceDescriptor sndd)
		{
			if (sndd.Template.Tag != TemplateTag.SNDD)
			{
				throw new ArgumentException("descriptor");
			}
			SoundData soundData = new SoundData();
			int length;
			int offset;
			using (BinaryReader binaryReader = sndd.OpenRead())
			{
				if (sndd.IsMacFile)
				{
					soundData.ChannelCount = (binaryReader.ReadInt32() >> 1) + 1;
					soundData.SampleRate = 22050;
					binaryReader.Skip(4);
				}
				else
				{
					binaryReader.Skip(6);
					soundData.ChannelCount = binaryReader.ReadInt16();
					soundData.SampleRate = binaryReader.ReadInt32();
					binaryReader.Skip(44);
				}
				length = binaryReader.ReadInt32();
				offset = binaryReader.ReadInt32();
			}
			using (BinaryReader binaryReader2 = sndd.GetRawReader(offset))
			{
				soundData.Data = binaryReader2.ReadBytes(length);
				return soundData;
			}
		}
	}
}
