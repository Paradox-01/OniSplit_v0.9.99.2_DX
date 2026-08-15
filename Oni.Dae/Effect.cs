using System.Collections.Generic;

namespace Oni.Dae
{
	internal class Effect : Entity
	{
		private readonly List<EffectParameter> parameters;

		private readonly EffectParameter emission;

		private readonly EffectParameter ambient;

		private readonly EffectParameter diffuse;

		private readonly EffectParameter specular;

		private readonly EffectParameter shininess;

		private readonly EffectParameter reflective;

		private readonly EffectParameter reflectivity;

		private readonly EffectParameter transparent;

		private readonly EffectParameter transparency;

		private readonly EffectParameter indexOfRefraction;

		public EffectType Type { get; set; }

		public List<EffectParameter> Parameters
		{
			get
			{
				return parameters;
			}
		}

		public IEnumerable<EffectTexture> Textures
		{
			get
			{
				EffectParameter[] array = new EffectParameter[6] { diffuse, ambient, specular, reflective, transparent, emission };
				foreach (EffectParameter effectParameter in array)
				{
					EffectTexture effectTexture = effectParameter.Value as EffectTexture;
					if (effectTexture != null)
					{
						yield return effectTexture;
					}
				}
			}
		}

		public EffectParameter Emission
		{
			get
			{
				return emission;
			}
		}

		public EffectParameter Ambient
		{
			get
			{
				return ambient;
			}
		}

		public object AmbientValue
		{
			get
			{
				return ambient.Value;
			}
			set
			{
				ambient.Value = value;
			}
		}

		public EffectParameter Diffuse
		{
			get
			{
				return diffuse;
			}
		}

		public object DiffuseValue
		{
			get
			{
				return diffuse.Value;
			}
			set
			{
				diffuse.Value = value;
			}
		}

		public EffectParameter Specular
		{
			get
			{
				return specular;
			}
		}

		public object SpecularValue
		{
			get
			{
				return specular.Value;
			}
			set
			{
				specular.Value = value;
			}
		}

		public EffectParameter Shininess
		{
			get
			{
				return shininess;
			}
		}

		public EffectParameter Reflective
		{
			get
			{
				return reflective;
			}
		}

		public EffectParameter Reflectivity
		{
			get
			{
				return reflectivity;
			}
		}

		public EffectParameter Transparent
		{
			get
			{
				return transparent;
			}
		}

		public object TransparentValue
		{
			get
			{
				return transparent.Value;
			}
			set
			{
				transparent.Value = value;
			}
		}

		public EffectParameter Transparency
		{
			get
			{
				return transparency;
			}
		}

		public EffectParameter IndexOfRefraction
		{
			get
			{
				return indexOfRefraction;
			}
		}

		public Effect()
		{
			parameters = new List<EffectParameter>();
			Vector4 vector = new Vector4(0f, 0f, 0f, 1f);
			emission = new EffectParameter("emission", vector, this);
			ambient = new EffectParameter("ambient", vector, this);
			diffuse = new EffectParameter("diffuse", vector, this);
			specular = new EffectParameter("specular", vector, this);
			shininess = new EffectParameter("shininess", 20f, this);
			reflective = new EffectParameter("reflective", 1f, this);
			reflectivity = new EffectParameter("reflectivity", Vector4.One, this);
			transparent = new EffectParameter("transparent", vector, this);
			transparency = new EffectParameter("transparency", 1f, this);
			indexOfRefraction = new EffectParameter("index_of_refraction", 1f, this);
		}
	}
}
