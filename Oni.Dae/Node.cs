using System.Collections.Generic;

namespace Oni.Dae
{
	internal class Node : Entity
	{
		private TransformCollection transforms;

		private List<Instance> instances;

		private List<Node> nodes;

		public TransformCollection Transforms
		{
			get
			{
				if (transforms == null)
				{
					transforms = new TransformCollection();
				}
				return transforms;
			}
		}

		public List<Instance> Instances
		{
			get
			{
				if (instances == null)
				{
					instances = new List<Instance>();
				}
				return instances;
			}
		}

		public IEnumerable<GeometryInstance> GeometryInstances
		{
			get
			{
				if (instances == null)
				{
					return new GeometryInstance[0];
				}
				return instances.OfType<GeometryInstance>();
			}
		}

		public List<Node> Nodes
		{
			get
			{
				if (nodes == null)
				{
					nodes = new List<Node>();
				}
				return nodes;
			}
		}
	}
}
