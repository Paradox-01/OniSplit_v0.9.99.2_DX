using Oni.Akira;
using Oni.Motoko;

namespace Oni.Physics
{
	internal class ObjectGeometry
	{
		public Geometry Geometry;

		public GunkFlags Flags;

		public ObjectGeometry()
		{
		}

		public ObjectGeometry(Geometry geometry)
		{
			Geometry = geometry;
		}
	}
}
