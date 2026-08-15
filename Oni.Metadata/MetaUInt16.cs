namespace Oni.Metadata
{
	internal class MetaUInt16 : MetaPrimitiveType
	{
		internal MetaUInt16()
			: base("UInt16", 2)
		{
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitUInt16(this);
		}
	}
}
