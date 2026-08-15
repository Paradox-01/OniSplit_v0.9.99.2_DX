using System;
using System.Collections.Generic;
using Oni.Dae;

namespace Oni.Akira
{
	internal class RoomExtractor
	{
		private readonly IEnumerable<string> fromFiles;

		private readonly string outputFilePath;

		private PolygonMesh mesh;

		private List<Vector3> positions;

		private Stack<Matrix> nodeTransformStack;

		private Matrix nodeTransform;

		private string nodeName;

		public RoomExtractor(IEnumerable<string> fromFiles, string outputFilePath)
		{
			this.fromFiles = fromFiles;
			this.outputFilePath = outputFilePath;
		}

		public void Extract()
		{
			mesh = new PolygonMesh(new MaterialLibrary());
			positions = mesh.Points;
			nodeTransformStack = new Stack<Matrix>();
			nodeTransform = Matrix.Identity;
			foreach (string fromFile in fromFiles)
			{
				ReadScene(Reader.ReadFile(fromFile));
			}
			PolygonQuadrangulate polygonQuadrangulate = new PolygonQuadrangulate(mesh);
			polygonQuadrangulate.Execute();
			RoomDaeWriter.Write(mesh, outputFilePath);
		}

		private void ReadScene(Scene scene)
		{
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
			nodeName = node.Name;
			foreach (GeometryInstance geometryInstance in node.GeometryInstances)
			{
				ReadGeometryInstance(geometryInstance);
			}
			foreach (Node node2 in node.Nodes)
			{
				ReadNode(node2);
			}
			nodeTransform = nodeTransformStack.Pop();
		}

		private void ReadGeometryInstance(GeometryInstance instance)
		{
			foreach (MeshPrimitives primitives in instance.Target.Primitives)
			{
				if (primitives.PrimitiveType != MeshPrimitiveType.Polygons)
				{
					Console.Error.WriteLine("Unsupported primitive type '{0}' found in geometry '{1}', ignoring.", primitives.PrimitiveType, instance.Name);
					continue;
				}
				ReadPolygonPrimitives(primitives, instance.Materials.Find((MaterialInstance m) => m.Symbol == primitives.MaterialSymbol));
			}
		}

		private void ReadPolygonPrimitives(MeshPrimitives primitives, MaterialInstance materialInstance)
		{
			int[] array = null;
			foreach (IndexedInput input in primitives.Inputs)
			{
				Semantic semantic = input.Semantic;
				if (semantic == Semantic.Position)
				{
					array = ReadInputIndexed(input, positions, Source.ReadVector3);
				}
			}
			int[] array2 = array;
			foreach (int index in array2)
			{
				positions[index] = Vector3.Transform(positions[index], ref nodeTransform);
			}
			int num = 0;
			foreach (int vertexCount in primitives.VertexCounts)
			{
				int[] array3 = new int[vertexCount];
				Array.Copy(array, num, array3, 0, vertexCount);
				Polygon polygon = new Polygon(mesh, array3);
				if (Vector3.Dot(polygon.Plane.Normal, Vector3.UnitY) >= 0.3420201f)
				{
					mesh.Polygons.Add(polygon);
				}
				num += vertexCount;
			}
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
