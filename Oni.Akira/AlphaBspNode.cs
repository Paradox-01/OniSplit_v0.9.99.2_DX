namespace Oni.Akira
{
	internal class AlphaBspNode : BspNode<AlphaBspNode>
	{
		public readonly Polygon Polygon;

		public AlphaBspNode(Polygon polygon, AlphaBspNode frontChild, AlphaBspNode backChild)
			: base(polygon.Plane, frontChild, backChild)
		{
			Polygon = polygon;
		}
	}
}
