using System;

namespace Oni.Objects
{
	[Flags]
	internal enum TurretTargetTeams : uint
	{
		None = 0u,
		Konoko = 1u,
		TCTF = 2u,
		Syndicate = 4u,
		Neutral = 8u,
		SecurityGuard = 0x10u,
		RogueKonoko = 0x20u,
		Switzerland = 0x40u,
		SyndicateAccessory = 0x80u
	}
}
