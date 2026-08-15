using System.Collections.Generic;

namespace Oni.Metadata
{
	internal class LinkVisitor : MetaTypeVisitor
	{
		private readonly List<int> links = new List<int>();

		private readonly BinaryReader reader;

		public ICollection<int> Links
		{
			get
			{
				return links;
			}
		}

		public LinkVisitor(BinaryReader reader)
		{
			this.reader = reader;
		}

		public override void VisitByte(MetaByte type)
		{
			reader.Position += type.Size;
		}

		public override void VisitInt16(MetaInt16 type)
		{
			reader.Position += type.Size;
		}

		public override void VisitUInt16(MetaUInt16 type)
		{
			reader.Position += type.Size;
		}

		public override void VisitInt32(MetaInt32 type)
		{
			reader.Position += type.Size;
		}

		public override void VisitUInt32(MetaUInt32 type)
		{
			reader.Position += type.Size;
		}

		public override void VisitInt64(MetaInt64 type)
		{
			reader.Position += type.Size;
		}

		public override void VisitUInt64(MetaUInt64 type)
		{
			reader.Position += type.Size;
		}

		public override void VisitFloat(MetaFloat type)
		{
			reader.Position += type.Size;
		}

		public override void VisitColor(MetaColor type)
		{
			reader.Position += type.Size;
		}

		public override void VisitRawOffset(MetaRawOffset type)
		{
			reader.Position += type.Size;
		}

		public override void VisitSepOffset(MetaSepOffset type)
		{
			reader.Position += type.Size;
		}

		public override void VisitPointer(MetaPointer type)
		{
			int num = reader.ReadInt32();
			if (num != 0)
			{
				links.Add(num);
			}
		}

		public override void VisitString(MetaString type)
		{
			reader.Position += type.Size;
		}

		public override void VisitPadding(MetaPadding type)
		{
			reader.Position += type.Size;
		}

		public override void VisitStruct(MetaStruct type)
		{
			foreach (Field field in type.Fields)
			{
				field.Type.Accept(this);
			}
		}

		public override void VisitArray(MetaArray type)
		{
			VisitArray(type.ElementType, type.Count);
		}

		public override void VisitVarArray(MetaVarArray type)
		{
			VisitArray(count: (type.CountField.Type != MetaType.Int16) ? reader.ReadInt32() : reader.ReadUInt16(), elementType: type.ElementType);
		}

		private void VisitArray(MetaType elementType, int count)
		{
			if (elementType is MetaPointer || elementType is MetaStruct)
			{
				for (int i = 0; i < count; i++)
				{
					elementType.Accept(this);
				}
			}
			else
			{
				reader.Position += elementType.Size * count;
			}
		}
	}
}
