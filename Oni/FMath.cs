using System;

namespace Oni
{
	internal static class FMath
	{
		public static float Sign(float x)
		{
			if (x > 0f)
			{
				return 1f;
			}
			if (x < 0f)
			{
				return -1f;
			}
			return 0f;
		}

		public static float Sqrt(float x)
		{
			return (float)Math.Sqrt(x);
		}

		public static float Sqr(float x)
		{
			return x * x;
		}

		public static float Atan2(float y, float x)
		{
			return (float)Math.Atan2(y, x);
		}

		public static float Cos(float x)
		{
			return (float)Math.Cos(x);
		}

		public static float Sin(float x)
		{
			return (float)Math.Sin(x);
		}

		public static float Acos(float x)
		{
			return (float)Math.Acos(x);
		}

		public static float Round(float x, int digits)
		{
			return (float)Math.Round(x, digits);
		}

		public static int RoundToInt32(float f)
		{
			return (int)Math.Round(f);
		}

		public static int TruncateToInt32(float f)
		{
			return (int)Math.Truncate(f);
		}
	}
}
