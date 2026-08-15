namespace Oni
{
	internal struct Polygon2
	{
		private readonly Vector2[] points;

		public int Length
		{
			get
			{
				return points.Length;
			}
		}

		public Vector2 this[int index]
		{
			get
			{
				return points[index];
			}
		}

		public Polygon2(Vector2[] points)
		{
			this.points = points;
		}
	}
}
