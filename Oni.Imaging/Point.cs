namespace Oni.Imaging
{
	internal struct Point
	{
		public static readonly Point UnitX = new Point(1, 0);

		public static readonly Point UnitY = new Point(0, 1);

		public int X;

		public int Y;

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		public static Point operator +(Point p0, Point p1)
		{
			return new Point(p0.X + p1.X, p0.Y + p1.Y);
		}

		public static Point operator -(Point p0, Point p1)
		{
			return new Point(p0.X - p1.X, p0.Y - p1.Y);
		}

		public static bool operator ==(Point p0, Point p1)
		{
			if (p0.X == p1.X)
			{
				return p0.Y == p1.Y;
			}
			return false;
		}

		public static bool operator !=(Point p0, Point p1)
		{
			if (p0.X == p1.X)
			{
				return p0.Y != p1.Y;
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			if (obj is Point)
			{
				return (Point)obj == this;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}
	}
}
