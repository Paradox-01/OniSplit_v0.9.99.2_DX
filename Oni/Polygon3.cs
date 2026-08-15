namespace Oni
{
	internal struct Polygon3
	{
		private readonly Vector3[] points;

		public int Length
		{
			get
			{
				return points.Length;
			}
		}

		public Vector3 this[int index]
		{
			get
			{
				return points[index];
			}
		}

		public Polygon3(Vector3[] points)
		{
			this.points = points;
		}
	}
}
