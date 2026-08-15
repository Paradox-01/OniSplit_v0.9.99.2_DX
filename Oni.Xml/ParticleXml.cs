using Oni.Particles;

namespace Oni.Xml
{
	internal class ParticleXml
	{
		protected class EventActionParameterInfo
		{
			public static readonly EventActionParameterInfo[] EmptyActionParameterInfos = new EventActionParameterInfo[0];

			public string Name;

			public StorageType Type;

			public EventActionParameterInfo(string name, StorageType type)
			{
				Name = name;
				Type = type;
			}
		}

		protected class EventActionInfo
		{
			public int OutCount;

			public EventActionParameterInfo[] Parameters;

			public EventActionInfo(int outCount, params EventActionParameterInfo[] parmeters)
			{
				OutCount = outCount;
				Parameters = parmeters;
			}
		}

		protected static readonly ParticleFlags1[] optionFlags1 = new ParticleFlags1[3]
		{
			ParticleFlags1.Decorative,
			ParticleFlags1.CollideWithWalls,
			ParticleFlags1.CollideWithChars
		};

		protected static readonly ParticleFlags2[] optionFlags2 = new ParticleFlags2[6]
		{
			ParticleFlags2.InitiallyHidden,
			ParticleFlags2.DrawAsSky,
			ParticleFlags2.DontAttractThroughWalls,
			ParticleFlags2.ExpireOnCutscene,
			ParticleFlags2.DieOnCutscene,
			ParticleFlags2.LockPositionToLink
		};

		protected static readonly ParticleFlags1[] appearanceFlags1 = new ParticleFlags1[0];

		protected static readonly ParticleFlags1[] appearanceExFlags1 = new ParticleFlags1[2]
		{
			ParticleFlags1.ScaleToVelocity,
			ParticleFlags1.UseSeparateYScale
		};

		protected static readonly ParticleFlags2[] appearanceFlags2 = new ParticleFlags2[2]
		{
			ParticleFlags2.Invisible,
			ParticleFlags2.IsContrailEmitter
		};

		protected static readonly ParticleFlags2[] appearanceExFlags2 = new ParticleFlags2[6]
		{
			ParticleFlags2.Invisible,
			ParticleFlags2.UseSpecialTint,
			ParticleFlags2.FadeOutOnEdge,
			ParticleFlags2.OneSidedEdgeFade,
			ParticleFlags2.LensFlare,
			ParticleFlags2.DecalFullBrightness
		};

		protected static readonly EventActionInfo[] eventActionInfoTable = new EventActionInfo[90]
		{
			new EventActionInfo(1, new EventActionParameterInfo("Target", StorageType.Float), new EventActionParameterInfo("Rate", StorageType.Float)),
			new EventActionInfo(2, new EventActionParameterInfo("Target", StorageType.Float), new EventActionParameterInfo("Velocity", StorageType.Float), new EventActionParameterInfo("Acceleration", StorageType.Float)),
			new EventActionInfo(1, new EventActionParameterInfo("Target", StorageType.Float), new EventActionParameterInfo("Min", StorageType.Float), new EventActionParameterInfo("Max", StorageType.Float), new EventActionParameterInfo("Rate", StorageType.Float)),
			new EventActionInfo(2, new EventActionParameterInfo("Target", StorageType.Float), new EventActionParameterInfo("State", StorageType.PingPongState), new EventActionParameterInfo("Min", StorageType.Float), new EventActionParameterInfo("Max", StorageType.Float), new EventActionParameterInfo("Rate", StorageType.Float)),
			new EventActionInfo(1, new EventActionParameterInfo("Target", StorageType.Float), new EventActionParameterInfo("Min", StorageType.Float), new EventActionParameterInfo("Max", StorageType.Float), new EventActionParameterInfo("Rate", StorageType.Float)),
			new EventActionInfo(1, new EventActionParameterInfo("Target", StorageType.Float), new EventActionParameterInfo("Rate", StorageType.Float), new EventActionParameterInfo("Value", StorageType.Float)),
			new EventActionInfo(1, new EventActionParameterInfo("Target", StorageType.Color), new EventActionParameterInfo("Color0", StorageType.Color), new EventActionParameterInfo("Color1", StorageType.Color), new EventActionParameterInfo("Amount", StorageType.Float)),
			null,
			new EventActionInfo(0, new EventActionParameterInfo("TimeToDie", StorageType.Float)),
			new EventActionInfo(0, new EventActionParameterInfo("Action", StorageType.ActionIndex), new EventActionParameterInfo("Lifetime", StorageType.Float)),
			new EventActionInfo(0, new EventActionParameterInfo("Action", StorageType.ActionIndex), new EventActionParameterInfo("Lifetime", StorageType.Float)),
			new EventActionInfo(0),
			new EventActionInfo(0, new EventActionParameterInfo("Time", StorageType.Float)),
			new EventActionInfo(0, new EventActionParameterInfo("Emitter", StorageType.Emitter)),
			new EventActionInfo(0, new EventActionParameterInfo("Emitter", StorageType.Emitter)),
			new EventActionInfo(0, new EventActionParameterInfo("Emitter", StorageType.Emitter), new EventActionParameterInfo("Particles", StorageType.Float)),
			new EventActionInfo(0, new EventActionParameterInfo("Emitter", StorageType.Emitter)),
			new EventActionInfo(0, new EventActionParameterInfo("Emitter", StorageType.Emitter)),
			new EventActionInfo(0, new EventActionParameterInfo("Emitter", StorageType.Emitter)),
			null,
			new EventActionInfo(0, new EventActionParameterInfo("Sound", StorageType.AmbientSoundName)),
			new EventActionInfo(0),
			new EventActionInfo(0),
			new EventActionInfo(0, new EventActionParameterInfo("Sound", StorageType.ImpulseSoundName)),
			null,
			null,
			new EventActionInfo(0, new EventActionParameterInfo("Damage", StorageType.Float), new EventActionParameterInfo("StunDamage", StorageType.Float), new EventActionParameterInfo("KnockBack", StorageType.Float), new EventActionParameterInfo("DamageType", StorageType.DamageType), new EventActionParameterInfo("SelfImmune", StorageType.Boolean), new EventActionParameterInfo("CanHitMultiple", StorageType.Boolean)),
			new EventActionInfo(0, new EventActionParameterInfo("Damage", StorageType.Float), new EventActionParameterInfo("StunDamage", StorageType.Float), new EventActionParameterInfo("KnockBack", StorageType.Float), new EventActionParameterInfo("Radius", StorageType.Float), new EventActionParameterInfo("FallOff", StorageType.BlastFalloff), new EventActionParameterInfo("DamageType", StorageType.DamageType), new EventActionParameterInfo("SelfImmune", StorageType.Boolean), new EventActionParameterInfo("DamageEnvironment", StorageType.Boolean)),
			new EventActionInfo(0),
			new EventActionInfo(0, new EventActionParameterInfo("Damage", StorageType.Float)),
			new EventActionInfo(0, new EventActionParameterInfo("BlastVelocity", StorageType.Float), new EventActionParameterInfo("Radius", StorageType.Float)),
			new EventActionInfo(0),
			null,
			new EventActionInfo(0, new EventActionParameterInfo("Space", StorageType.CoordFrame), new EventActionParameterInfo("Rate", StorageType.Float), new EventActionParameterInfo("RotateVelocity", StorageType.Boolean)),
			new EventActionInfo(0, new EventActionParameterInfo("Space", StorageType.CoordFrame), new EventActionParameterInfo("Rate", StorageType.Float), new EventActionParameterInfo("RotateVelocity", StorageType.Boolean)),
			new EventActionInfo(0, new EventActionParameterInfo("Space", StorageType.CoordFrame), new EventActionParameterInfo("Rate", StorageType.Float), new EventActionParameterInfo("RotateVelocity", StorageType.Boolean)),
			null,
			new EventActionInfo(0, new EventActionParameterInfo("DelayTime", StorageType.Float)),
			new EventActionInfo(0, new EventActionParameterInfo("Gravity", StorageType.Float), new EventActionParameterInfo("MaxG", StorageType.Float), new EventActionParameterInfo("HorizontalOnly", StorageType.Boolean)),
			new EventActionInfo(0, new EventActionParameterInfo("TurnSpeed", StorageType.Float), new EventActionParameterInfo("PredictPosition", StorageType.Boolean), new EventActionParameterInfo("HorizontalOnly", StorageType.Boolean)),
			new EventActionInfo(0, new EventActionParameterInfo("AccelRate", StorageType.Float), new EventActionParameterInfo("MaxAccel", StorageType.Float), new EventActionParameterInfo("DesiredDistance", StorageType.Float)),
			null,
			null,
			null,
			null,
			null,
			null,
			new EventActionInfo(0),
			new EventActionInfo(0, new EventActionParameterInfo("Fraction", StorageType.Float)),
			new EventActionInfo(1, new EventActionParameterInfo("Theta", StorageType.Float), new EventActionParameterInfo("Radius", StorageType.Float), new EventActionParameterInfo("RotateSpeed", StorageType.Float)),
			new EventActionInfo(0, new EventActionParameterInfo("Resistance", StorageType.Float), new EventActionParameterInfo("MinimumVelocity", StorageType.Float)),
			new EventActionInfo(0, new EventActionParameterInfo("Acceleration", StorageType.Float), new EventActionParameterInfo("MaxSpeed", StorageType.Float), new EventActionParameterInfo("SidewaysDecay", StorageType.Float), new EventActionParameterInfo("DirX", StorageType.Float), new EventActionParameterInfo("DirY", StorageType.Float), new EventActionParameterInfo("DirZ", StorageType.Float), new EventActionParameterInfo("Space", StorageType.CoordFrame)),
			new EventActionInfo(0, new EventActionParameterInfo("Speed", StorageType.Float), new EventActionParameterInfo("Space", StorageType.CoordFrame), new EventActionParameterInfo("NoSideways", StorageType.Boolean)),
			new EventActionInfo(0, new EventActionParameterInfo("Theta", StorageType.Float), new EventActionParameterInfo("Radius", StorageType.Float), new EventActionParameterInfo("Rotate_speed", StorageType.Float)),
			new EventActionInfo(0, new EventActionParameterInfo("Direction", StorageType.Direction), new EventActionParameterInfo("Value", StorageType.Float)),
			new EventActionInfo(0, new EventActionParameterInfo("Effect", StorageType.ImpactName), new EventActionParameterInfo("WallOffset", StorageType.Float), new EventActionParameterInfo("Orientation", StorageType.CollisionOrient), new EventActionParameterInfo("Attach", StorageType.Boolean)),
			new EventActionInfo(0),
			new EventActionInfo(0, new EventActionParameterInfo("ElasticDirect", StorageType.Float), new EventActionParameterInfo("ElasticGlancing", StorageType.Float)),
			new EventActionInfo(0),
			new EventActionInfo(0),
			new EventActionInfo(0, new EventActionParameterInfo("ImpactType", StorageType.ImpactName), new EventActionParameterInfo("ImpactModifier", StorageType.ImpactModifier)),
			null,
			new EventActionInfo(0),
			new EventActionInfo(0),
			new EventActionInfo(0, new EventActionParameterInfo("Tick", StorageType.Float)),
			new EventActionInfo(0),
			null,
			null,
			null,
			null,
			new EventActionInfo(1, new EventActionParameterInfo("Target", StorageType.Float), new EventActionParameterInfo("Value", StorageType.Float)),
			new EventActionInfo(0),
			new EventActionInfo(0, new EventActionParameterInfo("Action", StorageType.ActionIndex), new EventActionParameterInfo("Var", StorageType.Float), new EventActionParameterInfo("Threshold", StorageType.Float)),
			new EventActionInfo(0, new EventActionParameterInfo("Action", StorageType.ActionIndex), new EventActionParameterInfo("Var", StorageType.Float), new EventActionParameterInfo("Threshold", StorageType.Float)),
			new EventActionInfo(0, new EventActionParameterInfo("Action", StorageType.ActionIndex)),
			new EventActionInfo(0, new EventActionParameterInfo("Action", StorageType.ActionIndex)),
			null,
			new EventActionInfo(0, new EventActionParameterInfo("Emitter", StorageType.Emitter), new EventActionParameterInfo("FuseTime", StorageType.Float)),
			new EventActionInfo(0),
			new EventActionInfo(5, new EventActionParameterInfo("AxisX", StorageType.Float), new EventActionParameterInfo("AxisY", StorageType.Float), new EventActionParameterInfo("AxisZ", StorageType.Float), new EventActionParameterInfo("CurrentAngle", StorageType.Float), new EventActionParameterInfo("TimeUntilCheck", StorageType.Float), new EventActionParameterInfo("SenseDistance", StorageType.Float), new EventActionParameterInfo("TurningSpeed", StorageType.Float), new EventActionParameterInfo("TurningDecay", StorageType.Float)),
			new EventActionInfo(1, new EventActionParameterInfo("SwirlAngle", StorageType.Float), new EventActionParameterInfo("SwirlBaseRate", StorageType.Float), new EventActionParameterInfo("SwirlDeltaRate", StorageType.Float), new EventActionParameterInfo("SwirlSpeed", StorageType.Float)),
			new EventActionInfo(0, new EventActionParameterInfo("Height", StorageType.Float)),
			new EventActionInfo(0, new EventActionParameterInfo("Speed", StorageType.Float)),
			new EventActionInfo(1, new EventActionParameterInfo("Variable", StorageType.Float), new EventActionParameterInfo("BaseValue", StorageType.Float), new EventActionParameterInfo("DeltaValue", StorageType.Float), new EventActionParameterInfo("MinValue", StorageType.Float), new EventActionParameterInfo("MaxValue", StorageType.Float)),
			new EventActionInfo(0),
			new EventActionInfo(0),
			new EventActionInfo(0),
			null,
			null,
			null
		};
	}
}
