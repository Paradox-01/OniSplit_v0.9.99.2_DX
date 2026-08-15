namespace Oni.Akira
{
	internal class PolygonEdge
	{
		private static readonly PolygonEdge[] emptyEdges = new PolygonEdge[0];

		private readonly Polygon polygon;

		private readonly int index;

		private PolygonEdge[] adjacency = emptyEdges;

		public Polygon Polygon
		{
			get
			{
				return polygon;
			}
		}

		public int Index
		{
			get
			{
				return index;
			}
		}

		public int EndIndex
		{
			get
			{
				return (index + 1) % polygon.Edges.Length;
			}
		}

		public int Point0Index
		{
			get
			{
				return polygon.PointIndices[index];
			}
		}

		public int Point1Index
		{
			get
			{
				return polygon.PointIndices[EndIndex];
			}
		}

		public PolygonEdge[] Adjancency
		{
			get
			{
				return adjacency;
			}
			set
			{
				adjacency = value;
			}
		}

		public PolygonEdge(Polygon polygon, int index)
		{
			this.polygon = polygon;
			this.index = index;
		}
	}
}
