namespace Oni.Metadata
{
	internal class MetaUInt32 : MetaPrimitiveType
	{
		internal MetaUInt32()
			: base("UInt32", 4)
		{
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitUInt32(this);
		}
	}
}
