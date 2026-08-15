using System;

namespace Oni.Metadata
{
	internal class BinaryPartField : Field
	{
		private string sizeFieldName;

		private int sizeMultiplier;

		private MetaType rawType;

		public string SizeFieldName
		{
			get
			{
				return sizeFieldName;
			}
		}

		public int SizeMultiplier
		{
			get
			{
				return sizeMultiplier;
			}
		}

		public MetaType RawType
		{
			get
			{
				return rawType;
			}
		}

		public BinaryPartField(MetaType offsetType, string name)
			: this(offsetType, name, null, 0)
		{
		}

		public BinaryPartField(MetaType offsetType, string name, string sizeFieldName)
			: this(offsetType, name, sizeFieldName, 1)
		{
		}

		public BinaryPartField(MetaType offsetType, string name, int size)
			: this(offsetType, name, null, size)
		{
		}

		public BinaryPartField(MetaType offsetType, string name, int size, MetaType rawType)
			: this(offsetType, name, null, size, rawType)
		{
		}

		public BinaryPartField(MetaType offsetType, string name, string sizeFieldName, int sizeMultiplier)
			: this(offsetType, name, sizeFieldName, sizeMultiplier, null)
		{
		}

		public BinaryPartField(MetaType offsetType, string name, string sizeFieldName, int sizeMultiplier, MetaType rawType)
			: base(offsetType, name)
		{
			if (offsetType != MetaType.RawOffset && offsetType != MetaType.SepOffset)
			{
				throw new ArgumentException("Offset type can only be RawOffset or SepOffset", "offsetType");
			}
			this.sizeFieldName = sizeFieldName;
			this.sizeMultiplier = sizeMultiplier;
			this.rawType = rawType;
		}
	}
}
