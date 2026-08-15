using System;

namespace Oni
{
	internal struct Matrix : IEquatable<Matrix>
	{
		public float M11;

		public float M12;

		public float M13;

		public float M14;

		public float M21;

		public float M22;

		public float M23;

		public float M24;

		public float M31;

		public float M32;

		public float M33;

		public float M34;

		public float M41;

		public float M42;

		public float M43;

		public float M44;

		private static readonly Matrix identity = new Matrix(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);

		public Vector3 XAxis
		{
			get
			{
				return new Vector3(M11, M12, M13);
			}
			set
			{
				M11 = value.X;
				M12 = value.Y;
				M13 = value.Z;
			}
		}

		public Vector3 YAxis
		{
			get
			{
				return new Vector3(M21, M22, M23);
			}
			set
			{
				M21 = value.X;
				M22 = value.Y;
				M23 = value.Z;
			}
		}

		public Vector3 ZAxis
		{
			get
			{
				return new Vector3(M31, M32, M33);
			}
			set
			{
				M31 = value.X;
				M32 = value.Y;
				M33 = value.Z;
			}
		}

		public Vector3 Scale
		{
			get
			{
				return new Vector3(M11, M22, M33);
			}
			set
			{
				M11 = value.X;
				M22 = value.Y;
				M33 = value.Z;
			}
		}

		public Vector3 Translation
		{
			get
			{
				return new Vector3(M41, M42, M43);
			}
			set
			{
				M41 = value.X;
				M42 = value.Y;
				M43 = value.Z;
			}
		}

		public static Matrix Identity
		{
			get
			{
				return identity;
			}
		}

		public Matrix(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44)
		{
			M11 = m11;
			M12 = m12;
			M13 = m13;
			M14 = m14;
			M21 = m21;
			M22 = m22;
			M23 = m23;
			M24 = m24;
			M31 = m31;
			M32 = m32;
			M33 = m33;
			M34 = m34;
			M41 = m41;
			M42 = m42;
			M43 = m43;
			M44 = m44;
		}

		public Matrix(float[] values)
		{
			M11 = values[0];
			M12 = values[4];
			M13 = values[8];
			M14 = values[12];
			M21 = values[1];
			M22 = values[5];
			M23 = values[9];
			M24 = values[13];
			M31 = values[2];
			M32 = values[6];
			M33 = values[10];
			M34 = values[14];
			M41 = values[3];
			M42 = values[7];
			M43 = values[11];
			M44 = values[15];
		}

		public void CopyTo(float[] values)
		{
			values[0] = M11;
			values[1] = M21;
			values[2] = M31;
			values[3] = M41;
			values[4] = M12;
			values[5] = M22;
			values[6] = M32;
			values[7] = M42;
			values[8] = M13;
			values[9] = M23;
			values[10] = M33;
			values[11] = M43;
			values[12] = M14;
			values[13] = M24;
			values[14] = M34;
			values[15] = M44;
		}

		public static Matrix CreateTranslation(float x, float y, float z)
		{
			Matrix result = Identity;
			result.M41 = x;
			result.M42 = y;
			result.M43 = z;
			return result;
		}

		public static Matrix CreateTranslation(Vector3 v)
		{
			return CreateTranslation(v.X, v.Y, v.Z);
		}

		public static Matrix CreateScale(float sx, float sy, float sz)
		{
			Matrix result = Identity;
			result.M11 = sx;
			result.M22 = sy;
			result.M33 = sz;
			return result;
		}

		public static Matrix CreateScale(float s)
		{
			return CreateScale(s, s, s);
		}

		public static Matrix CreateScale(Vector3 s)
		{
			return CreateScale(s.X, s.Y, s.Z);
		}

		public static Matrix CreateRotationX(float angle)
		{
			float num = FMath.Cos(angle);
			float num2 = FMath.Sin(angle);
			Matrix result = Identity;
			result.M22 = num;
			result.M23 = num2;
			result.M32 = 0f - num2;
			result.M33 = num;
			return result;
		}

		public static Matrix CreateRotationY(float angle)
		{
			float num = FMath.Cos(angle);
			float num2 = FMath.Sin(angle);
			Matrix result = Identity;
			result.M11 = num;
			result.M13 = 0f - num2;
			result.M31 = num2;
			result.M33 = num;
			return result;
		}

		public static Matrix CreateRotationZ(float angle)
		{
			float num = FMath.Cos(angle);
			float num2 = FMath.Sin(angle);
			Matrix result = Identity;
			result.M11 = num;
			result.M12 = num2;
			result.M21 = 0f - num2;
			result.M22 = num;
			return result;
		}

		public static Matrix CreateFromAxisAngle(Vector3 axis, float angle)
		{
			float num = FMath.Sin(angle);
			float num2 = FMath.Cos(angle);
			float x = axis.X;
			float y = axis.Y;
			float z = axis.Z;
			float num3 = x * x;
			float num4 = y * y;
			float num5 = z * z;
			float num6 = x * y;
			float num7 = x * z;
			float num8 = y * z;
			Matrix result = Identity;
			result.M11 = num3 + num2 * (1f - num3);
			result.M12 = num6 - num2 * num6 + num * z;
			result.M13 = num7 - num2 * num7 - num * y;
			result.M21 = num6 - num2 * num6 - num * z;
			result.M22 = num4 + num2 * (1f - num4);
			result.M23 = num8 - num2 * num8 + num * x;
			result.M31 = num7 - num2 * num7 + num * y;
			result.M32 = num8 - num2 * num8 - num * x;
			result.M33 = num5 + num2 * (1f - num5);
			return result;
		}

		public static Matrix CreateFromQuaternion(Quaternion q)
		{
			float num = q.X * q.X;
			float num2 = q.Y * q.Y;
			float num3 = q.Z * q.Z;
			float num4 = q.X * q.Y;
			float num5 = q.Z * q.W;
			float num6 = q.Z * q.X;
			float num7 = q.Y * q.W;
			float num8 = q.Y * q.Z;
			float num9 = q.X * q.W;
			Matrix result = Identity;
			result.M11 = 1f - 2f * (num2 + num3);
			result.M12 = 2f * (num4 + num5);
			result.M13 = 2f * (num6 - num7);
			result.M21 = 2f * (num4 - num5);
			result.M22 = 1f - 2f * (num3 + num);
			result.M23 = 2f * (num8 + num9);
			result.M31 = 2f * (num6 + num7);
			result.M32 = 2f * (num8 - num9);
			result.M33 = 1f - 2f * (num2 + num);
			return result;
		}

		public static Matrix operator +(Matrix m1, Matrix m2)
		{
			m1.M11 += m2.M11;
			m1.M12 += m2.M12;
			m1.M13 += m2.M13;
			m1.M14 += m2.M14;
			m1.M21 += m2.M21;
			m1.M22 += m2.M22;
			m1.M23 += m2.M23;
			m1.M24 += m2.M24;
			m1.M31 += m2.M31;
			m1.M32 += m2.M32;
			m1.M33 += m2.M33;
			m1.M34 += m2.M34;
			m1.M41 += m2.M41;
			m1.M42 += m2.M42;
			m1.M43 += m2.M43;
			m1.M44 += m2.M44;
			return m1;
		}

		public static Matrix operator -(Matrix m1, Matrix m2)
		{
			m1.M11 -= m2.M11;
			m1.M12 -= m2.M12;
			m1.M13 -= m2.M13;
			m1.M14 -= m2.M14;
			m1.M21 -= m2.M21;
			m1.M22 -= m2.M22;
			m1.M23 -= m2.M23;
			m1.M24 -= m2.M24;
			m1.M31 -= m2.M31;
			m1.M32 -= m2.M32;
			m1.M33 -= m2.M33;
			m1.M34 -= m2.M34;
			m1.M41 -= m2.M41;
			m1.M42 -= m2.M42;
			m1.M43 -= m2.M43;
			m1.M44 -= m2.M44;
			return m1;
		}

		public static Matrix operator *(Matrix m, float s)
		{
			m.M11 *= s;
			m.M12 *= s;
			m.M13 *= s;
			m.M14 *= s;
			m.M21 *= s;
			m.M22 *= s;
			m.M23 *= s;
			m.M24 *= s;
			m.M31 *= s;
			m.M32 *= s;
			m.M33 *= s;
			m.M34 *= s;
			m.M41 *= s;
			m.M42 *= s;
			m.M43 *= s;
			m.M44 *= s;
			return m;
		}

		public static Matrix operator *(float s, Matrix m)
		{
			return m * s;
		}

		public static Matrix operator /(Matrix m, float s)
		{
			return m * (1f / s);
		}

		public static Matrix operator *(Matrix m1, Matrix m2)
		{
			Matrix result = default(Matrix);
			result.M11 = m1.M11 * m2.M11 + m1.M12 * m2.M21 + m1.M13 * m2.M31 + m1.M14 * m2.M41;
			result.M12 = m1.M11 * m2.M12 + m1.M12 * m2.M22 + m1.M13 * m2.M32 + m1.M14 * m2.M42;
			result.M13 = m1.M11 * m2.M13 + m1.M12 * m2.M23 + m1.M13 * m2.M33 + m1.M14 * m2.M43;
			result.M14 = m1.M11 * m2.M14 + m1.M12 * m2.M24 + m1.M13 * m2.M34 + m1.M14 * m2.M44;
			result.M21 = m1.M21 * m2.M11 + m1.M22 * m2.M21 + m1.M23 * m2.M31 + m1.M24 * m2.M41;
			result.M22 = m1.M21 * m2.M12 + m1.M22 * m2.M22 + m1.M23 * m2.M32 + m1.M24 * m2.M42;
			result.M23 = m1.M21 * m2.M13 + m1.M22 * m2.M23 + m1.M23 * m2.M33 + m1.M24 * m2.M43;
			result.M24 = m1.M21 * m2.M14 + m1.M22 * m2.M24 + m1.M23 * m2.M34 + m1.M24 * m2.M44;
			result.M31 = m1.M31 * m2.M11 + m1.M32 * m2.M21 + m1.M33 * m2.M31 + m1.M34 * m2.M41;
			result.M32 = m1.M31 * m2.M12 + m1.M32 * m2.M22 + m1.M33 * m2.M32 + m1.M34 * m2.M42;
			result.M33 = m1.M31 * m2.M13 + m1.M32 * m2.M23 + m1.M33 * m2.M33 + m1.M34 * m2.M43;
			result.M34 = m1.M31 * m2.M14 + m1.M32 * m2.M24 + m1.M33 * m2.M34 + m1.M34 * m2.M44;
			result.M41 = m1.M41 * m2.M11 + m1.M42 * m2.M21 + m1.M43 * m2.M31 + m1.M44 * m2.M41;
			result.M42 = m1.M41 * m2.M12 + m1.M42 * m2.M22 + m1.M43 * m2.M32 + m1.M44 * m2.M42;
			result.M43 = m1.M41 * m2.M13 + m1.M42 * m2.M23 + m1.M43 * m2.M33 + m1.M44 * m2.M43;
			result.M44 = m1.M41 * m2.M14 + m1.M42 * m2.M24 + m1.M43 * m2.M34 + m1.M44 * m2.M44;
			return result;
		}

		public Matrix Transpose()
		{
			Matrix result = default(Matrix);
			result.M11 = M11;
			result.M12 = M21;
			result.M13 = M31;
			result.M14 = M41;
			result.M21 = M12;
			result.M22 = M22;
			result.M23 = M32;
			result.M24 = M42;
			result.M31 = M13;
			result.M32 = M23;
			result.M33 = M33;
			result.M34 = M43;
			result.M41 = M14;
			result.M42 = M24;
			result.M43 = M34;
			result.M44 = M44;
			return result;
		}

		public static bool operator ==(Matrix m1, Matrix m2)
		{
			return m1.Equals(m2);
		}

		public static bool operator !=(Matrix m1, Matrix m2)
		{
			return !m1.Equals(m2);
		}

		public bool Equals(Matrix other)
		{
			if (M11 == other.M11 && M12 == other.M12 && M13 == other.M13 && M14 == other.M14 && M21 == other.M21 && M22 == other.M22 && M23 == other.M23 && M24 == other.M24 && M31 == other.M31 && M32 == other.M32 && M33 == other.M33 && M34 == other.M34 && M41 == other.M41 && M42 == other.M42 && M43 == other.M43)
			{
				return M44 == other.M44;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is Matrix)
			{
				return Equals((Matrix)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return M11.GetHashCode() ^ M12.GetHashCode() ^ M13.GetHashCode() ^ M14.GetHashCode() ^ M11.GetHashCode() ^ M12.GetHashCode() ^ M13.GetHashCode() ^ M14.GetHashCode() ^ M11.GetHashCode() ^ M12.GetHashCode() ^ M13.GetHashCode() ^ M14.GetHashCode() ^ M11.GetHashCode() ^ M12.GetHashCode() ^ M13.GetHashCode() ^ M14.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("{{M11:{0} M12:{1} M13:{2} M14:{3}}}\n{{M21:{4} M22:{5} M23:{6} M24:{7}}}\n{{M31:{8} M32:{9} M33:{10} M34:{11}}}\n{{M41:{12} M42:{13} M43:{14} M44:{15}}}", M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44);
		}

		public Vector3 ToEuler()
		{
			float m = M11;
			float m2 = M21;
			float num;
			float num2;
			float x;
			if (m2 == 0f)
			{
				num = FMath.Sign(m);
				num2 = 0f;
				x = Math.Abs(m);
			}
			else if (m == 0f)
			{
				num = 0f;
				num2 = FMath.Sign(m2);
				x = Math.Abs(m2);
			}
			else if (Math.Abs(m2) > Math.Abs(m))
			{
				float num3 = m / m2;
				float num4 = FMath.Sign(m2) * FMath.Sqrt(1f + num3 * num3);
				num2 = 1f / num4;
				num = num2 * num3;
				x = m2 * num4;
			}
			else
			{
				float num5 = m2 / m;
				float num6 = FMath.Sign(m) * FMath.Sqrt(1f + num5 * num5);
				num = 1f / num6;
				num2 = num * num5;
				x = m * num6;
			}
			Vector3 result = default(Vector3);
			result.Z = MathHelper.ToDegrees(0f - FMath.Atan2(num2, num));
			result.Y = MathHelper.ToDegrees(FMath.Atan2(M31, x));
			result.X = MathHelper.ToDegrees(0f - FMath.Atan2(M32, M33));
			return result;
		}

		public float Determinant()
		{
			float m = M11;
			float m2 = M12;
			float m3 = M13;
			float m4 = M14;
			float m5 = M21;
			float m6 = M22;
			float m7 = M23;
			float m8 = M24;
			float m9 = M31;
			float m10 = M32;
			float m11 = M33;
			float m12 = M34;
			float m13 = M41;
			float m14 = M42;
			float m15 = M43;
			float m16 = M44;
			float num = m11 * m16 - m12 * m15;
			float num2 = m10 * m16 - m12 * m14;
			float num3 = m10 * m15 - m11 * m14;
			float num4 = m9 * m16 - m12 * m13;
			float num5 = m9 * m15 - m11 * m13;
			float num6 = m9 * m14 - m10 * m13;
			return m * (m6 * num - m7 * num2 + m8 * num3) - m2 * (m5 * num - m7 * num4 + m8 * num5) + m3 * (m5 * num2 - m6 * num4 + m8 * num6) - m4 * (m5 * num3 - m6 * num5 + m7 * num6);
		}
	}
}
