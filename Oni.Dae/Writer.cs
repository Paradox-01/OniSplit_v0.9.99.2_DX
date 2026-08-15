using System;
using System.IO;
using Oni.Dae.IO;

namespace Oni.Dae
{
	internal class Writer
	{
		public static void WriteFile(string filePath, Scene scene)
		{
			string extension = Path.GetExtension(filePath);
			if (string.Equals(extension, ".dae", StringComparison.OrdinalIgnoreCase))
			{
				DaeWriter.WriteFile(filePath, scene);
				return;
			}
			if (string.Equals(extension, ".obj", StringComparison.OrdinalIgnoreCase))
			{
				ObjWriter.WriteFile(filePath, scene);
				return;
			}
			throw new NotSupportedException(string.Format("Unsupported 3D file type {0}", extension));
		}
	}
}
