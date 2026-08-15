using System;

namespace Oni.Dae
{
	internal class TransformRotate : Transform
	{
		private static readonly string[] valueNames = new string[4] { "X", "Y", "Z", "ANGLE" };

		public Vector3 Axis
		{
			get
			{
				return new Vector3(base.Values);
			}
			set
			{
				value.CopyTo(base.Values);
			}
		}

		public float Angle
		{
			get
			{
				return base.Values[3];
			}
			set
			{
				base.Values[3] = value;
			}
		}

		public Sampler AngleAnimation
		{
			get
			{
				return GetAnimation(3);
			}
		}

		public TransformRotate()
			: base(4)
		{
		}

		public TransformRotate(Vector3 axis, float angle)
			: base(4)
		{
			Axis = axis;
			Angle = angle;
		}

		public TransformRotate(string sid, Vector3 axis, float angle)
			: base(sid, 4)
		{
			Axis = axis;
			Angle = angle;
		}

		public override Matrix ToMatrix()
		{
			return Matrix.CreateFromAxisAngle(Axis, MathHelper.ToRadians(Angle));
		}

		public Quaternion ToQuaternion()
		{
			return Quaternion.CreateFromAxisAngle(Axis, MathHelper.ToRadians(Angle));
		}

		public override int ValueNameToValueIndex(string name)
		{
			return Array.FindIndex(valueNames, (string x) => string.Equals(x, name, StringComparison.OrdinalIgnoreCase));
		}

		public override string ValueIndexToValueName(int index)
		{
			return valueNames[index];
		}
	}
}
