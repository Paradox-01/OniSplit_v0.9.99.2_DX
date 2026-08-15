using System.Collections.Generic;

namespace Oni.Motoko
{
	internal class Quadify
	{
		private class Face
		{
			public readonly Geometry mesh;

			public readonly int[] indices;

			public readonly Edge[] edges;

			public readonly Vector3 normal;

			public Face(Geometry mesh, int[] pointIndices, Vector3 normal)
			{
				this.mesh = mesh;
				indices = pointIndices;
				this.normal = normal;
				edges = new Edge[indices.Length];
				for (int i = 0; i < edges.Length; i++)
				{
					edges[i] = new Edge(this, i);
				}
			}
		}

		private class Edge
		{
			private static readonly Edge[] emptyEdges = new Edge[0];

			public readonly Face face;

			public readonly int i0;

			public readonly int i1;

			public Edge[] adjacency;

			public int Point0Index
			{
				get
				{
					return face.indices[i0];
				}
			}

			public int Point1Index
			{
				get
				{
					return face.indices[i1];
				}
			}

			public Edge(Face polygon, int index)
			{
				face = polygon;
				i0 = index;
				i1 = (index + 1) % face.edges.Length;
				adjacency = emptyEdges;
			}

			public bool IsShared(Edge e)
			{
				if (Point0Index == e.Point1Index)
				{
					return Point1Index == e.Point0Index;
				}
				return false;
			}
		}

		private class QuadCandidateComparer : IComparer<QuadCandidate>
		{
			public int Compare(QuadCandidate x, QuadCandidate y)
			{
				return x.length.CompareTo(y.length);
			}
		}

		private class QuadCandidate
		{
			public readonly Edge e1;

			public readonly Edge e2;

			public readonly float length;

			public QuadCandidate(Edge e1, Edge e2)
			{
				this.e1 = e1;
				this.e2 = e2;
				Vector3[] points = e1.face.mesh.Points;
				length = (points[e1.Point0Index] - points[e1.Point1Index]).LengthSquared();
			}

			public int[] CreateQuad()
			{
				int[] array = new int[4];
				int num = 0;
				array[num] = e1.face.indices[e1.i1];
				num++;
				for (int i = 0; i < 3; i++)
				{
					if (i != e1.i0 && i != e1.i1)
					{
						array[num] = e1.face.indices[i];
						num++;
						break;
					}
				}
				array[num] = e1.face.indices[e1.i0];
				num++;
				for (int j = 0; j < 3; j++)
				{
					if (j != e2.i0 && j != e2.i1)
					{
						array[num] = e2.face.indices[j];
						num++;
						break;
					}
				}
				return array;
			}
		}

		private readonly Geometry mesh;

		private readonly List<Face> faces;

		public Quadify(Geometry mesh)
		{
			this.mesh = mesh;
			faces = new List<Face>();
			for (int i = 0; i < mesh.Triangles.Length; i += 3)
			{
				Plane plane = new Plane(mesh.Points[mesh.Triangles[i]], mesh.Points[mesh.Triangles[i + 1]], mesh.Points[mesh.Triangles[i + 2]]);
				faces.Add(new Face(mesh, new int[3]
				{
					mesh.Triangles[i],
					mesh.Triangles[i + 1],
					mesh.Triangles[i + 2]
				}, plane.Normal));
			}
		}

		public static List<int[]> Do(Geometry mesh)
		{
			Quadify quadify = new Quadify(mesh);
			return quadify.Execute();
		}

		public List<int[]> Execute()
		{
			GenerateAdjacency();
			List<QuadCandidate> list = new List<QuadCandidate>();
			int[][] array = new int[faces.Count][];
			bool[] array2 = new bool[faces.Count];
			int num = 0;
			for (int i = 0; i < faces.Count; i++)
			{
				Face face = faces[i];
				if (array2[i])
				{
					continue;
				}
				list.Clear();
				Edge[] edges = face.edges;
				foreach (Edge edge in edges)
				{
					Edge[] adjacency = edge.adjacency;
					foreach (Edge edge2 in adjacency)
					{
						if (!array2[faces.IndexOf(edge2.face)])
						{
							list.Add(new QuadCandidate(edge, edge2));
						}
					}
				}
				if (list.Count > 0)
				{
					list.Sort(new QuadCandidateComparer());
					array[i] = list[0].CreateQuad();
					int num2 = faces.IndexOf(list[0].e2.face);
					array2[i] = true;
					array2[num2] = true;
					num++;
				}
			}
			List<int[]> list2 = new List<int[]>(faces.Count - num);
			for (int l = 0; l < faces.Count; l++)
			{
				if (array[l] != null)
				{
					list2.Add(array[l]);
				}
				else if (!array2[l])
				{
					list2.Add(faces[l].indices);
				}
			}
			return list2;
		}

		private void GenerateAdjacency()
		{
			Vector3[] points = mesh.Points;
			int[] array = new int[points.Length];
			int[][] array2 = new int[points.Length][];
			foreach (Face face2 in faces)
			{
				int[] indices = face2.indices;
				foreach (int num in indices)
				{
					array[num]++;
				}
			}
			for (int j = 0; j < faces.Count; j++)
			{
				int[] indices2 = faces[j].indices;
				foreach (int num2 in indices2)
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
			List<Edge> list = new List<Edge>();
			foreach (Face face3 in faces)
			{
				Edge[] edges = face3.edges;
				foreach (Edge edge in edges)
				{
					int[] array4 = array2[edge.Point0Index];
					int[] array5 = array2[edge.Point1Index];
					if (array4 == null || array5 == null)
					{
						continue;
					}
					list.Clear();
					foreach (int item in MatchSortedArrays(array4, array5))
					{
						Face face = faces[item];
						if (face == face3 || (face.normal - face3.normal).Length() > 0.01f)
						{
							continue;
						}
						Edge[] edges2 = face.edges;
						foreach (Edge edge2 in edges2)
						{
							if (edge.IsShared(edge2))
							{
								list.Add(edge2);
							}
						}
					}
					edge.adjacency = list.ToArray();
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
