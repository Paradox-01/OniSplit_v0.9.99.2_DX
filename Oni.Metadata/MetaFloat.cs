namespace Oni.Metadata
{
	internal class MetaFloat : MetaPrimitiveType
	{
		internal MetaFloat()
			: base("Float", 4)
		{
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitFloat(this);
		}
	}
}
