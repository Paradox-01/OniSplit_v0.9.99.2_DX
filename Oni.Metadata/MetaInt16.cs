namespace Oni.Metadata
{
	internal class MetaInt16 : MetaPrimitiveType
	{
		internal MetaInt16()
			: base("Int16", 2)
		{
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitInt16(this);
		}
	}
}
