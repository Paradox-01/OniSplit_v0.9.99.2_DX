namespace Oni.Metadata
{
	internal class MetaColor : MetaType
	{
		internal MetaColor()
			: base("Color", 4)
		{
		}

		protected override bool IsLeafImpl()
		{
			return true;
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitColor(this);
		}
	}
}
