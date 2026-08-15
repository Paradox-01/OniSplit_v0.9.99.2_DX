namespace Oni.Akira
{
	internal class RoomAdjacency
	{
		private readonly Room adjacentRoom;

		private readonly Polygon ghost;

		public Room AdjacentRoom
		{
			get
			{
				return adjacentRoom;
			}
		}

		public Polygon Ghost
		{
			get
			{
				return ghost;
			}
		}

		public RoomAdjacency(Room room, Polygon ghost)
		{
			adjacentRoom = room;
			this.ghost = ghost;
		}
	}
}
