using System.Collections.Generic;

namespace Oni.Dae
{
	internal class MeshPrimitives
	{
		private readonly MeshPrimitiveType primitiveType;

		private readonly List<IndexedInput> inputs;

		private readonly List<int> vertexCounts;

		public MeshPrimitiveType PrimitiveType
		{
			get
			{
				return primitiveType;
			}
		}

		public string MaterialSymbol { get; set; }

		public List<IndexedInput> Inputs
		{
			get
			{
				return inputs;
			}
		}

		public List<int> VertexCounts
		{
			get
			{
				return vertexCounts;
			}
		}

		public MeshPrimitives(MeshPrimitiveType primitiveType)
		{
			this.primitiveType = primitiveType;
			inputs = new List<IndexedInput>(3);
			vertexCounts = new List<int>();
		}

		public MeshPrimitives(MeshPrimitiveType primitiveType, IEnumerable<IndexedInput> inputs)
		{
			this.primitiveType = primitiveType;
			this.inputs = new List<IndexedInput>(inputs);
			vertexCounts = new List<int>();
		}
	}
}
