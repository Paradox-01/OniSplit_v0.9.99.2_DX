using System;

namespace Oni
{
	internal struct Plane : IEquatable<Plane>
	{
		public Vector3 Normal;

		public float D;

		public Plane(Vector3 normal, float d)
		{
			Normal = normal;
			D = d;
		}

		public Plane(Vector3 point1, Vector3 point2, Vector3 point3)
		{
			Normal = Vector3.Normalize(Vector3.Cross(point2 - point1, point3 - point1));
			D = 0f - Vector3.Dot(Normal, point1);
		}

		public float DotCoordinate(Vector3 point)
		{
			return Vector3.Dot(Normal, point) + D;
		}

		public float DotNormal(Vector3 value)
		{
			return Vector3.Dot(Normal, value);
		}

		public void Flip()
		{
			Normal = -Normal;
			D = 0f - D;
		}

		public static Plane Flip(Plane plane)
		{
			plane.Normal = -plane.Normal;
			plane.D = 0f - plane.D;
			return plane;
		}

		public static bool operator ==(Plane p1, Plane p2)
		{
			if (p1.D == p2.D)
			{
				return p1.Normal == p2.Normal;
			}
			return false;
		}

		public static bool operator !=(Plane p1, Plane p2)
		{
			if (p1.D == p2.D)
			{
				return p1.Normal != p2.Normal;
			}
			return true;
		}

		public bool Equals(Plane other)
		{
			if (other.D == D)
			{
				return other.Normal == Normal;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is Plane)
			{
				return Equals((Plane)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Normal.GetHashCode() ^ D.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("{{Normal:{0} D:{1}}}", Normal, D);
		}

		public int Intersects(BoundingBox box)
		{
			Vector3 v = default(Vector3);
			Vector3 v2 = default(Vector3);
			if (Normal.X >= 0f)
			{
				v.X = box.Min.X;
				v2.X = box.Max.X;
			}
			else
			{
				v.X = box.Max.X;
				v2.X = box.Min.X;
			}
			if (Normal.Y >= 0f)
			{
				v.Y = box.Min.Y;
				v2.Y = box.Max.Y;
			}
			else
			{
				v.Y = box.Max.Y;
				v2.Y = box.Min.Y;
			}
			if (Normal.Z >= 0f)
			{
				v.Z = box.Min.Z;
				v2.Z = box.Max.Z;
			}
			else
			{
				v.Z = box.Max.Z;
				v2.Z = box.Min.Z;
			}
			if (Vector3.Dot(Normal, v) + D > 0f)
			{
				return 1;
			}
			if (Vector3.Dot(Normal, v2) + D < 0f)
			{
				return -1;
			}
			return 0;
		}
	}
}
