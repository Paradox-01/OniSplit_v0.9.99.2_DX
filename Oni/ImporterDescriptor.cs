namespace Oni
{
	internal abstract class ImporterDescriptor
	{
		private readonly ImporterFile file;

		private readonly TemplateTag tag;

		private readonly int index;

		private readonly string name;

		public ImporterFile File
		{
			get
			{
				return file;
			}
		}

		public TemplateTag Tag
		{
			get
			{
				return tag;
			}
		}

		public int Index
		{
			get
			{
				return index;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
		}

		protected ImporterDescriptor(ImporterFile file, TemplateTag tag, int index, string name)
		{
			this.file = file;
			this.tag = tag;
			this.index = index;
			this.name = name;
		}

		public abstract BinaryWriter OpenWrite();

		public abstract BinaryWriter OpenWrite(int offset);
	}
}
