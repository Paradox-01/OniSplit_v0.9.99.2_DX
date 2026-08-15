using System;
using System.Xml;
using Oni.Metadata;
using Oni.Xml;

namespace Oni.Objects
{
	internal class TriggerVolume : ObjectBase
	{
		public string Name;

		public string EntryScript;

		public string InsideScript;

		public string ExitScript;

		public TurretTargetTeams Teams;

		public Vector3 Size;

		public int ScriptId;

		public int ParentId;

		public string Notes;

		public TriggerVolumeFlags Flags;

		public TriggerVolume()
		{
			base.TypeId = ObjectType.TriggerVolume;
		}

		protected override void WriteOsd(BinaryWriter writer)
		{
			writer.Write(Name, 63);
			writer.Write(EntryScript, 32);
			writer.Write(InsideScript, 32);
			writer.Write(ExitScript, 32);
			writer.Write((uint)Teams);
			writer.Write(Size);
			writer.Write(ScriptId);
			writer.Write(ParentId);
			writer.Write(Notes, 128);
			writer.Write((uint)Flags);
		}

		protected override void ReadOsd(BinaryReader reader)
		{
			Name = reader.ReadString(63);
			EntryScript = reader.ReadString(32);
			InsideScript = reader.ReadString(32);
			ExitScript = reader.ReadString(32);
			Teams = (TurretTargetTeams)(reader.ReadUInt32() & 0xFF);
			Size = reader.ReadVector3();
			ScriptId = reader.ReadInt32();
			ParentId = reader.ReadInt32();
			Notes = reader.ReadString(128);
			Flags = (TriggerVolumeFlags)(reader.ReadUInt32() & 0xFF);
		}

		protected override void WriteOsd(XmlWriter xml)
		{
			throw new NotImplementedException();
		}

		protected override void ReadOsd(XmlReader xml, ObjectLoadContext context)
		{
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "Name":
					Name = xml.ReadElementContentAsString();
					break;
				case "Scripts":
					xml.ReadStartElement();
					EntryScript = xml.ReadElementContentAsString("Entry", "");
					InsideScript = xml.ReadElementContentAsString("Inside", "");
					ExitScript = xml.ReadElementContentAsString("Exit", "");
					xml.ReadEndElement();
					break;
				case "Teams":
					Teams = xml.ReadElementContentAsEnum<TurretTargetTeams>();
					break;
				case "Size":
					Size = xml.ReadElementContentAsVector3();
					break;
				case "TriggerVolumeId":
				case "ScriptId":
					ScriptId = xml.ReadElementContentAsInt();
					break;
				case "ParentId":
					ParentId = xml.ReadElementContentAsInt();
					break;
				case "Notes":
					Notes = xml.ReadElementContentAsString();
					break;
				case "Flags":
					Flags = xml.ReadElementContentAsEnum<TriggerVolumeFlags>();
					break;
				default:
					xml.Skip();
					break;
				}
			}
		}
	}
}
