using System;
using System.Collections.Generic;
using System.Xml;
using Oni.Metadata;

namespace Oni.Particles
{
	internal class ImpactEffect
	{
		private string impactName;

		private int impactIndex;

		private string materialName;

		private int materialIndex;

		private ImpactEffectComponent component;

		private ImpactEffectModifier modifier;

		private ImpactEffectParticle[] particles;

		private int particleIndex;

		private ImpactEffectSound sound;

		private int soundIndex;

		public string ImpactName
		{
			get
			{
				return impactName;
			}
		}

		public int ImpactIndex
		{
			get
			{
				return impactIndex;
			}
			set
			{
				impactIndex = value;
			}
		}

		public string MaterialName
		{
			get
			{
				return materialName;
			}
		}

		public int MaterialIndex
		{
			get
			{
				return materialIndex;
			}
			set
			{
				materialIndex = value;
			}
		}

		public ImpactEffectComponent Component
		{
			get
			{
				return component;
			}
		}

		public ImpactEffectModifier Modifier
		{
			get
			{
				return modifier;
			}
		}

		public ImpactEffectSound Sound
		{
			get
			{
				return sound;
			}
		}

		public int SoundIndex
		{
			get
			{
				return soundIndex;
			}
			set
			{
				soundIndex = value;
			}
		}

		public int ParticleIndex
		{
			get
			{
				return particleIndex;
			}
			set
			{
				particleIndex = value;
			}
		}

		public ImpactEffectParticle[] Particles
		{
			get
			{
				return particles;
			}
		}

		public ImpactEffect(BinaryReader reader, string[] impacts, string[] materials, ImpactEffectParticle[] particles, ImpactEffectSound[] sounds)
		{
			impactIndex = reader.ReadInt16();
			impactName = impacts[impactIndex];
			materialIndex = reader.ReadInt16();
			materialName = materials[materialIndex];
			component = (ImpactEffectComponent)reader.ReadInt16();
			modifier = (ImpactEffectModifier)reader.ReadInt16();
			int num = reader.ReadInt16();
			reader.Skip(2);
			soundIndex = reader.ReadInt32();
			particleIndex = reader.ReadInt32();
			if (soundIndex != -1)
			{
				sound = sounds[soundIndex];
			}
			if (num > 0)
			{
				this.particles = new ImpactEffectParticle[num];
				Array.Copy(particles, particleIndex, this.particles, 0, this.particles.Length);
			}
		}

		public void Write(BinaryWriter writer)
		{
			writer.WriteInt16(impactIndex);
			writer.WriteInt16(materialIndex);
			writer.WriteInt16((short)component);
			writer.WriteInt16((short)modifier);
			writer.WriteInt16(particles.Length);
			writer.Skip(2);
			writer.Write(soundIndex);
			writer.Write(particleIndex);
		}

		public ImpactEffect(XmlReader xml, string impact, string material)
		{
			impactName = impact;
			materialName = material;
			component = MetaEnum.Parse<ImpactEffectComponent>(xml.ReadElementContentAsString("Component", ""));
			modifier = MetaEnum.Parse<ImpactEffectModifier>(xml.ReadElementContentAsString("Modifier", ""));
			if (xml.IsStartElement("Sound"))
			{
				if (xml.IsEmptyElement)
				{
					xml.Skip();
				}
				else
				{
					xml.ReadStartElement();
					sound = new ImpactEffectSound(xml);
					xml.ReadEndElement();
				}
			}
			List<ImpactEffectParticle> list = new List<ImpactEffectParticle>();
			if (xml.IsStartElement("Particles"))
			{
				if (xml.IsEmptyElement)
				{
					xml.Skip();
				}
				else
				{
					xml.ReadStartElement();
					while (xml.IsStartElement("Particle"))
					{
						xml.ReadStartElement();
						list.Add(new ImpactEffectParticle(xml));
						xml.ReadEndElement();
					}
					xml.ReadEndElement();
				}
			}
			particles = list.ToArray();
		}

		public void Write(XmlWriter writer)
		{
			writer.WriteElementString("Component", component.ToString());
			writer.WriteElementString("Modifier", modifier.ToString());
			writer.WriteStartElement("Sound");
			if (sound != null)
			{
				sound.Write(writer);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("Particles");
			if (particles != null)
			{
				ImpactEffectParticle[] array = particles;
				foreach (ImpactEffectParticle impactEffectParticle in array)
				{
					writer.WriteStartElement("Particle");
					impactEffectParticle.Write(writer);
					writer.WriteEndElement();
				}
			}
			writer.WriteEndElement();
		}
	}
}
