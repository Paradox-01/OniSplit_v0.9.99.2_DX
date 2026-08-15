using System;

namespace Oni.Particles
{
	[Flags]
	internal enum ParticleFlags2 : uint
	{
		None = 0u,
		UseSpecialTint = 1u,
		DontAttractThroughWalls = 2u,
		ExpireOnCutscene = 8u,
		DieOnCutscene = 0x10u,
		DisableLevel0 = 0x20u,
		DisableLevel1 = 0x40u,
		DrawAsSky = 0x100000u,
		DecalFullBrightness = 0x200000u,
		Decal = 0x800000u,
		InitiallyHidden = 0x1000000u,
		Invisible = 0x2000000u,
		FadeOutOnEdge = 0x4000000u,
		Vector = 0x8000000u,
		LockPositionToLink = 0x10000000u,
		IsContrailEmitter = 0x20000000u,
		LensFlare = 0x40000000u,
		OneSidedEdgeFade = 0x80000000u,
		DisableLevelMask = DisableLevel0 | DisableLevel1
	}
}
