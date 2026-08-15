using System;
using System.Collections.Generic;

namespace Oni
{
	internal struct BoundingBox : IEquatable<BoundingBox>
	{
		public Vector3 Min;

		public Vector3 Max;

		public float Height
		{
			get
			{
				return Max.Y - Min.Y;
			}
		}

		public float Width
		{
			get
			{
				return Max.X - Min.X;
			}
		}

		public float Depth
		{
			get
			{
				return Max.Z - Min.Z;
			}
		}

		public Vector3 Size
		{
			get
			{
				return Max - Min;
			}
		}

		public BoundingBox(Vector3 min, Vector3 max)
		{
			Min = min;
			Max = max;
		}

		public static BoundingBox CreateFromSphere(BoundingSphere sphere)
		{
			Vector3 vector = new Vector3(sphere.Radius);
			return new BoundingBox(sphere.Center - vector, sphere.Center + vector);
		}

		public static BoundingBox CreateFromPoints(IEnumerable<Vector3> points)
		{
			Vector3 v = new Vector3(float.MaxValue);
			Vector3 v2 = new Vector3(float.MinValue);
			foreach (Vector3 point in points)
			{
				Vector3 v3 = point;
				Vector3.Min(ref v, ref v3, out v);
				Vector3.Max(ref v2, ref v3, out v2);
			}
			return new BoundingBox(v, v2);
		}

		public bool Contains(Vector3 point)
		{
			if (point.X >= Min.X && point.X <= Max.X && point.Y >= Min.Y && point.Y <= Max.Y && point.Z >= Min.Z)
			{
				return point.Z <= Max.Z;
			}
			return false;
		}

		public bool Contains(BoundingBox box)
		{
			if (Min.X <= box.Min.X && box.Max.X <= Max.X && Min.Y <= box.Min.Y && box.Max.Y <= Max.Y && Min.Z <= box.Min.Z)
			{
				return box.Max.Z <= Max.Z;
			}
			return false;
		}

		public bool Intersects(BoundingBox box)
		{
			if (Max.X >= box.Min.X && Min.X <= box.Max.X && Max.Y >= box.Min.Y && Min.Y <= box.Max.Y && Max.Z >= box.Min.Z)
			{
				return Min.Z <= box.Max.Z;
			}
			return false;
		}

		public bool Intersects(Plane plane)
		{
			Vector3 v = default(Vector3);
			v.X = ((plane.Normal.X >= 0f) ? Max.X : Min.X);
			Vector3 v2 = default(Vector3);
			v2.X = ((plane.Normal.X >= 0f) ? Min.X : Max.X);
			v.Y = ((plane.Normal.Y >= 0f) ? Max.Y : Min.Y);
			v2.Y = ((plane.Normal.Y >= 0f) ? Min.Y : Max.Y);
			v.Z = ((plane.Normal.Z >= 0f) ? Max.Z : Min.Z);
			v2.Z = ((plane.Normal.Z >= 0f) ? Min.Z : Max.Z);
			if (plane.Normal.Dot(ref v2) <= 0f - plane.D)
			{
				return plane.Normal.Dot(ref v) >= 0f - plane.D;
			}
			return false;
		}

		public Vector3[] GetCorners()
		{
			return new Vector3[8]
			{
				new Vector3(Min.X, Max.Y, Max.Z),
				new Vector3(Max.X, Max.Y, Max.Z),
				new Vector3(Max.X, Min.Y, Max.Z),
				new Vector3(Min.X, Min.Y, Max.Z),
				new Vector3(Min.X, Max.Y, Min.Z),
				new Vector3(Max.X, Max.Y, Min.Z),
				new Vector3(Max.X, Min.Y, Min.Z),
				new Vector3(Min.X, Min.Y, Min.Z)
			};
		}

		public static bool operator ==(BoundingBox b1, BoundingBox b2)
		{
			if (b1.Min == b2.Min)
			{
				return b1.Max == b2.Max;
			}
			return false;
		}

		public static bool operator !=(BoundingBox b1, BoundingBox b2)
		{
			if (!(b1.Min != b2.Min))
			{
				return b1.Max != b2.Max;
			}
			return true;
		}

		public bool Equals(BoundingBox other)
		{
			if (Min == other.Min)
			{
				return Max == other.Max;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is BoundingBox)
			{
				return Equals((BoundingBox)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Min.GetHashCode() ^ Max.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("{{{0} {1}}}", Min, Max);
		}

		public float Volume()
		{
			Vector3 vector = Max - Min;
			return vector.X * vector.Y * vector.Z;
		}

		public void Inflate(Vector3 v)
		{
			Min -= v;
			Max += v;
		}
	}
}
