namespace Oni.Metadata
{
	internal class MetaPlane : MetaStruct
	{
		internal MetaPlane()
			: base("Plane", new Field(MetaType.Vector3, "Normal"), new Field(MetaType.Float, "D"))
		{
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitPlane(this);
		}
	}
}
