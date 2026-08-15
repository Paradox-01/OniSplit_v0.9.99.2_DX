using System;

namespace Oni.Totoro
{
	[Flags]
	internal enum BoneMask : uint
	{
		None = 0u,
		Pelvis = 1u,
		LeftThigh = 2u,
		LeftCalf = 4u,
		LeftFoot = 8u,
		RightThigh = 0x10u,
		RightCalf = 0x20u,
		RightFoot = 0x40u,
		Mid = 0x80u,
		Chest = 0x100u,
		Neck = 0x200u,
		Head = 0x400u,
		LeftShoulder = 0x800u,
		LeftArm = 0x1000u,
		LeftWrist = 0x2000u,
		LeftFist = 0x4000u,
		RightShoulder = 0x8000u,
		RightArm = 0x10000u,
		RightWrist = 0x20000u,
		RightFist = 0x40000u
	}
}
