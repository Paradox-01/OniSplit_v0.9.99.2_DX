using System;
using System.Collections.Generic;

namespace Oni.Dae
{
	internal class AxisConverterLocalFlipXYZ
	{
		private Scene scene;

		public static void ConvertMain(Scene scene)
		{
			AxisConverterLocalFlipXYZ axisConverterLocalFlipXYZ = new AxisConverterLocalFlipXYZ
			{
				scene = scene
			};
			axisConverterLocalFlipXYZ.Convert();
		}

		private void Convert()
		{
			Convert(scene);
		}

		private void Convert(Node node)
		{
			foreach (Transform transform in node.Transforms)
			{
				Convert(transform);
			}
			foreach (Instance instance in node.Instances)
			{
				Convert(instance);
			}
			foreach (Node node2 in node.Nodes)
			{
				Convert(node2);
			}
		}

		private void Convert(Instance instance)
		{
			GeometryInstance geometryInstance = instance as GeometryInstance;
			if (geometryInstance != null)
			{
				Convert(geometryInstance.Target);
			}
		}

		private void Convert(Geometry geometry)
		{
			foreach (MeshPrimitives primitive in geometry.Primitives)
			{
				foreach (IndexedInput input in primitive.Inputs)
				{
					if (input.Semantic == Semantic.Position || input.Semantic == Semantic.Normal)
					{
						ConvertPosition(input.Source.FloatData, input.Source.Stride);
					}
				}
			}
		}

		private void Convert(Transform transform)
		{
			TransformScale transformScale = transform as TransformScale;
			if (transformScale != null)
			{
				ConvertScale(transformScale.Values, 3);
				if (transform.HasAnimations)
				{
					ConvertScaleAnimation(transform);
				}
				return;
			}
			TransformRotate transformRotate = transform as TransformRotate;
			if (transformRotate != null)
			{
				ConvertPosition(transformRotate.Values, 3);
				if (transformRotate.HasAnimations)
				{
					Console.WriteLine("Has rotate animations");
					ConvertRotationAnimation(transformRotate);
				}
				return;
			}
			TransformTranslate transformTranslate = transform as TransformTranslate;
			if (transformTranslate != null)
			{
				ConvertPosition(transformTranslate.Values, 3);
				if (transformTranslate.HasAnimations)
				{
					Console.WriteLine("Has translate animations");
					ConvertPositionAnimation(transformTranslate);
				}
			}
			else
			{
				TransformMatrix transformMatrix = transform as TransformMatrix;
				if (transformMatrix != null)
				{
					ConvertMatrix(transformMatrix);
				}
			}
		}

		private void ConvertMatrix(TransformMatrix transform)
		{
		}

		private void ConvertPosition(float[] values, int stride)
		{
			for (int i = 0; i + stride - 1 < values.Length; i += stride)
			{
				Convert(values, i, (float f) => 0f - f);
			}
		}

		private void ConvertPositionAnimation(Transform transform)
		{
			Convert(transform.Animations, 0, (Sampler s) => (s == null) ? null : s.Scale(-10f));
		}

		private void ConvertRotationAnimation(Transform transform)
		{
			ConvertPositionAnimation(transform);
		}

		private void ConvertScale(float[] values, int stride)
		{
			for (int i = 0; i + stride - 1 < values.Length; i += stride)
			{
				Convert(values, i, null);
			}
		}

		private void ConvertScaleAnimation(Transform transform)
		{
			Convert(transform.Animations, 0, null);
		}

		private void Convert<T>(IList<T> list, int baseIndex, Func<T, T> negate)
		{
			T value = list[baseIndex];
			T val = list[baseIndex + 1];
			T value2 = list[baseIndex + 2];
			list[baseIndex] = value2;
			list[baseIndex + 1] = ((negate != null) ? negate(val) : val);
			list[baseIndex + 2] = value;
		}
	}
}
