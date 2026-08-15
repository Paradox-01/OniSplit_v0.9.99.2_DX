namespace Oni.Particles
{
	internal class VariableReference
	{
		public const int ByteSize = 24;

		public static readonly VariableReference Empty = new VariableReference(string.Empty);

		private string name;

		public bool IsDefined
		{
			get
			{
				return !string.IsNullOrEmpty(name);
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
		}

		public VariableReference(string name)
		{
			this.name = name;
		}

		public VariableReference(BinaryReader reader)
		{
			name = reader.ReadString(16);
			reader.Skip(8);
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write(name, 16);
			writer.Skip(8);
		}
	}
}
