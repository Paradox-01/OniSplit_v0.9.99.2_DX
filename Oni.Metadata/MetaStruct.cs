using System.Collections.Generic;

namespace Oni.Metadata
{
	internal class MetaStruct : MetaType
	{
		private readonly List<Field> fields = new List<Field>();

		private readonly MetaStruct baseStruct;

		public IEnumerable<Field> Fields
		{
			get
			{
				return fields;
			}
		}

		public MetaStruct(params Field[] declaredFields)
			: this(null, null, declaredFields)
		{
		}

		public MetaStruct(MetaStruct baseStruct, params Field[] declaredFields)
			: this(null, null, declaredFields)
		{
		}

		public MetaStruct(string name, params Field[] declaredFields)
			: this(name, null, declaredFields)
		{
		}

		public MetaStruct(string name, MetaStruct baseStruct, params Field[] declaredFields)
		{
			this.baseStruct = baseStruct;
			if (baseStruct != null)
			{
				fields.AddRange(baseStruct.fields);
			}
			fields.AddRange(declaredFields);
			int num = 0;
			foreach (Field field in fields)
			{
				num += field.Type.Size;
			}
			base.Name = name;
			base.Size = num;
		}

		protected override bool IsLeafImpl()
		{
			foreach (Field field in fields)
			{
				if (!field.Type.IsLeaf)
				{
					return false;
				}
			}
			return true;
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitStruct(this);
		}
	}
}
