using System;

namespace Oni.Objects
{
	[Flags]
	internal enum TriggerFlags : ushort
	{
		None = 0,
		InitialActive = 8,
		ReverseAnim = 0x10,
		PingPong = 0x20
	}
}
