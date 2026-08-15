using System;

namespace Oni.Particles
{
	[Flags]
	internal enum EmitterFlags
	{
		None = 0,
		InitiallyOn = 1,
		IncreaseParticleCount = 2,
		TurnOffAtTreshold = 4,
		EmitWithParentVelocity = 0x10,
		Unknown0020 = 0x20,
		OrientToVelocity = 0x40,
		InheritTint = 0x80,
		OnePerAttractor = 0x100,
		AtLeastOne = 0x200,
		CycleAttractors = 0x400
	}
}
