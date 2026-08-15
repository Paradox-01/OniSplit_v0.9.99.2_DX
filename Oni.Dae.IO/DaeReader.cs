using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using Oni.Xml;

namespace Oni.Dae.IO
{
	internal class DaeReader
	{
		private class Animation : Entity
		{
			private List<Animation> animations;

			private readonly List<Sampler> samplers = new List<Sampler>();

			public List<Animation> Animations
			{
				get
				{
					if (animations == null)
					{
						animations = new List<Animation>();
					}
					return animations;
				}
			}

			public List<Sampler> Samplers
			{
				get
				{
					return samplers;
				}
			}
		}

		private class TargetPath
		{
			private string nodeId;

			private string[] path;

			private string value;

			public string NodeId
			{
				get
				{
					return nodeId;
				}
			}

			public string[] Path
			{
				get
				{
					return path;
				}
			}

			public string Value
			{
				get
				{
					return value;
				}
			}

			private TargetPath()
			{
			}

			public static TargetPath Parse(string text)
			{
				TargetPath targetPath = new TargetPath();
				List<string> list = new List<string>();
				int num = text.IndexOf('/');
				if (num == -1)
				{
					num = text.Length;
				}
				targetPath.nodeId = text.Substring(0, num);
				for (int num2 = num + 1; num2 < text.Length; num2 = num + 1)
				{
					num = text.IndexOf('/', num2);
					if (num == -1)
					{
						num = text.IndexOf('.', num2);
						if (num == -1)
						{
							list.Add(text.Substring(num2));
							break;
						}
						list.Add(text.Substring(num2, num - num2));
						targetPath.value = text.Substring(num + 1);
						break;
					}
					list.Add(text.Substring(num2, num - num2));
				}
				if (list.Count > 0)
				{
					targetPath.path = list.ToArray();
				}
				return targetPath;
			}
		}

		public static string[] CommandLineArgs;

		private static readonly string[] emptyStrings = new string[0];

		private static readonly char[] whiteSpaceChars = new char[2] { ' ', '\t' };

		private static readonly Func<string, int> intConverter = XmlConvert.ToInt32;

		private static readonly Func<string, float> floatConverter = XmlConvert.ToSingle;

		private TextWriter error;

		private TextWriter info;

		private Scene mainScene;

		private Dictionary<string, Entity> entities;

		private XmlReader xml;

		private Axis upAxis = Axis.Y;

		private float unit = 1f;

		private List<Action> delayedBindActions;

		private Uri baseUrl;

		private string fileName;

		private List<Scene> scenes = new List<Scene>();

		private List<Light> lights = new List<Light>();

		private List<Animation> animations = new List<Animation>();

		private List<Geometry> geometries = new List<Geometry>();

		private List<Effect> effects = new List<Effect>();

		private List<Material> materials = new List<Material>();

		private List<Image> images = new List<Image>();

		private List<Camera> cameras = new List<Camera>();

		public static Scene ReadFile(string filePath)
		{
			DaeReader daeReader = new DaeReader();
			daeReader.baseUrl = new Uri("file://" + Path.GetDirectoryName(filePath).Replace('\\', '/').TrimEnd('/') + "/");
			daeReader.fileName = Path.GetFileName(filePath);
			daeReader.delayedBindActions = new List<Action>();
			daeReader.error = Console.Error;
			daeReader.info = Console.Out;
			DaeReader daeReader2 = daeReader;
			XmlReaderSettings settings = new XmlReaderSettings
			{
				IgnoreWhitespace = true,
				IgnoreProcessingInstructions = true,
				IgnoreComments = true
			};
			using (daeReader2.xml = XmlReader.Create(filePath, settings))
			{
				daeReader2.ReadRoot();
			}
			return daeReader2.mainScene;
		}

		private void ReadRoot()
		{
			while (xml.NodeType != XmlNodeType.Element)
			{
				xml.Read();
			}
			if (xml.LocalName != "COLLADA")
			{
				throw new InvalidDataException(string.Format("Unknown root element {0} found", xml.LocalName));
			}
			string attribute = xml.GetAttribute("version");
			if (attribute != "1.4.0" && attribute != "1.4.1")
			{
				throw new NotSupportedException(string.Format("Unsupported Collada file version {0}", attribute));
			}
			if (!xml.IsEmptyElement)
			{
				xml.ReadStartElement();
				ReadAsset();
				ReadContent();
				ReadExtra();
			}
			foreach (Action delayedBindAction in delayedBindActions)
			{
				delayedBindAction();
			}
			if (mainScene == null && scenes.Count > 0)
			{
				mainScene = scenes[0];
			}
			BindNodes(mainScene);
			float num = 1f;
			bool flag = false;
			if (CommandLineArgs != null)
			{
				flag = CommandLineArgs.Any((string a) => a == "-blender");
				string text = Array.Find(CommandLineArgs, (string x) => x.StartsWith("-dae-scale:", StringComparison.Ordinal));
				if (text != null)
				{
					num = float.Parse(text.Substring(11), CultureInfo.InvariantCulture);
				}
			}
			mainScene.CustomAxisConversion = flag;
			if (!flag && upAxis != Axis.Y)
			{
				AxisConverter.Convert(mainScene, upAxis, Axis.Y);
			}
			else
			{
				mainScene.SceneZUP = upAxis == Axis.Z;
			}
			if (unit != 0.1f || num != 1f)
			{
				UnitConverter.Convert(mainScene, 10f * unit * num);
			}
		}

		private void ReadContent()
		{
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "library_cameras":
					ReadLibrary(cameras, "camera", ReadCamera);
					break;
				case "library_images":
					ReadLibrary(images, "image", ReadImage);
					break;
				case "library_effects":
					ReadLibrary(effects, "effect", ReadEffect);
					break;
				case "library_materials":
					ReadLibrary(materials, "material", ReadMaterial);
					break;
				case "library_geometries":
					ReadLibrary(geometries, "geometry", ReadGeometry);
					break;
				case "library_nodes":
					ReadLibrary(scenes, "node", ReadNode);
					break;
				case "library_visual_scenes":
					ReadLibrary(scenes, "visual_scene", ReadScene);
					break;
				case "library_animations":
					ReadLibrary(animations, "animation", ReadAnimation);
					break;
				case "library_lights":
					ReadLibrary(lights, "light", ReadLight);
					break;
				case "scene":
					ReadScene();
					break;
				default:
					xml.Skip();
					break;
				}
			}
		}

		private void ReadLibrary<T>(ICollection<T> library, string elementName, Action<T> entityReader) where T : Entity, new()
		{
			if (!xml.SkipEmpty())
			{
				xml.ReadStartElement();
				ReadAsset();
				while (xml.IsStartElement(elementName))
				{
					ReadEntity(library, entityReader);
				}
				ReadExtra();
				xml.ReadEndElement();
			}
		}

		private void ReadEntity<T>(ICollection<T> entityCollection, Action<T> entityReader) where T : Entity, new()
		{
			T item = ReadEntity(entityReader);
			entityCollection.Add(item);
		}

		private T ReadEntity<T>(Action<T> entityReader) where T : Entity, new()
		{
			string attribute = xml.GetAttribute("id");
			T val = new T
			{
				Name = xml.GetAttribute("name"),
				FileName = fileName
			};
			AddEntity(attribute, val);
			if (string.IsNullOrEmpty(val.Name))
			{
				val.Name = attribute;
			}
			if (xml.IsEmptyElement)
			{
				xml.ReadStartElement();
				return val;
			}
			xml.ReadStartElement();
			ReadAsset();
			entityReader(val);
			ReadExtra();
			xml.ReadEndElement();
			return val;
		}

		private void ReadCamera(Camera camera)
		{
			ReadAsset();
			xml.ReadStartElement("optics");
			xml.ReadStartElement("technique_common");
			if (xml.IsStartElement("perspective"))
			{
				ReadCameraParameters(camera, CameraType.Perspective);
			}
			else if (xml.IsStartElement("orthographic"))
			{
				ReadCameraParameters(camera, CameraType.Orthographic);
			}
			else if (xml.IsStartElement())
			{
				xml.Skip();
			}
			xml.ReadEndElement();
			while (xml.IsStartElement())
			{
				xml.Skip();
			}
			xml.ReadEndElement();
			if (xml.IsStartElement("imager"))
			{
				xml.Skip();
			}
			ReadExtra();
		}

		private void ReadCameraParameters(Camera camera, CameraType type)
		{
			xml.ReadStartElement();
			camera.Type = type;
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "xfov":
					camera.XFov = xml.ReadElementContentAsFloat();
					break;
				case "yfov":
					camera.YFov = xml.ReadElementContentAsFloat();
					break;
				case "xmag":
					camera.XMag = xml.ReadElementContentAsFloat();
					break;
				case "ymag":
					camera.YMag = xml.ReadElementContentAsFloat();
					break;
				case "aspect_ratio":
					camera.AspectRatio = xml.ReadElementContentAsFloat();
					break;
				case "znear":
					camera.ZNear = xml.ReadElementContentAsFloat();
					break;
				case "zfar":
					camera.ZFar = xml.ReadElementContentAsFloat();
					break;
				default:
					xml.Skip();
					break;
				}
			}
			xml.ReadEndElement();
		}

		private void ReadImage(Image image)
		{
			ReadAsset();
			if (xml.IsStartElement("init_from"))
			{
				string text = xml.ReadElementContentAsString();
				if (!string.IsNullOrEmpty(text))
				{
					Uri uri = new Uri(baseUrl, text);
					image.FilePath = uri.LocalPath;
				}
				ReadExtra();
				return;
			}
			if (xml.IsStartElement("data"))
			{
				throw new NotSupportedException("Embedded image data is not supported");
			}
			throw new InvalidDataException();
		}

		private void ReadEffect(Effect effect)
		{
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "image":
					ReadEntity(images, ReadImage);
					break;
				case "newparam":
					ReadEffectParameterDecl(effect);
					break;
				case "profile_COMMON":
					ReadEffectProfileCommon(effect);
					break;
				default:
					xml.Skip();
					break;
				}
			}
			ReadExtra();
		}

		private void ReadEffectProfileCommon(Effect effect)
		{
			xml.ReadStartElement();
			ReadAsset();
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "image":
					ReadEntity(images, ReadImage);
					break;
				case "newparam":
					ReadEffectParameterDecl(effect);
					break;
				case "technique":
					ReadEffectTechniqueCommon(effect);
					break;
				default:
					xml.Skip();
					break;
				}
			}
			ReadExtra();
			xml.ReadEndElement();
		}

		private void ReadEffectTechniqueCommon(Effect effect)
		{
			xml.ReadStartElement();
			ReadAsset();
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "image":
					ReadEntity(images, ReadImage);
					break;
				case "constant":
				case "lambert":
				case "phong":
				case "blinn":
					xml.ReadStartElement();
					ReadEffectTechniqueParameters(effect);
					xml.ReadEndElement();
					break;
				default:
					xml.Skip();
					break;
				}
			}
			ReadExtra();
			xml.ReadEndElement();
		}

		private void ReadEffectParameterDecl(Effect effect)
		{
			EffectParameter effectParameter = new EffectParameter();
			effectParameter.Sid = xml.GetAttribute("sid");
			xml.ReadStartElement();
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "semantic":
					effectParameter.Semantic = xml.ReadElementContentAsString();
					break;
				case "float":
					effectParameter.Value = xml.ReadElementContentAsFloat();
					break;
				case "float2":
					effectParameter.Value = xml.ReadElementContentAsVector2();
					break;
				case "float3":
					effectParameter.Value = xml.ReadElementContentAsVector3();
					break;
				case "surface":
					effectParameter.Value = ReadEffectSurface(effect);
					break;
				case "sampler2D":
					effectParameter.Value = ReadEffectSampler2D(effect);
					break;
				default:
					xml.Skip();
					break;
				}
			}
			xml.ReadEndElement();
			effect.Parameters.Add(effectParameter);
		}

		private EffectSurface ReadEffectSurface(Effect effect)
		{
			EffectSurface surface = new EffectSurface();
			xml.ReadStartElement();
			while (xml.IsStartElement())
			{
				string localName = xml.LocalName;
				if (localName != null && localName == "init_from")
				{
					BindId(xml.ReadElementContentAsString(), delegate(Image image)
					{
						surface.InitFrom = image;
					});
				}
				else
				{
					xml.Skip();
				}
			}
			xml.ReadEndElement();
			return surface;
		}

		private EffectSampler ReadEffectSampler2D(Effect effect)
		{
			EffectSampler effectSampler = new EffectSampler();
			xml.ReadStartElement();
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "source":
				{
					string text = xml.ReadElementContentAsString();
					foreach (EffectParameter parameter in effect.Parameters)
					{
						if (parameter.Sid == text)
						{
							effectSampler.Surface = parameter.Value as EffectSurface;
						}
					}
					break;
				}
				case "wrap_s":
					effectSampler.WrapS = (EffectSamplerWrap)Enum.Parse(typeof(EffectSamplerWrap), xml.ReadElementContentAsString(), true);
					break;
				case "wrap_t":
					effectSampler.WrapT = (EffectSamplerWrap)Enum.Parse(typeof(EffectSamplerWrap), xml.ReadElementContentAsString(), true);
					break;
				default:
					xml.Skip();
					break;
				}
			}
			xml.ReadEndElement();
			return effectSampler;
		}

		private void ReadEffectTechniqueParameters(Effect effect)
		{
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "emission":
					ReadColorEffectParameter(effect, effect.Emission, EffectTextureChannel.Emission);
					break;
				case "ambient":
					ReadColorEffectParameter(effect, effect.Ambient, EffectTextureChannel.Ambient);
					break;
				case "diffuse":
					ReadColorEffectParameter(effect, effect.Diffuse, EffectTextureChannel.Diffuse);
					break;
				case "specular":
					ReadColorEffectParameter(effect, effect.Specular, EffectTextureChannel.Specular);
					break;
				case "shininess":
					ReadFloatEffectParameter(effect.Shininess);
					break;
				case "reflective":
					ReadColorEffectParameter(effect, effect.Reflective, EffectTextureChannel.Reflective);
					break;
				case "reflectivity":
					ReadFloatEffectParameter(effect.Reflectivity);
					break;
				case "transparent":
					ReadColorEffectParameter(effect, effect.Transparent, EffectTextureChannel.Transparent);
					break;
				case "transparency":
					ReadFloatEffectParameter(effect.Transparency);
					break;
				case "index_of_refraction":
					ReadFloatEffectParameter(effect.IndexOfRefraction);
					break;
				default:
					xml.Skip();
					break;
				}
			}
		}

		private void ReadFloatEffectParameter(EffectParameter parameter)
		{
			xml.ReadStartElement();
			if (xml.IsStartElement("float"))
			{
				parameter.Sid = xml.GetAttribute("sid");
				parameter.Value = xml.ReadElementContentAsFloat();
			}
			else if (xml.IsStartElement("param"))
			{
				parameter.Reference = xml.GetAttribute("ref");
				xml.Skip();
			}
			xml.ReadEndElement();
		}

		private void ReadColorEffectParameter(Effect effect, EffectParameter parameter, EffectTextureChannel channel)
		{
			xml.ReadStartElement();
			if (xml.IsStartElement("color"))
			{
				parameter.Sid = xml.GetAttribute("sid");
				parameter.Value = xml.ReadElementContentAsVector4();
			}
			else if (xml.IsStartElement("param"))
			{
				parameter.Sid = null;
				parameter.Reference = xml.GetAttribute("ref");
			}
			else if (xml.IsStartElement("texture"))
			{
				parameter.Sid = null;
				string attribute = xml.GetAttribute("texcoord");
				string attribute2 = xml.GetAttribute("texture");
				xml.Skip();
				while (xml.IsStartElement("texture"))
				{
					xml.Skip();
				}
				EffectSampler effectSampler = null;
				foreach (EffectParameter parameter2 in effect.Parameters)
				{
					if (parameter2.Sid == attribute2)
					{
						effectSampler = parameter2.Value as EffectSampler;
						break;
					}
				}
				if (effectSampler == null)
				{
					info.WriteLine("COLLADA: cannot find sampler {0} in effect {1}, trying to use image directly", attribute2, effect.Name);
					EffectSurface surface = new EffectSurface();
					effectSampler = new EffectSampler(surface);
					BindId(attribute2, delegate(Image image)
					{
						surface.InitFrom = image;
					});
				}
				EffectTexture value = new EffectTexture
				{
					Channel = channel,
					TexCoordSemantic = attribute,
					Sampler = effectSampler
				};
				parameter.Value = value;
			}
			xml.ReadEndElement();
		}

		private void ReadMaterial(Material material)
		{
			if (xml.IsStartElement("instance_effect"))
			{
				BindUrlAttribute("url", delegate(Effect effect)
				{
					material.Effect = effect;
				});
				xml.Skip();
			}
		}

		private void ReadGeometry(Geometry geometry)
		{
			if (xml.IsStartElement("mesh"))
			{
				ReadMesh(geometry);
				return;
			}
			throw new NotSupportedException(string.Format("Geometry content of type {0} is not supported", xml.LocalName));
		}

		private void ReadMesh(Geometry geometry)
		{
			xml.ReadStartElement();
			while (xml.IsStartElement("source"))
			{
				ReadGeometrySource();
			}
			if (xml.IsStartElement("vertices"))
			{
				ReadMeshVertices(geometry);
			}
			while (xml.IsStartElement())
			{
				MeshPrimitives meshPrimitives = ReadMeshPrimitives(geometry);
				if (meshPrimitives == null)
				{
					break;
				}
				geometry.Primitives.Add(meshPrimitives);
			}
			ReadExtra();
			xml.ReadEndElement();
		}

		private Source ReadGeometrySource()
		{
			string attribute = xml.GetAttribute("id");
			string attribute2 = xml.GetAttribute("name");
			xml.ReadStartElement();
			ReadAsset();
			float[] data = ReadFloatArray();
			Source source = new Source(data, 1)
			{
				Name = attribute2
			};
			if (xml.IsStartElement("technique_common"))
			{
				xml.ReadStartElement();
				if (xml.IsStartElement("accessor"))
				{
					source.Stride = ReadIntAttribute("stride", 1);
					xml.ReadStartElement();
					while (xml.IsStartElement("param"))
					{
						ReadParam();
					}
					xml.ReadEndElement();
				}
				xml.ReadEndElement();
			}
			xml.SkipSequence("technique");
			xml.ReadEndElement();
			AddEntity(attribute, source);
			return source;
		}

		private string ReadParam()
		{
			string attribute = xml.GetAttribute("name");
			xml.Skip();
			return attribute;
		}

		private void ReadMeshVertices(Geometry mesh)
		{
			string attribute = xml.GetAttribute("id");
			xml.ReadStartElement();
			while (xml.IsStartElement("input"))
			{
				Semantic semantic = ReadSemanticAttribute();
				if (semantic != Semantic.None && semantic != Semantic.Vertex)
				{
					Input input = new Input();
					input.Semantic = semantic;
					BindUrlAttribute("source", delegate(Source s)
					{
						input.Source = s;
					});
					mesh.Vertices.Add(input);
				}
				xml.Skip();
			}
			ReadExtra();
			xml.ReadEndElement();
		}

		private MeshPrimitives ReadMeshPrimitives(Geometry mesh)
		{
			int num = ReadIntAttribute("count", 0);
			int num2 = 0;
			bool flag = false;
			MeshPrimitives primitives;
			switch (xml.LocalName)
			{
			case "lines":
				primitives = new MeshPrimitives(MeshPrimitiveType.Lines);
				num2 = 2;
				break;
			case "triangles":
				primitives = new MeshPrimitives(MeshPrimitiveType.Polygons);
				num2 = 3;
				break;
			case "linestrips":
				primitives = new MeshPrimitives(MeshPrimitiveType.LineStrips);
				flag = true;
				break;
			case "trifans":
				primitives = new MeshPrimitives(MeshPrimitiveType.TriangleFans);
				flag = true;
				break;
			case "tristrips":
				primitives = new MeshPrimitives(MeshPrimitiveType.TriangleStrips);
				flag = true;
				break;
			case "polygons":
				primitives = new MeshPrimitives(MeshPrimitiveType.Polygons);
				flag = true;
				break;
			case "polylist":
				primitives = new MeshPrimitives(MeshPrimitiveType.Polygons);
				break;
			default:
				return null;
			}
			primitives.MaterialSymbol = xml.GetAttribute("material");
			bool flag2 = false;
			xml.ReadStartElement();
			while (xml.IsStartElement("input"))
			{
				Semantic semantic = ReadSemanticAttribute();
				if (semantic != Semantic.None)
				{
					int offset = ReadIntAttribute("offset");
					string attribute = xml.GetAttribute("source");
					int set = ReadIntAttribute("set", -1);
					if (semantic == Semantic.Vertex)
					{
						if (flag2)
						{
							error.WriteLine("Duplicate vertex input found");
						}
						else
						{
							flag2 = true;
							foreach (Input vertex in mesh.Vertices)
							{
								primitives.Inputs.Add(new IndexedInput
								{
									Source = vertex.Source,
									Offset = offset,
									Set = set,
									Semantic = vertex.Semantic
								});
							}
						}
					}
					else
					{
						IndexedInput input = new IndexedInput
						{
							Offset = offset,
							Semantic = semantic,
							Set = set
						};
						BindUrl(attribute, delegate(Source s)
						{
							if (s.Count > 0)
							{
								input.Source = s;
								primitives.Inputs.Add(input);
							}
						});
					}
				}
				xml.Skip();
			}
			if (!flag2)
			{
				throw new InvalidDataException("no vertex input");
			}
			if (num > 0)
			{
				primitives.VertexCounts.Capacity = num;
			}
			int num3 = 0;
			while (xml.IsStartElement("vcount"))
			{
				if ((num2 != 0) | flag)
				{
					xml.Skip();
					continue;
				}
				foreach (string item in xml.ReadElementContentAsList())
				{
					int num4 = XmlConvert.ToInt32(item);
					num3 += num4;
					primitives.VertexCounts.Add(num4);
				}
			}
			if (num2 != 0)
			{
				for (int num5 = 0; num5 < num; num5++)
				{
					primitives.VertexCounts.Add(num2);
				}
				num3 = num2 * num;
			}
			else if (!flag && primitives.VertexCounts.Count == 0)
			{
				throw new InvalidDataException("no vcount");
			}
			int num6 = primitives.Inputs.Max((IndexedInput x) => x.Offset);
			List<int>[] array = new List<int>[num6 + 1];
			foreach (IndexedInput input2 in primitives.Inputs)
			{
				List<int> list = array[input2.Offset];
				if (list == null)
				{
					list = new List<int>(num3);
					array[input2.Offset] = list;
				}
			}
			if (!flag)
			{
				while (xml.IsStartElement("p"))
				{
					ReadInterleavedInputIndices(array);
				}
			}
			else
			{
				while (xml.IsStartElement())
				{
					if (xml.IsStartElement("p"))
					{
						primitives.VertexCounts.Add(ReadInterleavedInputIndices(array));
						continue;
					}
					if (!xml.IsStartElement("ph"))
					{
						break;
					}
					xml.ReadStartElement();
					while (xml.IsStartElement())
					{
						if (xml.LocalName == "p")
						{
							primitives.VertexCounts.Add(ReadInterleavedInputIndices(array));
						}
						else
						{
							xml.Skip();
						}
					}
					xml.ReadEndElement();
				}
			}
			foreach (IndexedInput input3 in primitives.Inputs)
			{
				input3.Indices.AddRange(array[input3.Offset]);
			}
			ReadExtra();
			xml.ReadEndElement();
			return primitives;
		}

		private int ReadInterleavedInputIndices(List<int>[] inputs)
		{
			int num = 0;
			int num2 = 0;
			foreach (string item in xml.ReadElementContentAsList())
			{
				List<int> list = inputs[num2++];
				if (list != null)
				{
					list.Add(XmlConvert.ToInt32(item));
				}
				if (num2 >= inputs.Length)
				{
					num2 = 0;
					num++;
				}
			}
			return num;
		}

		private void ReadScene(Scene scene)
		{
			while (xml.IsStartElement("node"))
			{
				ReadEntity(scene.Nodes, ReadNode);
			}
		}

		private void ReadNode(Node node)
		{
			ReadTransforms(node.Transforms);
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "node":
					ReadEntity(node.Nodes, ReadNode);
					break;
				case "instance_geometry":
					node.Instances.Add(ReadGeometryInstance());
					break;
				case "instance_light":
					node.Instances.Add(ReadLightInstance());
					break;
				case "instance_camera":
					node.Instances.Add(ReadCameraInstance());
					break;
				case "instance_node":
					node.Instances.Add(ReadNodeInstance());
					break;
				default:
					xml.Skip();
					break;
				}
			}
		}

		private void ReadSimpleInstance<T>(Instance<T> instance) where T : Entity
		{
			instance.Sid = xml.GetAttribute("sid");
			instance.Name = xml.GetAttribute("name");
			BindUrlAttribute("url", delegate(T camera)
			{
				instance.Target = camera;
			});
			xml.Skip();
		}

		private NodeInstance ReadNodeInstance()
		{
			NodeInstance nodeInstance = new NodeInstance();
			ReadSimpleInstance(nodeInstance);
			return nodeInstance;
		}

		private CameraInstance ReadCameraInstance()
		{
			CameraInstance cameraInstance = new CameraInstance();
			ReadSimpleInstance(cameraInstance);
			return cameraInstance;
		}

		private LightInstance ReadLightInstance()
		{
			LightInstance lightInstance = new LightInstance();
			ReadSimpleInstance(lightInstance);
			return lightInstance;
		}

		private void ReadTransforms(ICollection<Transform> transforms)
		{
			while (xml.IsStartElement())
			{
				Transform transform = null;
				switch (xml.LocalName)
				{
				default:
					return;
				case "matrix":
					transform = new TransformMatrix();
					break;
				case "rotate":
					transform = new TransformRotate();
					break;
				case "scale":
					transform = new TransformScale();
					break;
				case "translate":
					transform = new TransformTranslate();
					break;
				case "skew":
				case "lookat":
					xml.Skip();
					break;
				}
				if (transform != null)
				{
					transform.Sid = xml.GetAttribute("sid");
					xml.ReadElementContentAsArray(floatConverter, transform.Values);
					transforms.Add(transform);
				}
			}
		}

		private void ReadInstances(ICollection<Instance> instances)
		{
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				default:
					return;
				case "instance_geometry":
					instances.Add(ReadGeometryInstance());
					break;
				case "instance_camera":
				case "instance_controller":
				case "instance_light":
				case "instance_node":
					xml.Skip();
					break;
				}
			}
		}

		private GeometryInstance ReadGeometryInstance()
		{
			GeometryInstance instance = new GeometryInstance
			{
				Name = xml.GetAttribute("name"),
				Sid = xml.GetAttribute("sid")
			};
			string attribute = xml.GetAttribute("url");
			BindUrl(attribute, delegate(Geometry geometry)
			{
				instance.Target = geometry;
			});
			if (!xml.SkipEmpty())
			{
				xml.ReadStartElement();
				if (xml.IsStartElement("bind_material"))
				{
					ReadBindMaterial(instance, attribute);
				}
				ReadExtra();
				xml.ReadEndElement();
			}
			return instance;
		}

		private void ReadBindMaterial(GeometryInstance geometryInstance, string geometryUrl)
		{
			xml.ReadStartElement();
			while (xml.IsStartElement())
			{
				if (xml.LocalName != "technique_common")
				{
					xml.Skip();
					continue;
				}
				xml.ReadStartElement();
				while (xml.IsStartElement())
				{
					if (xml.LocalName == "instance_material")
					{
						ReadMaterialInstance(geometryInstance, geometryUrl);
					}
					else
					{
						xml.Skip();
					}
				}
				xml.ReadEndElement();
			}
			xml.ReadEndElement();
		}

		private void ReadMaterialInstance(GeometryInstance geometryInstance, string geometryUrl)
		{
			MaterialInstance instance = new MaterialInstance();
			instance.Symbol = xml.GetAttribute("symbol");
			BindUrlAttribute("target", delegate(Material material)
			{
				instance.Target = material;
			});
			geometryInstance.Materials.Add(instance);
			if (xml.SkipEmpty())
			{
				return;
			}
			xml.ReadStartElement();
			while (xml.IsStartElement())
			{
				if (xml.LocalName == "bind")
				{
					MaterialBinding binding = new MaterialBinding();
					binding.Semantic = xml.GetAttribute("semantic");
					string attribute = xml.GetAttribute("target");
					BindId(attribute, delegate(Source s)
					{
						BindUrl(geometryUrl, delegate(Geometry g)
						{
							MeshPrimitives meshPrimitives = g.Primitives.Find((MeshPrimitives p) => p.MaterialSymbol == instance.Symbol);
							if (meshPrimitives != null)
							{
								IndexedInput indexedInput = meshPrimitives.Inputs.Find((IndexedInput i) => i.Source == s);
								if (indexedInput != null)
								{
									binding.VertexInput = indexedInput;
									instance.Bindings.Add(binding);
								}
							}
						});
					});
				}
				else if (xml.LocalName == "bind_vertex_input")
				{
					MaterialBinding binding2 = new MaterialBinding();
					binding2.Semantic = xml.GetAttribute("semantic");
					Semantic inputSemantic = ReadSemanticAttribute("input_semantic");
					int inputSet = ReadIntAttribute("input_set", 0);
					BindUrl(geometryUrl, delegate(Geometry g)
					{
						MeshPrimitives meshPrimitives = g.Primitives.Find((MeshPrimitives p) => p.MaterialSymbol == instance.Symbol);
						if (meshPrimitives != null)
						{
							IndexedInput indexedInput = meshPrimitives.Inputs.Find((IndexedInput i) => i.Semantic == inputSemantic && i.Set == inputSet);
							if (indexedInput != null)
							{
								binding2.VertexInput = indexedInput;
								instance.Bindings.Add(binding2);
							}
						}
					});
				}
				xml.Skip();
			}
			xml.ReadEndElement();
		}

		private void ReadAnimation(Animation animation)
		{
			while (xml.IsStartElement("animation"))
			{
				ReadEntity(animation.Animations, ReadAnimation);
			}
			while (xml.IsStartElement("source"))
			{
				ReadAnimationSource();
			}
			while (xml.IsStartElement("sampler"))
			{
				animation.Samplers.Add(ReadAnimationSampler());
			}
			while (xml.IsStartElement("channel"))
			{
				BindAnimationSampler(xml.GetAttribute("source"), xml.GetAttribute("target"));
				xml.Skip();
			}
		}

		private Source ReadAnimationSource()
		{
			string attribute = xml.GetAttribute("id");
			string text = xml.GetAttribute("name");
			if (string.IsNullOrEmpty(text))
			{
				text = attribute;
			}
			xml.ReadStartElement();
			ReadAsset();
			Source source;
			if (xml.IsStartElement("float_array"))
			{
				source = new Source(ReadFloatArray(), 1);
			}
			else
			{
				if (!xml.IsStartElement("Name_array"))
				{
					throw new NotSupportedException(string.Format("Animation sources of type {0} are not supported", xml.LocalName));
				}
				source = new Source(ReadNameArray(), 1);
			}
			source.Name = text;
			if (xml.IsStartElement("technique_common"))
			{
				xml.ReadStartElement();
				if (xml.IsStartElement("accessor"))
				{
					source.Stride = ReadIntAttribute("stride", 1);
					xml.ReadStartElement();
					while (xml.IsStartElement("param"))
					{
						ReadParam();
					}
					xml.ReadEndElement();
				}
				xml.ReadEndElement();
			}
			xml.SkipSequence("technique");
			xml.ReadEndElement();
			AddEntity(attribute, source);
			return source;
		}

		private Input ReadAnimationInput()
		{
			Input input = new Input();
			input.Semantic = ReadSemanticAttribute();
			BindUrlAttribute("source", delegate(Source source)
			{
				input.Source = source;
			});
			xml.Skip();
			return input;
		}

		private Sampler ReadAnimationSampler()
		{
			string attribute = xml.GetAttribute("id");
			xml.ReadStartElement();
			Sampler sampler = new Sampler();
			while (xml.IsStartElement())
			{
				string localName = xml.LocalName;
				if (localName != null && localName == "input")
				{
					sampler.Inputs.Add(ReadAnimationInput());
				}
				else
				{
					xml.Skip();
				}
			}
			xml.ReadEndElement();
			AddEntity(attribute, sampler);
			return sampler;
		}

		private void ReadLight(Light light)
		{
			if (!xml.IsStartElement("technique_common"))
			{
				xml.Skip();
				return;
			}
			xml.ReadStartElement();
			if (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "ambient":
					light.Type = LightType.Ambient;
					break;
				case "directional":
					light.Type = LightType.Directional;
					break;
				case "point":
					light.Type = LightType.Point;
					break;
				case "spot":
					light.Type = LightType.Spot;
					break;
				}
				xml.ReadStartElement();
				light.Color = xml.ReadElementContentAsVector3("color");
				if (light.Type == LightType.Point || light.Type == LightType.Spot)
				{
					if (xml.LocalName == "constant_attenuation")
					{
						light.ConstantAttenuation = xml.ReadElementContentAsFloat();
					}
					if (xml.LocalName == "linear_attenuation")
					{
						light.LinearAttenuation = xml.ReadElementContentAsFloat();
					}
					if (light.Type == LightType.Point)
					{
						light.QuadraticAttenuation = xml.ReadElementContentAsFloat("quadratic_attenuation", string.Empty);
						if (xml.LocalName == "zfar")
						{
							light.ZFar = xml.ReadElementContentAsFloat();
						}
					}
					else if (light.Type == LightType.Spot)
					{
						if (xml.LocalName == "quadratic_attenuation")
						{
							light.QuadraticAttenuation = xml.ReadElementContentAsFloat();
						}
						if (xml.LocalName == "falloff_angle")
						{
							light.FalloffAngle = xml.ReadElementContentAsFloat();
						}
						if (xml.LocalName == "falloff_exponent")
						{
							light.FalloffExponent = xml.ReadElementContentAsFloat();
						}
					}
				}
				xml.ReadEndElement();
			}
			xml.ReadEndElement();
		}

		private void ReadScene()
		{
			if (!xml.IsStartElement("scene"))
			{
				return;
			}
			xml.ReadStartElement();
			xml.SkipSequence("instance_physics_scene");
			if (xml.IsStartElement("instance_visual_scene"))
			{
				BindUrlAttribute("url", delegate(Scene scene)
				{
					mainScene = scene;
				});
				xml.Skip();
			}
			ReadExtra();
			xml.ReadEndElement();
		}

		private void ReadAsset()
		{
			if (!xml.IsStartElement("asset"))
			{
				return;
			}
			xml.ReadStartElement();
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "up_axis":
					upAxis = ReadUpAxis();
					break;
				case "contributor":
					ReadAssetContributor();
					break;
				case "unit":
					unit = XmlConvert.ToSingle(xml.GetAttribute("meter"));
					xml.Skip();
					break;
				default:
					xml.Skip();
					break;
				}
			}
			xml.ReadEndElement();
		}

		private void ReadAssetContributor()
		{
			xml.ReadStartElement();
			while (xml.IsStartElement())
			{
				string localName = xml.LocalName;
				xml.Skip();
			}
			xml.ReadEndElement();
		}

		private void ReadExtra()
		{
			while (xml.IsStartElement("extra"))
			{
				xml.Skip();
			}
		}

		private float[] ReadFloatArray()
		{
			if (!xml.IsStartElement("float_array"))
			{
				return null;
			}
			string attribute = xml.GetAttribute("id");
			int num = ReadIntAttribute("count");
			float[] array = new float[num];
			int num2 = 0;
			foreach (string item in xml.ReadElementContentAsList())
			{
				if (num2 < array.Length)
				{
					array[num2++] = XmlConvert.ToSingle(item);
				}
			}
			return array;
		}

		private string[] ReadNameArray()
		{
			if (!xml.IsStartElement("Name_array"))
			{
				return null;
			}
			string attribute = xml.GetAttribute("id");
			int num = ReadIntAttribute("count");
			string[] array = new string[num];
			int num2 = 0;
			foreach (string item in xml.ReadElementContentAsList())
			{
				if (num2 < array.Length)
				{
					array[num2++] = item;
				}
			}
			return array;
		}

		private int ReadIntAttribute(string name)
		{
			string attribute = xml.GetAttribute(name);
			if (string.IsNullOrEmpty(attribute))
			{
				throw new InvalidDataException(name + " attribute not found");
			}
			return XmlConvert.ToInt32(attribute);
		}

		private int ReadIntAttribute(string name, int defaultValue)
		{
			string attribute = xml.GetAttribute(name);
			if (string.IsNullOrEmpty(attribute))
			{
				return defaultValue;
			}
			return XmlConvert.ToInt32(attribute);
		}

		private int? ReadNullableIntAttribute(string name)
		{
			string attribute = xml.GetAttribute(name);
			if (string.IsNullOrEmpty(attribute))
			{
				return null;
			}
			return XmlConvert.ToInt32(attribute);
		}

		private Semantic ReadSemanticAttribute()
		{
			return ReadSemanticAttribute("semantic");
		}

		private Semantic ReadSemanticAttribute(string name)
		{
			switch (xml.GetAttribute(name))
			{
			case "POSITION":
				return Semantic.Position;
			case "TEXCOORD":
				return Semantic.TexCoord;
			case "NORMAL":
				return Semantic.Normal;
			case "COLOR":
				return Semantic.Color;
			case "VERTEX":
				return Semantic.Vertex;
			case "INPUT":
				return Semantic.Input;
			case "IN_TANGENT":
				return Semantic.InTangent;
			case "OUT_TANGENT":
				return Semantic.OutTangent;
			case "INTERPOLATION":
				return Semantic.Interpolation;
			case "OUTPUT":
				return Semantic.Output;
			default:
				return Semantic.None;
			}
		}

		private string[] ReadStringListAttribute(string name)
		{
			string attribute = xml.GetAttribute(name);
			if (string.IsNullOrEmpty(attribute))
			{
				return emptyStrings;
			}
			return attribute.Split(whiteSpaceChars, StringSplitOptions.RemoveEmptyEntries);
		}

		private Axis ReadUpAxis()
		{
			switch (xml.ReadElementContentAsString())
			{
			case "X_UP":
				return Axis.X;
			case "Z_UP":
				return Axis.Z;
			default:
				return Axis.Y;
			}
		}

		private void AddEntity(string id, Entity entity)
		{
			if (!string.IsNullOrEmpty(id))
			{
				if (entities == null)
				{
					entities = new Dictionary<string, Entity>();
				}
				if (entities.ContainsKey(id))
				{
					error.WriteLine(string.Format("COLLADA error: duplicate id {0}", id));
				}
				else
				{
					entities.Add(id, entity);
				}
				entity.Id = id;
			}
		}

		private T GetEntity<T>(string id) where T : Entity
		{
			Entity value;
			if (entities == null || string.IsNullOrEmpty(id) || !entities.TryGetValue(id, out value))
			{
				return null;
			}
			return value as T;
		}

		private void BindUrlAttribute<T>(string name, Action<T> action) where T : Entity
		{
			BindUrl(xml.GetAttribute(name), action);
		}

		private void BindUrl<T>(string url, Action<T> action) where T : Entity
		{
			if (!string.IsNullOrEmpty(url))
			{
				if (url[0] != '#')
				{
					throw new NotSupportedException(string.Format("External reference '{0}' is not supported", url));
				}
				BindId(url.Substring(1), action);
			}
		}

		private void BindId<T>(string id, Action<T> action) where T : Entity
		{
			if (string.IsNullOrEmpty(id))
			{
				return;
			}
			T entity = GetEntity<T>(id);
			if (entity != null)
			{
				action(entity);
				return;
			}
			delayedBindActions.Add(delegate
			{
				T entity2 = GetEntity<T>(id);
				if (entity2 != null)
				{
					action(entity2);
				}
			});
		}

		private void BindAnimationSampler(string sourceId, string targetPath)
		{
			TargetPath path = TargetPath.Parse(targetPath);
			BindUrlAttribute("source", delegate(Sampler sampler)
			{
				Input input = sampler.Inputs.Find((Input i) => i.Semantic == Semantic.Output);
				if (input != null && input.Source != null)
				{
					int stride = input.Source.Stride;
					if (stride != 1)
					{
						for (int offset = 0; offset < stride; offset++)
						{
							Sampler newSampler = sampler.Split(offset);
							BindId(path.NodeId, delegate(Node node)
							{
								Transform transform = FindTransform(node, path.Path[0]);
								if (transform != null)
								{
									transform.BindAnimation(string.Format(CultureInfo.InvariantCulture, "({0})", new object[1] { offset }), newSampler);
								}
							});
						}
					}
					else
					{
						BindId(path.NodeId, delegate(Node node)
						{
							Transform transform = FindTransform(node, path.Path[0]);
							if (transform != null)
							{
								transform.BindAnimation(path.Value, sampler);
							}
						});
					}
				}
			});
		}

		private Transform FindTransform(Node node, string sid)
		{
			return node.Transforms.Find((Transform t) => t.Sid == sid);
		}

		private void BindNodes(Node node)
		{
			foreach (NodeInstance item in node.Instances.OfType<NodeInstance>().ToList())
			{
				node.Instances.Remove(item);
				if (item.Target != node)
				{
					node.Nodes.Add(item.Target);
				}
			}
			foreach (Node node2 in node.Nodes)
			{
				BindNodes(node2);
			}
		}
	}
}
