using Oni.Physics;

namespace Oni.Objects
{
	internal class DoorClass : GunkObjectClass
	{
		public ObjectNode Geometry;

		public string AnimationName;

		public ObjectAnimation Animation;

		public float SoundAttenuation;

		public int AllowedSounds;

		public int SoundType;

		public float SoundVolume;

		public string OpenSound;

		public string CloseSound;

		public override ObjectGeometry[] GunkNodes
		{
			get
			{
				return Geometry.Geometries;
			}
		}

		public static DoorClass Read(InstanceDescriptor door)
		{
			DoorClass doorClass = new DoorClass();
			InstanceDescriptor instanceDescriptor;
			InstanceDescriptor instanceDescriptor2;
			using (BinaryReader binaryReader = door.OpenRead())
			{
				instanceDescriptor = binaryReader.ReadInstance();
				binaryReader.Skip(4);
				instanceDescriptor2 = binaryReader.ReadInstance();
				doorClass.SoundAttenuation = binaryReader.ReadSingle();
				doorClass.AllowedSounds = binaryReader.ReadInt32();
				doorClass.SoundType = binaryReader.ReadInt32();
				doorClass.SoundVolume = binaryReader.ReadSingle();
				doorClass.OpenSound = binaryReader.ReadString(32);
				doorClass.CloseSound = binaryReader.ReadString(32);
			}
			if (instanceDescriptor != null)
			{
				doorClass.Geometry = ObjectDatReader.ReadObjectGeometry(instanceDescriptor);
			}
			if (instanceDescriptor2 != null)
			{
				doorClass.AnimationName = instanceDescriptor2.Name;
				doorClass.Animation = ObjectDatReader.ReadAnimation(instanceDescriptor2);
			}
			return doorClass;
		}
	}
}
