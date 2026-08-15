using System;
using System.Collections.Generic;

namespace Oni.Akira
{
	internal class PolygonQuadrangulate
	{
		private class QuadCandidate : IComparable<QuadCandidate>
		{
			private PolygonEdge e1;

			private PolygonEdge e2;

			private float l;

			public Polygon Polygon1
			{
				get
				{
					return e1.Polygon;
				}
			}

			public Polygon Polygon2
			{
				get
				{
					return e2.Polygon;
				}
			}

			public static bool IsQuadCandidate(PolygonEdge e1, PolygonEdge e2)
			{
				Polygon polygon = e1.Polygon;
				Polygon polygon2 = e2.Polygon;
				if (polygon.Edges.Length == 3 && polygon2.Edges.Length == 3 && polygon.Plane == polygon2.Plane)
				{
					return polygon.Material == polygon2.Material;
				}
				return false;
			}

			public QuadCandidate(PolygonEdge e1, PolygonEdge e2)
			{
				this.e1 = e1;
				this.e2 = e2;
				List<Vector3> points = e1.Polygon.Mesh.Points;
				l = Vector3.DistanceSquared(points[e1.Point0Index], points[e1.Point1Index]);
			}

			public int CompareTo(QuadCandidate other)
			{
				return l.CompareTo(other.l);
			}

			public Polygon CreateQuad(PolygonMesh mesh)
			{
				int[] array = new int[4];
				int[] array2 = new int[4];
				int num = 0;
				array[num] = e1.Polygon.PointIndices[e1.EndIndex];
				array2[num] = e1.Polygon.TexCoordIndices[e1.EndIndex];
				num++;
				for (int i = 0; i < 3; i++)
				{
					if (i != e1.Index && i != e1.EndIndex)
					{
						array[num] = e1.Polygon.PointIndices[i];
						array2[num] = e1.Polygon.TexCoordIndices[i];
						num++;
						break;
					}
				}
				array[num] = e1.Polygon.PointIndices[e1.Index];
				array2[num] = e1.Polygon.TexCoordIndices[e1.Index];
				num++;
				for (int j = 0; j < 3; j++)
				{
					if (j != e2.Index && j != e2.EndIndex)
					{
						array[num] = e2.Polygon.PointIndices[j];
						array2[num] = e2.Polygon.TexCoordIndices[j];
						num++;
						break;
					}
				}
				return new Polygon(mesh, array, e1.Polygon.Plane)
				{
					TexCoordIndices = array2,
					Material = e1.Polygon.Material
				};
			}
		}

		private readonly PolygonMesh mesh;

		public PolygonQuadrangulate(PolygonMesh mesh)
		{
			this.mesh = mesh;
		}

		public void Execute()
		{
			GenerateAdjacency();
			List<QuadCandidate> list = new List<QuadCandidate>();
			List<Polygon> polygons = mesh.Polygons;
			Polygon[] array = new Polygon[polygons.Count];
			bool[] array2 = new bool[polygons.Count];
			int num = 0;
			for (int i = 0; i < polygons.Count; i++)
			{
				Polygon polygon = polygons[i];
				if (array2[i] || polygon.Edges.Length != 3)
				{
					continue;
				}
				list.Clear();
				PolygonEdge[] edges = polygon.Edges;
				foreach (PolygonEdge polygonEdge in edges)
				{
					PolygonEdge[] adjancency = polygonEdge.Adjancency;
					foreach (PolygonEdge polygonEdge2 in adjancency)
					{
						if (!array2[polygons.IndexOf(polygonEdge2.Polygon)] && QuadCandidate.IsQuadCandidate(polygonEdge, polygonEdge2))
						{
							list.Add(new QuadCandidate(polygonEdge, polygonEdge2));
						}
					}
				}
				if (list.Count > 0)
				{
					list.Sort();
					array[i] = list[0].CreateQuad(mesh);
					int num2 = polygons.IndexOf(list[0].Polygon2);
					array2[i] = true;
					array2[num2] = true;
					num++;
				}
			}
			List<Polygon> list2 = new List<Polygon>(polygons.Count - num);
			for (int l = 0; l < polygons.Count; l++)
			{
				if (array[l] != null)
				{
					list2.Add(array[l]);
				}
				else if (!array2[l])
				{
					list2.Add(polygons[l]);
				}
			}
			polygons = list2;
		}

		private void GenerateAdjacency()
		{
			List<Vector3> points = mesh.Points;
			List<Polygon> polygons = mesh.Polygons;
			int[] array = new int[points.Count];
			int[][] array2 = new int[points.Count][];
			foreach (Polygon item in polygons)
			{
				int[] pointIndices = item.PointIndices;
				foreach (int num in pointIndices)
				{
					array[num]++;
				}
			}
			for (int j = 0; j < polygons.Count; j++)
			{
				int[] pointIndices2 = polygons[j].PointIndices;
				foreach (int num2 in pointIndices2)
				{
					int num3 = array[num2];
					int[] array3 = array2[num2];
					if (array3 == null)
					{
						array3 = (array2[num2] = new int[num3]);
					}
					array3[array3.Length - num3] = j;
					array[num2] = num3 - 1;
				}
			}
			List<PolygonEdge> list = new List<PolygonEdge>();
			foreach (Polygon item2 in polygons)
			{
				PolygonEdge[] edges = item2.Edges;
				foreach (PolygonEdge polygonEdge in edges)
				{
					list.Clear();
					int[] array4 = array2[polygonEdge.Point0Index];
					int[] array5 = array2[polygonEdge.Point1Index];
					if (array4 == null || array5 == null)
					{
						continue;
					}
					foreach (int item3 in MatchSortedArrays(array4, array5))
					{
						Polygon polygon = polygons[item3];
						if (polygon == item2)
						{
							continue;
						}
						PolygonEdge[] edges2 = polygon.Edges;
						foreach (PolygonEdge polygonEdge2 in edges2)
						{
							if ((polygonEdge.Point0Index == polygonEdge2.Point1Index && polygonEdge.Point1Index == polygonEdge2.Point0Index) || (polygonEdge.Point0Index == polygonEdge2.Point0Index && polygonEdge.Point1Index == polygonEdge2.Point1Index))
							{
								list.Add(polygonEdge2);
							}
						}
					}
					polygonEdge.Adjancency = list.ToArray();
				}
			}
		}

		private static IEnumerable<int> MatchSortedArrays(int[] a1, int[] a2)
		{
			int l1 = a1.Length;
			int l2 = a2.Length;
			int i1 = 0;
			int i2 = 0;
			while (i1 < l1 && i2 < l2)
			{
				int num = a1[i1];
				int num2 = a2[i2];
				if (num < num2)
				{
					i1++;
					continue;
				}
				if (num > num2)
				{
					i2++;
					continue;
				}
				i1++;
				i2++;
				yield return num;
			}
		}
	}
}
