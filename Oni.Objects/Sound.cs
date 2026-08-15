using System;
using System.Xml;
using Oni.Xml;

namespace Oni.Objects
{
	internal class Sound : ObjectBase
	{
		public string ClassName;

		public float Volume = 1f;

		public float Pitch = 1f;

		public SoundVolumeType Type;

		public BoundingBox Box;

		public float MinDistance;

		public float MaxDistance;

		public Sound()
		{
			base.TypeId = ObjectType.Sound;
		}

		protected override void WriteOsd(BinaryWriter writer)
		{
			writer.Write(ClassName, 32);
			writer.Write((uint)Type);
			if (Type == SoundVolumeType.Box)
			{
				writer.Write(Box);
			}
			else
			{
				writer.Write(MinDistance);
				writer.Write(MaxDistance);
			}
			writer.Write(Volume);
			writer.Write(Pitch);
		}

		protected override void ReadOsd(BinaryReader reader)
		{
			ClassName = reader.ReadString(32);
			Type = (SoundVolumeType)reader.ReadInt32();
			if (Type == SoundVolumeType.Box)
			{
				Box = reader.ReadBoundingBox();
			}
			else if (Type == SoundVolumeType.Sphere)
			{
				MinDistance = reader.ReadSingle();
				MaxDistance = reader.ReadSingle();
			}
			Volume = reader.ReadSingle();
			Pitch = reader.ReadSingle();
		}

		protected override void ReadOsd(XmlReader xml, ObjectLoadContext context)
		{
			ClassName = xml.ReadElementContentAsString("Class", "");
			if (xml.IsStartElement("Volume"))
			{
				Volume = xml.ReadElementContentAsFloat("Volume", "");
				Pitch = xml.ReadElementContentAsFloat("Pitch", "");
				if (xml.IsStartElement("Box", ""))
				{
					Type = SoundVolumeType.Box;
					xml.ReadStartElement();
					Box.Min = xml.ReadElementContentAsVector3("Min");
					Box.Max = xml.ReadElementContentAsVector3("Max");
					xml.ReadEndElement();
				}
				else
				{
					Type = SoundVolumeType.Sphere;
					xml.ReadStartElement("Spheres");
					MinDistance = xml.ReadElementContentAsFloat("Min", "");
					MaxDistance = xml.ReadElementContentAsFloat("Max", "");
					xml.ReadEndElement();
				}
			}
			else
			{
				if (xml.IsStartElement("Box", ""))
				{
					Type = SoundVolumeType.Box;
					xml.ReadStartElement();
					Box.Min = xml.ReadElementContentAsVector3("Min");
					Box.Max = xml.ReadElementContentAsVector3("Max");
					xml.ReadEndElement();
				}
				else
				{
					Type = SoundVolumeType.Sphere;
					xml.ReadStartElement("Sphere");
					MinDistance = xml.ReadElementContentAsFloat("MinRadius", "");
					MaxDistance = xml.ReadElementContentAsFloat("MaxRadius", "");
					xml.ReadEndElement();
				}
				Volume = xml.ReadElementContentAsFloat("Volume", "");
				Pitch = xml.ReadElementContentAsFloat("Pitch", "");
			}
		}

		protected override void WriteOsd(XmlWriter xml)
		{
			throw new NotImplementedException();
		}
	}
}
