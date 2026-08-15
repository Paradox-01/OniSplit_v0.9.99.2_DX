namespace Oni.Dae
{
	internal class Visitor
	{
		public virtual void VisitScene(Scene scene)
		{
			foreach (Node node in scene.Nodes)
			{
				VisitNode(node);
			}
		}

		public virtual void VisitNode(Node node)
		{
			foreach (Transform transform in node.Transforms)
			{
				VisitTransform(transform);
			}
			foreach (Instance instance in node.Instances)
			{
				if (instance is GeometryInstance)
				{
					VisitGeometryInstance((GeometryInstance)instance);
				}
				else if (instance is LightInstance)
				{
					VisitLightInstance((LightInstance)instance);
				}
				else if (instance is CameraInstance)
				{
					VisitCameraInstance((CameraInstance)instance);
				}
			}
			foreach (Node node2 in node.Nodes)
			{
				VisitNode(node2);
			}
		}

		public virtual void VisitGeometryInstance(GeometryInstance instance)
		{
			foreach (MaterialInstance material in instance.Materials)
			{
				VisitMaterialInstance(material);
			}
			VisitGeometry(instance.Target);
		}

		public virtual void VisitGeometry(Geometry geometry)
		{
			foreach (Input vertex in geometry.Vertices)
			{
				VisitInput(vertex);
			}
			foreach (MeshPrimitives primitive in geometry.Primitives)
			{
				VisitMeshPrimitives(primitive);
			}
		}

		public virtual void VisitMeshPrimitives(MeshPrimitives primitives)
		{
			foreach (IndexedInput input in primitives.Inputs)
			{
				VisitInput(input);
			}
		}

		public virtual void VisitMaterialInstance(MaterialInstance instance)
		{
			VisitMaterial(instance.Target);
		}

		public virtual void VisitMaterial(Material material)
		{
			VisitEffect(material.Effect);
		}

		public virtual void VisitLightInstance(LightInstance instance)
		{
			VisitLight(instance.Target);
		}

		public virtual void VisitLight(Light light)
		{
		}

		public virtual void VisitCameraInstance(CameraInstance instance)
		{
			VisitCamera(instance.Target);
		}

		public virtual void VisitCamera(Camera camera)
		{
		}

		public virtual void VisitTransform(Transform transform)
		{
			if (!transform.HasAnimations)
			{
				return;
			}
			foreach (Sampler item in transform.Animations.Where((Sampler s) => s != null))
			{
				VisitSampler(item);
			}
		}

		public virtual void VisitSampler(Sampler sampler)
		{
			foreach (Input input in sampler.Inputs)
			{
				VisitInput(input);
			}
		}

		public virtual void VisitEffect(Effect effect)
		{
			foreach (EffectParameter parameter in effect.Parameters)
			{
				VisitEffectParameter(parameter);
			}
			VisitEffectParameter(effect.Ambient);
			VisitEffectParameter(effect.Diffuse);
			VisitEffectParameter(effect.Emission);
			VisitEffectParameter(effect.IndexOfRefraction);
			VisitEffectParameter(effect.Reflective);
			VisitEffectParameter(effect.Shininess);
			VisitEffectParameter(effect.Specular);
			VisitEffectParameter(effect.Transparency);
			VisitEffectParameter(effect.Transparent);
		}

		public virtual void VisitEffectParameter(EffectParameter parameter)
		{
			if (parameter.Value is EffectTexture)
			{
				VisitEffectTexture((EffectTexture)parameter.Value);
			}
		}

		public virtual void VisitEffectTexture(EffectTexture texture)
		{
			VisitEffectSampler(texture.Sampler);
		}

		public virtual void VisitEffectSampler(EffectSampler sampler)
		{
			VisitEffectSurface(sampler.Surface);
		}

		public virtual void VisitEffectSurface(EffectSurface surface)
		{
			VisitImage(surface.InitFrom);
		}

		public virtual void VisitInput(Input input)
		{
			VisitSource(input.Source);
		}

		public virtual void VisitSource(Source source)
		{
		}

		public virtual void VisitImage(Image image)
		{
		}
	}
}
