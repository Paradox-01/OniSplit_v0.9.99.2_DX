using System.Collections.Generic;

namespace Oni.Akira
{
	internal static class OctreeBuilder
	{
		private static readonly BoundingBox rootBoundingBox = new BoundingBox(new Vector3(-4096f), new Vector3(4096f));

		public static OctreeNode Build(PolygonMesh mesh, bool debug)
		{
			IEnumerable<Polygon> enumerable = mesh.Polygons;
			if (debug)
			{
				enumerable = enumerable.Concatenate(mesh.Ghosts);
			}
			OctreeNode octreeNode = new OctreeNode(rootBoundingBox, enumerable, mesh.Rooms);
			octreeNode.Build();
			return octreeNode;
		}

		public static OctreeNode Build(PolygonMesh mesh, GunkFlags excludeFlags)
		{
			OctreeNode octreeNode = new OctreeNode(rootBoundingBox, mesh.Polygons.Where((Polygon p) => (p.Flags & excludeFlags) == 0), mesh.Rooms);
			octreeNode.Build();
			return octreeNode;
		}

		public static OctreeNode Build(PolygonMesh mesh, Func<Polygon, bool> polygonFilter)
		{
			OctreeNode octreeNode = new OctreeNode(rootBoundingBox, mesh.Polygons.Where(polygonFilter), mesh.Rooms);
			octreeNode.Build();
			return octreeNode;
		}

		public static OctreeNode BuildRoomsOctree(PolygonMesh mesh)
		{
			OctreeNode octreeNode = new OctreeNode(rootBoundingBox, new Polygon[0], mesh.Rooms);
			octreeNode.Build();
			return octreeNode;
		}
	}
}
