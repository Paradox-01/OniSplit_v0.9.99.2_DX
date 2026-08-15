using Oni.Metadata;

namespace Oni
{
	internal sealed class Template
	{
		private readonly TemplateTag tag;

		private readonly string description;

		private readonly MetaStruct type;

		private readonly long checksum;

		public TemplateTag Tag
		{
			get
			{
				return tag;
			}
		}

		public MetaStruct Type
		{
			get
			{
				return type;
			}
		}

		public long Checksum
		{
			get
			{
				return checksum;
			}
		}

		public string Description
		{
			get
			{
				return description;
			}
		}

		public bool IsLeaf
		{
			get
			{
				return type.IsLeaf;
			}
		}

		internal Template(TemplateTag tag, MetaStruct type, long checksum, string description)
		{
			this.tag = tag;
			this.type = type;
			this.checksum = checksum;
			this.description = description;
		}
	}
}
