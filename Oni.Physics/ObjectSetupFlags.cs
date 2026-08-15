using System;

namespace Oni.Physics
{
	[Flags]
	internal enum ObjectSetupFlags
	{
		None = 0,
		InUse = 0x200,
		NoCollision = 0x400,
		NoGravity = 0x800,
		FaceCollision = 0x1000
	}
}
