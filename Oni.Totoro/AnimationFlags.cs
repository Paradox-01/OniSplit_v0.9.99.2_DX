using System;

namespace Oni.Totoro
{
	[Flags]
	internal enum AnimationFlags
	{
		RuntimeLoaded = 1,
		Invulnerable = 2,
		BlockHigh = 4,
		BlockLow = 8,
		Attack = 0x10,
		DropWeapon = 0x20,
		InAir = 0x40,
		Atomic = 0x80,
		NoTurn = 0x100,
		AttackForward = 0x200,
		AttackLeft = 0x400,
		AttackRight = 0x800,
		AttackBackward = 0x1000,
		Overlay = 0x2000,
		DontInterpolateVelocity = 0x4000,
		ThrowSource = 0x8000,
		ThrowTarget = 0x10000,
		RealWorld = 0x20000,
		DoAim = 0x40000,
		DontAim = 0x80000,
		CanPickup = 0x100000,
		Aim360 = 0x200000,
		DisableShield = 0x400000,
		NoAIPickup = 0x800000
	}
}
