using System;
using System.Collections.Generic;
using Oni.Collections;
using Oni.Dae;

namespace Oni.Akira
{
	internal class RoomGridBuilder
	{
		private readonly Scene roomsScene;

		private readonly PolygonMesh geometryMesh;

		private PolygonMesh roomsMesh;

		private OctreeNode geometryOcttree;

		private OctreeNode dangerOcttree;

		public PolygonMesh Mesh
		{
			get
			{
				return roomsMesh;
			}
		}

		public RoomGridBuilder(Scene roomsScene, PolygonMesh geometryMesh)
		{
			this.roomsScene = roomsScene;
			this.geometryMesh = geometryMesh;
		}

		public void Build()
		{
			roomsMesh = RoomDaeReader.Read(roomsScene);
			RoomBuilder.BuildRooms(roomsMesh);
			Console.Error.WriteLine("Read {0} rooms", roomsMesh.Rooms.Count);
			geometryOcttree = OctreeBuilder.Build(geometryMesh, GunkFlags.NoCollision | GunkFlags.NoCharacterCollision);
			dangerOcttree = OctreeBuilder.Build(geometryMesh, (Polygon p) => (p.Flags & GunkFlags.Danger) != 0);
			ProcessStairsCollision();
			Parallel.ForEach(roomsMesh.Rooms, delegate(Room room)
			{
				BuildGrid(room);
			});
		}

		private void ProcessStairsCollision()
		{
			Vector3 verticalTolerance1 = new Vector3(0f, 0.1f, 0f);
			Vector3 verticalTolerance2 = new Vector3(0f, 7.5f, 0f);
			foreach (Polygon item in geometryMesh.Polygons.Where((Polygon p) => p.IsStairs && p.VertexCount == 4))
			{
				Vector3[] array = item.Points.Select((Vector3 v) => v + verticalTolerance1).ToArray();
				Vector3[] array2 = item.Points.Select((Vector3 v) => v + verticalTolerance2).ToArray();
				BoundingBox box = BoundingBox.CreateFromPoints(array.Concatenate(array2));
				Plane plane = new Plane(array[0], array[1], array[2]);
				Plane plane2 = new Plane(array2[0], array2[1], array2[2]);
				foreach (OctreeNode item2 in geometryOcttree.FindLeafs(box))
				{
					foreach (Polygon polygon in item2.Polygons)
					{
						if ((polygon.Flags & (GunkFlags.NoCollision | GunkFlags.NoCharacterCollision)) != GunkFlags.None || !polygon.BoundingBox.Intersects(box))
						{
							continue;
						}
						List<Vector3> points = polygon.Points.ToList();
						points = PolygonUtils.ClipToPlane(points, plane);
						if (points != null)
						{
							points = PolygonUtils.ClipToPlane(points, plane2);
							if (points == null)
							{
								polygon.Flags |= GunkFlags.NoCharacterCollision;
							}
						}
					}
				}
			}
		}

		private void BuildGrid(Room room)
		{
			Polygon floorPolygon = room.FloorPolygon;
			BoundingBox boundingBox = room.BoundingBox;
			RoomGridRasterizer roomGridRasterizer = new RoomGridRasterizer(boundingBox);
			roomGridRasterizer.Clear(RoomGridWeight.Danger);
			boundingBox.Inflate(2f * new Vector3(roomGridRasterizer.TileSize, 0f, roomGridRasterizer.TileSize));
			BoundingBox box = boundingBox;
			box.Min.X--;
			box.Min.Y = boundingBox.Min.Y - 6f;
			box.Min.Z--;
			box.Max.X++;
			box.Max.Y = boundingBox.Max.Y - 6f;
			box.Max.Z++;
			Set<Polygon> set = new Set<Polygon>();
			Set<Polygon> set2 = new Set<Polygon>();
			foreach (OctreeNode item in geometryOcttree.FindLeafs(box))
			{
				set.UnionWith(item.Polygons);
			}
			foreach (OctreeNode item2 in dangerOcttree.FindLeafs(box))
			{
				set2.UnionWith(item2.Polygons);
			}
			foreach (Polygon item3 in set)
			{
				if (item3.Plane.Normal.Y > 0.5f)
				{
					roomGridRasterizer.DrawFloor(item3.Points);
				}
			}
			if (room.FloorPlane.Normal.Y >= 0.999f)
			{
				float y = floorPolygon.BoundingBox.Max.Y;
				Plane plane = new Plane(floorPolygon.Plane.Normal, floorPolygon.Plane.D - 4f);
				Plane plane2 = new Plane(-floorPolygon.Plane.Normal, 0f - (floorPolygon.Plane.D - 20f));
				foreach (Polygon item4 in set)
				{
					if ((item4.Flags & (GunkFlags.Stairs | GunkFlags.NoCharacterCollision | GunkFlags.Impassable)) == 0)
					{
						BoundingBox boundingBox2 = item4.BoundingBox;
						if (Math.Abs(item4.Plane.Normal.Y) < 1E-05f && boundingBox2.Height <= 4f && Math.Abs(boundingBox2.Max.Y - y) <= 4f)
						{
							item4.Flags |= GunkFlags.NoCharacterCollision;
							continue;
						}
					}
					if ((item4.Flags & (GunkFlags.Stairs | GunkFlags.NoCollision | GunkFlags.NoCharacterCollision | GunkFlags.GridIgnore)) != GunkFlags.None)
					{
						continue;
					}
					List<Vector3> points = item4.Points.ToList();
					points = PolygonUtils.ClipToPlane(points, plane);
					if (points == null)
					{
						continue;
					}
					points = PolygonUtils.ClipToPlane(points, plane2);
					if (points != null)
					{
						if (Math.Abs(item4.Plane.Normal.Y) <= 0.1f)
						{
							roomGridRasterizer.DrawWall(points);
						}
						else
						{
							roomGridRasterizer.DrawImpassable(points);
						}
					}
				}
				foreach (Polygon item5 in set2)
				{
					roomGridRasterizer.DrawDanger(item5.Points);
				}
				roomGridRasterizer.AddBorders();
			}
			room.Grid = roomGridRasterizer.GetGrid();
			if (room.Grid.XTiles * room.Grid.ZTiles > 65536)
			{
				Console.Error.WriteLine("Warning: pathfinding grid too large");
			}
		}
	}
}
