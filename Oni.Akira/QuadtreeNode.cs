using System.Collections.Generic;

namespace Oni.Akira
{
	internal class QuadtreeNode
	{
		public enum Axis
		{
			U,
			V
		}

		public struct ChildPosition
		{
			private readonly int index;

			public int Index
			{
				get
				{
					return index;
				}
			}

			public int U
			{
				get
				{
					return this[Axis.U];
				}
			}

			public int V
			{
				get
				{
					return this[Axis.V];
				}
			}

			public int this[Axis axis]
			{
				get
				{
					return (index >> (int)axis) & 1;
				}
			}

			public static IEnumerable<ChildPosition> All
			{
				get
				{
					for (int i = 0; i < 4; i++)
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

		public const int ChildCount = 4;

		private QuadtreeNode[] nodes = new QuadtreeNode[4];

		private OctreeNode[] leafs = new OctreeNode[4];

		private readonly Vector3 center;

		private readonly float size;

		private readonly OctreeNode.Face face;

		private int index;

		public QuadtreeNode[] Nodes
		{
			get
			{
				return nodes;
			}
		}

		public OctreeNode[] Leafs
		{
			get
			{
				return leafs;
			}
		}

		public int Index
		{
			get
			{
				return index;
			}
		}

		public QuadtreeNode(Vector3 center, float size, OctreeNode.Face face)
		{
			this.center = center;
			this.size = size;
			this.face = face;
		}

		public void Build(OctreeNode adjacentNode)
		{
			float num = size * 0.5f;
			foreach (ChildPosition item in ChildPosition.All)
			{
				Vector3 childNodeCenter = GetChildNodeCenter(item);
				Vector3 point = OctreeNode.MovePoint(childNodeCenter, face, num * 0.5f);
				OctreeNode octreeNode = adjacentNode.FindLargestOrEqual(point, num);
				if (octreeNode.IsLeaf)
				{
					leafs[item.Index] = octreeNode;
					continue;
				}
				QuadtreeNode quadtreeNode = new QuadtreeNode(childNodeCenter, num, face);
				quadtreeNode.Build(octreeNode);
				nodes[item.Index] = quadtreeNode;
			}
		}

		private Vector3 GetChildNodeCenter(ChildPosition position)
		{
			float num = size * 0.25f;
			float num2 = ((position.U == 0) ? (0f - num) : num);
			float num3 = ((position.V == 0) ? (0f - num) : num);
			Vector3 result = center;
			if (face.Axis == OctreeNode.Axis.X)
			{
				result.Y += num2;
				result.Z += num3;
			}
			else if (face.Axis == OctreeNode.Axis.Y)
			{
				result.X += num2;
				result.Z += num3;
			}
			else
			{
				result.X += num2;
				result.Y += num3;
			}
			return result;
		}

		public List<QuadtreeNode> GetDfsList()
		{
			List<QuadtreeNode> list = new List<QuadtreeNode>();
			DfsTraversal(delegate(QuadtreeNode node)
			{
				node.index = list.Count;
				list.Add(node);
			});
			return list;
		}

		private void DfsTraversal(Action<QuadtreeNode> action)
		{
			action(this);
			QuadtreeNode[] array = nodes;
			foreach (QuadtreeNode quadtreeNode in array)
			{
				if (quadtreeNode != null)
				{
					quadtreeNode.DfsTraversal(action);
				}
			}
		}
	}
}
