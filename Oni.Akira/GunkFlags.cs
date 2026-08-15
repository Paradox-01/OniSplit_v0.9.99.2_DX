using System;

namespace Oni.Akira
{
	[Flags]
	internal enum GunkFlags : uint
	{
		None = 0u,
		DoorFrame = 1u,
		Ghost = 2u,
		StairsUp = 4u,
		StairsDown = 8u,
		Stairs = 0x10u,
		Transparent = 0x80u,
		TwoSided = 0x200u,
		NoCollision = 0x800u,
		Invisible = 0x2000u,
		NoObjectCollision = 0x4000u,
		NoCharacterCollision = 0x8000u,
		NoOcclusion = 0x10000u,
		Danger = 0x20000u,
		GridIgnore = 0x400000u,
		NoDecals = 0x800000u,
		Furniture = 0x1000000u,
		SoundTransparent = 0x8000000u,
		Impassable = 0x10000000u,
		Triangle = 0x40u,
		Horizontal = 0x80000u,
		Vertical = 0x100000u,
		ProjectionBit0 = 0x2000000u,
		ProjectionBit1 = 0x4000000u
	}
}
