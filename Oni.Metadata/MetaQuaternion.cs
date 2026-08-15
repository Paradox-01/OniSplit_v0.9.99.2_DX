namespace Oni.Metadata
{
	internal class MetaQuaternion : MetaStruct
	{
		internal MetaQuaternion()
			: base("Quaternion", new Field(MetaType.Float, "X"), new Field(MetaType.Float, "Y"), new Field(MetaType.Float, "Z"), new Field(MetaType.Float, "W"))
		{
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitQuaternion(this);
		}
	}
}
