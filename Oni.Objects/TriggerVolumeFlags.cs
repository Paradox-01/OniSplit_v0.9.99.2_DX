using System;

namespace Oni.Objects
{
	[Flags]
	internal enum TriggerVolumeFlags : uint
	{
		None = 0u,
		OneTimeEnter = 1u,
		OneTimeInside = 2u,
		OneTimeExit = 4u,
		EnterDisabled = 8u,
		InsideDisabled = 0x10u,
		ExitDisabled = 0x20u,
		Disabled = 0x40u,
		PlayerOnly = 0x80u
	}
}
