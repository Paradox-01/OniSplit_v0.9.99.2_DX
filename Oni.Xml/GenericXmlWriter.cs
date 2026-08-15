using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Oni.Imaging;
using Oni.Metadata;

namespace Oni.Xml
{
	internal class GenericXmlWriter : IMetaTypeVisitor
	{
		private readonly Action<InstanceDescriptor> exportChild;

		private readonly XmlWriter xml;

		private readonly Stack<int> startOffsetStack;

		private InstanceFile instanceFile;

		private BinaryReader reader;

		public static void Write(XmlWriter xml, Action<InstanceDescriptor> exportChild, InstanceDescriptor descriptor)
		{
			GenericXmlWriter genericXmlWriter = new GenericXmlWriter(xml, exportChild);
			genericXmlWriter.WriteInstance(descriptor);
		}

		private GenericXmlWriter(XmlWriter xml, Action<InstanceDescriptor> exportChild)
		{
			this.exportChild = exportChild;
			this.xml = xml;
			startOffsetStack = new Stack<int>();
		}

		private void WriteInstance(InstanceDescriptor descriptor)
		{
			using (reader = descriptor.OpenRead())
			{
				BeginXmlInstance(descriptor);
				descriptor.Template.Type.Accept(this);
				EndXmlInstance();
			}
		}

		private void BeginXmlInstance(InstanceDescriptor descriptor)
		{
			instanceFile = descriptor.File;
			startOffsetStack.Push(reader.Position - 8);
			string localName = descriptor.Template.Tag.ToString();
			xml.WriteStartElement(localName);
			xml.WriteAttributeString("id", XmlConvert.ToString(descriptor.Index));
		}

		private void EndXmlInstance()
		{
			startOffsetStack.Pop();
			xml.WriteEndElement();
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
				xml.WriteValue(string.Format(CultureInfo.InvariantCulture, "{0} {1} {2}", new object[3] { color.R, color.G, color.B }));
			}
			else
			{
				xml.WriteValue(string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3}", color.R, color.G, color.B, color.A));
			}
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
			int num = reader.ReadInt32();
			if (num == 0)
			{
				xml.WriteString(string.Empty);
			}
			else
			{
				exportChild(instanceFile.GetDescriptor(num));
			}
		}

		void IMetaTypeVisitor.VisitString(MetaString type)
		{
			xml.WriteValue(reader.ReadString(type.Count));
		}

		void IMetaTypeVisitor.VisitPadding(MetaPadding type)
		{
			reader.Skip(type.Count);
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
					text = string.Format(CultureInfo.InvariantCulture, "Offset_{0:X4}", new object[1] { num });
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
	}
}
