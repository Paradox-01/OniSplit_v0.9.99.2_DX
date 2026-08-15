using System;

namespace Oni.Dae
{
	internal class TransformScale : Transform
	{
		private static readonly string[] valueNames = new string[3] { "X", "Y", "Z" };

		public Vector3 Scale
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

		public TransformScale()
			: base(3)
		{
		}

		public TransformScale(string sid, Vector3 scale)
			: base(sid, 3)
		{
			Scale = scale;
		}

		public override Matrix ToMatrix()
		{
			return Matrix.CreateScale(Scale);
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
