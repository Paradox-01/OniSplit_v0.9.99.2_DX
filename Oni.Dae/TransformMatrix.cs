using System;

namespace Oni.Dae
{
	internal class TransformMatrix : Transform
	{
		public Matrix Matrix
		{
			get
			{
				return new Matrix(base.Values);
			}
			set
			{
				value.CopyTo(base.Values);
			}
		}

		public TransformMatrix()
			: base(16)
		{
		}

		public override Matrix ToMatrix()
		{
			return Matrix;
		}

		public override int ValueNameToValueIndex(string name)
		{
			return -1;
		}

		public override string ValueIndexToValueName(int index)
		{
			throw new NotImplementedException();
		}
	}
}
