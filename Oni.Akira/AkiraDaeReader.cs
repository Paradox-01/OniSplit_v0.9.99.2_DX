using System;
using System.Collections.Generic;
using System.IO;
using Oni.Dae;
using Oni.Imaging;

namespace Oni.Akira
{
	internal class AkiraDaeReader
	{
		private readonly PolygonMesh mesh;

		private readonly List<Vector3> positions;

		private readonly Dictionary<Vector3, int> uniquePositions;

		private readonly List<Vector3> normals;

		private readonly List<Vector2> texCoords;

		private readonly Dictionary<Oni.Dae.Material, Material> materialMap;

		private readonly Dictionary<string, Material> materialFileMap;

		private readonly Stack<Matrix> nodeTransformStack;

		private Scene scene;

		private Dictionary<string, AkiraDaeNodeProperties> properties;

		private Matrix nodeTransform;

		private string nodeName;

		public PolygonMesh Mesh
		{
			get
			{
				return mesh;
			}
		}

		public static PolygonMesh Read(IEnumerable<string> filePaths)
		{
			AkiraDaeReader akiraDaeReader = new AkiraDaeReader();
			Dictionary<string, AkiraDaeNodeProperties> dictionary = new Dictionary<string, AkiraDaeNodeProperties>();
			foreach (string filePath in filePaths)
			{
				akiraDaeReader.ReadScene(Reader.ReadFile(filePath), dictionary);
			}
			return akiraDaeReader.mesh;
		}

		public AkiraDaeReader()
		{
			mesh = new PolygonMesh(new MaterialLibrary());
			positions = mesh.Points;
			uniquePositions = new Dictionary<Vector3, int>();
			texCoords = mesh.TexCoords;
			normals = mesh.Normals;
			materialMap = new Dictionary<Oni.Dae.Material, Material>();
			materialFileMap = new Dictionary<string, Material>(StringComparer.OrdinalIgnoreCase);
			nodeTransformStack = new Stack<Matrix>();
			nodeTransform = Matrix.Identity;
		}

		public void ReadScene(Scene scene, Dictionary<string, AkiraDaeNodeProperties> properties)
		{
			this.scene = scene;
			this.properties = properties;
			AkiraDaeNodeProperties value;
			properties.TryGetValue(scene.Id, out value);
			foreach (Node node in scene.Nodes)
			{
				ReadNode(node, value);
			}
		}

		private void ReadNode(Node node, AkiraDaeNodeProperties parentNodeProperties)
		{
			AkiraDaeNodeProperties value;
			if (node.Id == null || !properties.TryGetValue(node.Id, out value))
			{
				value = parentNodeProperties;
			}
			else if (value.HasPhysics)
			{
				return;
			}
			nodeTransformStack.Push(nodeTransform);
			foreach (Transform transform in node.Transforms)
			{
				nodeTransform = transform.ToMatrix() * nodeTransform;
			}
			nodeName = node.Name;
			foreach (GeometryInstance geometryInstance in node.GeometryInstances)
			{
				ReadGeometryInstance(node, value, geometryInstance);
			}
			foreach (Node node2 in node.Nodes)
			{
				ReadNode(node2, value);
			}
			nodeTransform = nodeTransformStack.Pop();
		}

		private void ReadGeometryInstance(Node node, AkiraDaeNodeProperties nodeProperties, GeometryInstance instance)
		{
			foreach (MeshPrimitives primitives in instance.Target.Primitives)
			{
				if (primitives.PrimitiveType != MeshPrimitiveType.Polygons)
				{
					Console.Error.WriteLine("Unsupported primitive type '{0}' found in geometry '{1}', ignoring.", primitives.PrimitiveType, instance.Name);
					continue;
				}
				ReadPolygonPrimitives(node, nodeProperties, primitives, instance.Materials.Find((MaterialInstance m) => m.Symbol == primitives.MaterialSymbol));
			}
		}

		private Material ReadMaterial(Oni.Dae.Material material)
		{
			if (material == null || material.Effect == null)
			{
				return null;
			}
			Material value;
			if (materialMap.TryGetValue(material, out value))
			{
				return value;
			}
			EffectSampler effectSampler = null;
			EffectSampler effectSampler2 = null;
			foreach (EffectTexture texture in material.Effect.Textures)
			{
				if (texture.Channel == EffectTextureChannel.Diffuse)
				{
					effectSampler = texture.Sampler;
				}
				else if (texture.Channel == EffectTextureChannel.Transparent)
				{
					effectSampler2 = texture.Sampler;
				}
			}
			if (effectSampler == null || effectSampler.Surface == null || effectSampler.Surface.InitFrom == null)
			{
				return null;
			}
			Image initFrom = effectSampler.Surface.InitFrom;
			if (materialFileMap.TryGetValue(initFrom.FilePath, out value))
			{
				return value;
			}
			value = mesh.Materials.GetMaterial(Path.GetFileNameWithoutExtension(initFrom.FilePath));
			value.ImageFilePath = initFrom.FilePath;
			if (effectSampler2 == effectSampler)
			{
				value.Flags |= GunkFlags.Transparent | GunkFlags.TwoSided | GunkFlags.NoOcclusion;
			}
			materialFileMap.Add(initFrom.FilePath, value);
			materialMap.Add(material, value);
			return value;
		}

		private void ReadPolygonPrimitives(Node node, AkiraDaeNodeProperties nodeProperties, MeshPrimitives primitives, MaterialInstance materialInstance)
		{
			Material material = null;
			if (materialInstance != null)
			{
				material = ReadMaterial(materialInstance.Target);
			}
			if (material == null)
			{
				material = mesh.Materials.NotFound;
			}
			int[] sourceArray = null;
			int[] array = null;
			int[] array2 = null;
			Color[] array3 = null;
			foreach (IndexedInput input in primitives.Inputs)
			{
				switch (input.Semantic)
				{
				case Semantic.Position:
					sourceArray = ReadInputIndexed(input, positions, uniquePositions, PositionReader);
					break;
				case Semantic.TexCoord:
					array = ReadInputIndexed(input, texCoords, Source.ReadTexCoord);
					break;
				case Semantic.Normal:
					array2 = ReadInputIndexed(input, normals, Source.ReadVector3);
					break;
				case Semantic.Color:
					array3 = ReadInput(input, Source.ReadColor);
					break;
				}
			}
			if (array == null)
			{
				Console.Error.WriteLine("Geometry '{0}' does not contain texture coordinates.", nodeName);
			}
			int num = 0;
			int num2 = 0;
			foreach (int vertexCount in primitives.VertexCounts)
			{
				int[] array4 = new int[vertexCount];
				Array.Copy(sourceArray, num, array4, 0, vertexCount);
				if (CheckDegenerate(positions, array4))
				{
					num2++;
					num += vertexCount;
					continue;
				}
				Polygon polygon = new Polygon(mesh, array4)
				{
					FileName = node.FileName,
					ObjectName = node.Name,
					Material = material
				};
				if (array != null)
				{
					polygon.TexCoordIndices = new int[vertexCount];
					Array.Copy(array, num, polygon.TexCoordIndices, 0, vertexCount);
				}
				else
				{
					polygon.TexCoordIndices = new int[vertexCount];
				}
				if (array2 != null)
				{
					polygon.NormalIndices = new int[vertexCount];
					Array.Copy(array2, num, polygon.NormalIndices, 0, vertexCount);
				}
				if (array3 != null)
				{
					polygon.Colors = new Color[vertexCount];
					Array.Copy(array3, num, polygon.Colors, 0, vertexCount);
				}
				num += vertexCount;
				if (nodeProperties != null)
				{
					polygon.ScriptId = nodeProperties.ScriptId;
					polygon.Flags |= nodeProperties.GunkFlags;
				}
				if (material == mesh.Materials.Markers.Ghost)
				{
					mesh.Ghosts.Add(polygon);
				}
				else if (material == mesh.Materials.Markers.DoorFrame)
				{
					mesh.Doors.Add(polygon);
				}
				else if (material.Name.StartsWith("bnv_grid_", StringComparison.Ordinal))
				{
					mesh.Floors.Add(polygon);
				}
				else
				{
					mesh.Polygons.Add(polygon);
				}
			}
			if (num2 > 0)
			{
				Console.Error.WriteLine("Ignoring {0} degenerate polygons", num2);
			}
		}

		private static bool CheckDegenerate(List<Vector3> positions, int[] positionIndices)
		{
			if (positionIndices.Length < 3)
			{
				return true;
			}
			Vector3 vector = positions[positionIndices[0]];
			Vector3 vector2 = positions[positionIndices[1]];
			for (int i = 2; i < positionIndices.Length; i++)
			{
				Vector3 vector3 = positions[positionIndices[i]];
				Vector3 v = vector - vector2;
				Vector3 v2 = vector3 - vector2;
				Vector3 r;
				Vector3.Cross(ref v, ref v2, out r);
				if (Math.Abs(r.LengthSquared()) < 0.0001f && Vector3.Dot(v, v2) > 0f)
				{
					return true;
				}
				vector = vector2;
				vector2 = vector3;
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

		private static int[] ReadInputIndexed<T>(IndexedInput input, List<T> list, Dictionary<T, int> uniqueList, Func<Source, int, T> elementReader) where T : struct
		{
			int[] array = new int[input.Indices.Count];
			for (int i = 0; i < input.Indices.Count; i++)
			{
				T val = elementReader(input.Source, input.Indices[i]);
				int value;
				if (!uniqueList.TryGetValue(val, out value))
				{
					value = list.Count;
					list.Add(val);
					uniqueList.Add(val, value);
				}
				array[i] = value;
			}
			return array;
		}

		private static T[] ReadInput<T>(IndexedInput input, Func<Source, int, T> elementReader) where T : struct
		{
			T[] array = new T[input.Indices.Count];
			for (int i = 0; i < input.Indices.Count; i++)
			{
				array[i] = elementReader(input.Source, input.Indices[i]);
			}
			return array;
		}

		private Vector3 PositionReader(Source source, int index)
		{
			Vector3 v = Source.ReadVector3(source, index);
			Vector3 r;
			Vector3.Transform(ref v, ref nodeTransform, out r);
			return r;
		}
	}
}
