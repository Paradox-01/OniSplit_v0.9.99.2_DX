using System;
using System.Collections.Generic;
using System.Xml;
using Oni.Imaging;
using Oni.Metadata;

namespace Oni.Xml
{
	internal class RawXmlExporter : IMetaTypeVisitor
	{
		private readonly BinaryReader reader;

		private readonly XmlWriter xml;

		private Stack<int> startOffsetStack;

		protected BinaryReader Reader
		{
			get
			{
				return reader;
			}
		}

		protected XmlWriter Xml
		{
			get
			{
				return xml;
			}
		}

		public RawXmlExporter(BinaryReader reader, XmlWriter xml)
		{
			this.reader = reader;
			this.xml = xml;
			BeginStruct(reader.Position);
		}

		protected void BeginStruct(int startOffset)
		{
			startOffsetStack = new Stack<int>();
			startOffsetStack.Push(startOffset);
		}

		void IMetaTypeVisitor.VisitEnum(MetaEnum type)
		{
			type.BinaryToXml(reader, xml);
		}

		void IMetaTypeVisitor.VisitByte(MetaByte type)
		{
			xml.WriteValue(reader.ReadByte());
		}

		void IMetaTypeVisitor.VisitInt16(MetaInt16 type)
		{
			xml.WriteValue(reader.ReadInt16());
		}

		void IMetaTypeVisitor.VisitUInt16(MetaUInt16 type)
		{
			xml.WriteValue(reader.ReadUInt16());
		}

		void IMetaTypeVisitor.VisitInt32(MetaInt32 type)
		{
			xml.WriteValue(reader.ReadInt32());
		}

		void IMetaTypeVisitor.VisitUInt32(MetaUInt32 type)
		{
			xml.WriteValue(reader.ReadUInt32());
		}

		void IMetaTypeVisitor.VisitInt64(MetaInt64 type)
		{
			xml.WriteValue(reader.ReadInt64());
		}

		void IMetaTypeVisitor.VisitUInt64(MetaUInt64 type)
		{
			xml.WriteValue(XmlConvert.ToString(reader.ReadUInt64()));
		}

		void IMetaTypeVisitor.VisitFloat(MetaFloat type)
		{
			xml.WriteValue(reader.ReadSingle());
		}

		void IMetaTypeVisitor.VisitVector2(MetaVector2 type)
		{
			xml.WriteFloatArray(reader.ReadSingleArray(2));
		}

		void IMetaTypeVisitor.VisitVector3(MetaVector3 type)
		{
			xml.WriteFloatArray(reader.ReadSingleArray(3));
		}

		void IMetaTypeVisitor.VisitMatrix4x3(MetaMatrix4x3 type)
		{
			xml.WriteFloatArray(reader.ReadSingleArray(12));
		}

		void IMetaTypeVisitor.VisitPlane(MetaPlane type)
		{
			xml.WriteFloatArray(reader.ReadSingleArray(4));
		}

		void IMetaTypeVisitor.VisitQuaternion(MetaQuaternion type)
		{
			xml.WriteFloatArray(reader.ReadSingleArray(4));
		}

		void IMetaTypeVisitor.VisitBoundingSphere(MetaBoundingSphere type)
		{
			WriteFields(type.Fields);
		}

		void IMetaTypeVisitor.VisitBoundingBox(MetaBoundingBox type)
		{
			WriteFields(type.Fields);
		}

		void IMetaTypeVisitor.VisitColor(MetaColor type)
		{
			Color color = reader.ReadColor();
			if (color.A == byte.MaxValue)
			{
				xml.WriteValue(string.Format("{0} {1} {2}", color.R, color.G, color.B));
				return;
			}
			xml.WriteValue(string.Format("{0} {1} {2} {3}", color.R, color.G, color.B, color.A));
		}

		void IMetaTypeVisitor.VisitRawOffset(MetaRawOffset type)
		{
			xml.WriteValue(reader.ReadInt32());
		}

		void IMetaTypeVisitor.VisitSepOffset(MetaSepOffset type)
		{
			xml.WriteValue(reader.ReadInt32());
		}

		void IMetaTypeVisitor.VisitPointer(MetaPointer type)
		{
			VisitLink(type);
		}

		void IMetaTypeVisitor.VisitString(MetaString type)
		{
			string s = reader.ReadString(type.Count);
			s = CleanupInvalidCharacters(s);
			xml.WriteValue(s);
		}

		void IMetaTypeVisitor.VisitPadding(MetaPadding type)
		{
			reader.Position += type.Count;
		}

		void IMetaTypeVisitor.VisitStruct(MetaStruct type)
		{
			WriteFields(type.Fields);
		}

		void IMetaTypeVisitor.VisitArray(MetaArray type)
		{
			WriteArray(type.ElementType, type.Count);
		}

		void IMetaTypeVisitor.VisitVarArray(MetaVarArray type)
		{
			WriteArray(count: (type.CountField.Type != MetaType.Int16) ? reader.ReadInt32() : reader.ReadInt16(), elementType: type.ElementType);
		}

		protected virtual void VisitLink(MetaPointer link)
		{
			throw new NotSupportedException();
		}

		private void WriteFields(IEnumerable<Field> fields)
		{
			foreach (Field field in fields)
			{
				if (field.Type is MetaPadding)
				{
					field.Type.Accept(this);
					continue;
				}
				string text = field.Name;
				if (string.IsNullOrEmpty(text))
				{
					int num = reader.Position - startOffsetStack.Peek();
					text = string.Format("Offset_{0:X4}", num);
				}
				xml.WriteStartElement(text);
				field.Type.Accept(this);
				xml.WriteEndElement();
			}
		}

		private void WriteArray(MetaType elementType, int count)
		{
			bool isBlittable = elementType.IsBlittable;
			isBlittable = false;
			for (int i = 0; i < count; i++)
			{
				startOffsetStack.Push(reader.Position);
				if (!isBlittable)
				{
					xml.WriteStartElement(elementType.Name);
				}
				elementType.Accept(this);
				if (!isBlittable)
				{
					xml.WriteEndElement();
				}
				else if (i != count - 1)
				{
					xml.WriteWhitespace(" \n");
				}
				startOffsetStack.Pop();
			}
		}

		private static string CleanupInvalidCharacters(string s)
		{
			char[] array = null;
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] >= ' ')
				{
					continue;
				}
				switch (s[i])
				{
				case '\t':
				case '\n':
				case '\r':
					continue;
				}
				if (array == null)
				{
					array = s.ToCharArray();
				}
				array[i] = ' ';
			}
			if (array == null)
			{
				return s;
			}
			return new string(array);
		}
	}
}
