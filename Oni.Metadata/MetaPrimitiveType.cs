namespace Oni.Metadata
{
	internal abstract class MetaPrimitiveType : MetaType
	{
		protected MetaPrimitiveType(string name, int size)
			: base(name, size)
		{
		}

		protected override bool IsLeafImpl()
		{
			return true;
		}
	}
}
