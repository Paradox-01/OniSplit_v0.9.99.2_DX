namespace Oni.Metadata
{
	internal class MetaRawOffset : MetaType
	{
		internal MetaRawOffset()
			: base("RawOffset", 4)
		{
		}

		protected override bool IsLeafImpl()
		{
			return false;
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitRawOffset(this);
		}
	}
}
