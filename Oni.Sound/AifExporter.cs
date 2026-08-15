using System.IO;

namespace Oni.Sound
{
	internal class AifExporter : SoundExporter
	{
		private const int fcc_FORM = 1179603533;

		private const int fcc_AIFC = 1095321155;

		private const int fcc_COMM = 1129270605;

		private const int fcc_ima4 = 1768775988;

		private const int fcc_SSND = 1397968452;

		private static readonly byte[] sampleRate = new byte[10] { 64, 13, 172, 68, 0, 0, 0, 0, 0, 0 };

		public AifExporter(InstanceFileManager fileManager, string outputDirPath)
			: base(fileManager, outputDirPath)
		{
		}

		protected override void ExportInstance(InstanceDescriptor descriptor)
		{
			SoundData soundData = SoundData.Read(descriptor);
			using (FileStream stream = File.Create(Path.Combine(base.OutputDirPath, descriptor.FullName + ".aif")))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(stream))
				{
					binaryWriter.Write(Utils.ByteSwap(1179603533));
					binaryWriter.Write(Utils.ByteSwap(50 + soundData.Data.Length));
					binaryWriter.Write(Utils.ByteSwap(1095321155));
					binaryWriter.Write(Utils.ByteSwap(1129270605));
					binaryWriter.Write(Utils.ByteSwap(22));
					binaryWriter.Write(Utils.ByteSwap((short)soundData.ChannelCount));
					binaryWriter.Write(Utils.ByteSwap(soundData.Data.Length / (soundData.ChannelCount * 34)));
					binaryWriter.Write(Utils.ByteSwap((short)16));
					binaryWriter.Write(sampleRate);
					binaryWriter.Write(Utils.ByteSwap(1768775988));
					binaryWriter.Write(Utils.ByteSwap(1397968452));
					binaryWriter.Write(Utils.ByteSwap(8 + soundData.Data.Length));
					binaryWriter.Write(0);
					binaryWriter.Write(0);
					binaryWriter.Write(soundData.Data);
				}
			}
		}
	}
}
