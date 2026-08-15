namespace Oni.Level
{
	internal class Corpse
	{
		public bool IsFixed;

		public bool IsUsed;

		public string FileName;

		public string CharacterClass;

		public readonly Matrix[] Transforms = new Matrix[19];

		public BoundingBox BoundingBox;

		public int Order
		{
			get
			{
				if (IsFixed)
				{
					return 1;
				}
				if (IsUsed)
				{
					return 2;
				}
				return 3;
			}
		}
	}
}
