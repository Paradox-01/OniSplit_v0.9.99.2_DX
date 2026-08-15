using System;

namespace Oni
{
	internal struct Vector3 : IEquatable<Vector3>
	{
		public float X;

		public float Y;

		public float Z;

		private static Vector3 zero = default(Vector3);

		private static Vector3 one = new Vector3(1f);

		private static Vector3 up = new Vector3(0f, 1f, 0f);

		private static Vector3 down = new Vector3(0f, -1f, 0f);

		private static Vector3 right = new Vector3(1f, 0f, 0f);

		private static Vector3 left = new Vector3(-1f, 0f, 0f);

		private static Vector3 backward = new Vector3(0f, 0f, 1f);

		private static Vector3 forward = new Vector3(0f, 0f, -1f);

		public Vector2 XZ
		{
			get
			{
				return new Vector2(X, Z);
			}
		}

		public static Vector3 Zero
		{
			get
			{
				return zero;
			}
		}

		public static Vector3 One
		{
			get
			{
				return one;
			}
		}

		public static Vector3 Up
		{
			get
			{
				return up;
			}
		}

		public static Vector3 Down
		{
			get
			{
				return down;
			}
		}

		public static Vector3 Left
		{
			get
			{
				return left;
			}
		}

		public static Vector3 Right
		{
			get
			{
				return right;
			}
		}

		public static Vector3 Backward
		{
			get
			{
				return backward;
			}
		}

		public static Vector3 Forward
		{
			get
			{
				return forward;
			}
		}

		public static Vector3 UnitX
		{
			get
			{
				return right;
			}
		}

		public static Vector3 UnitY
		{
			get
			{
				return up;
			}
		}

		public static Vector3 UnitZ
		{
			get
			{
				return backward;
			}
		}

		public float this[int i]
		{
			get
			{
				if (i == 1)
				{
					return Y;
				}
				if (i < 1)
				{
					return X;
				}
				return Z;
			}
		}

		public Vector3(float all)
		{
			X = all;
			Y = all;
			Z = all;
		}

		public Vector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Vector3(float[] values, int index = 0)
		{
			int num = index * 3;
			X = values[num];
			Y = values[num + 1];
			Z = values[num + 2];
		}

		public void CopyTo(float[] values, int index = 0)
		{
			values[index] = X;
			values[index + 1] = Y;
			values[index + 2] = Z;
		}

		public static Vector3 operator +(Vector3 v1, Vector3 v2)
		{
			v1.X += v2.X;
			v1.Y += v2.Y;
			v1.Z += v2.Z;
			return v1;
		}

		public static Vector3 operator -(Vector3 v1, Vector3 v2)
		{
			v1.X -= v2.X;
			v1.Y -= v2.Y;
			v1.Z -= v2.Z;
			return v1;
		}

		public static Vector3 operator -(Vector3 v)
		{
			v.X = 0f - v.X;
			v.Y = 0f - v.Y;
			v.Z = 0f - v.Z;
			return v;
		}

		public static Vector3 operator *(Vector3 v, float s)
		{
			v.X *= s;
			v.Y *= s;
			v.Z *= s;
			return v;
		}

		public static Vector3 operator *(float s, Vector3 v)
		{
			v.X *= s;
			v.Y *= s;
			v.Z *= s;
			return v;
		}

		public static Vector3 operator *(Vector3 v1, Vector3 v2)
		{
			return new Vector3
			{
				X = v1.X * v2.X,
				Y = v1.Y * v2.Y,
				Z = v1.Z * v2.Z
			};
		}

		public static Vector3 operator /(Vector3 v, float s)
		{
			return v * (1f / s);
		}

		public static Vector3 operator /(Vector3 v1, Vector3 v2)
		{
			return new Vector3
			{
				X = (v1.X /= v2.X),
				Y = (v1.Y /= v2.Y),
				Z = (v1.Z /= v2.Z)
			};
		}

		public static void Add(ref Vector3 v1, ref Vector3 v2, out Vector3 r)
		{
			r.X = v1.X + v2.X;
			r.Y = v1.Y + v2.Y;
			r.Z = v1.Z + v2.Z;
		}

		public static void Substract(ref Vector3 v1, ref Vector3 v2, out Vector3 r)
		{
			r.X = v1.X - v2.X;
			r.Y = v1.Y - v2.Y;
			r.Z = v1.Z - v2.Z;
		}

		public static void Multiply(ref Vector3 v, float f, out Vector3 r)
		{
			r.X = v.X * f;
			r.Y = v.Y * f;
			r.Z = v.Z * f;
		}

		public void Scale(float scale)
		{
			X *= scale;
			Y *= scale;
			Z *= scale;
		}

		public static Vector3 Clamp(Vector3 v, Vector3 min, Vector3 max)
		{
			float x = v.X;
			x = ((x > max.X) ? max.X : x);
			x = ((x < min.X) ? min.X : x);
			float y = v.Y;
			y = ((y > max.Y) ? max.Y : y);
			y = ((y < min.Y) ? min.Y : y);
			float z = v.Z;
			z = ((z > max.Z) ? max.Z : z);
			z = ((z < min.Z) ? min.Z : z);
			Vector3 result = default(Vector3);
			result.X = x;
			result.Y = y;
			result.Z = z;
			return result;
		}

		public static Vector3 Cross(Vector3 v1, Vector3 v2)
		{
			return new Vector3(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);
		}

		public static void Cross(ref Vector3 v1, ref Vector3 v2, out Vector3 r)
		{
			r = new Vector3(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);
		}

		public static float Dot(Vector3 v1, Vector3 v2)
		{
			return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
		}

		public static float Dot(ref Vector3 v1, ref Vector3 v2)
		{
			return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
		}

		public float Dot(ref Vector3 v)
		{
			return X * v.X + Y * v.Y + Z * v.Z;
		}

		public static Vector3 Transform(Vector3 v, Quaternion q)
		{
			Quaternion quaternion = new Quaternion(v, 0f);
			q = q * quaternion * Quaternion.Conjugate(q);
			return new Vector3(q.X, q.Y, q.Z);
		}

		public static Vector3 Transform(Vector3 v, ref Matrix m)
		{
			return new Vector3(v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31 + m.M41, v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32 + m.M42, v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33 + m.M43);
		}

		public static void Transform(ref Vector3 v, ref Matrix m, out Vector3 r)
		{
			r.X = v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31 + m.M41;
			r.Y = v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32 + m.M42;
			r.Z = v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33 + m.M43;
		}

		public static Vector3 TransformNormal(Vector3 v, ref Matrix m)
		{
			return new Vector3(v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31, v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32, v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33);
		}

		public static void Transform(Vector3[] v, ref Matrix m, Vector3[] r)
		{
			for (int i = 0; i < v.Length; i++)
			{
				float x = v[i].X;
				float y = v[i].Y;
				float z = v[i].Z;
				r[i].X = x * m.M11 + y * m.M21 + z * m.M31 + m.M41;
				r[i].Y = x * m.M12 + y * m.M22 + z * m.M32 + m.M42;
				r[i].Z = x * m.M13 + y * m.M23 + z * m.M33 + m.M43;
			}
		}

		public static Vector3[] Transform(Vector3[] v, ref Matrix m)
		{
			Vector3[] array = new Vector3[v.Length];
			Transform(v, ref m, array);
			return array;
		}

		public static void TransformNormal(Vector3[] v, ref Matrix m, Vector3[] r)
		{
			for (int i = 0; i < v.Length; i++)
			{
				float x = v[i].X;
				float y = v[i].Y;
				float z = v[i].Z;
				r[i].X = x * m.M11 + y * m.M21 + z * m.M31;
				r[i].Y = x * m.M12 + y * m.M22 + z * m.M32;
				r[i].Z = x * m.M13 + y * m.M23 + z * m.M33;
			}
		}

		public static Vector3[] TransformNormal(Vector3[] v, ref Matrix m)
		{
			Vector3[] array = new Vector3[v.Length];
			TransformNormal(v, ref m, array);
			return array;
		}

		public static Vector3 Min(Vector3 v1, Vector3 v2)
		{
			if (v2.X < v1.X)
			{
				v1.X = v2.X;
			}
			if (v2.Y < v1.Y)
			{
				v1.Y = v2.Y;
			}
			if (v2.Z < v1.Z)
			{
				v1.Z = v2.Z;
			}
			return v1;
		}

		public static void Min(ref Vector3 v1, ref Vector3 v2, out Vector3 r)
		{
			r.X = ((v1.X < v2.X) ? v1.X : v2.X);
			r.Y = ((v1.Y < v2.Y) ? v1.Y : v2.Y);
			r.Z = ((v1.Z < v2.Z) ? v1.Z : v2.Z);
		}

		public static Vector3 Max(Vector3 v1, Vector3 v2)
		{
			if (v2.X > v1.X)
			{
				v1.X = v2.X;
			}
			if (v2.Y > v1.Y)
			{
				v1.Y = v2.Y;
			}
			if (v2.Z > v1.Z)
			{
				v1.Z = v2.Z;
			}
			return v1;
		}

		public static void Max(ref Vector3 v1, ref Vector3 v2, out Vector3 r)
		{
			r.X = ((v1.X > v2.X) ? v1.X : v2.X);
			r.Y = ((v1.Y > v2.Y) ? v1.Y : v2.Y);
			r.Z = ((v1.Z > v2.Z) ? v1.Z : v2.Z);
		}

		public static Vector3 Normalize(Vector3 v)
		{
			return v * (1f / v.Length());
		}

		public void Normalize()
		{
			float num = 1f / Length();
			X *= num;
			Y *= num;
			Z *= num;
		}

		public float LengthSquared()
		{
			return X * X + Y * Y + Z * Z;
		}

		public float Length()
		{
			return FMath.Sqrt(LengthSquared());
		}

		public static float Distance(Vector3 v1, Vector3 v2)
		{
			return FMath.Sqrt((v2 - v1).LengthSquared());
		}

		public static float DistanceSquared(Vector3 v1, Vector3 v2)
		{
			return (v2 - v1).LengthSquared();
		}

		public static Vector3 Lerp(Vector3 v1, Vector3 v2, float amount)
		{
			return v1 + (v2 - v1) * amount;
		}

		public static bool EqualsEps(Vector3 v1, Vector3 v2)
		{
			Vector3 vector = v2 - v1;
			float num = Math.Abs(vector.X);
			float num2 = Math.Abs(vector.Y);
			float num3 = Math.Abs(vector.Z);
			if (num < 0.0001f && num2 < 0.0001f)
			{
				return num3 < 0.0001f;
			}
			return false;
		}

		public static bool operator ==(Vector3 v1, Vector3 v2)
		{
			if (v1.X == v2.X && v1.Y == v2.Y)
			{
				return v1.Z == v2.Z;
			}
			return false;
		}

		public static bool operator !=(Vector3 v1, Vector3 v2)
		{
			if (v1.X == v2.X && v1.Y == v2.Y)
			{
				return v1.Z != v2.Z;
			}
			return true;
		}

		public bool Equals(Vector3 other)
		{
			if (X == other.X && Y == other.Y)
			{
				return Z == other.Z;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is Vector3)
			{
				return Equals((Vector3)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("{{{0} {1} {2}}}", X, Y, Z);
		}
	}
}
