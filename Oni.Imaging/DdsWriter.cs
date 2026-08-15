using System.Collections.Generic;
using System.IO;

namespace Oni.Imaging
{
	internal class DdsWriter
	{
		public static void Write(IList<Surface> surfaces, string filePath)
		{
			using (FileStream stream = File.Create(filePath))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(stream))
				{
					DdsHeader ddsHeader = DdsHeader.Create(surfaces);
					ddsHeader.Write(binaryWriter);
					foreach (Surface surface in surfaces)
					{
						binaryWriter.Write(surface.Data);
					}
				}
			}
		}
	}
}
