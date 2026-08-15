namespace Oni.Particles
{
	internal class Variable
	{
		private string name;

		private StorageType storageType;

		private Value value;

		private int storageOffset;

		public string Name
		{
			get
			{
				return name;
			}
		}

		public StorageType StorageType
		{
			get
			{
				return storageType;
			}
		}

		public int StorageOffset
		{
			get
			{
				return storageOffset;
			}
		}

		public Value Value
		{
			get
			{
				return value;
			}
		}

		public Variable(string name, StorageType type, Value value)
		{
			this.name = name;
			storageType = type;
			this.value = value;
		}

		public Variable(BinaryReader reader)
		{
			name = reader.ReadString(16);
			storageType = (StorageType)reader.ReadInt32();
			storageOffset = reader.ReadInt32();
			value = Value.Read(reader);
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write(name, 16);
			writer.Write((int)storageType);
			writer.Write(0);
			value.Write(writer);
		}
	}
}
