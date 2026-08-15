using System;
using System.Collections.Generic;

namespace Oni
{
	internal struct BoundingSphere : IEquatable<BoundingSphere>
	{
		public Vector3 Center;

		public float Radius;

		public BoundingSphere(Vector3 center, float radius)
		{
			Center = center;
			Radius = radius;
		}

		public static BoundingSphere CreateFromBoundingBox(BoundingBox bbox)
		{
			BoundingSphere result = default(BoundingSphere);
			result.Center = (bbox.Min + bbox.Max) * 0.5f;
			result.Radius = Vector3.Distance(result.Center, bbox.Min);
			return result;
		}

		public static BoundingSphere CreateFromPoints(IEnumerable<Vector3> points)
		{
			Vector3 zero = Vector3.Zero;
			int num = 0;
			foreach (Vector3 point in points)
			{
				zero += point;
				num++;
			}
			zero /= (float)num;
			float num2 = 0f;
			foreach (Vector3 point2 in points)
			{
				float num3 = Vector3.DistanceSquared(zero, point2);
				if (num3 > num2)
				{
					num2 = num3;
				}
			}
			num2 = FMath.Sqrt(num2);
			return new BoundingSphere(zero, num2);
		}

		public static bool operator ==(BoundingSphere s1, BoundingSphere s2)
		{
			if (s1.Radius == s2.Radius)
			{
				return s1.Center == s2.Center;
			}
			return false;
		}

		public static bool operator !=(BoundingSphere s1, BoundingSphere s2)
		{
			if (s1.Radius == s2.Radius)
			{
				return s1.Center != s2.Center;
			}
			return true;
		}

		public bool Equals(BoundingSphere other)
		{
			if (other.Radius == Radius)
			{
				return other.Center == Center;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is BoundingSphere)
			{
				return Equals((BoundingSphere)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Radius.GetHashCode() ^ Center.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("{{{0} {1}}}", Center, Radius);
		}
	}
}
