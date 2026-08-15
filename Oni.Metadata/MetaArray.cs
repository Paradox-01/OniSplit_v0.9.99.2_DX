using System.Globalization;

namespace Oni.Metadata
{
	internal class MetaArray : MetaType
	{
		private readonly MetaType elementType;

		private readonly int count;

		public MetaType ElementType
		{
			get
			{
				return elementType;
			}
		}

		public int Count
		{
			get
			{
				return count;
			}
		}

		public MetaArray(MetaType elementType, int count)
		{
			this.elementType = elementType;
			this.count = count;
			base.Name = string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", new object[2] { elementType.Name, count });
			base.Size = elementType.Size * count;
		}

		protected override bool IsLeafImpl()
		{
			return elementType.IsLeaf;
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitArray(this);
		}
	}
}
