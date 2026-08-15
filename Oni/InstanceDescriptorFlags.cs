using System;

namespace Oni
{
	[Flags]
	internal enum InstanceDescriptorFlags
	{
		None = 0,
		Private = 1,
		Placeholder = 2,
		Shared = 8
	}
}
