namespace Oni.Dae
{
	internal class MaterialBinding
	{
		public string Semantic { get; set; }

		public IndexedInput VertexInput { get; set; }

		public MaterialBinding()
		{
		}

		public MaterialBinding(string semantic, IndexedInput input)
		{
			Semantic = semantic;
			VertexInput = input;
		}
	}
}
