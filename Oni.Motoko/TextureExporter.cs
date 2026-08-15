using System;
using System.Collections.Generic;
using Oni.Imaging;

namespace Oni.Motoko
{
	internal class TextureExporter : Exporter
	{
		protected string fileType;

		public TextureExporter(InstanceFileManager fileManager, string outputDirPath, string fileType)
			: base(fileManager, outputDirPath)
		{
			this.fileType = fileType;
		}

		protected override List<InstanceDescriptor> GetSupportedDescriptors(InstanceFile file)
		{
			return file.GetNamedDescriptors(TemplateTag.TXMP);
		}

		protected override void ExportInstance(InstanceDescriptor descriptor)
		{
			Texture texture = TextureDatReader.Read(descriptor);
			string filePath = CreateFileName(descriptor, "." + fileType);
			switch (fileType)
			{
			case "tga":
				TgaWriter.Write(texture.Surfaces[0], filePath);
				break;
			case "dds":
				DdsWriter.Write(texture.Surfaces, filePath);
				break;
			case "png":
			case "jpg":
			case "bmp":
			case "tif":
				SysWriter.Write(texture.Surfaces[0], filePath);
				break;
			default:
				throw new NotSupportedException(string.Format("Extracting textures as '{0}' is not supported", fileType));
			}
		}
	}
}
