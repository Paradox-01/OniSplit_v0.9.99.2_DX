using System;

namespace Oni.Metadata
{
	internal class MetaVarArray : MetaType
	{
		private readonly MetaType elementType;

		private readonly Field lengthField;

		public Field CountField
		{
			get
			{
				return lengthField;
			}
		}

		public MetaType ElementType
		{
			get
			{
				return elementType;
			}
		}

		public MetaVarArray(MetaType lengthType, MetaType elementType)
		{
			if (lengthType != MetaType.Int16 && lengthType != MetaType.Int32)
			{
				throw new ArgumentException("lengthType must be Int16 or Int32", "lengthType");
			}
			lengthField = new Field(lengthType, "Length");
			this.elementType = elementType;
			base.Name = string.Format("{0}[{1}]", elementType.Name, lengthType.Name);
			base.Size = lengthType.Size;
		}

		protected override bool IsLeafImpl()
		{
			return elementType.IsLeaf;
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitVarArray(this);
		}
	}
}
