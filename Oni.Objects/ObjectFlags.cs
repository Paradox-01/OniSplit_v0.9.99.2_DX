using System;

namespace Oni.Objects
{
	[Flags]
	internal enum ObjectFlags
	{
		None = 0,
		Locked = 1,
		PlacedInGame = 2,
		Temporary = 4,
		Gunk = 8
	}
}
