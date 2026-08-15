namespace Oni.Metadata
{
	internal class MetaSepOffset : MetaType
	{
		internal MetaSepOffset()
			: base("SepOffset", 4)
		{
		}

		protected override bool IsLeafImpl()
		{
			return false;
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitSepOffset(this);
		}
	}
}
