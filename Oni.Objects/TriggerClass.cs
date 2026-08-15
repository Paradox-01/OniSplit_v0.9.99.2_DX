using Oni.Akira;
using Oni.Imaging;
using Oni.Motoko;
using Oni.Physics;

namespace Oni.Objects
{
	internal class TriggerClass : GunkObjectClass
	{
		public Color Color;

		public int TimeOn;

		public int TimeOff;

		public float StartOffset;

		public float AnimScale;

		public Geometry RailGeometry;

		public GunkFlags RailGunkFlags;

		public string ActiveSoundName;

		public string HitSoundName;

		public override ObjectGeometry[] GunkNodes
		{
			get
			{
				return new ObjectGeometry[1]
				{
					new ObjectGeometry
					{
						Geometry = RailGeometry,
						Flags = RailGunkFlags
					}
				};
			}
		}

		public static TriggerClass Read(InstanceDescriptor trig)
		{
			TriggerClass triggerClass = new TriggerClass();
			InstanceDescriptor instanceDescriptor;
			using (BinaryReader binaryReader = trig.OpenRead())
			{
				triggerClass.Color = binaryReader.ReadColor();
				triggerClass.TimeOn = binaryReader.ReadUInt16();
				triggerClass.TimeOff = binaryReader.ReadUInt16();
				triggerClass.StartOffset = binaryReader.ReadSingle();
				triggerClass.AnimScale = binaryReader.ReadSingle();
				instanceDescriptor = binaryReader.ReadInstance();
				binaryReader.Skip(4);
				triggerClass.RailGunkFlags = (GunkFlags)binaryReader.ReadInt32();
				binaryReader.Skip(8);
				triggerClass.ActiveSoundName = binaryReader.ReadString(32) + ".amb";
				triggerClass.HitSoundName = binaryReader.ReadString(32) + ".imp";
				binaryReader.Skip(8);
			}
			if (instanceDescriptor != null)
			{
				triggerClass.RailGeometry = GeometryDatReader.Read(instanceDescriptor);
			}
			return triggerClass;
		}
	}
}
