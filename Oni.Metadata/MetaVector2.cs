namespace Oni.Metadata
{
	internal class MetaVector2 : MetaStruct
	{
		internal MetaVector2()
			: base("Vector2", new Field(MetaType.Float, "X"), new Field(MetaType.Float, "Y"))
		{
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitVector2(this);
		}
	}
}
