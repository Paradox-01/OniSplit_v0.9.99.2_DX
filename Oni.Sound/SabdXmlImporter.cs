using System;
using System.Collections.Generic;
using System.Xml;
using Oni.Xml;

namespace Oni.Sound
{
	internal class SabdXmlImporter : RawXmlImporter
	{
		private class SoundAnimationData
		{
			private enum Tag
			{
				SAFT = 1413890387,
				SAVT = 1414938963,
				SASA = 1095975251
			}

			private string variant;

			private List<SoundAssignment> assignments;

			public SoundAnimationData(XmlReader xml)
			{
				variant = xml.GetAttribute("Variant");
				xml.ReadStartElement("SoundAnimation");
				assignments = new List<SoundAssignment>();
				while (xml.IsStartElement("Assignment"))
				{
					xml.ReadStartElement();
					assignments.Add(new SoundAssignment(xml));
					xml.ReadEndElement();
				}
				xml.ReadEndElement();
			}

			public void Write(BinaryWriter writer)
			{
				writer.Write(1413890387);
				writer.Write(4);
				writer.Write(6);
				writer.Write(1414938963);
				writer.Write(32);
				writer.Write(variant, 32);
				foreach (SoundAssignment assignment in assignments)
				{
					writer.Write(1095975251);
					writer.Write(132);
					assignment.Write(writer);
				}
			}
		}

		private class SoundAssignment
		{
			private int frame;

			private string modifier;

			private string type;

			private string animationName;

			private string soundName;

			public SoundAssignment(XmlReader xml)
			{
				xml.ReadStartElement("Target");
				if (xml.LocalName == "Animation")
				{
					type = "Animation";
					animationName = xml.ReadElementContentAsString("Animation", "");
				}
				else
				{
					type = xml.ReadElementContentAsString("Type", "");
					animationName = string.Empty;
					if (!typeMap.TryGetValue(type, out type))
					{
						throw new NotSupportedException(string.Format("Unknown assignment type '{0}' found", type));
					}
				}
				if (xml.IsStartElement("Modifier"))
				{
					modifier = xml.ReadElementContentAsString();
				}
				else
				{
					modifier = "Any";
				}
				if (!modifierMap.TryGetValue(modifier, out modifier))
				{
					throw new NotSupportedException(string.Format("Unknown assignment modifier '{0}' found", modifier));
				}
				xml.ReadStartElement("Frame");
				frame = xml.ReadContentAsInt();
				xml.ReadEndElement();
				xml.ReadEndElement();
				soundName = xml.ReadElementContentAsString("Sound", "");
			}

			public void Write(BinaryWriter writer)
			{
				writer.Write(frame);
				writer.Write(modifier, 32);
				writer.Write(type, 32);
				writer.Write(animationName, 32);
				writer.Write(soundName, 32);
			}
		}

		private static readonly Dictionary<string, string> modifierMap;

		private static readonly Dictionary<string, string> typeMap;

		static SabdXmlImporter()
		{
			modifierMap = new Dictionary<string, string>();
			typeMap = new Dictionary<string, string>();
			string[] array = new string[6] { "Any", "Crouch", "Jump", "Heavy Damage", "Medium Damage", "Light Damage" };
			string[] array2 = new string[28]
			{
				"Block", "Draw Weapon", "Fall", "Fly", "Getting Hit", "Holster", "Kick", "Knockdown", "Land", "Jump",
				"Pickup", "Punch", "Reload Pistol", "Reload Rifle", "Reload Stream", "Reload Superball", "Reload Vandegraf", "Reload Scram Cannon", "Reload Mercury Bow", "Reload Screamer",
				"Run", "Slide", "Stand", "Starle", "Walk", "Powerup", "Roll", "Falling Flail"
			};
			string[] array3 = array;
			foreach (string text in array3)
			{
				modifierMap.Add(text.Replace(" ", ""), text);
			}
			string[] array4 = array2;
			foreach (string text2 in array4)
			{
				typeMap.Add(text2.Replace(" ", ""), text2);
			}
		}

		private SabdXmlImporter(XmlReader reader, BinaryWriter writer)
			: base(reader, writer)
		{
		}

		public static void Import(XmlReader reader, BinaryWriter writer)
		{
			SabdXmlImporter sabdXmlImporter = new SabdXmlImporter(reader, writer);
			sabdXmlImporter.Import();
		}

		private void Import()
		{
			SoundAnimationData soundAnimationData = new SoundAnimationData(base.Xml);
			soundAnimationData.Write(base.Writer);
		}
	}
}
