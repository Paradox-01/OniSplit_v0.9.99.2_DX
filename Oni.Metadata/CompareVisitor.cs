namespace Oni.Metadata
{
	internal class CompareVisitor : MetaTypeVisitor
	{
		private readonly BinaryReader reader1;

		private readonly BinaryReader reader2;

		private bool equals = true;

		public bool AreEquals
		{
			get
			{
				return equals;
			}
		}

		public CompareVisitor(BinaryReader reader1, BinaryReader reader2)
		{
			this.reader1 = reader1;
			this.reader2 = reader2;
		}

		public override void VisitByte(MetaByte type)
		{
			equals = reader1.ReadByte() == reader2.ReadByte();
		}

		public override void VisitInt16(MetaInt16 type)
		{
			equals = reader1.ReadInt16() == reader2.ReadInt16();
		}

		public override void VisitInt32(MetaInt32 type)
		{
			equals = reader1.ReadInt32() == reader2.ReadInt32();
		}

		public override void VisitUInt32(MetaUInt32 type)
		{
			equals = reader1.ReadUInt32() == reader2.ReadUInt32();
		}

		public override void VisitInt64(MetaInt64 type)
		{
			equals = reader1.ReadInt64() == reader2.ReadInt64();
		}

		public override void VisitUInt64(MetaUInt64 type)
		{
			equals = reader1.ReadUInt64() == reader2.ReadUInt64();
		}

		public override void VisitFloat(MetaFloat type)
		{
			equals = reader1.ReadSingle() == reader2.ReadSingle();
		}

		public override void VisitColor(MetaColor type)
		{
			equals = reader1.ReadInt32() == reader2.ReadInt32();
		}

		public override void VisitRawOffset(MetaRawOffset type)
		{
			equals = reader1.ReadInt32() == reader2.ReadInt32();
		}

		public override void VisitSepOffset(MetaSepOffset type)
		{
			equals = reader1.ReadInt32() == reader2.ReadInt32();
		}

		public override void VisitPointer(MetaPointer type)
		{
			equals = reader1.ReadInt32() == reader2.ReadInt32();
		}

		public override void VisitString(MetaString type)
		{
			equals = reader1.ReadString(type.Count) == reader2.ReadString(type.Count);
		}

		public override void VisitPadding(MetaPadding type)
		{
			reader1.Skip(type.Count);
			reader2.Skip(type.Count);
		}

		public override void VisitStruct(MetaStruct type)
		{
			foreach (Field field in type.Fields)
			{
				field.Type.Accept(this);
				if (!equals)
				{
					break;
				}
			}
		}

		public override void VisitArray(MetaArray type)
		{
			CompareArray(type.ElementType, type.Count);
		}

		public override void VisitVarArray(MetaVarArray type)
		{
			int num;
			int num2;
			if (type.CountField.Type == MetaType.Int16)
			{
				num = reader1.ReadInt16();
				num2 = reader2.ReadInt16();
			}
			else
			{
				num = reader1.ReadInt32();
				num2 = reader2.ReadInt32();
			}
			equals = num == num2;
			if (equals)
			{
				CompareArray(type.ElementType, num);
			}
		}

		private void CompareArray(MetaType elementType, int count)
		{
			for (int i = 0; i < count; i++)
			{
				elementType.Accept(this);
				if (!equals)
				{
					break;
				}
			}
		}
	}
}
