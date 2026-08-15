using System.Collections.Generic;

namespace Oni.Dae
{
	internal class TransformCollection : List<Transform>
	{
		public Matrix ToMatrix()
		{
			Matrix identity = Matrix.Identity;
			foreach (Transform item in Utils.Reverse(this))
			{
				identity *= item.ToMatrix();
			}
			return identity;
		}

		public TransformScale Scale(string sid, Vector3 scale)
		{
			TransformScale transformScale = new TransformScale(sid, scale);
			Add(transformScale);
			return transformScale;
		}

		public TransformRotate Rotate(string sid, Vector3 axis, float angle)
		{
			TransformRotate transformRotate = new TransformRotate(sid, axis, angle);
			Add(transformRotate);
			return transformRotate;
		}

		public TransformTranslate Translate(string sid, Vector3 translate)
		{
			TransformTranslate transformTranslate = new TransformTranslate(sid, translate);
			Add(transformTranslate);
			return transformTranslate;
		}
	}
}
