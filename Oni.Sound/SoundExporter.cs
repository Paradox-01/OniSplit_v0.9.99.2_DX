using System.Collections.Generic;

namespace Oni.Sound
{
	internal abstract class SoundExporter : Exporter
	{
		public SoundExporter(InstanceFileManager fileManager, string outputDirPath)
			: base(fileManager, outputDirPath)
		{
		}

		protected override List<InstanceDescriptor> GetSupportedDescriptors(InstanceFile file)
		{
			return file.GetNamedDescriptors(TemplateTag.SNDD);
		}
	}
}
