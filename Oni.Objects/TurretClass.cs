using Oni.Akira;
using Oni.Motoko;
using Oni.Physics;

namespace Oni.Objects
{
	internal class TurretClass : GunkObjectClass
	{
		public string BaseName;

		public int Flags;

		public int FreeTime;

		public int ReloadTime;

		public int BarrelCount;

		public int RecoilAnimType;

		public int ReloadAnimType;

		public int MaxAmmo;

		public int AttachmentCount;

		public int ShooterCount;

		public float AimingSpeed;

		public Geometry BaseGeometry;

		public GunkFlags BaseGunkFlags;

		public Geometry TurretGeometry;

		public GunkFlags TurretGunkFlags;

		public Geometry BarrelGeometry;

		public GunkFlags BarrelGunkFlags;

		public Vector3 TurretPosition;

		public Vector3 BarrelPosition;

		public override ObjectGeometry[] GunkNodes
		{
			get
			{
				return new ObjectGeometry[1]
				{
					new ObjectGeometry
					{
						Geometry = BaseGeometry,
						Flags = BaseGunkFlags
					}
				};
			}
		}

		public static TurretClass Read(InstanceDescriptor turr)
		{
			TurretClass turretClass = new TurretClass();
			InstanceDescriptor instanceDescriptor;
			InstanceDescriptor instanceDescriptor2;
			InstanceDescriptor instanceDescriptor3;
			using (BinaryReader binaryReader = turr.OpenRead())
			{
				turretClass.Name = binaryReader.ReadString(32);
				turretClass.BaseName = binaryReader.ReadString(32);
				turretClass.Flags = binaryReader.ReadUInt16();
				turretClass.FreeTime = binaryReader.ReadUInt16();
				turretClass.ReloadTime = binaryReader.ReadUInt16();
				turretClass.BarrelCount = binaryReader.ReadUInt16();
				turretClass.RecoilAnimType = binaryReader.ReadUInt16();
				turretClass.ReloadAnimType = binaryReader.ReadUInt16();
				turretClass.MaxAmmo = binaryReader.ReadUInt16();
				turretClass.AttachmentCount = binaryReader.ReadUInt16();
				turretClass.ShooterCount = binaryReader.ReadUInt16();
				binaryReader.Skip(2);
				turretClass.AimingSpeed = binaryReader.ReadSingle();
				instanceDescriptor = binaryReader.ReadInstance();
				binaryReader.Skip(4);
				turretClass.BaseGunkFlags = (GunkFlags)binaryReader.ReadInt32();
				instanceDescriptor2 = binaryReader.ReadInstance();
				turretClass.TurretGunkFlags = (GunkFlags)binaryReader.ReadInt32();
				instanceDescriptor3 = binaryReader.ReadInstance();
				turretClass.BarrelGunkFlags = (GunkFlags)binaryReader.ReadInt32();
				turretClass.TurretPosition = binaryReader.ReadVector3();
				turretClass.BarrelPosition = binaryReader.ReadVector3();
			}
			if (instanceDescriptor != null)
			{
				turretClass.BaseGeometry = GeometryDatReader.Read(instanceDescriptor);
			}
			if (instanceDescriptor3 != null)
			{
				turretClass.BarrelGeometry = GeometryDatReader.Read(instanceDescriptor3);
			}
			if (instanceDescriptor2 != null)
			{
				turretClass.TurretGeometry = GeometryDatReader.Read(instanceDescriptor2);
			}
			return turretClass;
		}
	}
}
