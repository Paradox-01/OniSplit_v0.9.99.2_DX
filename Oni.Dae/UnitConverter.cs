using Oni.Collections;

namespace Oni.Dae
{
	internal class UnitConverter
	{
		private Scene scene;

		private float scale;

		private Set<float[]> scaledValues;

		public static void Convert(Scene scene, float scale)
		{
			UnitConverter unitConverter = new UnitConverter
			{
				scene = scene,
				scale = scale,
				scaledValues = new Set<float[]>()
			};
			unitConverter.Convert();
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
					if (input.Semantic == Semantic.Position)
					{
						Scale(input.Source.FloatData, input.Source.Stride);
					}
				}
			}
		}

		private void Convert(Transform transform)
		{
			TransformTranslate transformTranslate = transform as TransformTranslate;
			if (transformTranslate != null)
			{
				Scale(transformTranslate.Values, 3);
				if (transformTranslate.HasAnimations)
				{
					for (int i = 0; i < transformTranslate.Animations.Length; i++)
					{
						Sampler sampler = transformTranslate.Animations[i];
						transformTranslate.Animations[i] = ((sampler == null) ? null : sampler.Scale(scale));
					}
				}
			}
			else
			{
				TransformMatrix transformMatrix = transform as TransformMatrix;
				if (transformMatrix != null)
				{
					transformMatrix.Values[3] *= scale;
					transformMatrix.Values[7] *= scale;
					transformMatrix.Values[11] *= scale;
				}
			}
		}

		private void Scale(float[] values, int stride)
		{
			if (scaledValues.Add(values))
			{
				for (int i = 0; i + stride - 1 < values.Length; i += stride)
				{
					values[i] *= scale;
					values[i + 1] *= scale;
					values[i + 2] *= scale;
				}
			}
		}
	}
}
