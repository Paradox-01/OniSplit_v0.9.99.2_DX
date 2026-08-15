namespace Oni.Metadata
{
	internal class BinaryMetadata
	{
		private static MetaStruct bobj = new MetaStruct("ObjectHeader", new Field(MetaType.Int32, "ObjectId"), new Field(MetaType.Int32, "ObjectFlags"), new Field(MetaType.Vector3, "Position"), new Field(MetaType.Vector3, "Rotation"));

		private static MetaStruct cons = new MetaStruct("Console", bobj, new Field(MetaType.String(63), "ClassName"), new Field(MetaType.Int16, "DoorId"), new Field(MetaType.Int16, "Flags"), new Field(MetaType.String(63), "DisabledTexture"), new Field(MetaType.String(63), "EnabledTexture"), new Field(MetaType.String(63), "UsedTexture"), new Field(MetaType.ShortVarArray(new MetaStruct("ConsoleAction", new Field(MetaType.Int16, "ActionType"), new Field(MetaType.Int16, "TargetId"), new Field(MetaType.String32, "ScriptFunction"))), "Actions"));

		private static MetaStruct door = new MetaStruct("Door", bobj, new Field(MetaType.String(63), "ClassName"), new Field(MetaType.Int16, "DoorId"), new Field(MetaType.Int16, ""), new Field(MetaType.Int16, "Flags"), new Field(MetaType.Vector3, "Center"), new Field(MetaType.Float, "ActivationRadius"), new Field(MetaType.String(63), "Texture1"), new Field(MetaType.String(63), "Texture2"), new Field(MetaType.Padding(5)));

		private static MetaStruct flag = new MetaStruct("Flag", bobj, new Field(MetaType.Color, "Color"), new Field(MetaType.Int16, "Prefix"), new Field(MetaType.Int16, "FlagId"), new Field(MetaType.String128, "Notes"));

		private static MetaStruct furn = new MetaStruct("Furniture", bobj, new Field(MetaType.String32, "ClassName"), new Field(MetaType.String48, "Particle"));

		private static MetaStruct part = new MetaStruct("Particle", bobj, new Field(MetaType.String64, "ClassName"), new Field(MetaType.String48, "Name"), new Field(MetaType.Int16, "Flags"), new Field(MetaType.Float, "DecalXScale"), new Field(MetaType.Float, "DecalYScale"), new Field(MetaType.Int16, "DecalRotation"));

		private static MetaStruct pwru = new MetaStruct("Powerup", bobj, new Field(MetaType.Int32, "Type"));

		private static MetaStruct sndg = new MetaStruct("Sound", bobj, new Field(MetaType.String32, "ClassName"), new Field(MetaType.Int32, "GeometryType"), new Field(MetaType.BoundingBox, "BoundingBox"), new Field(MetaType.Float, "MinVolumeRadius"), new Field(MetaType.Float, "MaxVolumeRadius"), new Field(MetaType.Float, "Volume"), new Field(MetaType.Float, "Pitch"));

		private static MetaStruct trge = new MetaStruct("Trigger", bobj, new Field(MetaType.String(63), "ClassName"), new Field(MetaType.Int16, "TriggerId"), new Field(MetaType.Int16, "Flags"), new Field(MetaType.Color, "Color"), new Field(MetaType.Float, "StartPosition"), new Field(MetaType.Float, "Speed"), new Field(MetaType.Int32, "Count"), new Field(MetaType.Int16, ""), new Field(MetaType.ShortVarArray(new MetaStruct("TriggerAction", new Field(MetaType.Int16, "ActionType"), new Field(MetaType.Int16, "TurretId"), new Field(MetaType.String(34), "ScriptFunction"))), "Actions"), new Field(MetaType.Byte, ""));

		private static MetaStruct turr = new MetaStruct("Turret", bobj, new Field(MetaType.String(63), "ClassName"), new Field(MetaType.Int16, "TurretId"), new Field(MetaType.Int16, "Flags"), new Field(MetaType.Padding(36)), new Field(MetaType.Int32, "TargetedTeams"), new Field(MetaType.Padding(1)));

		private static MetaStruct weap = new MetaStruct("Weapon", bobj, new Field(MetaType.String32, "ClassName"));

		private static MetaType osbdAmbient = new MetaStruct("OSBDAmbient", new Field(MetaType.Int32, "Version"), new Field(MetaType.Int32, "Priority"), new Field(MetaType.Int32, "Flags"), new Field(MetaType.Float, "Unknown1"), new Field(MetaType.Float, "MinElapsedTime"), new Field(MetaType.Float, "MaxElapsedTime"), new Field(MetaType.Float, "MinVolumeDistance"), new Field(MetaType.Float, "MaxVolumeDistance"), new Field(MetaType.String32, "DetailGroup"), new Field(MetaType.String32, "Track1Group"), new Field(MetaType.String32, "Track2Group"), new Field(MetaType.String32, "InGroup"), new Field(MetaType.String32, "OutGroup"), new Field(MetaType.Int32, "Unknown2"), new Field(MetaType.Float, "Unknown3"));

		private static MetaType osbdImpulse = new MetaStruct("OSBDImpulse", new Field(MetaType.Int32, "Version"), new Field(MetaType.String32, "Group"), new Field(MetaType.Int32, "Priority"), new Field(MetaType.Float, "MinVolumeDistance"), new Field(MetaType.Float, "MaxVolumeDistance"), new Field(MetaType.Float, "MinVolumeAngle"), new Field(MetaType.Float, "MaxVolumeAngle"), new Field(MetaType.Int32, "Unknown1"), new Field(MetaType.Int32, "Unknown2"), new Field(MetaType.String32, "AlternateImpulse"), new Field(MetaType.Int32, "Unknown3"), new Field(MetaType.Int32, "Unknown4"));

		private static MetaType osbdGroup = new MetaStruct("OSBDGroup", new Field(MetaType.Int32, "Version"), new Field(MetaType.Float, "Volume"), new Field(MetaType.Float, "Pitch"), new Field(MetaType.Int16, "PreventRepeat"), new Field(MetaType.Int16, "PreviousPermutation"), new Field(MetaType.Int32, "ChannelCount"), new Field(MetaType.VarArray(new MetaStruct("OSBDGroupPermutation", new Field(MetaType.Int32, "Weight"), new Field(MetaType.Float, "MinVolume"), new Field(MetaType.Float, "MaxVolume"), new Field(MetaType.Float, "MinPitch"), new Field(MetaType.Float, "MaxPitch"), new Field(MetaType.String32, "Sound"))), "Permutations"));
	}
}
