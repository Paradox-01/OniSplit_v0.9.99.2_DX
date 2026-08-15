namespace Oni.Totoro
{
	internal class Position
	{
		public float X;

		public float Z;

		public float Height;

		public float YOffset;

		public Vector2 XZ
		{
			get
			{
				return new Vector2(X, Z);
			}
		}
	}
}
