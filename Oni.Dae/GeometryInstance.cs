using System.Collections.Generic;

namespace Oni.Dae
{
	internal class GeometryInstance : Instance<Geometry>
	{
		private readonly List<MaterialInstance> materials = new List<MaterialInstance>(1);

		public List<MaterialInstance> Materials
		{
			get
			{
				return materials;
			}
		}

		public GeometryInstance()
		{
		}

		public GeometryInstance(Geometry geometry)
			: base(geometry)
		{
		}
	}
}
