using Oni.Imaging;

namespace Oni.Akira
{
	internal class Material
	{
		private bool isMarker;

		private string name;

		private GunkFlags flags;

		private string imageFilePath;

		private Surface image;

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public bool IsMarker
		{
			get
			{
				return isMarker;
			}
		}

		public GunkFlags Flags
		{
			get
			{
				return flags;
			}
			set
			{
				flags = value;
			}
		}

		public string ImageFilePath
		{
			get
			{
				return imageFilePath;
			}
			set
			{
				imageFilePath = value;
			}
		}

		public Surface Image
		{
			get
			{
				return image;
			}
			set
			{
				image = value;
			}
		}

		public Material(string name)
		{
			this.name = name;
		}

		public Material(string name, bool isMarker)
		{
			this.name = name;
			this.isMarker = isMarker;
		}
	}
}
