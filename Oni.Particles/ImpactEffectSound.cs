using System.Xml;
using Oni.Metadata;

namespace Oni.Particles
{
	internal class ImpactEffectSound
	{
		private string soundName;

		private bool aiCanHear;

		private InstanceMetadata.DOORSoundType aiSoundType;

		private float aiSoundRadius;

		public string SoundName
		{
			get
			{
				return soundName;
			}
		}

		public bool AICanHear
		{
			get
			{
				return aiCanHear;
			}
		}

		public InstanceMetadata.DOORSoundType AISoundType
		{
			get
			{
				return aiSoundType;
			}
		}

		public float AISoundRadius
		{
			get
			{
				return aiSoundRadius;
			}
		}

		public ImpactEffectSound(BinaryReader reader)
		{
			soundName = reader.ReadString(32);
			reader.Skip(8);
			aiCanHear = reader.ReadInt16() != 0;
			aiSoundType = (InstanceMetadata.DOORSoundType)reader.ReadInt16();
			aiSoundRadius = reader.ReadSingle();
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write(soundName, 32);
			writer.Skip(8);
			writer.WriteInt16(aiCanHear ? 1 : 0);
			writer.WriteInt16((short)aiSoundType);
			writer.Write(aiSoundRadius);
		}

		public ImpactEffectSound(XmlReader xml)
		{
			soundName = xml.ReadElementContentAsString("Name", "");
			aiCanHear = bool.Parse(xml.ReadElementContentAsString("AICanHear", ""));
			aiSoundType = MetaEnum.Parse<InstanceMetadata.DOORSoundType>(xml.ReadElementContentAsString("AISoundType", ""));
			aiSoundRadius = XmlConvert.ToSingle(xml.ReadElementContentAsString("AISoundRadius", ""));
		}

		public void Write(XmlWriter writer)
		{
			writer.WriteElementString("Name", soundName);
			writer.WriteElementString("AICanHear", XmlConvert.ToString(aiCanHear));
			writer.WriteElementString("AISoundType", aiSoundType.ToString());
			writer.WriteElementString("AISoundRadius", XmlConvert.ToString(aiSoundRadius));
		}
	}
}
