namespace Oni.Dae
{
	internal class Image : Entity
	{
		public string FilePath { get; set; }

		public Image()
		{
		}

		public Image(string filePath)
		{
			FilePath = filePath;
		}
	}
}
