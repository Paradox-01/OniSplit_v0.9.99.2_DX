using System;

namespace Oni.Dae
{
	internal class Input
	{
		public Semantic Semantic { get; set; }

		public Source Source { get; set; }

		public Input()
		{
		}

		public Input(Semantic semantic, Source source)
		{
			if (semantic == Semantic.None)
			{
				throw new ArgumentException("Semantic None is not a valid value", "semantic");
			}
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			Semantic = semantic;
			Source = source;
		}
	}
}
