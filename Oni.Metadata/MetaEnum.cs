using System;
using System.Xml;

namespace Oni.Metadata
{
	internal class MetaEnum : MetaType
	{
		private MetaType baseType;

		private Type enumType;

		public MetaType BaseType
		{
			get
			{
				return baseType;
			}
		}

		public Type EnumType
		{
			get
			{
				return enumType;
			}
		}

		public bool IsFlags
		{
			get
			{
				return Utils.IsFlagsEnum(enumType);
			}
		}

		public MetaEnum(MetaType baseType, Type enumType)
			: base("Enum", baseType.Size)
		{
			if (baseType != MetaType.Byte && baseType != MetaType.Int16 && baseType != MetaType.UInt16 && baseType != MetaType.Int32 && baseType != MetaType.UInt32 && baseType != MetaType.Int64 && baseType != MetaType.UInt64)
			{
				throw new ArgumentException("Invalid enum base type", "baseType");
			}
			this.baseType = baseType;
			this.enumType = enumType;
		}

		protected override bool IsLeafImpl()
		{
			return true;
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			visitor.VisitEnum(this);
		}

		public void BinaryToXml(BinaryReader reader, XmlWriter writer)
		{
			object obj = ((baseType == MetaType.Byte) ? System.Enum.ToObject(enumType, reader.ReadByte()) : ((baseType == MetaType.Int16) ? System.Enum.ToObject(enumType, reader.ReadInt16()) : ((baseType == MetaType.UInt16) ? System.Enum.ToObject(enumType, reader.ReadUInt16()) : ((baseType == MetaType.Int32) ? System.Enum.ToObject(enumType, reader.ReadInt32()) : ((baseType == MetaType.UInt32) ? System.Enum.ToObject(enumType, reader.ReadUInt32()) : ((baseType != MetaType.Int64) ? System.Enum.ToObject(enumType, reader.ReadUInt64()) : System.Enum.ToObject(enumType, reader.ReadInt64())))))));
			string text = obj.ToString().Replace(",", string.Empty);
			if (text == "None" && IsFlags)
			{
				text = string.Empty;
			}
			writer.WriteValue(text);
		}

		public void XmlToBinary(XmlReader reader, BinaryWriter writer)
		{
			string text = reader.ReadElementContentAsString();
			if (string.IsNullOrEmpty(text) && IsFlags)
			{
				if (baseType == MetaType.Byte)
				{
					writer.WriteByte(0);
				}
				else if (baseType == MetaType.Int16 || baseType == MetaType.UInt16)
				{
					writer.WriteUInt16(0);
				}
				else if (baseType == MetaType.Int32 || baseType == MetaType.UInt32)
				{
					writer.Write(0);
				}
				else
				{
					writer.Write(0L);
				}
				return;
			}
			object obj = null;
			try
			{
				obj = System.Enum.Parse(enumType, text.Trim().Replace(' ', ','));
			}
			catch
			{
			}
			if (obj == null)
			{
				throw new FormatException(string.Format("{0} is not a valid value name. Run onisplit -help enums to see a list of possible names.", text));
			}
			if (baseType == MetaType.Byte)
			{
				writer.WriteByte(Convert.ToByte(obj));
			}
			else if (baseType == MetaType.Int16)
			{
				writer.Write(Convert.ToInt16(obj));
			}
			else if (baseType == MetaType.UInt16)
			{
				writer.Write(Convert.ToUInt16(obj));
			}
			else if (baseType == MetaType.Int32)
			{
				writer.Write(Convert.ToInt32(obj));
			}
			else if (baseType == MetaType.UInt32)
			{
				writer.Write(Convert.ToUInt32(obj));
			}
			else if (baseType == MetaType.Int64)
			{
				writer.Write(Convert.ToInt64(obj));
			}
			else
			{
				writer.Write(Convert.ToUInt64(obj));
			}
		}

		public static T Parse<T>(string text) where T : struct
		{
			object obj = null;
			if (string.IsNullOrEmpty(text) && Utils.IsFlagsEnum(typeof(T)))
			{
				obj = System.Enum.Parse(typeof(T), "None");
			}
			else
			{
				try
				{
					string[] value = text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
					text = string.Join(", ", value);
					obj = System.Enum.Parse(typeof(T), text, true);
				}
				catch
				{
				}
			}
			if (obj == null)
			{
				throw new FormatException(string.Format("{0} is not a valid value name. Run onisplit -help enums to see a list of possible names.", text));
			}
			return (T)obj;
		}

		public static string ToString<T>(T value) where T : struct
		{
			string text = value.ToString().Replace(",", string.Empty);
			if (text == "None" && Utils.IsFlagsEnum(typeof(T)))
			{
				text = string.Empty;
			}
			return text;
		}
	}
}
