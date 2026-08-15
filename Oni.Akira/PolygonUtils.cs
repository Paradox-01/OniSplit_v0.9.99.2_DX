using System;
using System.Collections.Generic;

namespace Oni.Akira
{
	internal class PolygonUtils
	{
		public static List<Vector3> ClipToPlane(List<Vector3> points, Plane plane)
		{
			int[] array = new int[points.Count];
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < points.Count; i++)
			{
				array[i] = RelativePosition(points[i], plane);
				if (array[i] >= 0)
				{
					num2++;
				}
				if (array[i] <= 0)
				{
					num++;
				}
			}
			if (num == points.Count)
			{
				return null;
			}
			if (num2 == points.Count)
			{
				return points;
			}
			List<Vector3> list = new List<Vector3>();
			for (int j = 0; j < points.Count; j++)
			{
				int num4 = (j + num3) % points.Count;
				int num5 = (j + num3 + 1) % points.Count;
				int num6 = array[num4];
				int num7 = array[num5];
				if (num6 >= 0)
				{
					list.Add(points[num4]);
					if (num6 > 0 && num7 < 0)
					{
						list.Add(Intersect(points[num4], points[num5], plane));
					}
				}
				else if (num6 < 0 && num7 > 0)
				{
					list.Add(Intersect(points[num5], points[num4], plane));
				}
			}
			return list;
		}

		private static Vector3 Intersect(Vector3 p0, Vector3 p1, Plane plane)
		{
			Vector3 vector = p1 - p0;
			float num = plane.DotNormal(vector);
			if (Math.Abs(num) < 1E-05f)
			{
				throw new InvalidOperationException();
			}
			float num2 = (0f - plane.D - plane.DotNormal(p0)) / num;
			if (num2 < 0f)
			{
				if (num2 < -1E-05f)
				{
					throw new InvalidOperationException();
				}
				return p0;
			}
			return p0 + vector * num2;
		}

		private static int RelativePosition(Vector3 point, Plane plane)
		{
			float num = plane.DotCoordinate(point);
			if (num < -1E-05f)
			{
				return -1;
			}
			if (num > 1E-05f)
			{
				return 1;
			}
			return 0;
		}
	}
}
