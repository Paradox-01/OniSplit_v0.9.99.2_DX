using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Oni.Dae;
using Oni.Imaging;

namespace Oni.Akira
{
	internal class RoomDaeWriter
	{
		private class DaePolygon
		{
			private readonly Polygon source;

			private readonly Material material;

			private readonly int[] pointIndices;

			private readonly int[] texCoordIndices;

			private readonly int[] colorIndices;

			public Polygon Source
			{
				get
				{
					return source;
				}
			}

			public Material Material
			{
				get
				{
					return material;
				}
			}

			public int[] PointIndices
			{
				get
				{
					return pointIndices;
				}
			}

			public int[] TexCoordIndices
			{
				get
				{
					return texCoordIndices;
				}
			}

			public int[] ColorIndices
			{
				get
				{
					return colorIndices;
				}
			}

			public DaePolygon(Polygon source, int[] pointIndices, int[] texCoordIndices, int[] colorIndices)
			{
				this.source = source;
				material = source.Material;
				this.pointIndices = pointIndices;
				this.texCoordIndices = texCoordIndices;
				this.colorIndices = colorIndices;
			}

			public DaePolygon(Material material, int[] pointIndices, int[] texCoordIndices)
			{
				this.material = material;
				this.pointIndices = pointIndices;
				this.texCoordIndices = texCoordIndices;
			}
		}

		private class DaeMeshBuilder
		{
			private readonly List<DaePolygon> polygons = new List<DaePolygon>();

			private readonly List<Vector3> points = new List<Vector3>();

			private readonly Dictionary<Vector3, int> uniquePoints = new Dictionary<Vector3, int>();

			private readonly List<Vector2> texCoords = new List<Vector2>();

			private readonly Dictionary<Vector2, int> uniqueTexCoords = new Dictionary<Vector2, int>();

			private readonly List<Color> colors = new List<Color>();

			private readonly Dictionary<Color, int> uniqueColors = new Dictionary<Color, int>();

			private string name;

			private Vector3 translation;

			private Geometry geometry;

			public string Name
			{
				get
				{
					return name;
				}
				set
				{
					name = value;
				}
			}

			public Vector3 Translation
			{
				get
				{
					return translation;
				}
			}

			public IEnumerable<Polygon> Polygons
			{
				get
				{
					return from p in polygons
						where p.Source != null
						select p.Source;
				}
			}

			public Geometry Geometry
			{
				get
				{
					return geometry;
				}
			}

			public DaeMeshBuilder(string name)
			{
				this.name = name;
			}

			public void ResetTransform()
			{
				Vector3 center = BoundingSphere.CreateFromPoints(points).Center;
				center.Y = BoundingBox.CreateFromPoints(points).Min.Y;
				translation = center;
				for (int i = 0; i < points.Count; i++)
				{
					points[i] -= center;
				}
			}

			public void AddPolygon(Polygon polygon)
			{
				polygons.Add(new DaePolygon(polygon, Remap(polygon.Mesh.Points, polygon.PointIndices, points, uniquePoints), null, null));
			}

			private static int[] Remap<T>(IList<T> values, int[] indices, List<T> list, Dictionary<T, int> unique) where T : struct
			{
				int[] array = new int[indices.Length];
				for (int i = 0; i < indices.Length; i++)
				{
					array[i] = AddUnique(list, unique, values[indices[i]]);
				}
				return array;
			}

			private static int[] Remap<T>(IList<T> values, List<T> list, Dictionary<T, int> unique) where T : struct
			{
				int[] array = new int[values.Count];
				for (int i = 0; i < values.Count; i++)
				{
					array[i] = AddUnique(list, unique, values[i]);
				}
				return array;
			}

			private static int AddUnique<T>(List<T> list, Dictionary<T, int> unique, T value) where T : struct
			{
				int value2;
				if (!unique.TryGetValue(value, out value2))
				{
					value2 = list.Count;
					unique.Add(value, value2);
					list.Add(value);
				}
				return value2;
			}

			public void Build()
			{
				Source source = new Source(points);
				MeshPrimitives meshPrimitives = new MeshPrimitives(MeshPrimitiveType.Polygons);
				IndexedInput indexedInput = new IndexedInput(Semantic.Position, source);
				meshPrimitives.Inputs.Add(indexedInput);
				foreach (DaePolygon polygon in polygons)
				{
					meshPrimitives.VertexCounts.Add(polygon.PointIndices.Length);
					indexedInput.Indices.AddRange(polygon.PointIndices);
				}
				geometry = new Geometry
				{
					Name = Name + "_geo",
					Vertices = 
					{
						new Input(Semantic.Position, source)
					},
					Primitives = { meshPrimitives }
				};
			}
		}

		private class DaeSceneBuilder
		{
			private readonly Scene scene;

			private readonly Dictionary<string, DaeMeshBuilder> nameMeshBuilder;

			private readonly List<DaeMeshBuilder> meshBuilders;

			private readonly Dictionary<Material, Oni.Dae.Material> materials;

			private string imagesFolder = "images";

			public string ImagesFolder
			{
				get
				{
					return imagesFolder;
				}
				set
				{
					imagesFolder = value;
				}
			}

			public IEnumerable<DaeMeshBuilder> MeshBuilders
			{
				get
				{
					return meshBuilders;
				}
			}

			public DaeSceneBuilder()
			{
				scene = new Scene();
				nameMeshBuilder = new Dictionary<string, DaeMeshBuilder>(StringComparer.Ordinal);
				meshBuilders = new List<DaeMeshBuilder>();
				materials = new Dictionary<Material, Oni.Dae.Material>();
			}

			public DaeMeshBuilder GetMeshBuilder(string name)
			{
				DaeMeshBuilder value;
				if (!nameMeshBuilder.TryGetValue(name, out value))
				{
					value = new DaeMeshBuilder(name);
					nameMeshBuilder.Add(name, value);
					meshBuilders.Add(value);
				}
				return value;
			}

			public Oni.Dae.Material GetMaterial(Material material)
			{
				Oni.Dae.Material value;
				if (!materials.TryGetValue(material, out value))
				{
					value = new Oni.Dae.Material();
					materials.Add(material, value);
				}
				return value;
			}

			public void Build()
			{
				BuildNodes();
				BuildMaterials();
			}

			private void BuildNodes()
			{
				foreach (DaeMeshBuilder meshBuilder in meshBuilders)
				{
					meshBuilder.Build();
					GeometryInstance item = new GeometryInstance(meshBuilder.Geometry);
					Node node = new Node();
					node.Name = meshBuilder.Name;
					node.Instances.Add(item);
					if (meshBuilder.Translation != Vector3.Zero)
					{
						node.Transforms.Add(new TransformTranslate(meshBuilder.Translation));
					}
					scene.Nodes.Add(node);
				}
			}

			private void BuildMaterials()
			{
				foreach (KeyValuePair<Material, Oni.Dae.Material> material in materials)
				{
					Material key = material.Key;
					Image initFrom = new Image
					{
						FilePath = "./" + GetImageFileName(key).Replace('\\', '/'),
						Name = key.Name + "_img"
					};
					EffectSurface effectSurface = new EffectSurface(initFrom);
					EffectSampler effectSampler = new EffectSampler(effectSurface);
					EffectTexture effectTexture = new EffectTexture(effectSampler, "diffuse_TEXCOORD");
					Effect effect = new Effect
					{
						Name = key.Name + "_fx",
						AmbientValue = Vector4.One,
						SpecularValue = Vector4.Zero,
						DiffuseValue = effectTexture,
						TransparentValue = (key.Image.HasAlpha ? effectTexture : null),
						Parameters = 
						{
							new EffectParameter("surface", effectSurface),
							new EffectParameter("sampler", effectSampler)
						}
					};
					Oni.Dae.Material value = material.Value;
					value.Name = key.Name;
					value.Effect = effect;
				}
			}

			private string GetImageFileName(Material material)
			{
				if (material.IsMarker)
				{
					return Path.Combine("markers", material.Name + ".tga");
				}
				return Path.Combine(imagesFolder, material.Name + ".tga");
			}

			public void Write(string filePath)
			{
				string directoryName = Path.GetDirectoryName(filePath);
				foreach (Material key in materials.Keys)
				{
					TgaWriter.Write(key.Image, Path.Combine(directoryName, GetImageFileName(key)));
				}
				Writer.WriteFile(filePath, scene);
			}
		}

		private readonly PolygonMesh source;

		private DaeSceneBuilder world;

		private static readonly string[] objectTypeNames = new string[19]
		{
			"", "char", "patr", "door", "flag", "furn", "", "", "part", "pwru",
			"sndg", "trgv", "weap", "trig", "turr", "cons", "cmbt", "mele", "neut"
		};

		public static void Write(PolygonMesh mesh, string filePath)
		{
			RoomDaeWriter roomDaeWriter = new RoomDaeWriter(mesh);
			roomDaeWriter.WriteGeometry();
			roomDaeWriter.world.Write(filePath);
		}

		private RoomDaeWriter(PolygonMesh source)
		{
			this.source = source;
		}

		private void WriteGeometry()
		{
			world = new DaeSceneBuilder();
			for (int i = 0; i < source.Polygons.Count; i++)
			{
				Polygon polygon = source.Polygons[i];
				string name = string.Format(CultureInfo.InvariantCulture, "floor_{0}", new object[1] { i });
				DaeMeshBuilder meshBuilder = world.GetMeshBuilder(name);
				meshBuilder.AddPolygon(polygon);
			}
			foreach (DaeMeshBuilder meshBuilder2 in world.MeshBuilders)
			{
				meshBuilder2.ResetTransform();
			}
			world.Build();
		}
	}
}
