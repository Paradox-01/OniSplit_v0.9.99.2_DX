using System.Collections.Generic;
using Oni.Collections;

namespace Oni.Dae
{
	internal class AxisConverter
	{
		private Scene scene;

		private Axis fromUpAxis;

		private Axis toUpAxis;

		private Set<float[]> convertedValues;

		public static void Convert(Scene scene, Axis fromUpAxis, Axis toUpAxis)
		{
			AxisConverter axisConverter = new AxisConverter
			{
				scene = scene,
				fromUpAxis = fromUpAxis,
				toUpAxis = toUpAxis,
				convertedValues = new Set<float[]>()
			};
			axisConverter.Convert();
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
				if (transform.HasAnimations)
				{
					ConvertRotationAnimation(transform);
				}
				return;
			}
			TransformTranslate transformTranslate = transform as TransformTranslate;
			if (transformTranslate != null)
			{
				ConvertPosition(transformTranslate.Values, 3);
				if (transform.HasAnimations)
				{
					ConvertPositionAnimation(transform);
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
			if (fromUpAxis == Axis.Z && toUpAxis == Axis.Y)
			{
				Matrix matrix = transform.Matrix;
				Matrix matrix2 = matrix;
				matrix2.M12 = matrix.M13;
				matrix2.M13 = 0f - matrix.M12;
				matrix2.M21 = matrix.M31;
				matrix2.M22 = matrix.M33;
				matrix2.M23 = 0f - matrix.M32;
				matrix2.M31 = 0f - matrix.M21;
				matrix2.M32 = 0f - matrix.M23;
				matrix2.M33 = matrix.M22;
				matrix2.M42 = matrix.M43;
				matrix2.M43 = 0f - matrix.M42;
				transform.Matrix = matrix2;
			}
		}

		private void ConvertPosition(float[] values, int stride)
		{
			if (!convertedValues.Add(values))
			{
				return;
			}
			for (int i = 0; i + stride - 1 < values.Length; i += stride)
			{
				Convert(values, i, (float f) => 0f - f);
			}
		}

		private void ConvertPositionAnimation(Transform transform)
		{
			Convert(transform.Animations, 0, (Sampler s) => (s == null) ? null : s.Scale(-1f));
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
			T val2 = list[baseIndex + 2];
			if (fromUpAxis == Axis.Z && toUpAxis == Axis.Y)
			{
				list[baseIndex] = value;
				list[baseIndex + 1] = val2;
				list[baseIndex + 2] = ((negate != null) ? negate(val) : val);
			}
			else if (fromUpAxis == Axis.Y && toUpAxis == Axis.Z)
			{
				list[baseIndex] = value;
				list[baseIndex + 1] = ((negate != null) ? negate(val2) : val2);
				list[baseIndex + 2] = val;
			}
			else if (fromUpAxis == Axis.X && toUpAxis == Axis.Y)
			{
				list[baseIndex] = ((negate != null) ? negate(val2) : val2);
				list[baseIndex + 1] = value;
				list[baseIndex + 2] = val;
			}
		}
	}
}
