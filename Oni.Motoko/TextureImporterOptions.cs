using Oni.Akira;

namespace Oni.Motoko
{
	internal class TextureImporterOptions
	{
		public string Name;

		public int Width;

		public int Height;

		public TextureFormat? Format;

		public TextureFlags Flags;

		public GunkFlags GunkFlags;

		public string EnvironmentMap;

		public int Speed = 1;

		public string[] Images;
	}
}
