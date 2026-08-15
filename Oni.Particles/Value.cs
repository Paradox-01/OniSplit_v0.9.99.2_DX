using Oni.Imaging;

namespace Oni.Particles
{
	internal class Value
	{
		public const int ByteSize = 28;

		public static readonly Value Empty = new Value(ValueType.Variable, string.Empty);

		public static readonly Value FloatZero = new Value(0f);

		public static readonly Value FloatOne = new Value(1f);

		private ValueType type;

		private string name;

		private float f1;

		private float f2;

		private Color c1;

		private Color c2;

		private int i;

		public ValueType Type
		{
			get
			{
				return type;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
		}

		public float Float1
		{
			get
			{
				return f1;
			}
		}

		public float Float2
		{
			get
			{
				return f2;
			}
		}

		public Color Color1
		{
			get
			{
				return c1;
			}
		}

		public Color Color2
		{
			get
			{
				return c2;
			}
		}

		public int Int
		{
			get
			{
				return i;
			}
		}

		public Value(ValueType type, float value1, float value2)
		{
			this.type = type;
			f1 = value1;
			f2 = value2;
		}

		public Value(float value)
			: this(ValueType.Float, value, 0f)
		{
		}

		public Value(int value)
		{
			type = ValueType.Int32;
			i = value;
		}

		public Value(ValueType type, string name)
		{
			this.type = type;
			this.name = name;
		}

		public Value(Color color)
			: this(ValueType.Color, color, Color.Black)
		{
		}

		public Value(ValueType type, Color color1, Color color2)
		{
			this.type = type;
			c1 = color1;
			c2 = color2;
		}

		public static Value Read(BinaryReader reader)
		{
			int position = reader.Position;
			ValueType valueType = (ValueType)reader.ReadInt32();
			Value result = null;
			switch (valueType)
			{
			case ValueType.Variable:
			{
				string value = reader.ReadString(16);
				if (!string.IsNullOrEmpty(value))
				{
					result = new Value(valueType, value);
				}
				break;
			}
			case ValueType.InstanceName:
				result = new Value(valueType, reader.ReadString(16));
				break;
			case ValueType.Float:
				result = new Value(reader.ReadSingle());
				break;
			case ValueType.FloatRandom:
			case ValueType.FloatBellCurve:
			case ValueType.TimeCycle:
				result = new Value(valueType, reader.ReadSingle(), reader.ReadSingle());
				break;
			case ValueType.Color:
				result = new Value(reader.ReadColor());
				break;
			case ValueType.ColorRandom:
			case ValueType.ColorBellCurve:
				result = new Value(valueType, reader.ReadColor(), reader.ReadColor());
				break;
			case ValueType.Int32:
				result = new Value(reader.ReadInt32());
				break;
			}
			reader.Position = position + 28;
			return result;
		}

		public void Write(BinaryWriter writer)
		{
			int position = writer.Position;
			writer.Write((int)type);
			switch (type)
			{
			case ValueType.Variable:
			case ValueType.InstanceName:
				writer.Write(name, 16);
				break;
			case ValueType.Float:
				writer.Write(f1);
				break;
			case ValueType.FloatRandom:
			case ValueType.FloatBellCurve:
			case ValueType.TimeCycle:
				writer.Write(f1);
				writer.Write(f2);
				break;
			case ValueType.Color:
				writer.Write(c1);
				break;
			case ValueType.ColorRandom:
			case ValueType.ColorBellCurve:
				writer.Write(c1);
				writer.Write(c2);
				break;
			case ValueType.Int32:
				writer.Write(i);
				break;
			}
			writer.Position = position + 28;
		}
	}
}
