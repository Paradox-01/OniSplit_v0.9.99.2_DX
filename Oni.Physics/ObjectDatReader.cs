using Oni.Akira;
using Oni.Motoko;
using Oni.Objects;

namespace Oni.Physics
{
	internal class ObjectDatReader
	{
		public static ObjectNode ReadObjectGeometry(InstanceDescriptor ofga)
		{
			ObjectGeometry[] geometries = null;
			ObjectParticle[] particles = new ObjectParticle[0];
			if (ofga.Template.Tag == TemplateTag.OFGA)
			{
				using (BinaryReader binaryReader = ofga.OpenRead(16))
				{
					InstanceDescriptor instanceDescriptor = binaryReader.ReadInstance();
					geometries = ReadGeometries(binaryReader);
					if (instanceDescriptor != null)
					{
						particles = ReadParticles(instanceDescriptor);
					}
				}
			}
			else if (ofga.Template.Tag == TemplateTag.M3GM)
			{
				geometries = new ObjectGeometry[1]
				{
					new ObjectGeometry
					{
						Flags = GunkFlags.NoOcclusion,
						Geometry = GeometryDatReader.Read(ofga)
					}
				};
			}
			return new ObjectNode(geometries, particles);
		}

		private static ObjectGeometry[] ReadGeometries(BinaryReader reader)
		{
			uint num = reader.ReadUInt32();
			ObjectGeometry[] array = new ObjectGeometry[num];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ReadGeometry(reader);
			}
			return array;
		}

		private static ObjectGeometry ReadGeometry(BinaryReader reader)
		{
			ObjectGeometry result = new ObjectGeometry
			{
				Flags = (GunkFlags)reader.ReadInt32(),
				Geometry = GeometryDatReader.Read(reader.ReadInstance())
			};
			reader.Skip(4);
			return result;
		}

		private static ObjectParticle[] ReadParticles(InstanceDescriptor particlesDescriptor)
		{
			using (BinaryReader binaryReader = particlesDescriptor.OpenRead(22))
			{
				ObjectParticle[] array = new ObjectParticle[binaryReader.ReadUInt16()];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = ReadParticle(binaryReader);
				}
				return array;
			}
		}

		private static ObjectParticle ReadParticle(BinaryReader reader)
		{
			ObjectParticle result = new ObjectParticle
			{
				ParticleClass = reader.ReadString(64),
				Tag = reader.ReadString(48),
				Matrix = reader.ReadMatrix4x3(),
				DecalScale = reader.ReadVector2(),
				Flags = (ParticleFlags)reader.ReadUInt16()
			};
			reader.Skip(38);
			return result;
		}

		public static ObjectAnimation ReadAnimation(InstanceDescriptor oban)
		{
			ObjectAnimation objectAnimation = new ObjectAnimation
			{
				Name = oban.Name
			};
			using (BinaryReader binaryReader = oban.OpenRead(12))
			{
				objectAnimation.Flags = (ObjectAnimationFlags)binaryReader.ReadInt32();
				binaryReader.Skip(48);
				Vector3 scale = binaryReader.ReadMatrix4x3().Scale;
				binaryReader.Skip(2);
				objectAnimation.Length = binaryReader.ReadUInt16();
				objectAnimation.Stop = binaryReader.ReadInt16();
				objectAnimation.Keys = new ObjectAnimationKey[binaryReader.ReadUInt16()];
				for (int i = 0; i < objectAnimation.Keys.Length; i++)
				{
					objectAnimation.Keys[i] = new ObjectAnimationKey
					{
						Scale = scale,
						Rotation = binaryReader.ReadQuaternion(),
						Translation = binaryReader.ReadVector3(),
						Time = binaryReader.ReadInt32()
					};
				}
				return objectAnimation;
			}
		}
	}
}
