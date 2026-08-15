namespace Oni.Metadata
{
	internal class MetaString : MetaArray
	{
		public MetaString(int length)
			: base(MetaType.Char, length)
		{
			base.Name = "String";
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitString(this);
		}
	}
}
