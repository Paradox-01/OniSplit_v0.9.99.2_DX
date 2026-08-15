using System;
using System.Collections.Generic;
using System.Xml;
using Oni.Imaging;
using Oni.Metadata;

namespace Oni.Xml
{
	internal class RawXmlImporter : IMetaTypeVisitor
	{
		private static readonly Func<string, float> floatConverter = XmlConvert.ToSingle;

		private static readonly Func<string, byte> byteConverter = XmlConvert.ToByte;

		private readonly XmlReader xml;

		private readonly BinaryWriter writer;

		private Stack<int> startOffsetStack;

		protected XmlReader Xml
		{
			get
			{
				return xml;
			}
		}

		protected BinaryWriter Writer
		{
			get
			{
				return writer;
			}
		}

		public RawXmlImporter(XmlReader xml, BinaryWriter writer)
		{
			this.xml = xml;
			this.writer = writer;
		}

		protected void BeginStruct(int startPosition)
		{
			startOffsetStack = new Stack<int>();
			startOffsetStack.Push(startPosition);
		}

		void IMetaTypeVisitor.VisitEnum(MetaEnum type)
		{
			type.XmlToBinary(xml, writer);
		}

		void IMetaTypeVisitor.VisitByte(MetaByte type)
		{
			writer.Write(XmlConvert.ToByte(xml.ReadElementContentAsString()));
		}

		void IMetaTypeVisitor.VisitInt16(MetaInt16 type)
		{
			writer.Write(XmlConvert.ToInt16(xml.ReadElementContentAsString()));
		}

		void IMetaTypeVisitor.VisitUInt16(MetaUInt16 type)
		{
			writer.Write(XmlConvert.ToUInt16(xml.ReadElementContentAsString()));
		}

		void IMetaTypeVisitor.VisitInt32(MetaInt32 type)
		{
			writer.Write(xml.ReadElementContentAsInt());
		}

		void IMetaTypeVisitor.VisitUInt32(MetaUInt32 type)
		{
			writer.Write(XmlConvert.ToUInt32(xml.ReadElementContentAsString()));
		}

		void IMetaTypeVisitor.VisitInt64(MetaInt64 type)
		{
			writer.Write(xml.ReadElementContentAsLong());
		}

		void IMetaTypeVisitor.VisitUInt64(MetaUInt64 type)
		{
			writer.Write(XmlConvert.ToUInt64(xml.ReadElementContentAsString()));
		}

		void IMetaTypeVisitor.VisitFloat(MetaFloat type)
		{
			writer.Write(xml.ReadElementContentAsFloat());
		}

		void IMetaTypeVisitor.VisitColor(MetaColor type)
		{
			byte[] array = xml.ReadElementContentAsArray(byteConverter);
			if (array.Length > 3)
			{
				writer.Write(new Color(array[0], array[1], array[2], array[3]));
			}
			else
			{
				writer.Write(new Color(array[0], array[1], array[2]));
			}
		}

		void IMetaTypeVisitor.VisitVector2(MetaVector2 type)
		{
			writer.Write(xml.ReadElementContentAsArray(floatConverter, 2));
		}

		void IMetaTypeVisitor.VisitVector3(MetaVector3 type)
		{
			writer.Write(xml.ReadElementContentAsArray(floatConverter, 3));
		}

		void IMetaTypeVisitor.VisitMatrix4x3(MetaMatrix4x3 type)
		{
			writer.WriteMatrix4x3(xml.ReadElementContentAsMatrix43());
		}

		void IMetaTypeVisitor.VisitPlane(MetaPlane type)
		{
			writer.Write(xml.ReadElementContentAsArray(floatConverter, 4));
		}

		void IMetaTypeVisitor.VisitQuaternion(MetaQuaternion type)
		{
			writer.Write(xml.ReadElementContentAsQuaternion());
		}

		void IMetaTypeVisitor.VisitBoundingSphere(MetaBoundingSphere type)
		{
			ReadFields(type.Fields);
		}

		void IMetaTypeVisitor.VisitBoundingBox(MetaBoundingBox type)
		{
			ReadFields(type.Fields);
		}

		void IMetaTypeVisitor.VisitRawOffset(MetaRawOffset type)
		{
			throw new NotImplementedException();
		}

		void IMetaTypeVisitor.VisitSepOffset(MetaSepOffset type)
		{
			throw new NotImplementedException();
		}

		void IMetaTypeVisitor.VisitString(MetaString type)
		{
			writer.Write(xml.ReadElementContentAsString(), type.Count);
		}

		void IMetaTypeVisitor.VisitPadding(MetaPadding type)
		{
			writer.Write(type.FillByte, type.Count);
		}

		void IMetaTypeVisitor.VisitPointer(MetaPointer type)
		{
			throw new NotImplementedException();
		}

		void IMetaTypeVisitor.VisitStruct(MetaStruct type)
		{
			ReadFields(type.Fields);
		}

		void IMetaTypeVisitor.VisitArray(MetaArray type)
		{
			int num = ReadArray(type.ElementType, type.Count);
			if (num < type.Count)
			{
				writer.Skip((type.Count - num) * type.ElementType.Size);
			}
		}

		void IMetaTypeVisitor.VisitVarArray(MetaVarArray type)
		{
			int position = writer.Position;
			int value;
			if (type.CountField.Type == MetaType.Int16)
			{
				writer.WriteInt16(0);
				value = ReadArray(type.ElementType, 65535);
			}
			else
			{
				writer.Write(0);
				value = ReadArray(type.ElementType, int.MaxValue);
			}
			int position2 = writer.Position;
			writer.Position = position;
			if (type.CountField.Type == MetaType.Int16)
			{
				writer.WriteUInt16(value);
			}
			else
			{
				writer.Write(value);
			}
			writer.Position = position2;
		}

		private void ReadFields(IEnumerable<Field> fields)
		{
			xml.ReadStartElement();
			xml.MoveToContent();
			foreach (Field field in fields)
			{
				try
				{
					field.Type.Accept(this);
				}
				catch (Exception innerException)
				{
					IXmlLineInfo xmlLineInfo = xml as IXmlLineInfo;
					int num = ((xmlLineInfo != null) ? xmlLineInfo.LineNumber : 0);
					throw new InvalidOperationException(string.Format("Cannot read field '{0}' (line {1})", field.Name, num), innerException);
				}
			}
			xml.ReadEndElement();
		}

		protected void ReadStruct(MetaStruct s)
		{
			foreach (Field field in s.Fields)
			{
				try
				{
					field.Type.Accept(this);
				}
				catch (Exception innerException)
				{
					throw new InvalidOperationException(string.Format("Cannot read field '{0}'", field.Name), innerException);
				}
			}
		}

		private int ReadArray(MetaType elementType, int maxCount)
		{
			if (xml.IsEmptyElement)
			{
				xml.Read();
				return 0;
			}
			xml.ReadStartElement();
			xml.MoveToContent();
			string localName = xml.LocalName;
			int i;
			for (i = 0; i < maxCount; i++)
			{
				if (!xml.IsStartElement(localName))
				{
					break;
				}
				startOffsetStack.Push(writer.Position);
				elementType.Accept(this);
				startOffsetStack.Pop();
			}
			xml.ReadEndElement();
			return i;
		}
	}
}
