namespace Oni.Metadata
{
	internal class MetaByte : MetaPrimitiveType
	{
		internal MetaByte()
			: base("UInt8", 1)
		{
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitByte(this);
		}
	}
}
