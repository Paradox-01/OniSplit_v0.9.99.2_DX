namespace Oni.Akira
{
	internal class RoomBspNode : BspNode<RoomBspNode>
	{
		public RoomBspNode(Plane plane, RoomBspNode backChild, RoomBspNode frontChild)
			: base(plane, backChild, frontChild)
		{
		}
	}
}
