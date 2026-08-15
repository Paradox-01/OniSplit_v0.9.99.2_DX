using System.Collections.Generic;
using System.IO;

namespace Oni.Metadata
{
	internal class DumpVisitor : MetaTypeVisitor
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

		private readonly InstanceMetadata metadata;

		private readonly TextWriter writer;

		private Stack<ActiveField> activeFields;

		private MetaType topLevelType;

		private MetaType currentType;

		private int fieldSize;

		private int position;

		private string indent = "";

		private Field field;

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

		public DumpVisitor(TextWriter writer, InstanceMetadata metadata)
		{
			this.writer = writer;
			this.metadata = metadata;
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
			writer.Write(indent);
			writer.Write("uint8_t");
		}

		public override void VisitInt16(MetaInt16 type)
		{
			writer.Write(indent);
			writer.Write("int16_t");
		}

		public override void VisitUInt16(MetaUInt16 type)
		{
			writer.Write(indent);
			writer.Write("uint16_t");
		}

		public override void VisitInt32(MetaInt32 type)
		{
			writer.Write(indent);
			writer.Write("int32_t");
		}

		public override void VisitUInt32(MetaUInt32 type)
		{
			writer.Write(indent);
			writer.Write("uint32_t");
		}

		public override void VisitInt64(MetaInt64 type)
		{
			writer.Write(indent);
			writer.Write("int64_t");
		}

		public override void VisitUInt64(MetaUInt64 type)
		{
			writer.Write(indent);
			writer.Write("uint64_t");
		}

		public override void VisitFloat(MetaFloat type)
		{
			writer.Write(indent);
			writer.Write("float");
		}

		public override void VisitColor(MetaColor type)
		{
			writer.Write(indent);
			writer.Write("uint32_t");
		}

		public override void VisitRawOffset(MetaRawOffset type)
		{
			writer.Write(indent);
			writer.Write("void*");
		}

		public override void VisitSepOffset(MetaSepOffset type)
		{
			writer.Write(indent);
			writer.Write("void*");
		}

		public override void VisitPointer(MetaPointer type)
		{
			writer.Write(indent);
			writer.Write("{0}*", metadata.GetTemplate(type.Tag).Type.Name);
		}

		public override void VisitBoundingBox(MetaBoundingBox type)
		{
			writer.Write(indent);
			writer.Write("bbox");
		}

		public override void VisitBoundingSphere(MetaBoundingSphere type)
		{
			writer.Write(indent);
			writer.Write("bsphere");
		}

		public override void VisitMatrix4x3(MetaMatrix4x3 type)
		{
			writer.Write(indent);
			writer.Write("matrix43");
		}

		public override void VisitPlane(MetaPlane type)
		{
			writer.Write(indent);
			writer.Write("plane");
		}

		public override void VisitQuaternion(MetaQuaternion type)
		{
			writer.Write(indent);
			writer.Write("quat");
		}

		public override void VisitVector2(MetaVector2 type)
		{
			writer.Write(indent);
			writer.Write("vec2");
		}

		public override void VisitVector3(MetaVector3 type)
		{
			writer.Write(indent);
			writer.Write("vec3");
		}

		public override void VisitString(MetaString type)
		{
			writer.Write(indent);
			writer.Write("const char*");
		}

		public override void VisitPadding(MetaPadding type)
		{
			writer.Write(indent);
			writer.Write("std::array<{0}>", type.Size);
		}

		public override void VisitStruct(MetaStruct type)
		{
			if (topLevelType == null)
			{
				topLevelType = type;
			}
			writer.WriteLine();
			writer.WriteLine("struct {0} {{", type.Name);
			indent += "\t";
			foreach (Field field in type.Fields)
			{
				field.Type.Accept(this);
				writer.WriteLine(" {0};", field.Name);
			}
			indent = indent.Substring(0, indent.Length - 1);
			writer.WriteLine("};");
		}

		public override void VisitArray(MetaArray type)
		{
			writer.Write(indent);
		}

		public override void VisitVarArray(MetaVarArray type)
		{
		}
	}
}
