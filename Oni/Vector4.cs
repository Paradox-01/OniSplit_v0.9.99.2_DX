using System;

namespace Oni
{
	internal struct Vector4 : IEquatable<Vector4>
	{
		public float X;

		public float Y;

		public float Z;

		public float W;

		private static Vector4 zero = default(Vector4);

		private static Vector4 one = new Vector4(1f);

		private static Vector4 unitX = new Vector4(1f, 0f, 0f, 0f);

		private static Vector4 unitY = new Vector4(0f, 1f, 0f, 0f);

		private static Vector4 unitZ = new Vector4(0f, 0f, 1f, 0f);

		private static Vector4 unitW = new Vector4(0f, 0f, 0f, 1f);

		public Vector3 XYZ
		{
			get
			{
				return new Vector3(X, Y, Z);
			}
			set
			{
				X = value.X;
				Y = value.Y;
				Z = value.Z;
			}
		}

		public static Vector4 Zero
		{
			get
			{
				return zero;
			}
		}

		public static Vector4 One
		{
			get
			{
				return one;
			}
		}

		public static Vector4 UnitX
		{
			get
			{
				return unitX;
			}
		}

		public static Vector4 UnitY
		{
			get
			{
				return unitY;
			}
		}

		public static Vector4 UnitZ
		{
			get
			{
				return unitZ;
			}
		}

		public static Vector4 UnitW
		{
			get
			{
				return unitW;
			}
		}

		public Vector4(float all)
		{
			X = all;
			Y = all;
			Z = all;
			W = all;
		}

		public Vector4(Vector3 v, float w)
		{
			X = v.X;
			Y = v.Y;
			Z = v.Z;
			W = w;
		}

		public Vector4(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public static Vector4 operator +(Vector4 v1, Vector4 v2)
		{
			v1.X += v2.X;
			v1.Y += v2.Y;
			v1.Z += v2.Z;
			v1.W += v2.W;
			return v1;
		}

		public static Vector4 operator -(Vector4 v1, Vector4 v2)
		{
			v1.X -= v2.X;
			v1.Y -= v2.Y;
			v1.Z -= v2.Z;
			v1.W -= v2.W;
			return v1;
		}

		public static Vector4 operator *(Vector4 v, float s)
		{
			v.X *= s;
			v.Y *= s;
			v.Z *= s;
			v.W *= s;
			return v;
		}

		public static Vector4 operator *(float s, Vector4 v)
		{
			return v * s;
		}

		public static Vector4 operator /(Vector4 v, float s)
		{
			return v * (1f / s);
		}

		public static float Dot(Vector4 v1, Vector4 v2)
		{
			return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z + v1.W * v2.W;
		}

		public static Vector4 Min(Vector4 v1, Vector4 v2)
		{
			v1.X = ((v1.X < v2.X) ? v1.X : v2.X);
			v1.Y = ((v1.Y < v2.Y) ? v1.Y : v2.Y);
			v1.Z = ((v1.Z < v2.Z) ? v1.Z : v2.Z);
			v1.W = ((v1.W < v2.W) ? v1.W : v2.W);
			return v1;
		}

		public static Vector4 Max(Vector4 v1, Vector4 v2)
		{
			v1.X = ((v1.X > v2.X) ? v1.X : v2.X);
			v1.Y = ((v1.Y > v2.Y) ? v1.Y : v2.Y);
			v1.Z = ((v1.Z > v2.Z) ? v1.Z : v2.Z);
			v1.W = ((v1.W > v2.W) ? v1.W : v2.W);
			return v1;
		}

		public static Vector4 Normalize(Vector4 v)
		{
			return v * (1f / v.Length());
		}

		public float LengthSquared()
		{
			return X * X + Y * Y + Z * Z + W * W;
		}

		public float Length()
		{
			return FMath.Sqrt(LengthSquared());
		}

		public static bool EqualsEps(Vector4 v1, Vector4 v2)
		{
			Vector4 vector = v2 - v1;
			float num = Math.Abs(vector.X);
			float num2 = Math.Abs(vector.Y);
			float num3 = Math.Abs(vector.Z);
			float num4 = Math.Abs(vector.W);
			if (num < 0.0001f && num2 < 0.0001f && num3 < 0.0001f)
			{
				return num4 < 0.0001f;
			}
			return false;
		}

		public static bool operator ==(Vector4 v1, Vector4 v2)
		{
			if (v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z)
			{
				return v1.W == v2.W;
			}
			return false;
		}

		public static bool operator !=(Vector4 v1, Vector4 v2)
		{
			if (v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z)
			{
				return v1.W != v2.W;
			}
			return true;
		}

		public bool Equals(Vector4 other)
		{
			if (X == other.X && Y == other.Y && Z == other.Z)
			{
				return W == other.W;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is Vector4)
			{
				return Equals((Vector4)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("{{{0} {1} {2} {3}}}", X, Y, Z, W);
		}
	}
}
