using System;

namespace Oni.Objects
{
	[Flags]
	internal enum NeutralFlags : uint
	{
		None = 0u,
		NoResume = 1u,
		NoResumeAfterGive = 2u,
		Uninterruptible = 4u
	}
}
