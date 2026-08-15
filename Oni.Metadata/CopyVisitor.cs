using System;
using System.Collections.Generic;
using System.IO;

namespace Oni.Metadata
{
	internal class CopyVisitor : MetaTypeVisitor
	{
		private class ActiveField
		{
			public Field Field;

			public int Index;

			public ActiveField(Field field)
			{
				Field = field;
				Index = -1;
			}
		}

		private BinaryReader input;

		private BinaryWriter output;

		private Action<CopyVisitor> callback;

		private Stack<ActiveField> activeFields;

		private MetaType topLevelType;

		private MetaType currentType;

		private byte[] buffer;

		private int fieldSize;

		private int position;

		public MetaType TopLevelType
		{
			get
			{
				return topLevelType;
			}
		}

		public MetaType Type
		{
			get
			{
				return currentType;
			}
		}

		public Field Field
		{
			get
			{
				if (activeFields.Count == 0)
				{
					return null;
				}
				return activeFields.Peek().Field;
			}
		}

		public int Position
		{
			get
			{
				return position;
			}
		}

		public CopyVisitor(BinaryReader reader, BinaryWriter writer, Action<CopyVisitor> callback)
		{
			input = reader;
			output = writer;
			this.callback = callback;
			activeFields = new Stack<ActiveField>();
		}

		public byte GetByte()
		{
			return buffer[0];
		}

		public short GetInt16()
		{
			return (short)(buffer[0] | (buffer[1] << 8));
		}

		public ushort GetUInt16()
		{
			return (ushort)(buffer[0] | (buffer[1] << 8));
		}

		public int GetInt32()
		{
			return buffer[0] | (buffer[1] << 8) | (buffer[2] << 16) | (buffer[3] << 24);
		}

		public uint GetUInt32()
		{
			return (uint)(buffer[0] | (buffer[1] << 8) | (buffer[2] << 16) | (buffer[3] << 24));
		}

		public void SetInt32(int value)
		{
			buffer[0] = (byte)value;
			buffer[1] = (byte)(value >> 8);
			buffer[2] = (byte)(value >> 16);
			buffer[3] = (byte)(value >> 24);
		}

		public string GetCurrentFieldName()
		{
			if (activeFields.Count == 0)
			{
				return null;
			}
			List<string> list = new List<string>();
			foreach (ActiveField activeField in activeFields)
			{
				if (string.IsNullOrEmpty(activeField.Field.Name))
				{
					return null;
				}
				if (activeField.Index >= 0)
				{
					list.Add(string.Format("{0}[{1}]", activeField.Field.Name, activeField.Index));
				}
				else
				{
					list.Add(activeField.Field.Name);
				}
			}
			list.Add(topLevelType.Name);
			list.Reverse();
			return string.Join(".", list.ToArray());
		}

		public string GetParentFieldName()
		{
			if (activeFields.Count == 0)
			{
				return null;
			}
			List<string> list = new List<string>();
			foreach (ActiveField activeField in activeFields)
			{
				if (string.IsNullOrEmpty(activeField.Field.Name))
				{
					return null;
				}
				if (activeField.Index >= 0)
				{
					list.Add(string.Format("{0}[{1}]", activeField.Field.Name, activeField.Index));
				}
				else
				{
					list.Add(activeField.Field.Name);
				}
			}
			list.Add(topLevelType.Name);
			list.Reverse();
			list.RemoveAt(list.Count - 1);
			return string.Join(".", list.ToArray());
		}

		public override void VisitByte(MetaByte type)
		{
			CopyBytes(type);
		}

		public override void VisitInt16(MetaInt16 type)
		{
			CopyBytes(type);
		}

		public override void VisitUInt16(MetaUInt16 type)
		{
			CopyBytes(type);
		}

		public override void VisitInt32(MetaInt32 type)
		{
			CopyBytes(type);
		}

		public override void VisitUInt32(MetaUInt32 type)
		{
			CopyBytes(type);
		}

		public override void VisitInt64(MetaInt64 type)
		{
			CopyBytes(type);
		}

		public override void VisitUInt64(MetaUInt64 type)
		{
			CopyBytes(type);
		}

		public override void VisitFloat(MetaFloat type)
		{
			CopyBytes(type);
		}

		public override void VisitColor(MetaColor type)
		{
			CopyBytes(type);
		}

		public override void VisitRawOffset(MetaRawOffset type)
		{
			CopyBytes(type);
		}

		public override void VisitSepOffset(MetaSepOffset type)
		{
			CopyBytes(type);
		}

		public override void VisitPointer(MetaPointer type)
		{
			CopyBytes(type);
		}

		public override void VisitBoundingBox(MetaBoundingBox type)
		{
			CopyBytes(type);
		}

		public override void VisitBoundingSphere(MetaBoundingSphere type)
		{
			CopyBytes(type);
		}

		public override void VisitMatrix4x3(MetaMatrix4x3 type)
		{
			CopyBytes(type);
		}

		public override void VisitPlane(MetaPlane type)
		{
			CopyBytes(type);
		}

		public override void VisitQuaternion(MetaQuaternion type)
		{
			CopyBytes(type);
		}

		public override void VisitVector2(MetaVector2 type)
		{
			CopyBytes(type);
		}

		public override void VisitVector3(MetaVector3 type)
		{
			CopyBytes(type);
		}

		private void CopyBytes(MetaType type)
		{
			BeginCopy(type, 1);
			EndCopy();
		}

		public override void VisitString(MetaString type)
		{
			BeginCopy(type, 1);
			bool flag = false;
			for (int i = 0; i < type.Size; i++)
			{
				if (flag)
				{
					buffer[i] = 0;
				}
				else if (buffer[i] == 0)
				{
					flag = true;
				}
			}
			EndCopy();
		}

		public override void VisitPadding(MetaPadding type)
		{
			BeginCopy(type, 1);
			if (type.FillByte == 0)
			{
				Array.Clear(buffer, 0, type.Size);
			}
			else
			{
				for (int i = 0; i < type.Size; i++)
				{
					buffer[i] = type.FillByte;
				}
			}
			EndCopy();
		}

		public override void VisitStruct(MetaStruct type)
		{
			if (topLevelType == null)
			{
				topLevelType = type;
			}
			foreach (Field field in type.Fields)
			{
				BeginCopyField(field);
				field.Type.Accept(this);
				EndCopyField(field);
			}
		}

		internal void BeginCopyField(Field field)
		{
			activeFields.Push(new ActiveField(field));
		}

		internal void EndCopyField(Field field)
		{
			if (activeFields.Peek().Field != field)
			{
				throw new InvalidOperationException();
			}
			activeFields.Pop();
		}

		public override void VisitArray(MetaArray type)
		{
			CopyArray(type.ElementType, type.Count);
		}

		public override void VisitVarArray(MetaVarArray type)
		{
			BeginCopyField(type.CountField);
			type.CountField.Type.Accept(this);
			EndCopyField(type.CountField);
			int num = ((type.CountField.Type != MetaType.Int16) ? GetInt32() : GetInt16());
			if (num < 0)
			{
				throw new InvalidDataException(string.Format("Invalid array length: 0x{0:x} at offset 0x{1:x}", num, position));
			}
			CopyArray(type.ElementType, num);
		}

		private void CopyArray(MetaType elementType, int count)
		{
			if (elementType.IsBlittable)
			{
				BeginCopy(elementType, count);
				EndCopy();
				return;
			}
			for (int i = 0; i < count; i++)
			{
				BeginCopyArrayElement(i);
				elementType.Accept(this);
				EndCopyArrayElement(i);
			}
		}

		private void BeginCopyArrayElement(int index)
		{
			if (activeFields.Count != 0)
			{
				activeFields.Peek().Index = index;
			}
		}

		private void EndCopyArrayElement(int index)
		{
			if (activeFields.Count != 0)
			{
				ActiveField activeField = activeFields.Peek();
				if (activeField.Index != index)
				{
					throw new InvalidOperationException();
				}
				activeField.Index = -1;
			}
		}

		private void BeginCopy(MetaType type, int count)
		{
			currentType = type;
			fieldSize = type.Size * count;
			if (buffer == null || buffer.Length < fieldSize)
			{
				buffer = new byte[fieldSize * 2];
			}
			input.Read(buffer, 0, fieldSize);
		}

		private void EndCopy()
		{
			if (callback != null)
			{
				callback(this);
			}
			if (output != null)
			{
				output.Write(buffer, 0, fieldSize);
			}
			position += fieldSize;
		}
	}
}
