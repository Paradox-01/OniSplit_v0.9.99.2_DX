using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Oni.Dae.IO
{
	internal class ObjReader
	{
		private struct ObjVertex : IEquatable<ObjVertex>
		{
			public int PointIndex;

			public int TexCoordIndex;

			public int NormalIndex;

			public ObjVertex(int pointIndex, int uvIndex, int normalIndex)
			{
				PointIndex = pointIndex;
				TexCoordIndex = uvIndex;
				NormalIndex = normalIndex;
			}

			public static bool operator ==(ObjVertex v1, ObjVertex v2)
			{
				if (v1.PointIndex == v2.PointIndex && v1.TexCoordIndex == v2.TexCoordIndex)
				{
					return v1.NormalIndex == v2.NormalIndex;
				}
				return false;
			}

			public static bool operator !=(ObjVertex v1, ObjVertex v2)
			{
				if (v1.PointIndex == v2.PointIndex && v1.TexCoordIndex == v2.TexCoordIndex)
				{
					return v1.NormalIndex != v2.NormalIndex;
				}
				return true;
			}

			public bool Equals(ObjVertex v)
			{
				return this == v;
			}

			public override bool Equals(object obj)
			{
				if (obj is ObjVertex)
				{
					return Equals((ObjVertex)obj);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return PointIndex ^ TexCoordIndex ^ NormalIndex;
			}
		}

		private class ObjFace
		{
			public string ObjectName;

			public string[] GroupsNames;

			public ObjVertex[] Vertices;
		}

		private class ObjMaterial
		{
			private readonly string name;

			private string textureFilePath;

			private Material material;

			public string Name
			{
				get
				{
					return name;
				}
			}

			public string TextureFilePath
			{
				get
				{
					return textureFilePath;
				}
				set
				{
					textureFilePath = value;
				}
			}

			public Material Material
			{
				get
				{
					if (material == null && TextureFilePath != null)
					{
						CreateMaterial();
					}
					return material;
				}
			}

			public ObjMaterial(string name)
			{
				this.name = name;
			}

			private void CreateMaterial()
			{
				Image initFrom = new Image
				{
					FilePath = TextureFilePath,
					Name = name + "_img"
				};
				EffectSurface effectSurface = new EffectSurface(initFrom);
				EffectSampler effectSampler = new EffectSampler(effectSurface);
				EffectTexture diffuseValue = new EffectTexture
				{
					Sampler = effectSampler,
					Channel = EffectTextureChannel.Diffuse,
					TexCoordSemantic = "diffuse_TEXCOORD"
				};
				material = new Material
				{
					Id = name,
					Name = name,
					Effect = new Effect
					{
						Id = name + "_fx",
						DiffuseValue = diffuseValue,
						Parameters = 
						{
							new EffectParameter("surface", effectSurface),
							new EffectParameter("sampler", effectSampler)
						}
					}
				};
			}
		}

		private class ObjPrimitives
		{
			public ObjMaterial Material;

			public readonly List<ObjFace> Faces = new List<ObjFace>(4);
		}

		private static readonly string[] emptyStrings = new string[0];

		private static readonly char[] whiteSpaceChars = new char[2] { ' ', '\t' };

		private static readonly char[] vertexSeparator = new char[1] { '/' };

		private Scene mainScene;

		private readonly List<Vector3> points = new List<Vector3>();

		private readonly List<Vector2> texCoords = new List<Vector2>();

		private readonly List<Vector3> normals = new List<Vector3>();

		private int pointCount;

		private int normalCount;

		private int texCoordCount;

		private readonly Dictionary<Vector3, int> pointIndex = new Dictionary<Vector3, int>();

		private readonly Dictionary<Vector3, int> normalIndex = new Dictionary<Vector3, int>();

		private readonly Dictionary<Vector2, int> texCoordIndex = new Dictionary<Vector2, int>();

		private readonly List<int> pointRemap = new List<int>();

		private readonly List<int> normalRemap = new List<int>();

		private readonly List<int> texCoordRemap = new List<int>();

		private readonly Dictionary<string, ObjMaterial> materials = new Dictionary<string, ObjMaterial>(StringComparer.Ordinal);

		private string currentObjectName;

		private string[] currentGroupNames;

		private readonly List<ObjPrimitives> primitives = new List<ObjPrimitives>();

		private ObjPrimitives currentPrimitives;

		public static Scene ReadFile(string filePath)
		{
			ObjReader objReader = new ObjReader();
			objReader.ReadObjFile(filePath);
			objReader.ImportObjects();
			return objReader.mainScene;
		}

		private void ReadObjFile(string filePath)
		{
			mainScene = new Scene();
			foreach (string item in ReadLines(filePath))
			{
				string[] array = item.Split(whiteSpaceChars, StringSplitOptions.RemoveEmptyEntries);
				switch (array[0])
				{
				case "o":
					ReadObject(array);
					break;
				case "g":
					ReadGroup(array);
					break;
				case "v":
					ReadPoint(array);
					break;
				case "vn":
					ReadNormal(array);
					break;
				case "vt":
					ReadTexCoord(array);
					break;
				case "f":
				case "fo":
					ReadFace(array);
					break;
				case "mtllib":
					ReadMtlLib(filePath, array);
					break;
				case "usemtl":
					ReadUseMtl(array);
					break;
				}
			}
		}

		private void ReadPoint(string[] tokens)
		{
			Vector3 point = new Vector3(float.Parse(tokens[1], CultureInfo.InvariantCulture), float.Parse(tokens[2], CultureInfo.InvariantCulture), float.Parse(tokens[3], CultureInfo.InvariantCulture));
			AddPoint(point);
			pointCount++;
		}

		private void AddPoint(Vector3 point)
		{
			int value;
			if (pointIndex.TryGetValue(point, out value))
			{
				pointRemap.Add(value);
				return;
			}
			pointRemap.Add(points.Count);
			pointIndex.Add(point, points.Count);
			points.Add(point);
		}

		private void ReadNormal(string[] tokens)
		{
			Vector3 normal = new Vector3(float.Parse(tokens[1], CultureInfo.InvariantCulture), float.Parse(tokens[2], CultureInfo.InvariantCulture), float.Parse(tokens[3], CultureInfo.InvariantCulture));
			AddNormal(normal);
			normalCount++;
		}

		private void AddNormal(Vector3 normal)
		{
			int value;
			if (normalIndex.TryGetValue(normal, out value))
			{
				normalRemap.Add(value);
				return;
			}
			normalRemap.Add(normals.Count);
			normalIndex.Add(normal, normals.Count);
			normals.Add(normal);
		}

		private void ReadTexCoord(string[] tokens)
		{
			Vector2 texCoord = new Vector2(float.Parse(tokens[1], CultureInfo.InvariantCulture), 1f - float.Parse(tokens[2], CultureInfo.InvariantCulture));
			AddTexCoord(texCoord);
			texCoordCount++;
		}

		private void AddTexCoord(Vector2 texCoord)
		{
			int value;
			if (texCoordIndex.TryGetValue(texCoord, out value))
			{
				texCoordRemap.Add(value);
				return;
			}
			texCoordRemap.Add(texCoords.Count);
			texCoordIndex.Add(texCoord, texCoords.Count);
			texCoords.Add(texCoord);
		}

		private void ReadFace(string[] tokens)
		{
			ObjVertex[] vertices = ReadVertices(tokens);
			if (currentPrimitives == null)
			{
				ReadUseMtl(emptyStrings);
			}
			currentPrimitives.Faces.Add(new ObjFace
			{
				ObjectName = currentObjectName,
				GroupsNames = currentGroupNames,
				Vertices = vertices
			});
		}

		private ObjVertex[] ReadVertices(string[] tokens)
		{
			ObjVertex[] array = new ObjVertex[tokens.Length - 1];
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = tokens[i + 1].Split(vertexSeparator);
				if (array2.Length == 0 || array2.Length > 3)
				{
					throw new InvalidDataException();
				}
				int num = int.Parse(array2[0], CultureInfo.InvariantCulture);
				int num2 = ((array2.Length > 1 && array2[1].Length > 0) ? int.Parse(array2[1], CultureInfo.InvariantCulture) : 0);
				int num3 = ((array2.Length > 2 && array2[2].Length > 0) ? int.Parse(array2[2], CultureInfo.InvariantCulture) : 0);
				if (num < 0)
				{
					num = pointCount + num + 1;
				}
				if (num2 < 0)
				{
					num2 = texCoordCount + num2 + 1;
				}
				if (num3 < 0)
				{
					num3 = normalCount + num3 + 1;
				}
				num--;
				num2--;
				num3--;
				num = pointRemap[num];
				num2 = ((num2 >= 0 && texCoordRemap.Count > num2) ? texCoordRemap[num2] : (-1));
				num3 = ((num3 >= 0 && normalRemap.Count > num3) ? normalRemap[num3] : (-1));
				array[i] = new ObjVertex
				{
					PointIndex = num,
					TexCoordIndex = num2,
					NormalIndex = num3
				};
			}
			return array;
		}

		private void ReadObject(string[] tokens)
		{
			currentObjectName = tokens[1];
		}

		private void ReadGroup(string[] tokens)
		{
			currentGroupNames = tokens;
		}

		private void ReadUseMtl(string[] tokens)
		{
			currentPrimitives = new ObjPrimitives();
			if (tokens.Length != 0)
			{
				materials.TryGetValue(tokens[1], out currentPrimitives.Material);
			}
			primitives.Add(currentPrimitives);
		}

		private void ReadMtlLib(string objFilePath, string[] tokens)
		{
			string text = tokens[1];
			if (Path.GetExtension(text).Length == 0)
			{
				text += ".mtl";
			}
			string directoryName = Path.GetDirectoryName(objFilePath);
			string text2 = Path.Combine(directoryName, text);
			if (!File.Exists(text2))
			{
				Console.Error.WriteLine("Material file {0} does not exist", text2);
			}
			else
			{
				ReadMtlFile(text2);
			}
		}

		private void ReadMtlFile(string filePath)
		{
			string directoryName = Path.GetDirectoryName(filePath);
			ObjMaterial objMaterial = null;
			foreach (string item in ReadLines(filePath))
			{
				string[] array = item.Split(whiteSpaceChars, StringSplitOptions.RemoveEmptyEntries);
				switch (array[0])
				{
				case "newmtl":
					objMaterial = new ObjMaterial(array[1]);
					materials[objMaterial.Name] = objMaterial;
					break;
				case "map_Kd":
				{
					string fullPath = Path.GetFullPath(Path.Combine(directoryName, array[1]));
					if (File.Exists(fullPath))
					{
						objMaterial.TextureFilePath = fullPath;
					}
					break;
				}
				}
			}
		}

		private void ImportObjects()
		{
			List<IndexedInput> list = new List<IndexedInput>();
			IndexedInput indexedInput = new IndexedInput(Semantic.Position, new Source(points));
			list.Add(indexedInput);
			IndexedInput indexedInput2;
			if (texCoords.Count > 0)
			{
				indexedInput2 = new IndexedInput(Semantic.TexCoord, new Source(texCoords));
				list.Add(indexedInput2);
			}
			else
			{
				indexedInput2 = null;
			}
			IndexedInput indexedInput3;
			if (normals.Count > 0)
			{
				indexedInput3 = new IndexedInput(Semantic.Normal, new Source(normals));
				list.Add(indexedInput3);
			}
			else
			{
				indexedInput3 = null;
			}
			Geometry geometry = new Geometry
			{
				Vertices = { (Input)indexedInput }
			};
			GeometryInstance geometryInstance = new GeometryInstance
			{
				Target = geometry
			};
			foreach (ObjPrimitives item in primitives.Where((ObjPrimitives p) => p.Faces.Count > 0))
			{
				MeshPrimitives meshPrimitives = new MeshPrimitives(MeshPrimitiveType.Polygons, list);
				foreach (ObjFace face in item.Faces)
				{
					meshPrimitives.VertexCounts.Add(face.Vertices.Length);
					ObjVertex[] vertices = face.Vertices;
					for (int num = 0; num < vertices.Length; num++)
					{
						ObjVertex objVertex = vertices[num];
						indexedInput.Indices.Add(objVertex.PointIndex);
						if (indexedInput2 != null)
						{
							indexedInput2.Indices.Add(objVertex.TexCoordIndex);
						}
						if (indexedInput3 != null)
						{
							indexedInput3.Indices.Add(objVertex.NormalIndex);
						}
					}
				}
				geometry.Primitives.Add(meshPrimitives);
				if (item.Material != null && item.Material.Material != null)
				{
					meshPrimitives.MaterialSymbol = "mat" + geometryInstance.Materials.Count;
					geometryInstance.Materials.Add(new MaterialInstance
					{
						Symbol = meshPrimitives.MaterialSymbol,
						Target = item.Material.Material,
						Bindings = 
						{
							new MaterialBinding("diffuse_TEXCOORD", indexedInput2)
						}
					});
				}
			}
			mainScene.Nodes.Add(new Node
			{
				Instances = { (Instance)geometryInstance }
			});
		}

		private Vector3 ComputeFaceNormal(ObjVertex[] vertices)
		{
			if (vertices.Length < 3)
			{
				return Vector3.Up;
			}
			Vector3 vector = points[vertices[0].PointIndex];
			Vector3 vector2 = points[vertices[1].PointIndex];
			Vector3 vector3 = points[vertices[2].PointIndex];
			return Vector3.Normalize(Vector3.Cross(vector2 - vector, vector3 - vector));
		}

		private static IEnumerable<string> ReadLines(string filePath)
		{
			using (StreamReader reader = File.OpenText(filePath))
			{
				for (string text = reader.ReadLine(); text != null; text = reader.ReadLine())
				{
					text = text.Trim();
					if (text.Length == 0)
					{
						continue;
					}
					int num = text.IndexOf('#');
					if (num != -1)
					{
						text = text.Substring(0, num).Trim();
						if (text.Length == 0)
						{
							continue;
						}
					}
					yield return text;
				}
			}
		}
	}
}
