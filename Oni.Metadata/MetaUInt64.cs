namespace Oni.Metadata
{
	internal class MetaUInt64 : MetaPrimitiveType
	{
		internal MetaUInt64()
			: base("UInt64", 8)
		{
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitUInt64(this);
		}
	}
}
