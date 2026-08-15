using System.Collections.Generic;

namespace Oni.Totoro
{
	internal class Attack
	{
		public int Start;

		public int End;

		public BoneMask Bones;

		public AttackFlags Flags;

		public float Knockback;

		public int HitPoints;

		public AnimationType HitType;

		public int HitLength;

		public int StunLength;

		public int StaggerLength;

		public readonly List<AttackExtent> Extents = new List<AttackExtent>();
	}
}
