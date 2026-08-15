using System.Collections.Generic;
using System.Xml;
using Oni.Particles;

namespace Oni.Xml
{
	internal class OnieXmlImporter : RawXmlImporter
	{
		private class ImpactNode
		{
			private int impactIndex;

			private List<MaterialNode> materialNodes;

			public int ImpactIndex
			{
				get
				{
					return impactIndex;
				}
			}

			public List<MaterialNode> MaterialNodes
			{
				get
				{
					return materialNodes;
				}
			}

			public ImpactNode(int impactIndex)
			{
				this.impactIndex = impactIndex;
				materialNodes = new List<MaterialNode>();
			}
		}

		private class MaterialNode
		{
			private int materialIndex;

			private List<ImpactEffect> impactEffects;

			public int MaterialIndex
			{
				get
				{
					return materialIndex;
				}
			}

			public List<ImpactEffect> ImpactEffects
			{
				get
				{
					return impactEffects;
				}
			}

			public MaterialNode(int materialIndex)
			{
				this.materialIndex = materialIndex;
				impactEffects = new List<ImpactEffect>();
			}
		}

		private List<ImpactEffect> impactEffects = new List<ImpactEffect>();

		private List<ImpactEffectSound> sounds = new List<ImpactEffectSound>();

		private List<ImpactEffectParticle> particles = new List<ImpactEffectParticle>();

		private Dictionary<string, int> materials = new Dictionary<string, int>();

		private List<KeyValuePair<string, int>> impactList;

		private List<KeyValuePair<string, int>> materialList;

		private List<ImpactNode> impactNodes;

		private List<MaterialNode> materialNodes;

		private OnieXmlImporter(XmlReader xml, BinaryWriter writer)
			: base(xml, writer)
		{
		}

		public static void Import(XmlReader xml, BinaryWriter writer)
		{
			OnieXmlImporter onieXmlImporter = new OnieXmlImporter(xml, writer);
			onieXmlImporter.Read();
			onieXmlImporter.Write();
		}

		private void Read()
		{
			impactList = new List<KeyValuePair<string, int>>();
			while (base.Xml.IsStartElement("Impact"))
			{
				int count = impactList.Count;
				string attribute = base.Xml.GetAttribute("Name");
				impactList.Add(new KeyValuePair<string, int>(attribute, count));
				if (base.Xml.IsEmptyElement)
				{
					base.Xml.ReadStartElement();
					continue;
				}
				base.Xml.ReadStartElement();
				while (base.Xml.IsStartElement("Material"))
				{
					string attribute2 = base.Xml.GetAttribute("Name");
					int value;
					if (!materials.TryGetValue(attribute2, out value))
					{
						value = materials.Count;
						materials.Add(attribute2, value);
					}
					if (base.Xml.IsEmptyElement)
					{
						base.Xml.ReadStartElement();
						continue;
					}
					base.Xml.ReadStartElement();
					while (base.Xml.IsStartElement("ImpactEffect"))
					{
						base.Xml.ReadStartElement();
						ImpactEffect impactEffect = new ImpactEffect(base.Xml, attribute, attribute2)
						{
							ImpactIndex = count,
							MaterialIndex = value
						};
						if (impactEffect.Sound != null)
						{
							impactEffect.SoundIndex = sounds.Count;
							sounds.Add(impactEffect.Sound);
						}
						else
						{
							impactEffect.SoundIndex = -1;
						}
						if (impactEffect.Particles != null && impactEffect.Particles.Length != 0)
						{
							impactEffect.ParticleIndex = particles.Count;
							particles.AddRange(impactEffect.Particles);
						}
						else
						{
							impactEffect.ParticleIndex = -1;
						}
						impactEffects.Add(impactEffect);
						base.Xml.ReadEndElement();
					}
					base.Xml.ReadEndElement();
				}
				base.Xml.ReadEndElement();
			}
			materialList = new List<KeyValuePair<string, int>>(materials);
			materialList.Sort((KeyValuePair<string, int> x, KeyValuePair<string, int> y) => x.Value.CompareTo(y.Value));
			impactNodes = new List<ImpactNode>();
			materialNodes = new List<MaterialNode>();
			foreach (KeyValuePair<string, int> impact in impactList)
			{
				ImpactNode impactNode = new ImpactNode(impact.Value);
				impactNodes.Add(impactNode);
				foreach (KeyValuePair<string, int> material in materialList)
				{
					MaterialNode materialNode = new MaterialNode(material.Value);
					foreach (ImpactEffect impactEffect2 in impactEffects)
					{
						if (impactEffect2.MaterialIndex == material.Value && impactEffect2.ImpactIndex == impact.Value)
						{
							materialNode.ImpactEffects.Add(impactEffect2);
						}
					}
					if (materialNode.ImpactEffects.Count > 0)
					{
						impactNode.MaterialNodes.Add(materialNode);
						materialNodes.Add(materialNode);
					}
				}
			}
		}

		private void Write()
		{
			base.Writer.Write(2);
			base.Writer.Write(impactList.Count);
			base.Writer.Write(materialList.Count);
			base.Writer.Write(particles.Count);
			base.Writer.Write(sounds.Count);
			base.Writer.Write(impactEffects.Count);
			base.Writer.Write(materialNodes.Count);
			foreach (KeyValuePair<string, int> impact in impactList)
			{
				base.Writer.Write(impact.Key, 128);
				base.Writer.Write(0);
			}
			foreach (KeyValuePair<string, int> material in materialList)
			{
				base.Writer.Write(material.Key, 128);
				base.Writer.Write(0);
			}
			int num = 0;
			foreach (ImpactNode impactNode in impactNodes)
			{
				base.Writer.WriteInt16(impactNode.ImpactIndex);
				base.Writer.WriteInt16(impactNode.MaterialNodes.Count);
				base.Writer.Write(num);
				num += impactNode.MaterialNodes.Count;
			}
			foreach (ImpactEffectParticle particle in particles)
			{
				particle.Write(base.Writer);
			}
			foreach (ImpactEffectSound sound in sounds)
			{
				sound.Write(base.Writer);
			}
			foreach (MaterialNode materialNode in materialNodes)
			{
				foreach (ImpactEffect impactEffect in materialNode.ImpactEffects)
				{
					impactEffect.Write(base.Writer);
				}
			}
			int num2 = 0;
			foreach (MaterialNode materialNode2 in materialNodes)
			{
				base.Writer.WriteInt16(materialNode2.MaterialIndex);
				base.Writer.WriteInt16(materialNode2.ImpactEffects.Count);
				base.Writer.Write(num2);
				num2 += materialNode2.ImpactEffects.Count;
			}
		}
	}
}
