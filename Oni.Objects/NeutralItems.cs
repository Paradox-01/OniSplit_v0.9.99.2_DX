using System;

namespace Oni.Objects
{
	[Flags]
	internal enum NeutralItems : byte
	{
		None = 0,
		Shield = 1,
		Invisibility = 2,
		LSI = 4
	}
}
