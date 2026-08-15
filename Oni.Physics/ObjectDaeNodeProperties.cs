using System.Collections.Generic;
using Oni.Akira;

namespace Oni.Physics
{
	internal class ObjectDaeNodeProperties : AkiraDaeNodeProperties
	{
		public ObjectSetupFlags ObjectFlags;

		public ObjectPhysicsType PhysicsType;

		public readonly List<ObjectAnimationClip> Animations = new List<ObjectAnimationClip>();

		public readonly List<ObjectParticle> Particles = new List<ObjectParticle>();
	}
}
