using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Oni
{
	internal sealed class SubtitleExporter : Exporter
	{
		public SubtitleExporter(InstanceFileManager fileManager, string outputDirPath)
			: base(fileManager, outputDirPath)
		{
		}

		protected override List<InstanceDescriptor> GetSupportedDescriptors(InstanceFile file)
		{
			return file.GetNamedDescriptors(TemplateTag.SUBT);
		}

		protected override void ExportInstance(InstanceDescriptor descriptor)
		{
			string path = Path.Combine(base.OutputDirPath, descriptor.FullName + ".txt");
			int offset;
			int[] array;
			using (BinaryReader binaryReader = descriptor.OpenRead(16))
			{
				offset = binaryReader.ReadInt32();
				array = binaryReader.ReadInt32Array(binaryReader.ReadInt32());
			}
			using (BinaryReader binaryReader2 = descriptor.GetRawReader(offset))
			{
				using (FileStream stream = File.Create(path))
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(stream))
					{
						int position = binaryReader2.Position;
						List<byte> list = new List<byte>();
						int[] array2 = array;
						foreach (int num in array2)
						{
							binaryReader2.Position = position + num;
							while (true)
							{
								byte b = binaryReader2.ReadByte();
								if (b == 0)
								{
									break;
								}
								list.Add(b);
							}
							list.Add(61);
							binaryWriter.Write(list.ToArray());
							list.Clear();
							while (true)
							{
								byte b2 = binaryReader2.ReadByte();
								if (b2 == 0)
								{
									break;
								}
								list.Add(b2);
							}
							list.AddRange(Encoding.UTF8.GetBytes(Environment.NewLine));
							binaryWriter.Write(list.ToArray());
							list.Clear();
						}
					}
				}
			}
		}
	}
}
