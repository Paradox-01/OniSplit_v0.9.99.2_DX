using System;

namespace Oni.Physics
{
	[Flags]
	internal enum ObjectAnimationFlags
	{
		None = 0,
		Loop = 1,
		PingPong = 2,
		RandomStart = 4,
		AutoStart = 8,
		Local = 0x10
	}
}
