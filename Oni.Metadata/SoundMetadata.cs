using System;

namespace Oni.Metadata
{
	internal class SoundMetadata
	{
		public enum OSAmPriority : uint
		{
			Low,
			Normal,
			High,
			Highest
		}

		[Flags]
		public enum OSAmFlags : uint
		{
			None = 0u,
			InterruptTracksOnStop = 1u,
			PlayOnce = 2u,
			CanPan = 4u
		}

		public enum OSImPriority : uint
		{
			Low,
			Normal,
			High,
			Highest
		}

		[Flags]
		public enum OSGrFlags : ushort
		{
			None = 0,
			PreventRepeat = 1
		}

		public const int OSAm = 1330856301;

		public const int OSGr = 1330857842;

		public const int OSIm = 1330858349;

		public static readonly MetaStruct osam4 = new MetaStruct("OSAm", new Field(MetaType.Enum<OSAmPriority>(), "Priority"), new Field(MetaType.Enum<OSAmFlags>(), "Flags"), new Field(new MetaStruct("OSAmDetailTrackProperties", new Field(MetaType.Float, "SphereRadius"), new Field(new MetaStruct(new Field(MetaType.Float, "Min"), new Field(MetaType.Float, "Max")), "ElapsedTime")), "DetailTrackProperties"), new Field(new MetaStruct(new Field(new MetaStruct(new Field(MetaType.Float, "Min"), new Field(MetaType.Float, "Max")), "Distance")), "Volume"), new Field(MetaType.String32, "DetailTrack"), new Field(MetaType.String32, "BaseTrack1"), new Field(MetaType.String32, "BaseTrack2"), new Field(MetaType.String32, "InSound"), new Field(MetaType.String32, "OutSound"));

		public static readonly MetaStruct osam5 = new MetaStruct("OSAm5", osam4, new Field(MetaType.UInt32, "Treshold"));

		public static readonly MetaStruct osam6 = new MetaStruct("OSAm6", osam5, new Field(MetaType.Float, "MinOcclusion"));

		public static readonly MetaStruct osim3 = new MetaStruct("OSIm3", new Field(MetaType.String32, "Group"), new Field(MetaType.Enum<OSImPriority>(), "Priority"), new Field(new MetaStruct("OSImVolume", new Field(new MetaStruct("OSImDistance", new Field(MetaType.Float, "Min"), new Field(MetaType.Float, "Max")), "Distance"), new Field(new MetaStruct("OSImAngle", new Field(MetaType.Float, "Min"), new Field(MetaType.Float, "Max"), new Field(MetaType.Float, "MinAttenuation")), "Angle")), "Volume"));

		public static readonly MetaStruct osim4 = new MetaStruct("OSIm4", osim3, new Field(new MetaStruct("OSImAlternateImpulse", new Field(MetaType.UInt32, "Treshold"), new Field(MetaType.String32, "Impulse")), "AlternateImpulse"));

		public static readonly MetaStruct osim5 = new MetaStruct("OSIm5", osim4, new Field(MetaType.Float, "ImpactVelocity"));

		public static readonly MetaStruct osim6 = new MetaStruct("OSIm6", osim5, new Field(MetaType.Float, "MinOcclusion"));

		public static readonly MetaStruct osgrPermutation = new MetaStruct("Permutation", new Field(MetaType.Int32, "Weight"), new Field(new MetaStruct(new Field(MetaType.Float, "Min"), new Field(MetaType.Float, "Max")), "Volume"), new Field(new MetaStruct(new Field(MetaType.Float, "Min"), new Field(MetaType.Float, "Max")), "Pitch"), new Field(MetaType.String32, "Sound"));

		public static readonly MetaStruct osgr1 = new MetaStruct("OSGr", new Field(MetaType.Int32, "NumberOfChannels"), new Field(MetaType.VarArray(osgrPermutation), "Permutations"));

		public static readonly MetaStruct osgr2 = new MetaStruct("OSGr", new Field(MetaType.Float, "Volume"), new Field(MetaType.Int32, "NumberOfChannels"), new Field(MetaType.VarArray(osgrPermutation), "Permutations"));

		public static readonly MetaStruct osgr3 = new MetaStruct("OSGr", new Field(MetaType.Float, "Volume"), new Field(MetaType.Float, "Pitch"), new Field(MetaType.Int32, "NumberOfChannels"), new Field(MetaType.VarArray(osgrPermutation), "Permutations"));

		public static readonly MetaStruct osgr6 = new MetaStruct("OSGr", new Field(MetaType.Float, "Volume"), new Field(MetaType.Float, "Pitch"), new Field(MetaType.Enum<OSGrFlags>(), "Flags"), new Field(MetaType.Padding(2)), new Field(MetaType.Int32, "NumberOfChannels"), new Field(MetaType.VarArray(osgrPermutation), "Permutations"));
	}
}
