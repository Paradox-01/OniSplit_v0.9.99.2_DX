using System;

namespace Oni.Objects
{
	[Flags]
	internal enum DoorFlags : ushort
	{
		None = 0,
		InitialLocked = 1,
		InDoorFrame = 4,
		Manual = 0x10,
		DoubleDoor = 0x80,
		Mirror = 0x100,
		OneWay = 0x200,
		Reverse = 0x400,
		Jammed = 0x800,
		InitialOpen = 0x1000
	}
}
