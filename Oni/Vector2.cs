using System;

namespace Oni
{
	internal struct Vector2 : IEquatable<Vector2>
	{
		public float X;

		public float Y;

		private static Vector2 zero = new Vector2(0f, 0f);

		private static Vector2 one = new Vector2(1f, 1f);

		private static Vector2 unitX = new Vector2(1f, 0f);

		private static Vector2 unitY = new Vector2(0f, 1f);

		public static Vector2 Zero
		{
			get
			{
				return zero;
			}
		}

		public static Vector2 One
		{
			get
			{
				return one;
			}
		}

		public static Vector2 UnitX
		{
			get
			{
				return unitX;
			}
		}

		public static Vector2 UnitY
		{
			get
			{
				return unitY;
			}
		}

		public Vector2(float x, float y)
		{
			X = x;
			Y = y;
		}

		public Vector2(float all)
		{
			X = all;
			Y = all;
		}

		public static Vector2 operator +(Vector2 v1, Vector2 v2)
		{
			return new Vector2
			{
				X = v1.X + v2.X,
				Y = v1.Y + v2.Y
			};
		}

		public static Vector2 operator -(Vector2 v1, Vector2 v2)
		{
			return new Vector2
			{
				X = v1.X - v2.X,
				Y = v1.Y - v2.Y
			};
		}

		public static float Dot(Vector2 v1, Vector2 v2)
		{
			return v1.X * v2.X + v1.Y * v2.Y;
		}

		public static Vector2 Normalize(Vector2 v)
		{
			return v * (1f / v.Length());
		}

		public void Normalize()
		{
			float num = 1f / Length();
			X *= num;
			Y *= num;
		}

		public float Length()
		{
			return FMath.Sqrt(X * X + Y * Y);
		}

		public static Vector2 operator *(Vector2 v, float s)
		{
			v.X *= s;
			v.Y *= s;
			return v;
		}

		public static Vector2 operator *(float s, Vector2 v)
		{
			v.X *= s;
			v.Y *= s;
			return v;
		}

		public static Vector2 operator /(Vector2 v, float s)
		{
			return v * (1f / s);
		}

		public static Vector2 Min(Vector2 v1, Vector2 v2)
		{
			return new Vector2
			{
				X = ((v1.X < v2.X) ? v1.X : v2.X),
				Y = ((v1.Y < v2.Y) ? v1.Y : v2.Y)
			};
		}

		public static Vector2 Max(Vector2 v1, Vector2 v2)
		{
			return new Vector2
			{
				X = ((v1.X > v2.X) ? v1.X : v2.X),
				Y = ((v1.Y > v2.Y) ? v1.Y : v2.Y)
			};
		}

		public static bool operator ==(Vector2 v1, Vector2 v2)
		{
			if (v1.X == v2.X)
			{
				return v1.Y == v2.Y;
			}
			return false;
		}

		public static bool operator !=(Vector2 v1, Vector2 v2)
		{
			if (v1.X == v2.X)
			{
				return v1.Y != v2.Y;
			}
			return true;
		}

		public bool Equals(Vector2 other)
		{
			if (X == other.X)
			{
				return Y == other.Y;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is Vector2)
			{
				return Equals((Vector2)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("{{{0} {1}}}", X, Y);
		}
	}
}
