using System;
using System.Collections.Generic;

namespace Oni.Akira
{
	internal struct Polygon2Clipper
	{
		private struct Line
		{
			private float a;

			private float c;

			private float d;

			public Line(Plane plane)
			{
				a = plane.Normal.X;
				c = plane.Normal.Z;
				d = plane.D;
			}

			public int RelativePosition(Vector2 point)
			{
				float num = a * point.X + c * point.Y + d;
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

			public Vector2 Intersect(Vector2 p0, Vector2 p1)
			{
				if (p0.X == p1.X)
				{
					float x = p0.X;
					float y = (d + a * x) / (0f - c);
					return new Vector2(x, y);
				}
				if (p0.Y == p1.Y)
				{
					float x2 = ((0f - c) * p0.Y - d) / a;
					float y2 = p0.Y;
					return new Vector2(x2, y2);
				}
				float num = (p1.Y - p0.Y) / (p1.X - p0.X);
				float num2 = (c * num * p0.X - c * p0.Y - d) / (a + c * num);
				float y3 = num * (num2 - p0.X) + p0.Y;
				return new Vector2(num2, y3);
			}
		}

		private readonly List<Polygon2> result;

		private readonly RoomBspNode bspTree;

		public Polygon2Clipper(RoomBspNode bspTree)
		{
			result = new List<Polygon2>();
			this.bspTree = bspTree;
		}

		public IEnumerable<Polygon2> Clip(Polygon2 polygon)
		{
			result.Clear();
			Clip(new Polygon2[1] { polygon }, bspTree);
			return result;
		}

		private void Clip(IEnumerable<Polygon2> polygons, RoomBspNode node)
		{
			List<Polygon2> list = new List<Polygon2>();
			List<Polygon2> list2 = new List<Polygon2>();
			Plane plane = node.Plane;
			if (Math.Abs(plane.Normal.Y) > 0.001f)
			{
				list.AddRange(polygons);
				list2.AddRange(polygons);
			}
			else
			{
				Line line = new Line(plane);
				foreach (Polygon2 polygon in polygons)
				{
					Clip(polygon, line, list, list2);
				}
			}
			if (node.FrontChild != null)
			{
				Clip(list2, node.FrontChild);
			}
			if (node.BackChild != null)
			{
				Clip(list, node.BackChild);
			}
			else
			{
				result.AddRange(list);
			}
		}

		private static void Clip(Polygon2 polygon, Line line, List<Polygon2> negative, List<Polygon2> positive)
		{
			int[] array = new int[polygon.Length];
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < polygon.Length; i++)
			{
				array[i] = line.RelativePosition(polygon[i]);
				if (array[i] >= 0)
				{
					num++;
				}
				if (array[i] <= 0)
				{
					num2++;
				}
			}
			if (num2 == polygon.Length)
			{
				negative.Add(polygon);
				return;
			}
			if (num == polygon.Length)
			{
				positive.Add(polygon);
				return;
			}
			List<Vector2> list = new List<Vector2>();
			List<Vector2> list2 = new List<Vector2>();
			int num3 = 0;
			Vector2 vector;
			int num4;
			do
			{
				vector = polygon[num3];
				num4 = array[num3];
				num3++;
			}
			while (num4 == 0);
			Vector2[] array2 = new Vector2[2];
			int num5 = 0;
			for (int j = 0; j < polygon.Length; j++)
			{
				Vector2 vector2 = polygon[(j + num3) % polygon.Length];
				int num6 = array[(j + num3) % polygon.Length];
				if (num4 == num6)
				{
					if (num4 < 0)
					{
						list.Add(vector);
					}
					else
					{
						list2.Add(vector);
					}
				}
				else if (num4 == 0)
				{
					if (num6 < 0)
					{
						list.Add(vector);
					}
					else
					{
						list2.Add(vector);
					}
				}
				else
				{
					Vector2 vector3 = ((num6 != 0) ? line.Intersect(vector, vector2) : vector2);
					array2[num5++] = vector3;
					if (num4 < 0)
					{
						list.Add(vector);
						if (num5 == 2)
						{
							list.Add(vector3);
							list2.Add(array2[0]);
						}
						if (num6 != 0)
						{
							list2.Add(vector3);
						}
					}
					else
					{
						list2.Add(vector);
						if (num5 == 2)
						{
							list2.Add(vector3);
							list.Add(array2[0]);
						}
						if (num6 != 0)
						{
							list.Add(vector3);
						}
					}
				}
				vector = vector2;
				num4 = num6;
			}
			negative.Add(new Polygon2(list.ToArray()));
			positive.Add(new Polygon2(list2.ToArray()));
		}
	}
}
