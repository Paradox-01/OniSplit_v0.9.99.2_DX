using System.Collections.Generic;

namespace Oni.Dae
{
	internal class Geometry : Entity
	{
		public readonly List<MeshPrimitives> primitives = new List<MeshPrimitives>(1);

		public readonly List<Input> vertices = new List<Input>(1);

		public List<Input> Vertices
		{
			get
			{
				return vertices;
			}
		}

		public List<MeshPrimitives> Primitives
		{
			get
			{
				return primitives;
			}
		}
	}
}
