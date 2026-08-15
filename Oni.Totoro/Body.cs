using System.Collections.Generic;

namespace Oni.Totoro
{
	internal class Body
	{
		public readonly List<BodyNode> Nodes = new List<BodyNode>();

		public BodyNode Root
		{
			get
			{
				return Nodes[0];
			}
		}
	}
}
