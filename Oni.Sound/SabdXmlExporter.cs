using System.Collections.Generic;
using System.IO;
using System.Xml;
using Oni.Xml;

namespace Oni.Sound
{
	internal class SabdXmlExporter : RawXmlExporter
	{
		private class SoundAnimationData
		{
			private enum Tag
			{
				SAFT = 1413890387,
				SAVT = 1414938963,
				SASA = 1095975251
			}

			private readonly string variant;

			private readonly List<SoundAssignment> assignments;

			public SoundAnimationData(BinaryReader reader)
			{
				int num = reader.ReadInt32() + reader.Position;
				int num2 = reader.ReadInt32();
				if (num2 != 1413890387)
				{
					throw new InvalidDataException(string.Format("Unknown tag {0:X} found in sound animation", num2));
				}
				int num3 = reader.ReadInt32();
				int num4 = reader.ReadInt32();
				num2 = reader.ReadInt32();
				if (num2 != 1414938963)
				{
					throw new InvalidDataException(string.Format("Unknown tag {0:X} found in sound animation", num2));
				}
				num3 = reader.ReadInt32();
				variant = reader.ReadString(32);
				assignments = new List<SoundAssignment>();
				while (reader.Position < num)
				{
					num2 = reader.ReadInt32();
					if (num2 != 1095975251)
					{
						throw new InvalidDataException(string.Format("Unknown tag {0:X} found in sound animation", num2));
					}
					num3 = reader.ReadInt32();
					assignments.Add(new SoundAssignment(reader));
				}
			}

			public void Write(XmlWriter xml)
			{
				xml.WriteStartElement("SoundAnimation");
				xml.WriteAttributeString("Variant", variant);
				foreach (SoundAssignment assignment in assignments)
				{
					assignment.Write(xml);
				}
				xml.WriteEndElement();
			}
		}

		private class SoundAssignment
		{
			private readonly int frame;

			private readonly string modifier;

			private readonly string type;

			private readonly string animationName;

			private readonly string soundName;

			public SoundAssignment(BinaryReader reader)
			{
				frame = reader.ReadInt32();
				modifier = reader.ReadString(32);
				type = reader.ReadString(32);
				animationName = reader.ReadString(32);
				soundName = reader.ReadString(32);
			}

			public void Write(XmlWriter xml)
			{
				xml.WriteStartElement("Assignment");
				xml.WriteStartElement("Target");
				if (type != "Animation")
				{
					xml.WriteElementString("Type", type.Replace(" ", ""));
				}
				else
				{
					xml.WriteElementString("Animation", animationName);
				}
				if (modifier != "Any")
				{
					xml.WriteElementString("Modifier", modifier.Replace(" ", ""));
				}
				xml.WriteElementString("Frame", XmlConvert.ToString(frame));
				xml.WriteEndElement();
				xml.WriteElementString("Sound", soundName);
				xml.WriteEndElement();
			}
		}

		private SabdXmlExporter(BinaryReader reader, XmlWriter xml)
			: base(reader, xml)
		{
		}

		public static void Export(BinaryReader reader, XmlWriter xml)
		{
			SabdXmlExporter sabdXmlExporter = new SabdXmlExporter(reader, xml);
			sabdXmlExporter.Export();
		}

		private void Export()
		{
			SoundAnimationData soundAnimationData = new SoundAnimationData(base.Reader);
			soundAnimationData.Write(base.Xml);
		}
	}
}
