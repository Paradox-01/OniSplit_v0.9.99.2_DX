using System;

namespace Oni.Particles
{
	[Flags]
	internal enum ParticleFlags1
	{
		None = 0,
		Decorative = 1,
		UseSeparateYScale = 8,
		SpriteMode0 = 0x20,
		SpriteMode1 = 0x40,
		SpriteMode2 = 0x80,
		Geometry = 0x100,
		CollideWithWalls = 0x200,
		CollideWithChars = 0x400,
		ScaleToVelocity = 0x800,
		HasVelocity = 0x1000,
		HasOrientation = 0x2000,
		HasPositionOffset = 0x4000,
		HasAttachmentMatrix = 0x8000,
		HasUnknown = 0x10000,
		HasDecalState = 0x20000,
		HasTextureStartTick = 0x40000,
		HasTextureTick = 0x80000,
		HasDamageOwner = 0x100000,
		HasContrailData = 0x200000,
		HasLensFlareState = 0x400000,
		HasAttractor = 0x800000,
		HasCollisionCache = 0x1000000,
		SpriteModeMask = SpriteMode0 | SpriteMode1 | SpriteMode2
	}
}
