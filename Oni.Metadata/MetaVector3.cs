namespace Oni.Metadata
{
	internal class MetaVector3 : MetaStruct
	{
		internal MetaVector3()
			: base("Vector3", new Field(MetaType.Float, "X"), new Field(MetaType.Float, "Y"), new Field(MetaType.Float, "Z"))
		{
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitVector3(this);
		}
	}
}
