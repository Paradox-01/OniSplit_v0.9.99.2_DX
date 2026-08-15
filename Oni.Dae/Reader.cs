using System;
using System.IO;
using Oni.Dae.IO;

namespace Oni.Dae
{
	internal class Reader
	{
		public static Scene ReadFile(string filePath, bool disableAxisConversion = false)
		{
			string extension = Path.GetExtension(filePath);
			if (string.Equals(extension, ".dae", StringComparison.OrdinalIgnoreCase))
			{
				return DaeReader.ReadFile(filePath);
			}
			if (string.Equals(extension, ".obj", StringComparison.OrdinalIgnoreCase))
			{
				return ObjReader.ReadFile(filePath);
			}
			throw new NotSupportedException("Unsupported 3D file type " + extension);
		}
	}
}
