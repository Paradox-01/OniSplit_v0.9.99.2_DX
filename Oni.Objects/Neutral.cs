using System;
using System.Collections.Generic;
using System.Xml;
using Oni.Metadata;
using Oni.Totoro;

namespace Oni.Objects
{
	internal class Neutral : ObjectBase
	{
		public string Name;

		public int Id;

		public NeutralFlags Flags;

		public float TriggerRange;

		public float TalkRange;

		public float FollowRange;

		public float AbortEnemyRange;

		public string TriggerSpeech;

		public string AbortSpeech;

		public string EnemySpeech;

		public string EndScript;

		public string WeaponClass;

		public int AmmoAmount;

		public int CellAmount;

		public int HypoAmount;

		public NeutralItems OtherItems;

		public NeutralDialogLine[] Lines;

		public Neutral()
		{
			base.TypeId = ObjectType.Neutral;
		}

		protected override void WriteOsd(BinaryWriter writer)
		{
			writer.Write(Name, 32);
			writer.WriteUInt16(Id);
			writer.WriteUInt16(Lines.Length);
			writer.Write((int)Flags);
			writer.Write(TriggerRange);
			writer.Write(TalkRange);
			writer.Write(FollowRange);
			writer.Write(AbortEnemyRange);
			writer.Write(TriggerSpeech, 32);
			writer.Write(AbortSpeech, 32);
			writer.Write(EnemySpeech, 32);
			writer.Write(EndScript, 32);
			writer.Write(WeaponClass, 32);
			writer.WriteByte(AmmoAmount);
			writer.WriteByte(CellAmount);
			writer.WriteByte(HypoAmount);
			writer.WriteByte((int)OtherItems);
			NeutralDialogLine[] lines = Lines;
			foreach (NeutralDialogLine neutralDialogLine in lines)
			{
				writer.Write((int)neutralDialogLine.Flags);
				writer.WriteUInt16((int)neutralDialogLine.AnimType);
				writer.WriteUInt16((int)neutralDialogLine.OtherAnimationType);
				writer.Write(neutralDialogLine.Speech, 32);
			}
		}

		protected override void ReadOsd(BinaryReader reader)
		{
			Name = reader.ReadString(32);
			Id = reader.ReadInt16();
			Lines = new NeutralDialogLine[reader.ReadInt16()];
			Flags = (NeutralFlags)reader.ReadInt32();
			TriggerRange = reader.ReadSingle();
			TalkRange = reader.ReadSingle();
			FollowRange = reader.ReadSingle();
			AbortEnemyRange = reader.ReadSingle();
			TriggerSpeech = reader.ReadString(32);
			AbortSpeech = reader.ReadString(32);
			EnemySpeech = reader.ReadString(32);
			EndScript = reader.ReadString(32);
			WeaponClass = reader.ReadString(32);
			AmmoAmount = reader.ReadByte();
			CellAmount = reader.ReadByte();
			HypoAmount = reader.ReadByte();
			OtherItems = (NeutralItems)reader.ReadByte();
			for (int i = 0; i < Lines.Length; i++)
			{
				Lines[i] = new NeutralDialogLine
				{
					Flags = (NeutralDialogLineFlags)reader.ReadInt32(),
					AnimType = (AnimationType)reader.ReadUInt16(),
					OtherAnimationType = (AnimationType)reader.ReadUInt16(),
					Speech = reader.ReadString(32)
				};
			}
		}

		protected override void ReadOsd(XmlReader xml, ObjectLoadContext context)
		{
			Name = xml.ReadElementContentAsString("Name", "");
			Id = xml.ReadElementContentAsInt("NeutralId", "");
			Flags = xml.ReadElementContentAsEnum<NeutralFlags>("Flags");
			xml.ReadStartElement("Ranges");
			TriggerRange = xml.ReadElementContentAsFloat("Trigger", "");
			TalkRange = xml.ReadElementContentAsFloat("Talk", "");
			FollowRange = xml.ReadElementContentAsFloat("Follow", "");
			AbortEnemyRange = xml.ReadElementContentAsFloat("Enemy", "");
			xml.ReadEndElement();
			xml.ReadStartElement("Speech");
			TriggerSpeech = xml.ReadElementContentAsString("Trigger", "");
			AbortSpeech = xml.ReadElementContentAsString("Abort", "");
			EnemySpeech = xml.ReadElementContentAsString("Enemy", "");
			xml.ReadEndElement();
			xml.ReadStartElement("Script");
			EndScript = xml.ReadElementContentAsString("AfterTalk", "");
			xml.ReadEndElement();
			xml.ReadStartElement("Rewards");
			WeaponClass = xml.ReadElementContentAsString("WeaponClass", "");
			AmmoAmount = xml.ReadElementContentAsInt("Ammo", "");
			CellAmount = xml.ReadElementContentAsInt("EnergyCell", "");
			HypoAmount = xml.ReadElementContentAsInt("Hypo", "");
			OtherItems = xml.ReadElementContentAsEnum<NeutralItems>("Other");
			xml.ReadEndElement();
			xml.ReadStartElement("DialogLines");
			List<NeutralDialogLine> list = new List<NeutralDialogLine>();
			while (xml.IsStartElement())
			{
				xml.ReadStartElement("DialogLine");
				list.Add(new NeutralDialogLine
				{
					Flags = xml.ReadElementContentAsEnum<NeutralDialogLineFlags>("Flags"),
					AnimType = xml.ReadElementContentAsEnum<AnimationType>("Anim"),
					OtherAnimationType = xml.ReadElementContentAsEnum<AnimationType>("OtherAnim"),
					Speech = xml.ReadElementContentAsString("SpeechName", "")
				});
				xml.ReadEndElement();
			}
			Lines = list.ToArray();
			xml.ReadEndElement();
		}

		protected override void WriteOsd(XmlWriter xml)
		{
			throw new NotImplementedException();
		}
	}
}
