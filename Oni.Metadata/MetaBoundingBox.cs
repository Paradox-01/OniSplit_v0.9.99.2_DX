namespace Oni.Metadata
{
	internal class MetaBoundingBox : MetaStruct
	{
		internal MetaBoundingBox()
			: base("BoundingBox", new Field(MetaType.Vector3, "Min"), new Field(MetaType.Vector3, "Max"))
		{
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitBoundingBox(this);
		}
	}
}
