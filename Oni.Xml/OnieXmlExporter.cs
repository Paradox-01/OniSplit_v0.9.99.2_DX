using System.Xml;
using Oni.Particles;

namespace Oni.Xml
{
	internal class OnieXmlExporter : RawXmlExporter
	{
		private OnieXmlExporter(BinaryReader reader, XmlWriter xml)
			: base(reader, xml)
		{
		}

		public static void Export(BinaryReader reader, XmlWriter xml)
		{
			OnieXmlExporter onieXmlExporter = new OnieXmlExporter(reader, xml);
			onieXmlExporter.Export();
		}

		private void Export()
		{
			base.Reader.Skip(8);
			string[] array = new string[base.Reader.ReadInt32()];
			string[] array2 = new string[base.Reader.ReadInt32()];
			ImpactEffectParticle[] array3 = new ImpactEffectParticle[base.Reader.ReadInt32()];
			ImpactEffectSound[] array4 = new ImpactEffectSound[base.Reader.ReadInt32()];
			ImpactEffect[] array5 = new ImpactEffect[base.Reader.ReadInt32()];
			base.Reader.Skip(4);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = base.Reader.ReadString(128);
				base.Reader.Skip(4);
			}
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = base.Reader.ReadString(128);
				base.Reader.Skip(4);
			}
			for (int k = 0; k < array.Length; k++)
			{
				base.Reader.Skip(8);
			}
			for (int l = 0; l < array3.Length; l++)
			{
				array3[l] = new ImpactEffectParticle(base.Reader);
			}
			for (int m = 0; m < array4.Length; m++)
			{
				array4[m] = new ImpactEffectSound(base.Reader);
			}
			for (int n = 0; n < array5.Length; n++)
			{
				array5[n] = new ImpactEffect(base.Reader, array, array2, array3, array4);
			}
			base.Xml.WriteStartElement("ImpactEffects");
			string[] array6 = array;
			foreach (string text in array6)
			{
				base.Xml.WriteStartElement("Impact");
				base.Xml.WriteAttributeString("Name", text);
				string[] array7 = array2;
				foreach (string text2 in array7)
				{
					bool flag = false;
					ImpactEffect[] array8 = array5;
					foreach (ImpactEffect impactEffect in array8)
					{
						if (impactEffect.ImpactName == text && impactEffect.MaterialName == text2)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						continue;
					}
					base.Xml.WriteStartElement("Material");
					base.Xml.WriteAttributeString("Name", text2);
					ImpactEffect[] array9 = array5;
					foreach (ImpactEffect impactEffect2 in array9)
					{
						if (impactEffect2.ImpactName == text && impactEffect2.MaterialName == text2)
						{
							base.Xml.WriteStartElement("ImpactEffect");
							impactEffect2.Write(base.Xml);
							base.Xml.WriteEndElement();
						}
					}
					base.Xml.WriteEndElement();
				}
				base.Xml.WriteEndElement();
			}
			base.Xml.WriteEndElement();
		}
	}
}
