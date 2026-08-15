using System.Collections.Generic;

namespace Oni
{
	internal sealed class DatUnpacker : Exporter
	{
		public DatUnpacker(InstanceFileManager fileManager, string outputDirPath)
			: base(fileManager, outputDirPath)
		{
		}

		protected override void ExportInstance(InstanceDescriptor descriptor)
		{
			List<InstanceDescriptor> referencedDescriptors = descriptor.GetReferencedDescriptors();
			InstanceFileWriter instanceFileWriter = InstanceFileWriter.CreateV32(referencedDescriptors);
			instanceFileWriter.Write(CreateFileName(descriptor, ".oni"));
		}
	}
}
