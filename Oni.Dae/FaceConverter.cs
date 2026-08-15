using System;
using System.Collections.Generic;

namespace Oni.Dae
{
	internal class FaceConverter
	{
		private Node root;

		private int maxEdges = 3;

		public static void Triangulate(Node root)
		{
			FaceConverter faceConverter = new FaceConverter
			{
				root = root
			};
			faceConverter.Convert();
		}

		private void Convert()
		{
			ConvertNode(root);
		}

		private void ConvertNode(Node node)
		{
			foreach (Instance instance in node.Instances)
			{
				ConvertInstance(instance);
			}
			foreach (Node node2 in node.Nodes)
			{
				ConvertNode(node2);
			}
		}

		private void ConvertInstance(Instance instance)
		{
			GeometryInstance geometryInstance = instance as GeometryInstance;
			if (geometryInstance != null)
			{
				ConvertGeometry(geometryInstance.Target);
			}
		}

		private void ConvertGeometry(Geometry geometry)
		{
			foreach (MeshPrimitives primitive in geometry.Primitives)
			{
				if (primitive.PrimitiveType == MeshPrimitiveType.Polygons && !primitive.VertexCounts.All((int c) => c == 3))
				{
					ConvertPolygons(geometry, primitive);
				}
			}
		}

		private void ConvertPolygons(Geometry geometry, MeshPrimitives primitives)
		{
			IndexedInput indexedInput = primitives.Inputs.FirstOrDefault((IndexedInput i) => i.Semantic == Semantic.Position);
			if (indexedInput == null)
			{
				Console.Error.WriteLine("{0}: cannot find position input", geometry.Name);
				return;
			}
			List<int> list = new List<int>(primitives.VertexCounts.Count * 2);
			List<int> list2 = new List<int>(primitives.VertexCounts.Count * 2);
			int num = 0;
			foreach (int vertexCount in primitives.VertexCounts)
			{
				if (vertexCount < 3)
				{
					Console.Error.WriteLine("{0}: skipping bad face (line)", geometry.Name);
				}
				else if (vertexCount <= maxEdges)
				{
					for (int num2 = 0; num2 < vertexCount; num2++)
					{
						list.Add(num + num2);
					}
					list2.Add(vertexCount);
				}
				else
				{
					ConvertPolygon(geometry, indexedInput, num, vertexCount, list, list2);
				}
				num += vertexCount;
			}
			primitives.VertexCounts.Clear();
			primitives.VertexCounts.AddRange(list2);
			int[][] array = new int[primitives.Inputs.Count][];
			for (int num3 = 0; num3 < primitives.Inputs.Count; num3++)
			{
				IndexedInput indexedInput2 = primitives.Inputs[num3];
				array[num3] = indexedInput2.Indices.ToArray();
				indexedInput2.Indices.Clear();
			}
			for (int num4 = 0; num4 < primitives.Inputs.Count; num4++)
			{
				List<int> indices = primitives.Inputs[num4].Indices;
				int[] array2 = array[num4];
				foreach (int item in list)
				{
					indices.Add(array2[item]);
				}
			}
		}

		private void ConvertPolygon(Geometry geometry, IndexedInput input, int offset, int vcount, List<int> newFaces, List<int> newVertexCounts)
		{
			Vector3[] array = new Vector3[vcount];
			for (int i = 0; i < vcount; i++)
			{
				array[i] = Source.ReadVector3(input.Source, input.Indices[offset + i]);
			}
			int num = -1;
			for (int j = 0; j < vcount; j++)
			{
				Vector3 v = array[j];
				Vector3 v2 = array[(j + 1) % vcount];
				if (Vector3.Dot(v, v2) < 0f)
				{
					num = j;
					break;
				}
			}
			if (num == -1)
			{
				for (int k = 0; k < vcount - 2; k++)
				{
					newFaces.Add(offset);
					newFaces.Add(offset + 1 + k);
					newFaces.Add(offset + 2 + k);
					newVertexCounts.Add(3);
				}
			}
			else if (vcount == 4)
			{
				newFaces.Add(offset + num);
				newFaces.Add(offset + (num + 1) % vcount);
				newFaces.Add(offset + (num + 2) % vcount);
				newVertexCounts.Add(3);
				newFaces.Add(offset + (num + vcount - 1) % vcount);
				newFaces.Add(offset + (num + vcount - 2) % vcount);
				newFaces.Add(offset + num);
				newVertexCounts.Add(3);
			}
			else
			{
				Console.Error.WriteLine("{0}: skipping bad face (concave {1}-gon)", geometry.Name, vcount);
			}
		}
	}
}
