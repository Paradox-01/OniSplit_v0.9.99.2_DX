using System;

namespace Oni.Objects
{
	[Flags]
	internal enum ParticleFlags : ushort
	{
		None = 0,
		NotInitiallyCreated = 2
	}
}
