namespace Oni.Metadata
{
	internal class MetaMatrix4x3 : MetaStruct
	{
		internal MetaMatrix4x3()
			: base("Matrix4x3", new Field(MetaType.Float, "M11"), new Field(MetaType.Float, "M12"), new Field(MetaType.Float, "M13"), new Field(MetaType.Float, "M21"), new Field(MetaType.Float, "M22"), new Field(MetaType.Float, "M23"), new Field(MetaType.Float, "M31"), new Field(MetaType.Float, "M32"), new Field(MetaType.Float, "M33"), new Field(MetaType.Float, "M41"), new Field(MetaType.Float, "M42"), new Field(MetaType.Float, "M43"))
		{
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitMatrix4x3(this);
		}
	}
}
