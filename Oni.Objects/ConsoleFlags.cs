using System;

namespace Oni.Objects
{
	[Flags]
	internal enum ConsoleFlags : ushort
	{
		None = 0,
		InitialActive = 8,
		Punch = 0x20,
		IsAlarm = 0x40
	}
}
