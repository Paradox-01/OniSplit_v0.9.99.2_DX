using Oni.Physics;

namespace Oni.Objects
{
	internal class FurnitureClass : GunkObjectClass
	{
		public ObjectNode Geometry;

		public override ObjectGeometry[] GunkNodes
		{
			get
			{
				return Geometry.Geometries;
			}
		}

		public static FurnitureClass Read(InstanceDescriptor ofga)
		{
			if (ofga == null)
			{
				return null;
			}
			return new FurnitureClass
			{
				Geometry = ObjectDatReader.ReadObjectGeometry(ofga)
			};
		}
	}
}
