using System;

namespace Oni.Metadata
{
	internal abstract class MetaType
	{
		private string name;

		private int size;

		private bool isFixedSize;

		private bool? isLeaf;

		public static readonly MetaChar Char = new MetaChar();

		public static readonly MetaByte Byte = new MetaByte();

		public static readonly MetaInt16 Int16 = new MetaInt16();

		public static readonly MetaUInt16 UInt16 = new MetaUInt16();

		public static readonly MetaInt32 Int32 = new MetaInt32();

		public static readonly MetaUInt32 UInt32 = new MetaUInt32();

		public static readonly MetaInt64 Int64 = new MetaInt64();

		public static readonly MetaUInt64 UInt64 = new MetaUInt64();

		public static readonly MetaColor Color = new MetaColor();

		public static readonly MetaFloat Float = new MetaFloat();

		public static readonly MetaVector2 Vector2 = new MetaVector2();

		public static readonly MetaVector3 Vector3 = new MetaVector3();

		public static readonly MetaQuaternion Quaternion = new MetaQuaternion();

		public static readonly MetaPlane Plane = new MetaPlane();

		public static readonly MetaBoundingBox BoundingBox = new MetaBoundingBox();

		public static readonly MetaBoundingSphere BoundingSphere = new MetaBoundingSphere();

		public static readonly MetaMatrix4x3 Matrix4x3 = new MetaMatrix4x3();

		public static readonly MetaRawOffset RawOffset = new MetaRawOffset();

		public static readonly MetaSepOffset SepOffset = new MetaSepOffset();

		public static readonly MetaString String16 = new MetaString(16);

		public static readonly MetaString String32 = new MetaString(32);

		public static readonly MetaString String48 = new MetaString(48);

		public static readonly MetaString String63 = new MetaString(63);

		public static readonly MetaString String64 = new MetaString(64);

		public static readonly MetaString String128 = new MetaString(128);

		public static readonly MetaString String256 = new MetaString(256);

		public string Name
		{
			get
			{
				return name;
			}
			protected set
			{
				name = value;
			}
		}

		public int Size
		{
			get
			{
				return size;
			}
			protected set
			{
				size = value;
			}
		}

		public bool IsFixedSize
		{
			get
			{
				return isFixedSize;
			}
			protected set
			{
				isFixedSize = value;
			}
		}

		public bool IsLeaf
		{
			get
			{
				if (!isLeaf.HasValue)
				{
					isLeaf = IsLeafImpl();
				}
				return isLeaf.Value;
			}
		}

		public bool IsBlittable
		{
			get
			{
				if (this != Byte && this != Int16 && this != UInt16 && this != Int32 && this != UInt32 && this != Int64 && this != UInt64 && this != Float && this != Color && this != Matrix4x3 && this != Plane && this != Quaternion && this != Vector2)
				{
					return this == Vector3;
				}
				return true;
			}
		}

		protected MetaType()
		{
		}

		protected MetaType(string name, int size)
		{
			this.size = size;
			this.name = name;
		}

		public static MetaPadding Padding(int length)
		{
			return new MetaPadding(length);
		}

		public static MetaPadding Padding(int length, byte fillByte)
		{
			return new MetaPadding(length, fillByte);
		}

		public static MetaArray Array(int length, MetaType elementType)
		{
			return new MetaArray(elementType, length);
		}

		public static MetaVarArray ShortVarArray(MetaType elementType)
		{
			return new MetaVarArray(Int16, elementType);
		}

		public static MetaVarArray VarArray(MetaType elementType)
		{
			return new MetaVarArray(Int32, elementType);
		}

		public static MetaString String(int length)
		{
			switch (length)
			{
			case 16:
				return String16;
			case 32:
				return String32;
			case 64:
				return String64;
			case 128:
				return String128;
			case 256:
				return String256;
			default:
				return new MetaString(length);
			}
		}

		public static MetaPointer Pointer(TemplateTag tag)
		{
			return new MetaPointer(tag);
		}

		public static MetaEnum Enum<T>()
		{
			Type underlyingType = System.Enum.GetUnderlyingType(typeof(T));
			if (underlyingType == typeof(byte))
			{
				return new MetaEnum(Byte, typeof(T));
			}
			if (underlyingType == typeof(short))
			{
				return new MetaEnum(Int16, typeof(T));
			}
			if (underlyingType == typeof(ushort))
			{
				return new MetaEnum(UInt16, typeof(T));
			}
			if (underlyingType == typeof(int))
			{
				return new MetaEnum(Int32, typeof(T));
			}
			if (underlyingType == typeof(uint))
			{
				return new MetaEnum(UInt32, typeof(T));
			}
			if (underlyingType == typeof(long))
			{
				return new MetaEnum(Int64, typeof(T));
			}
			if (underlyingType == typeof(ulong))
			{
				return new MetaEnum(UInt64, typeof(T));
			}
			throw new InvalidOperationException(string.Format("Unsupported enum type {0}", underlyingType));
		}

		protected abstract bool IsLeafImpl();

		public abstract void Accept(IMetaTypeVisitor visitor);

		internal int Copy(BinaryReader input, BinaryWriter output, Action<CopyVisitor> callback)
		{
			CopyVisitor copyVisitor = new CopyVisitor(input, output, callback);
			Accept(copyVisitor);
			return copyVisitor.Position;
		}
	}
}
