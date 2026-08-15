using System;

namespace Oni.Dae
{
	internal class TransformTranslate : Transform
	{
		private static readonly string[] valueNames = new string[3] { "X", "Y", "Z" };

		public Vector3 Translation
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

		public TransformTranslate()
			: base(3)
		{
		}

		public TransformTranslate(Vector3 translation)
			: base(3)
		{
			Translation = translation;
		}

		public TransformTranslate(string sid, Vector3 translation)
			: base(sid, 3)
		{
			Translation = translation;
		}

		public override Matrix ToMatrix()
		{
			return Matrix.CreateTranslation(Translation);
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
