using System;
using System.Collections.Generic;
using Oni.Dae;

namespace Oni.Akira
{
	internal class RoomDaeReader
	{
		private readonly PolygonMesh mesh;

		private readonly List<Vector3> positions;

		private readonly Stack<Matrix> nodeTransformStack;

		private Scene scene;

		private Matrix nodeTransform;

		public static PolygonMesh Read(Scene scene)
		{
			RoomDaeReader roomDaeReader = new RoomDaeReader();
			roomDaeReader.ReadScene(scene);
			return roomDaeReader.mesh;
		}

		private RoomDaeReader()
		{
			mesh = new PolygonMesh(new MaterialLibrary());
			positions = mesh.Points;
			nodeTransformStack = new Stack<Matrix>();
			nodeTransform = Matrix.Identity;
		}

		private void ReadScene(Scene scene)
		{
			this.scene = scene;
			foreach (Node node in scene.Nodes)
			{
				ReadNode(node);
			}
		}

		private void ReadNode(Node node)
		{
			nodeTransformStack.Push(nodeTransform);
			foreach (Transform transform in node.Transforms)
			{
				nodeTransform = transform.ToMatrix() * nodeTransform;
			}
			foreach (GeometryInstance geometryInstance in node.GeometryInstances)
			{
				ReadGeometryInstance(node, geometryInstance);
			}
			foreach (Node node2 in node.Nodes)
			{
				ReadNode(node2);
			}
			nodeTransform = nodeTransformStack.Pop();
		}

		private void ReadGeometryInstance(Node node, GeometryInstance instance)
		{
			Geometry target = instance.Target;
			foreach (MeshPrimitives primitives in target.Primitives)
			{
				if (primitives.PrimitiveType != MeshPrimitiveType.Polygons)
				{
					Console.Error.WriteLine("Unsupported primitive type '{0}' found in geometry '{1}', ignoring.", primitives.PrimitiveType, target.Id);
					continue;
				}
				ReadPolygonPrimitives(node, primitives, instance.Materials.Find((MaterialInstance m) => m.Symbol == primitives.MaterialSymbol));
			}
		}

		private void ReadPolygonPrimitives(Node node, MeshPrimitives primitives, MaterialInstance materialInstance)
		{
			IndexedInput input = primitives.Inputs.FirstOrDefault((IndexedInput i) => i.Semantic == Semantic.Position);
			int[] array = ReadInputIndexed(input, positions, Source.ReadVector3);
			int[] array2 = array;
			foreach (int index in array2)
			{
				positions[index] = Vector3.Transform(positions[index], ref nodeTransform);
			}
			int num2 = 0;
			foreach (int vertexCount in primitives.VertexCounts)
			{
				Polygon polygon = CreatePolygon(array, num2, vertexCount);
				num2 += vertexCount;
				if (polygon == null)
				{
					Console.Error.WriteLine("BNV polygon: discarded, polygon is degenerate");
					continue;
				}
				polygon.FileName = node.FileName;
				polygon.ObjectName = node.Name;
				if (Math.Abs(polygon.Plane.Normal.Y) < 0.0001f)
				{
					if (polygon.BoundingBox.Height < 1f)
					{
						Console.Error.WriteLine("BNV polygon: discarded, ghost height must be greater than 1, it is {0}", polygon.BoundingBox.Height);
					}
					else if (polygon.PointIndices.Length != 4)
					{
						Console.Error.WriteLine("BNV polygon: discarded, ghost is a {0}-gon", polygon.PointIndices.Length);
					}
					else
					{
						mesh.Ghosts.Add(polygon);
					}
				}
				else if ((polygon.Flags & GunkFlags.Horizontal) != GunkFlags.None)
				{
					mesh.Floors.Add(polygon);
				}
				else
				{
					Console.Error.WriteLine("BNV polygon: discarded, not a ghost and not a floor");
				}
			}
		}

		private Polygon CreatePolygon(int[] positionIndices, int startIndex, int vertexCount)
		{
			int num = startIndex + vertexCount;
			List<int> list = new List<int>(vertexCount);
			for (int i = startIndex; i < num; i++)
			{
				int num2 = positionIndices[(i == startIndex) ? (num - 1) : (i - 1)];
				int num3 = positionIndices[i];
				int index = positionIndices[(i + 1 == num) ? startIndex : (i + 1)];
				if (num2 == num3)
				{
					Console.Error.WriteLine("BNV polygon: discarding degenerate edge {0}", mesh.Points[num3]);
					continue;
				}
				Vector3 vector = mesh.Points[num2];
				Vector3 vector2 = mesh.Points[num3];
				Vector3 vector3 = mesh.Points[index];
				Vector3 v = vector2 - vector;
				Vector3 v2 = vector3 - vector2;
				if (!(Vector3.Cross(v2, v).LengthSquared() < 1E-06f))
				{
					list.Add(num3);
				}
			}
			int[] array = list.ToArray();
			if (CheckDegenerate(mesh.Points, array))
			{
				return null;
			}
			return new Polygon(mesh, array);
		}

		private static bool CheckDegenerate(List<Vector3> positions, int[] indices)
		{
			if (indices.Length < 3)
			{
				return true;
			}
			Vector3 v = positions[indices[0]];
			Vector3 v2 = positions[indices[1]];
			for (int i = 2; i < indices.Length; i++)
			{
				Vector3 v3 = positions[indices[i]];
				Vector3 r;
				Vector3.Substract(ref v, ref v2, out r);
				Vector3 r2;
				Vector3.Substract(ref v3, ref v2, out r2);
				Vector3 r3;
				Vector3.Cross(ref r, ref r2, out r3);
				if (Math.Abs(r3.LengthSquared()) < 0.0001f && Vector3.Dot(ref r, ref r2) > 0f)
				{
					return true;
				}
				v = v2;
				v2 = v3;
			}
			return false;
		}

		private static int[] ReadInputIndexed<T>(IndexedInput input, List<T> list, Func<Source, int, T> elementReader) where T : struct
		{
			int[] array = new int[input.Indices.Count];
			for (int i = 0; i < input.Indices.Count; i++)
			{
				T item = elementReader(input.Source, input.Indices[i]);
				array[i] = list.Count;
				list.Add(item);
			}
			return array;
		}
	}
}
