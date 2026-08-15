using System.Xml;
using Oni.Metadata;

namespace Oni.Level
{
	internal class ScriptCharacter
	{
		public string className;

		public string name;

		public string weaponClassName;

		public int flagId;

		public int scriptId;

		public InstanceMetadata.AISACharacterFlags flags;

		public InstanceMetadata.AISACharacterTeam team;

		public string onSpawn;

		public string onDeath;

		public string onSeenEnemy;

		public string onAlarmed;

		public string onHurt;

		public string onDefeated;

		public string onOutOfAmmo;

		public string onNoPath;

		public int ammo;

		public static ScriptCharacter Read(XmlReader xml)
		{
			xml.ReadStartElement("Character");
			ScriptCharacter scriptCharacter = new ScriptCharacter
			{
				name = xml.ReadElementContentAsString("Name", ""),
				scriptId = xml.ReadElementContentAsInt("ScriptId", ""),
				flagId = xml.ReadElementContentAsInt("FlagId", ""),
				flags = xml.ReadElementContentAsEnum<InstanceMetadata.AISACharacterFlags>("Flags"),
				team = xml.ReadElementContentAsEnum<InstanceMetadata.AISACharacterTeam>("Team"),
				className = xml.ReadElementContentAsString("Class", "")
			};
			xml.ReadStartElement("Scripts");
			scriptCharacter.onSpawn = xml.ReadElementContentAsString("Spawn", "");
			scriptCharacter.onDeath = xml.ReadElementContentAsString("Die", "");
			scriptCharacter.onSeenEnemy = xml.ReadElementContentAsString("Combat", "");
			scriptCharacter.onAlarmed = xml.ReadElementContentAsString("Alarm", "");
			scriptCharacter.onHurt = xml.ReadElementContentAsString("Hurt", "");
			scriptCharacter.onDefeated = xml.ReadElementContentAsString("Defeated", "");
			scriptCharacter.onOutOfAmmo = xml.ReadElementContentAsString("OutOfAmmo", "");
			scriptCharacter.onNoPath = xml.ReadElementContentAsString("NoPath", "");
			xml.ReadEndElement();
			scriptCharacter.weaponClassName = xml.ReadElementContentAsString("Weapon", "");
			scriptCharacter.ammo = xml.ReadElementContentAsInt("Ammo", "");
			xml.ReadEndElement();
			return scriptCharacter;
		}
	}
}
