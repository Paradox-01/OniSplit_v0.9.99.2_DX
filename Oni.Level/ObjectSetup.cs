using System.Collections.Generic;
using Oni.Physics;

namespace Oni.Level
{
	internal class ObjectSetup
	{
		public object[] Geometries;

		public ObjectAnimation Animation;

		public readonly List<ObjectParticle> Particles = new List<ObjectParticle>();

		public ObjectSetupFlags Flags;

		public int DoorScriptId;

		public ObjectPhysicsType PhysicsType;

		public int ScriptId = 65535;

		public Vector3 Position;

		public Quaternion Orientation = Quaternion.Identity;

		public float Scale = 1f;

		public Matrix Origin;

		public string Name;

		public string FileName;
	}
}
