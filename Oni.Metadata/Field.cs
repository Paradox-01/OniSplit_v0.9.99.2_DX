namespace Oni.Metadata
{
	internal class Field
	{
		private readonly string name;

		private readonly MetaType type;

		public MetaType Type
		{
			get
			{
				return type;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
		}

		public Field(MetaType type, string name = null)
		{
			this.type = type;
			this.name = name;
		}
	}
}
