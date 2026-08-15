using System;
using System.Globalization;
using Oni.Dae;

namespace Oni.Totoro
{
	internal class BodyDaeImporter
	{
		private readonly bool generateNormals;

		private readonly bool flatNormals;

		private readonly float shellOffset;

		public BodyDaeImporter(string[] args)
		{
			foreach (string text in args)
			{
				switch (text)
				{
				case "-normals":
					generateNormals = true;
					continue;
				case "-flat":
					flatNormals = true;
					continue;
				default:
					if (!text.StartsWith("-cel:", StringComparison.Ordinal))
					{
						continue;
					}
					break;
				case "-cel":
					break;
				}
				int num = text.IndexOf(':');
				if (num != -1)
				{
					shellOffset = float.Parse(text.Substring(num + 1), CultureInfo.InvariantCulture);
				}
				else
				{
					shellOffset = 0.07f;
				}
			}
		}

		public ImporterDescriptor Import(string filePath, ImporterFile importer)
		{
			Scene scene = Reader.ReadFile(filePath);
			FaceConverter.Triangulate(scene);
			Body body = BodyDaeReader.Read(scene, generateNormals, flatNormals, shellOffset);
			return BodyDatWriter.Write(body, importer);
		}
	}
}
