namespace Oni.Metadata
{
	internal class MetaPointer : MetaType
	{
		private readonly TemplateTag tag;

		public TemplateTag Tag
		{
			get
			{
				return tag;
			}
		}

		internal MetaPointer(TemplateTag tag)
			: base("Link", 4)
		{
			this.tag = tag;
		}

		protected override bool IsLeafImpl()
		{
			return false;
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitPointer(this);
		}
	}
}
