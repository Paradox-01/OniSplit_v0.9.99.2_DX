using System.Collections.Generic;
using Oni.Imaging;

namespace Oni.Akira
{
	internal class PolygonMesh
	{
		private readonly MaterialLibrary materialLibrary;

		private readonly List<Vector3> points = new List<Vector3>();

		private readonly List<Vector3> normals = new List<Vector3>();

		private readonly List<Vector2> texCoords = new List<Vector2>();

		private readonly List<Polygon> polygons = new List<Polygon>();

		private readonly List<Polygon> doors = new List<Polygon>();

		private readonly List<Room> rooms = new List<Room>();

		private readonly List<Polygon> ghosts = new List<Polygon>();

		private readonly List<Polygon> floors = new List<Polygon>();

		private bool hasDebugInfo;

		public MaterialLibrary Materials
		{
			get
			{
				return materialLibrary;
			}
		}

		public List<Vector3> Points
		{
			get
			{
				return points;
			}
		}

		public List<Vector2> TexCoords
		{
			get
			{
				return texCoords;
			}
		}

		public List<Vector3> Normals
		{
			get
			{
				return normals;
			}
		}

		public List<Polygon> Polygons
		{
			get
			{
				return polygons;
			}
		}

		public List<Polygon> Doors
		{
			get
			{
				return doors;
			}
		}

		public List<Room> Rooms
		{
			get
			{
				return rooms;
			}
		}

		public List<Polygon> Floors
		{
			get
			{
				return floors;
			}
		}

		public List<Polygon> Ghosts
		{
			get
			{
				return ghosts;
			}
		}

		public bool HasDebugInfo
		{
			get
			{
				return hasDebugInfo;
			}
			set
			{
				hasDebugInfo = value;
			}
		}

		public PolygonMesh(MaterialLibrary materialLibrary)
		{
			this.materialLibrary = materialLibrary;
		}

		public BoundingBox GetBoundingBox()
		{
			return BoundingBox.CreateFromPoints(points);
		}

		public void DoLighting()
		{
			Vector3 vector = new Vector3(0.6f, 0.6f, 0.6f);
			Vector3[] array = new Vector3[3]
			{
				new Vector3(-0.526f, -0.573f, -0.627f),
				new Vector3(0.719f, 0.342f, 0.604f),
				new Vector3(0.454f, 0.766f, 0.454f)
			};
			Vector3[] array2 = new Vector3[3]
			{
				new Vector3(1f, 1f, 1f),
				new Vector3(1f, 1f, 1f),
				new Vector3(1f, 1f, 1f)
			};
			foreach (Polygon polygon in polygons)
			{
				if (polygon.Colors != null)
				{
					continue;
				}
				Color[] array3 = new Color[polygon.VertexCount];
				if (polygon.NormalIndices != null)
				{
					for (int i = 0; i < array3.Length; i++)
					{
						Vector3 v = vector;
						for (int j = 0; j < array.Length; j++)
						{
							float num = Vector3.Dot(array[j], normals[polygon.NormalIndices[i]]);
							v += array2[j] * num;
						}
						array3[i] = new Color(Vector3.Clamp(v, Vector3.Zero, Vector3.One));
					}
				}
				else
				{
					Vector3 v2 = vector;
					for (int k = 0; k < array.Length; k++)
					{
						float num2 = Vector3.Dot(array[k], polygon.Plane.Normal);
						v2 += array2[k] * num2;
					}
					for (int l = 0; l < array3.Length; l++)
					{
						array3[l] = new Color(Vector3.Clamp(v2, Vector3.Zero, Vector3.One));
					}
				}
				polygon.Colors = array3;
			}
		}
	}
}
