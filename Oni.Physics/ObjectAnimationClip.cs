namespace Oni.Physics
{
	internal class ObjectAnimationClip
	{
		public string Name;

		public int Start;

		public int End = int.MaxValue;

		public int Stop;

		public ObjectAnimationFlags Flags;

		public ObjectAnimationClip()
		{
		}

		public ObjectAnimationClip(string name)
		{
			Name = name;
		}
	}
}
