using System;
using System.Xml;
using Oni.Metadata;

namespace Oni.Objects
{
	internal class Character : ObjectBase
	{
		public CharacterFlags Flags;

		public string ClassName;

		public string Name;

		public string WeaponClassName;

		public string OnSpawn;

		public string OnDeath;

		public string OnSeenEnemy;

		public string OnAlarmed;

		public string OnHurt;

		public string OnDefeated;

		public string OnOutOfAmmo;

		public string OnNoPath;

		public int AdditionalHealth;

		public CharacterJobType Job;

		public int PatrolPathId;

		public int CombatId;

		public int MeleeId;

		public int NeutralId;

		public int MaxAmmoUsed;

		public int MaxAmmoDropped;

		public int MaxCellsUsed;

		public int MaxCellsDropped;

		public int MaxHyposUsed;

		public int MaxHyposDropped;

		public int MaxShieldsUsed;

		public int MaxShieldsDropped;

		public int MaxCloakUsed;

		public int MaxCloakDropped;

		public CharacterTeam Team;

		public int AmmoPercent;

		public CharacterAlertStatus InitialAlertLevel;

		public CharacterAlertStatus MinimalAlertLevel;

		public CharacterAlertStatus JobStartingAlertLevel;

		public CharacterAlertStatus InvestigatingAlertLevel;

		public int AlarmGroups;

		public CharacterPursuitMode PursuitStrongUnseen;

		public CharacterPursuitMode PursuitWeakUnseen;

		public CharacterPursuitMode PursuitStrongSeen;

		public CharacterPursuitMode PursuitWeakSeen;

		public CharacterPursuitLostBehavior PursuitLost;

		public Character()
		{
			base.TypeId = ObjectType.Character;
		}

		protected override void WriteOsd(BinaryWriter writer)
		{
			writer.Write((int)Flags);
			writer.Write(ClassName, 64);
			writer.Write(Name, 32);
			writer.Write(WeaponClassName, 64);
			writer.Write(OnSpawn, 32);
			writer.Write(OnDeath, 32);
			writer.Write(OnSeenEnemy, 32);
			writer.Write(OnAlarmed, 32);
			writer.Write(OnHurt, 32);
			writer.Write(OnDefeated, 32);
			writer.Write(OnOutOfAmmo, 32);
			writer.Write(OnNoPath, 32);
			writer.Write(AdditionalHealth);
			writer.Write((int)Job);
			writer.WriteInt16(PatrolPathId);
			writer.WriteInt16(CombatId);
			writer.WriteInt16(MeleeId);
			writer.WriteInt16(NeutralId);
			writer.WriteInt16(MaxAmmoUsed);
			writer.WriteInt16(MaxAmmoDropped);
			writer.WriteInt16(MaxCellsUsed);
			writer.WriteInt16(MaxCellsDropped);
			writer.WriteInt16(MaxHyposUsed);
			writer.WriteInt16(MaxHyposDropped);
			writer.WriteInt16(MaxShieldsUsed);
			writer.WriteInt16(MaxShieldsDropped);
			writer.WriteInt16(MaxCloakUsed);
			writer.WriteInt16(MaxCloakDropped);
			writer.Skip(4);
			writer.Write((int)Team);
			writer.Write(AmmoPercent);
			writer.Write((int)InitialAlertLevel);
			writer.Write((int)MinimalAlertLevel);
			writer.Write((int)JobStartingAlertLevel);
			writer.Write((int)InvestigatingAlertLevel);
			writer.Write(AlarmGroups);
			writer.Write((int)PursuitStrongUnseen);
			writer.Write((int)PursuitWeakUnseen);
			writer.Write((int)PursuitStrongSeen);
			writer.Write((int)PursuitWeakSeen);
			writer.Write((int)PursuitLost);
		}

		protected override void ReadOsd(BinaryReader reader)
		{
			Flags = (CharacterFlags)reader.ReadInt32();
			ClassName = reader.ReadString(64);
			Name = reader.ReadString(32);
			WeaponClassName = reader.ReadString(64);
			OnSpawn = reader.ReadString(32);
			OnDeath = reader.ReadString(32);
			OnSeenEnemy = reader.ReadString(32);
			OnAlarmed = reader.ReadString(32);
			OnHurt = reader.ReadString(32);
			OnDefeated = reader.ReadString(32);
			OnOutOfAmmo = reader.ReadString(32);
			OnNoPath = reader.ReadString(32);
			AdditionalHealth = reader.ReadInt32();
			Job = (CharacterJobType)reader.ReadInt32();
			PatrolPathId = reader.ReadInt16();
			CombatId = reader.ReadInt16();
			MeleeId = reader.ReadInt16();
			NeutralId = reader.ReadInt16();
			MaxAmmoUsed = reader.ReadInt16();
			MaxAmmoDropped = reader.ReadInt16();
			MaxCellsUsed = reader.ReadInt16();
			MaxCellsDropped = reader.ReadInt16();
			MaxHyposUsed = reader.ReadInt16();
			MaxHyposDropped = reader.ReadInt16();
			MaxShieldsUsed = reader.ReadInt16();
			MaxShieldsDropped = reader.ReadInt16();
			MaxCloakUsed = reader.ReadInt16();
			MaxCloakDropped = reader.ReadInt16();
			reader.Skip(4);
			Team = (CharacterTeam)reader.ReadInt32();
			AmmoPercent = reader.ReadInt32();
			InitialAlertLevel = (CharacterAlertStatus)reader.ReadInt32();
			MinimalAlertLevel = (CharacterAlertStatus)reader.ReadInt32();
			JobStartingAlertLevel = (CharacterAlertStatus)reader.ReadInt32();
			InvestigatingAlertLevel = (CharacterAlertStatus)reader.ReadInt32();
			AlarmGroups = reader.ReadInt32();
			PursuitStrongUnseen = (CharacterPursuitMode)reader.ReadInt32();
			PursuitWeakUnseen = (CharacterPursuitMode)reader.ReadInt32();
			PursuitStrongSeen = (CharacterPursuitMode)reader.ReadInt32();
			PursuitWeakSeen = (CharacterPursuitMode)reader.ReadInt32();
			PursuitLost = (CharacterPursuitLostBehavior)reader.ReadInt32();
		}

		protected override void WriteOsd(XmlWriter xml)
		{
			throw new NotImplementedException();
		}

		protected override void ReadOsd(XmlReader xml, ObjectLoadContext context)
		{
			Flags = xml.ReadElementContentAsEnum<CharacterFlags>("Flags");
			ClassName = xml.ReadElementContentAsString("Class", "");
			Name = xml.ReadElementContentAsString("Name", "");
			WeaponClassName = xml.ReadElementContentAsString("Weapon", "");
			xml.ReadStartElement("Scripts");
			OnSpawn = xml.ReadElementContentAsString("Spawn", "");
			OnDeath = xml.ReadElementContentAsString("Die", "");
			OnSeenEnemy = xml.ReadElementContentAsString("Combat", "");
			OnAlarmed = xml.ReadElementContentAsString("Alarm", "");
			OnHurt = xml.ReadElementContentAsString("Hurt", "");
			OnDefeated = xml.ReadElementContentAsString("Defeated", "");
			OnOutOfAmmo = xml.ReadElementContentAsString("OutOfAmmo", "");
			OnNoPath = xml.ReadElementContentAsString("NoPath", "");
			xml.ReadEndElement();
			AdditionalHealth = xml.ReadElementContentAsInt("AdditionalHealth", "");
			xml.ReadStartElement("Job");
			Job = xml.ReadElementContentAsEnum<CharacterJobType>("Type");
			PatrolPathId = xml.ReadElementContentAsInt("PatrolPathId", "");
			xml.ReadEndElement();
			xml.ReadStartElement("Behaviors");
			CombatId = xml.ReadElementContentAsInt("CombatId", "");
			MeleeId = xml.ReadElementContentAsInt("MeleeId", "");
			NeutralId = xml.ReadElementContentAsInt("NeutralId", "");
			xml.ReadEndElement();
			xml.ReadStartElement("Inventory");
			xml.ReadStartElement("Ammo");
			MaxAmmoUsed = xml.ReadElementContentAsInt("Use", "");
			MaxAmmoDropped = xml.ReadElementContentAsInt("Drop", "");
			xml.ReadEndElement();
			xml.ReadStartElement("EnergyCell");
			MaxCellsUsed = xml.ReadElementContentAsInt("Use", "");
			MaxCellsDropped = xml.ReadElementContentAsInt("Drop", "");
			xml.ReadEndElement();
			xml.ReadStartElement("Hypo");
			MaxHyposUsed = xml.ReadElementContentAsInt("Use", "");
			MaxHyposDropped = xml.ReadElementContentAsInt("Drop", "");
			xml.ReadEndElement();
			xml.ReadStartElement("Shield");
			MaxShieldsUsed = xml.ReadElementContentAsInt("Use", "");
			MaxShieldsDropped = xml.ReadElementContentAsInt("Drop", "");
			xml.ReadEndElement();
			xml.ReadStartElement("Invisibility");
			MaxCloakUsed = xml.ReadElementContentAsInt("Use", "");
			MaxCloakDropped = xml.ReadElementContentAsInt("Drop", "");
			xml.ReadEndElement();
			xml.ReadEndElement();
			Team = xml.ReadElementContentAsEnum<CharacterTeam>("Team");
			AmmoPercent = xml.ReadElementContentAsInt("AmmoPercentage", "");
			xml.ReadStartElement("Alert");
			InitialAlertLevel = xml.ReadElementContentAsEnum<CharacterAlertStatus>("Initial");
			MinimalAlertLevel = xml.ReadElementContentAsEnum<CharacterAlertStatus>("Minimal");
			JobStartingAlertLevel = xml.ReadElementContentAsEnum<CharacterAlertStatus>("JobStart");
			InvestigatingAlertLevel = xml.ReadElementContentAsEnum<CharacterAlertStatus>("Investigate");
			xml.ReadEndElement();
			AlarmGroups = xml.ReadElementContentAsInt("AlarmGroups", "");
			xml.ReadStartElement("Pursuit");
			PursuitStrongUnseen = xml.ReadElementContentAsEnum<CharacterPursuitMode>("StrongUnseen");
			PursuitWeakUnseen = xml.ReadElementContentAsEnum<CharacterPursuitMode>("WeakUnseen");
			PursuitStrongSeen = xml.ReadElementContentAsEnum<CharacterPursuitMode>("StrongSeen");
			PursuitWeakSeen = xml.ReadElementContentAsEnum<CharacterPursuitMode>("WeakSeen");
			PursuitLost = xml.ReadElementContentAsEnum<CharacterPursuitLostBehavior>("Lost");
			xml.ReadEndElement();
		}
	}
}
