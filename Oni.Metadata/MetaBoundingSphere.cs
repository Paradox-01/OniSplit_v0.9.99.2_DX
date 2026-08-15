namespace Oni.Metadata
{
	internal class MetaBoundingSphere : MetaStruct
	{
		internal MetaBoundingSphere()
			: base("BoundingSphere", new Field(MetaType.Vector3, "Center"), new Field(MetaType.Float, "Radius"))
		{
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitBoundingSphere(this);
		}
	}
}
