using System;

namespace Oni.Akira
{
	[Flags]
	internal enum RoomFlags
	{
		None = 0,
		Stairs = 1,
		Room = 4,
		Simple = 0x10
	}
}
