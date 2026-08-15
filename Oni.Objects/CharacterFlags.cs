using System;

namespace Oni.Objects
{
	[Flags]
	internal enum CharacterFlags : uint
	{
		None = 0u,
		IsPlayer = 1u,
		RandomCostume = 2u,
		NotInitiallyPresent = 4u,
		NonCombatant = 8u,
		CanSpawnMultiple = 0x10u,
		Spawned = 0x20u,
		Unkillable = 0x40u,
		InfiniteAmmo = 0x80u,
		Omniscient = 0x100u,
		HasLSI = 0x200u,
		Boss = 0x400u,
		UpgradeDifficulty = 0x800u,
		NoAutoDrop = 0x1000u
	}
}
