namespace Oni.Metadata
{
	internal class MetaInt32 : MetaPrimitiveType
	{
		internal MetaInt32()
			: base("Int32", 4)
		{
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitInt32(this);
		}
	}
}
