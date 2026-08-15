using System;
using System.Collections.Generic;

namespace Oni.Akira
{
	internal class AlphaBspBuilder
	{
		private PolygonMesh mesh;

		private bool debug;

		public static AlphaBspNode Build(PolygonMesh mesh, bool debug)
		{
			AlphaBspBuilder alphaBspBuilder = new AlphaBspBuilder
			{
				mesh = mesh,
				debug = debug
			};
			return alphaBspBuilder.Build();
		}

		private AlphaBspNode Build()
		{
			List<Polygon> list = new List<Polygon>(1024);
			list.AddRange(mesh.Polygons.Where((Polygon p) => p.IsTransparent));
			if (debug)
			{
				list.AddRange(mesh.Ghosts.Where((Polygon p) => p.IsTransparent));
			}
			Console.Error.WriteLine("Building bsp tree for {0} transparent polygons...", list.Count);
			return Build(list);
		}

		private AlphaBspNode Build(List<Polygon> polygons)
		{
			if (polygons.Count == 0)
			{
				return null;
			}
			Plane plane = polygons[0].Plane;
			AlphaBspNode frontChild = null;
			AlphaBspNode backChild = null;
			if (polygons.Count > 1)
			{
				List<Polygon> list = new List<Polygon>(polygons.Count);
				List<Polygon> list2 = new List<Polygon>(polygons.Count);
				for (int i = 1; i < polygons.Count; i++)
				{
					Polygon polygon = polygons[i];
					Plane plane2 = polygon.Plane;
					bool flag = false;
					bool flag2 = false;
					if (Math.Abs(plane2.D - plane.D) < 0.001f && Vector3.Distance(plane2.Normal, plane.Normal) < 0.001f)
					{
						flag = true;
					}
					else
					{
						foreach (Vector3 point in polygon.Points)
						{
							if (plane.DotCoordinate(point) > 0f)
							{
								flag = true;
							}
							else
							{
								flag2 = true;
							}
						}
					}
					if (flag)
					{
						list.Add(polygon);
					}
					if (flag2)
					{
						list2.Add(polygon);
					}
				}
				frontChild = Build(list);
				backChild = Build(list2);
			}
			return new AlphaBspNode(polygons[0], frontChild, backChild);
		}
	}
}
