namespace Oni.Metadata
{
	internal class MetaPadding : MetaArray
	{
		private readonly byte fillByte;

		public byte FillByte
		{
			get
			{
				return fillByte;
			}
		}

		public MetaPadding(int length)
			: this(length, 0)
		{
		}

		public MetaPadding(int length, byte fillByte)
			: base(MetaType.Byte, length)
		{
			this.fillByte = fillByte;
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitPadding(this);
		}
	}
}
