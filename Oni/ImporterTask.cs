namespace Oni
{
	internal struct ImporterTask
	{
		private readonly string filePath;

		private readonly TemplateTag type;

		public string FilePath
		{
			get
			{
				return filePath;
			}
		}

		public TemplateTag Type
		{
			get
			{
				return type;
			}
		}

		public ImporterTask(string filePath)
			: this(filePath, TemplateTag.NONE)
		{
		}

		public ImporterTask(string filePath, TemplateTag type)
		{
			this.filePath = filePath;
			this.type = type;
		}
	}
}
