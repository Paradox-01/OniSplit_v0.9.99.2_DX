using System;
using System.IO;

namespace Oni.Sound
{
	internal class WavExporter : SoundExporter
	{
		private const int fcc_RIFF = 1179011410;

		private const int fcc_WAVE = 1163280727;

		private const int fcc_fmt = 544501094;

		private const int fcc_data = 1635017060;

		private static readonly byte[] formatTemplate = new byte[50]
		{
			2, 0, 0, 0, 34, 86, 0, 0, 0, 0,
			0, 0, 0, 2, 4, 0, 32, 0, 244, 3,
			7, 0, 0, 1, 0, 0, 0, 2, 0, 255,
			0, 0, 0, 0, 192, 0, 64, 0, 240, 0,
			0, 0, 204, 1, 48, 255, 136, 1, 24, 255
		};

		public WavExporter(InstanceFileManager fileManager, string outputDirPath)
			: base(fileManager, outputDirPath)
		{
		}

		protected override void ExportInstance(InstanceDescriptor descriptor)
		{
			SoundData soundData = SoundData.Read(descriptor);
			using (FileStream stream = File.Create(Path.Combine(base.OutputDirPath, descriptor.FullName + ".wav")))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(stream))
				{
					byte[] array = (byte[])formatTemplate.Clone();
					int num = 512 * soundData.ChannelCount * soundData.SampleRate / 22050;
					int num2 = 2 + (num - soundData.ChannelCount * 7) * 8 / soundData.ChannelCount / 4;
					int value = soundData.SampleRate * num / num2;
					Array.Copy(BitConverter.GetBytes(soundData.ChannelCount), 0, array, 2, 2);
					Array.Copy(BitConverter.GetBytes(soundData.SampleRate), 0, array, 4, 4);
					Array.Copy(BitConverter.GetBytes(value), 0, array, 8, 4);
					Array.Copy(BitConverter.GetBytes(num), 0, array, 12, 2);
					Array.Copy(BitConverter.GetBytes(num2), 0, array, 18, 2);
					binaryWriter.Write(1179011410);
					binaryWriter.Write(8 + array.Length + 8 + soundData.Data.Length);
					binaryWriter.Write(1163280727);
					binaryWriter.Write(544501094);
					binaryWriter.Write(array.Length);
					binaryWriter.Write(array);
					binaryWriter.Write(1635017060);
					binaryWriter.Write(soundData.Data.Length);
					binaryWriter.Write(soundData.Data);
				}
			}
		}
	}
}
