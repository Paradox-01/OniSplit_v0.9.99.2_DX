using System;

namespace Oni.Totoro
{
	[Flags]
	internal enum AnimationVarient
	{
		None = 0,
		Sprint = 0x100,
		Combat = 0x200,
		RightPistol = 0x800,
		LeftPistol = 0x1000,
		RightRifle = 0x2000,
		LeftRifle = 0x4000,
		Panic = 0x8000
	}
}
