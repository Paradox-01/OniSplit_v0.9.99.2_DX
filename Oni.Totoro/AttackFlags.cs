using System;

namespace Oni.Totoro
{
	[Flags]
	internal enum AttackFlags
	{
		None = 0,
		Unblockable = 1,
		High = 2,
		Low = 4,
		HalfDamage = 8
	}
}
