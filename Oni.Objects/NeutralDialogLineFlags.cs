using System;

namespace Oni.Objects
{
	[Flags]
	internal enum NeutralDialogLineFlags : ushort
	{
		None = 0,
		IsPlayer = 1,
		GiveItems = 2,
		AnimOnce = 4,
		OtherAnimOnce = 8
	}
}
