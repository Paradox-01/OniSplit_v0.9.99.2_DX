using System;

namespace Oni.Metadata
{
	internal class MetaChar : MetaPrimitiveType
	{
		internal MetaChar()
			: base("Char", 1)
		{
		}

		public override void Accept(IMetaTypeVisitor visitor)
		{
			throw new NotSupportedException();
		}
	}
}
