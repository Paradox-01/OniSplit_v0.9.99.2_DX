using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Oni.Imaging;

namespace Oni
{
	internal class BinaryWriter : System.IO.BinaryWriter
	{
		private static readonly byte[] padding = new byte[32];

		private static readonly Encoding encoding = Encoding.UTF8;

		private readonly Stack<int> positionStack = new Stack<int>();

		public override Stream BaseStream
		{
			get
			{
				return OutStream;
			}
		}

		public int Position
		{
			get
			{
				return (int)BaseStream.Position;
			}
			set
			{
				int num = (int)OutStream.Position;
				int num2 = value - num;
				if (num2 != 0)
				{
					if (0 < num2 && num2 <= 32 && Position == OutStream.Length)
					{
						OutStream.Write(padding, 0, num2);
					}
					else
					{
						OutStream.Position = value;
					}
				}
			}
		}

		public BinaryWriter(Stream stream)
			: base(stream, encoding)
		{
		}

		public void WriteInstanceId(int index)
		{
			Write(InstanceFileWriter.MakeInstanceId(index));
		}

		public void Write(IEnumerable<ImporterDescriptor> descriptors)
		{
			foreach (ImporterDescriptor descriptor in descriptors)
			{
				Write(descriptor);
			}
		}

		public void Write(ImporterDescriptor descriptor)
		{
			if (descriptor == null)
			{
				Write(0);
			}
			else
			{
				Write(InstanceFileWriter.MakeInstanceId(descriptor.Index));
			}
		}

		public void Write(Color c)
		{
			Write(c.ToBgra32());
		}

		public void Write(Vector2 v)
		{
			Write(v.X);
			Write(v.Y);
		}

		public void Write(Vector3 v)
		{
			Write(v.X);
			Write(v.Y);
			Write(v.Z);
		}

		public void Write(Vector4 v)
		{
			Write(v.X);
			Write(v.Y);
			Write(v.Z);
			Write(v.W);
		}

		public void Write(Quaternion q)
		{
			Write(q.X);
			Write(q.Y);
			Write(q.Z);
			Write(0f - q.W);
		}

		public void Write(Plane p)
		{
			Write(p.Normal);
			Write(p.D);
		}

		public void Write(BoundingBox bbox)
		{
			Write(bbox.Min);
			Write(bbox.Max);
		}

		public void Write(BoundingSphere bsphere)
		{
			Write(bsphere.Center);
			Write(bsphere.Radius);
		}

		public void WriteMatrix4x3(Matrix m)
		{
			Write(m.M11);
			Write(m.M12);
			Write(m.M13);
			Write(m.M21);
			Write(m.M22);
			Write(m.M23);
			Write(m.M31);
			Write(m.M32);
			Write(m.M33);
			Write(m.M41);
			Write(m.M42);
			Write(m.M43);
		}

		public void Write(short[] a)
		{
			foreach (short value in a)
			{
				Write(value);
			}
		}

		public void Write(ushort[] a)
		{
			foreach (ushort value in a)
			{
				Write(value);
			}
		}

		public void Write(int[] a)
		{
			foreach (int value in a)
			{
				Write(value);
			}
		}

		public void Write(int[] v, int startIndex, int length)
		{
			for (int i = startIndex; i < startIndex + length; i++)
			{
				Write(v[i]);
			}
		}

		public void Write(IEnumerable<float> a)
		{
			foreach (float item in a)
			{
				Write(item);
			}
		}

		public void Write(IEnumerable<int> a)
		{
			foreach (int item in a)
			{
				Write(item);
			}
		}

		public void Write(Color[] a)
		{
			foreach (Color c in a)
			{
				Write(c);
			}
		}

		public void Write(IEnumerable<Vector2> a)
		{
			foreach (Vector2 item in a)
			{
				Write(item);
			}
		}

		public void Write(IEnumerable<Vector3> a)
		{
			foreach (Vector3 item in a)
			{
				Write(item);
			}
		}

		public void Write(IEnumerable<Plane> a)
		{
			foreach (Plane item in a)
			{
				Write(item);
			}
		}

		public void Write(string s, int maxLength)
		{
			if (s == null)
			{
				Skip(maxLength);
				return;
			}
			if (encoding.GetByteCount(s) > maxLength)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The string '{0}' is too long (max length is {1})", new object[2] { s, maxLength }));
			}
			byte[] array = new byte[maxLength];
			encoding.GetBytes(s, 0, s.Length, array, 0);
			Write(array);
		}

		public void WriteByte(int value)
		{
			if (value < 0 || 255 < value)
			{
				throw new ArgumentOutOfRangeException("Value too large for Byte", "value");
			}
			Write((byte)value);
		}

		public void WriteInt16(int value)
		{
			if (value < -32768 || 32767 < value)
			{
				throw new ArgumentOutOfRangeException("Value too large for Int16", "value");
			}
			Write((short)value);
		}

		public void WriteUInt16(int value)
		{
			if (value < 0 || value > 65535)
			{
				throw new ArgumentOutOfRangeException("Value too large for UInt16", "value");
			}
			Write((ushort)value);
		}

		public void Write(byte value, int count)
		{
			if (value == 0 && Position == OutStream.Length)
			{
				Seek(count, SeekOrigin.Current);
				return;
			}
			for (int i = 0; i < count; i++)
			{
				Write(value);
			}
		}

		public void Skip(int length)
		{
			Position += length;
		}

		public void PushPosition(int newPosition)
		{
			positionStack.Push(Position);
			Position = newPosition;
		}

		public void PopPosition()
		{
			Position = positionStack.Pop();
		}

		public void WriteAt(int position, int value)
		{
			PushPosition(position);
			Write(value);
			PopPosition();
		}

		public void WriteAt(int position, short value)
		{
			PushPosition(position);
			Write(value);
			PopPosition();
		}

		public int Align32()
		{
			return Position = Utils.Align32(Position);
		}
	}
}
