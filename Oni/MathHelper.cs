using System;

namespace Oni
{
	internal static class MathHelper
	{
		public const float Eps = 1E-05f;

		public const float Pi = 3.141593f;

		public const float HalfPi = 1.5707965f;

		public const float PiOver4 = 0.78539824f;

		public const float TwoPi = 6.283186f;

		public static float ToDegrees(float radians)
		{
			return radians * 57.295773f;
		}

		public static float ToRadians(float degrees)
		{
			return degrees * 0.017453294f;
		}

		public static float Distance(float v1, float v2)
		{
			return Math.Abs(v2 - v1);
		}

		public static float Lerp(float v1, float v2, float amount)
		{
			return v1 + (v2 - v1) * amount;
		}

		public static int Lerp(int v1, int v2, float amount)
		{
			if (amount == 0f)
			{
				return v1;
			}
			if (amount == 1f)
			{
				return v2;
			}
			return (int)((float)v1 + (float)(v2 - v1) * amount);
		}

		public static float Clamp(float v, float min, float max)
		{
			v = ((v > max) ? max : v);
			v = ((v < min) ? min : v);
			return v;
		}

		public static int Clamp(int v, int min, int max)
		{
			v = ((v > max) ? max : v);
			v = ((v < min) ? min : v);
			return v;
		}

		public static float Area(Vector2[] points)
		{
			float num = 0f;
			for (int i = 0; i < points.Length; i++)
			{
				int num2 = (i + 1) % points.Length;
				num += points[i].X * points[num2].Y;
				num -= points[i].Y * points[num2].X;
			}
			return Math.Abs(num * 0.5f);
		}
	}
}
