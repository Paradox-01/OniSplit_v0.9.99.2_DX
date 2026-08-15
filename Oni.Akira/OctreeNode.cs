using System;
using System.Collections.Generic;

namespace Oni.Akira
{
	internal class OctreeNode
	{
		public enum Axis
		{
			Z,
			Y,
			X
		}

		public enum Direction
		{
			Negative,
			Positive
		}

		public struct Face
		{
			private readonly int index;

			public int Index
			{
				get
				{
					return index;
				}
			}

			public Axis Axis
			{
				get
				{
					return (Axis)(2 - ((index & 6) >> 1));
				}
			}

			public Direction Direction
			{
				get
				{
					return (Direction)(index & 1);
				}
			}

			public static IEnumerable<Face> All
			{
				get
				{
					for (int i = 0; i < 6; i++)
					{
						yield return new Face(i);
					}
				}
			}

			public Face(int index)
			{
				this.index = index;
			}
		}

		public struct ChildPosition
		{
			private int index;

			public int Index
			{
				get
				{
					return index;
				}
			}

			public int X
			{
				get
				{
					return this[Axis.X];
				}
			}

			public int Y
			{
				get
				{
					return this[Axis.Y];
				}
			}

			public int Z
			{
				get
				{
					return this[Axis.Z];
				}
			}

			public int this[Axis axis]
			{
				get
				{
					return (index >> (int)axis) & 1;
				}
				set
				{
					int num = 1 << (int)axis;
					if (value == 0)
					{
						index &= ~num;
					}
					else
					{
						index |= num;
					}
				}
			}

			public static IEnumerable<ChildPosition> All
			{
				get
				{
					for (int i = 0; i < 8; i++)
					{
						yield return new ChildPosition(i);
					}
				}
			}

			public ChildPosition(int index)
			{
				this.index = index;
			}
		}

		private struct TriangleBoxIntersector
		{
			private Vector3 center;

			private Vector3 size;

			private Vector3[] triangle;

			private Vector3 edge;

			private const int X = 0;

			private const int Y = 1;

			private const int Z = 2;

			public Vector3[] Triangle
			{
				get
				{
					return triangle;
				}
			}

			public TriangleBoxIntersector(ref BoundingBox box)
			{
				center = (box.Min + box.Max) * 0.5f;
				size = (box.Max - box.Min) * 0.5f;
				triangle = new Vector3[3];
				edge = Vector3.Zero;
			}

			public bool Intersect()
			{
				for (int i = 0; i < triangle.Length; i++)
				{
					triangle[i] -= center;
				}
				edge = triangle[1] - triangle[0];
				if (AxisTest(1, 2, 0, 2) || AxisTest(2, 0, 0, 2) || AxisTest(0, 1, 2, 1))
				{
					return false;
				}
				edge = triangle[2] - triangle[1];
				if (AxisTest(1, 2, 0, 2) || AxisTest(2, 0, 0, 2) || AxisTest(0, 1, 0, 1))
				{
					return false;
				}
				edge = triangle[0] - triangle[2];
				if (AxisTest(1, 2, 0, 1) || AxisTest(2, 0, 0, 1) || AxisTest(0, 1, 2, 1))
				{
					return false;
				}
				return true;
			}

			private bool AxisTest(int a1, int a2, int p0, int p1)
			{
				Vector3 vector = triangle[p0];
				Vector3 vector2 = triangle[p1];
				float num = edge[a1];
				float num2 = edge[a2];
				float num3 = num2 * vector[a1] - num * vector[a2];
				float num4 = num2 * vector2[a1] - num * vector2[a2];
				float num5 = Math.Abs(num2) * size[a1] + Math.Abs(num) * size[a2];
				if (!(num3 < num4))
				{
					if (!(num4 > num5))
					{
						return num3 < 0f - num5;
					}
					return true;
				}
				if (!(num3 > num5))
				{
					return num4 < 0f - num5;
				}
				return true;
			}
		}

		private struct PolygonBoxIntersector
		{
			private BoundingBox bbox;

			private TriangleBoxIntersector triangleBoxIntersector;

			public PolygonBoxIntersector(ref BoundingBox bbox)
			{
				this.bbox = bbox;
				triangleBoxIntersector = new TriangleBoxIntersector(ref bbox);
			}

			public bool Intersects(Polygon polygon)
			{
				if (!bbox.Intersects(polygon.BoundingBox))
				{
					return false;
				}
				if (!bbox.Intersects(polygon.Plane))
				{
					return false;
				}
				TriangleBoxIntersector triangleBoxIntersector = new TriangleBoxIntersector(ref bbox);
				List<Vector3> points = polygon.Mesh.Points;
				int[] pointIndices = polygon.PointIndices;
				triangleBoxIntersector.Triangle[0] = points[pointIndices[0]];
				triangleBoxIntersector.Triangle[1] = points[pointIndices[1]];
				triangleBoxIntersector.Triangle[2] = points[pointIndices[2]];
				if (triangleBoxIntersector.Intersect())
				{
					return true;
				}
				if (pointIndices.Length > 3)
				{
					triangleBoxIntersector.Triangle[0] = points[pointIndices[2]];
					triangleBoxIntersector.Triangle[1] = points[pointIndices[3]];
					triangleBoxIntersector.Triangle[2] = points[pointIndices[0]];
					if (triangleBoxIntersector.Intersect())
					{
						return true;
					}
				}
				return false;
			}
		}

		public const int FaceCount = 6;

		public const int ChildCount = 8;

		private const float MinNodeSize = 16f;

		private const int MaxQuadsPerLeaf = 4096;

		private const int MaxRoomsPerLeaf = 255;

		private int index;

		private BoundingBox bbox;

		private Polygon[] polygons;

		private OctreeNode[] children;

		private OctreeNode[] adjacency = new OctreeNode[6];

		private Room[] rooms;

		public int Index
		{
			get
			{
				return index;
			}
			set
			{
				index = value;
			}
		}

		public BoundingBox BoundingBox
		{
			get
			{
				return bbox;
			}
		}

		public OctreeNode[] Children
		{
			get
			{
				return children;
			}
		}

		public OctreeNode[] Adjacency
		{
			get
			{
				return adjacency;
			}
		}

		public bool IsLeaf
		{
			get
			{
				return polygons != null;
			}
		}

		public ICollection<Polygon> Polygons
		{
			get
			{
				return polygons;
			}
		}

		public ICollection<Room> Rooms
		{
			get
			{
				return rooms;
			}
		}

		private Vector3 Center
		{
			get
			{
				return (bbox.Min + bbox.Max) * 0.5f;
			}
		}

		private float Size
		{
			get
			{
				return bbox.Max.X - bbox.Min.X;
			}
		}

		public OctreeNode(BoundingBox bbox, IEnumerable<Polygon> polygons, IEnumerable<Room> rooms)
		{
			this.bbox = bbox;
			this.polygons = polygons.ToArray();
			this.rooms = rooms.ToArray();
		}

		private OctreeNode(BoundingBox bbox, Polygon[] polygons, Room[] rooms)
		{
			this.bbox = bbox;
			this.polygons = polygons;
			this.rooms = rooms;
		}

		public void Build()
		{
			BuildRecursive();
			if (children == null)
			{
				Split();
			}
		}

		private void BuildRecursive()
		{
			if ((polygons == null || polygons.Length <= 19) && (rooms == null || rooms.Length < 16))
			{
				return;
			}
			if (Size <= 16f)
			{
				if (polygons.Length > 4096)
				{
					throw new NotSupportedException(string.Format("Octtree: Quad density too big: current {0} max 4096 bbox {1}", polygons.Length, BoundingBox));
				}
				if (rooms.Length > 255)
				{
					throw new NotSupportedException(string.Format("Octtree: Room density too big: current {0} max 255 bbox {1}", rooms.Length, BoundingBox));
				}
			}
			else
			{
				Split();
			}
		}

		private void Split()
		{
			children = SplitCore();
			polygons = null;
			rooms = null;
			BuildSimpleAdjaceny();
			OctreeNode[] array = children;
			foreach (OctreeNode octreeNode in array)
			{
				octreeNode.BuildRecursive();
			}
		}

		private OctreeNode[] SplitCore()
		{
			OctreeNode[] array = new OctreeNode[8];
			Vector3 center = Center;
			List<Polygon> list = new List<Polygon>(polygons.Length);
			List<Room> list2 = new List<Room>(rooms.Length);
			foreach (ChildPosition item in ChildPosition.All)
			{
				BoundingBox boundingBox = new BoundingBox(center, center);
				if (item.X == 0)
				{
					boundingBox.Min.X = bbox.Min.X;
				}
				else
				{
					boundingBox.Max.X = bbox.Max.X;
				}
				if (item.Y == 0)
				{
					boundingBox.Min.Y = bbox.Min.Y;
				}
				else
				{
					boundingBox.Max.Y = bbox.Max.Y;
				}
				if (item.Z == 0)
				{
					boundingBox.Min.Z = bbox.Min.Z;
				}
				else
				{
					boundingBox.Max.Z = bbox.Max.Z;
				}
				list.Clear();
				list2.Clear();
				PolygonBoxIntersector polygonBoxIntersector = new PolygonBoxIntersector(ref boundingBox);
				Polygon[] array2 = polygons;
				foreach (Polygon polygon in array2)
				{
					if (polygonBoxIntersector.Intersects(polygon))
					{
						list.Add(polygon);
					}
				}
				Room[] array3 = rooms;
				foreach (Room room in array3)
				{
					if (room.Intersect(boundingBox))
					{
						list2.Add(room);
					}
				}
				array[item.Index] = new OctreeNode(boundingBox, list.ToArray(), list2.ToArray());
			}
			return array;
		}

		private void BuildSimpleAdjaceny()
		{
			foreach (ChildPosition item in ChildPosition.All)
			{
				OctreeNode octreeNode = children[item.Index];
				foreach (Face item2 in Face.All)
				{
					octreeNode.Adjacency[item2.Index] = GetChildAdjacency(item, item2);
				}
			}
		}

		private OctreeNode GetChildAdjacency(ChildPosition position, Face face)
		{
			if (face.Direction == Direction.Positive)
			{
				if (position[face.Axis] == 0)
				{
					position[face.Axis] = 1;
					return children[position.Index];
				}
			}
			else if (position[face.Axis] == 1)
			{
				position[face.Axis] = 0;
				return children[position.Index];
			}
			return adjacency[face.Index];
		}

		public void RefineAdjacency()
		{
			Vector3 center = Center;
			float size = Size;
			foreach (Face item in Face.All)
			{
				OctreeNode octreeNode = adjacency[item.Index];
				if (octreeNode != null && !octreeNode.IsLeaf && octreeNode.Size > Size)
				{
					Vector3 point = MovePoint(center, item, size);
					adjacency[item.Index] = octreeNode.FindLargestOrEqual(point, size);
				}
			}
		}

		public QuadtreeNode BuildFaceQuadTree(Face face)
		{
			Vector3 center = MovePoint(Center, face, Size * 0.5f);
			QuadtreeNode quadtreeNode = new QuadtreeNode(center, Size, face);
			quadtreeNode.Build(adjacency[face.Index]);
			return quadtreeNode;
		}

		public void DfsTraversal(Action<OctreeNode> action)
		{
			action(this);
			if (!IsLeaf)
			{
				OctreeNode[] array = children;
				foreach (OctreeNode octreeNode in array)
				{
					octreeNode.DfsTraversal(action);
				}
			}
		}

		public static Vector3 MovePoint(Vector3 point, Face face, float delta)
		{
			if (face.Direction == Direction.Negative)
			{
				delta = 0f - delta;
			}
			if (face.Axis == Axis.X)
			{
				point.X += delta;
			}
			else if (face.Axis == Axis.Y)
			{
				point.Y += delta;
			}
			else
			{
				point.Z += delta;
			}
			return point;
		}

		public OctreeNode FindLargestOrEqual(Vector3 point, float largestSize)
		{
			OctreeNode octreeNode = this;
			while (!octreeNode.IsLeaf && octreeNode.Size > largestSize)
			{
				Vector3 center = octreeNode.Center;
				int num = ((!(point.X < center.X)) ? 4 : 0);
				int num2 = ((!(point.Y < center.Y)) ? 2 : 0);
				int num3 = ((!(point.Z < center.Z)) ? 1 : 0);
				OctreeNode octreeNode2 = octreeNode.children[num + num2 + num3];
				if (octreeNode2.Size < largestSize)
				{
					break;
				}
				octreeNode = octreeNode2;
			}
			return octreeNode;
		}

		public OctreeNode FindLeaf(Vector3 point)
		{
			if (!bbox.Contains(point))
			{
				return null;
			}
			if (children == null)
			{
				return this;
			}
			Vector3 center = Center;
			int num = ((!(point.X < center.X)) ? 4 : 0);
			int num2 = ((!(point.Y < center.Y)) ? 2 : 0);
			int num3 = ((!(point.Z < center.Z)) ? 1 : 0);
			OctreeNode octreeNode = children[num + num2 + num3];
			return octreeNode.FindLeaf(point);
		}

		public IEnumerable<OctreeNode> FindLeafs(BoundingBox box)
		{
			Stack<OctreeNode> stack = new Stack<OctreeNode>();
			stack.Push(this);
			while (stack.Count > 0)
			{
				OctreeNode octreeNode = stack.Pop();
				if (!octreeNode.bbox.Intersects(box))
				{
					continue;
				}
				if (octreeNode.children != null)
				{
					OctreeNode[] array = octreeNode.children;
					foreach (OctreeNode item in array)
					{
						stack.Push(item);
					}
				}
				else
				{
					yield return octreeNode;
				}
			}
		}
	}
}
