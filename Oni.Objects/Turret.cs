using System.Xml;
using Oni.Metadata;

namespace Oni.Objects
{
	internal class Turret : GunkObject
	{
		public int ScriptId;

		public TurretFlags Flags;

		public TurretTargetTeams TargetTeams;

		public Turret()
		{
			base.TypeId = ObjectType.Turret;
		}

		protected override void WriteOsd(BinaryWriter writer)
		{
			writer.Write(base.ClassName, 63);
			writer.WriteUInt16(ScriptId);
			writer.WriteUInt16((int)Flags);
			writer.Skip(36);
			writer.Write((uint)TargetTeams);
		}

		protected override void ReadOsd(BinaryReader reader)
		{
			base.ClassName = reader.ReadString(63);
			ScriptId = reader.ReadUInt16();
			Flags = (TurretFlags)reader.ReadUInt16();
			reader.Skip(36);
			TargetTeams = (TurretTargetTeams)reader.ReadInt32();
		}

		protected override void WriteOsd(XmlWriter xml)
		{
			xml.WriteElementString("Class", base.ClassName);
			xml.WriteElementString("TurretId", XmlConvert.ToString(ScriptId));
			xml.WriteElementString("Flags", MetaEnum.ToString(Flags));
			xml.WriteElementString("TargetedTeams", MetaEnum.ToString(TargetTeams));
		}

		protected override void ReadOsd(XmlReader xml, ObjectLoadContext context)
		{
			string name = xml.ReadElementContentAsString("Class", "");
			ScriptId = xml.ReadElementContentAsInt("TurretId", "");
			Flags = xml.ReadElementContentAsEnum<TurretFlags>("Flags");
			TargetTeams = xml.ReadElementContentAsEnum<TurretTargetTeams>("TargetedTeams");
			base.GunkClass = context.GetClass(TemplateTag.TURR, name, TurretClass.Read);
		}
	}
}
