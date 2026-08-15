namespace Oni.Metadata
{
	internal class MetaInt64 : MetaPrimitiveType
	{
		internal MetaInt64()
			: base("Int64", 8)
		{
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitInt64(this);
		}
	}
}
