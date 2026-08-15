using System.Collections.Generic;

namespace Oni.Dae
{
	internal class IndexedInput : Input
	{
		private readonly List<int> indices = new List<int>();

		internal int Offset { get; set; }

		public int Set { get; set; }

		public List<int> Indices
		{
			get
			{
				return indices;
			}
		}

		public IndexedInput()
		{
		}

		public IndexedInput(Semantic semantic, Source source)
			: base(semantic, source)
		{
		}
	}
}
