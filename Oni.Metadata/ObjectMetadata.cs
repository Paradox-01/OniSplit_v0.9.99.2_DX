using System;

namespace Oni.Metadata
{
	internal class ObjectMetadata
	{
		internal enum TypeTag
		{
			CHAR = 1128808786,
			CMBT = 1129136724,
			CONS = 1129270867,
			DOOR = 1146048338,
			FLAG = 1179402567,
			FURN = 1179996750,
			MELE = 1296387141,
			NEUT = 1313166676,
			PART = 1346458196,
			PATR = 1346458706,
			PWRU = 1347899989,
			SNDG = 1397638215,
			TRGV = 1414678358,
			TRIG = 1414678855,
			TURR = 1414877778,
			WEAP = 1464156496
		}

		[Flags]
		public enum ObjectFlags : uint
		{
			None = 0u,
			Locked = 1u,
			PlacedInGame = 2u,
			Temporary = 4u,
			Gunk = 8u
		}

		[Flags]
		internal enum CharacterFlags : uint
		{
			None = 0u,
			IsPlayer = 1u,
			RandomCostume = 2u,
			NotInitiallyPresent = 4u,
			NonCombatant = 8u,
			CanSpawnMultiple = 0x10u,
			Spawned = 0x20u,
			Unkillable = 0x40u,
			InfiniteAmmo = 0x80u,
			Omniscient = 0x100u,
			HasLSI = 0x200u,
			Boss = 0x400u,
			UpgradeDifficulty = 0x800u,
			NoAutoDrop = 0x1000u
		}

		internal enum CharacterTeam : uint
		{
			Konoko,
			TCTF,
			Syndicate,
			Neutral,
			SecurityGuard,
			RogueKonoko,
			Switzerland,
			SyndicateAccessory
		}

		internal enum CharacterJobType : uint
		{
			None,
			Idle,
			Guard,
			Patrol,
			TeamBatle,
			Combat,
			Melee,
			Alarm,
			Neutral,
			Panic
		}

		internal enum CharacterAlertStatus : uint
		{
			Lull,
			Low,
			Medium,
			High,
			Combat
		}

		internal enum CharacterPursuitMode : uint
		{
			None,
			Forget,
			GoTo,
			Wait,
			Look,
			Move,
			Hunt,
			Glanc
		}

		internal enum CharacterPursuitLostBehavior : uint
		{
			ReturnToJob,
			KeepLooking,
			FindAlarm
		}

		internal enum CombatBehaviorType : uint
		{
			None,
			Stare,
			HoldAndFire,
			FiringCharge,
			Melee,
			BarabasShoot,
			BarabasAdvance,
			BarabasMelee,
			SuperNinjaFireball,
			SuperNinjaAdvance,
			SuperNinjaMelee,
			RunForAlarm,
			MutantMuroMelee,
			MuroThunderbolt
		}

		internal enum CombatMeleeOverride : uint
		{
			None,
			IfPunched,
			Cancelled,
			ShortRange,
			MediumRange,
			AlwaysMelee
		}

		internal enum CombatNoGunBehavior : uint
		{
			Melee,
			Retreat,
			RunForAlarm
		}

		[Flags]
		internal enum ConsoleFlags : ushort
		{
			None = 0,
			InitialActive = 8,
			Punch = 0x20,
			IsAlarm = 0x40
		}

		[Flags]
		internal enum DoorFlags : ushort
		{
			None = 0,
			InitialLocked = 1,
			InDoorFrame = 4,
			Manual = 0x10,
			DoubleDoor = 0x80,
			Mirror = 0x100,
			OneWay = 0x200,
			Reverse = 0x400,
			Jammed = 0x800,
			InitialOpen = 0x1000
		}

		[Flags]
		internal enum MeleeTechniqueFlags : uint
		{
			None = 0u,
			Interruptible = 1u,
			GenerousDir = 2u,
			Fearless = 4u
		}

		internal enum MeleeMoveCategory
		{
			Attack = 0,
			Position = 16,
			Maneuver = 32,
			Evade = 48,
			Throw = 64
		}

		internal enum MeleeMoveAttackType
		{
			P,
			PP,
			PPP,
			PPPP,
			PF,
			PL,
			PR,
			PB,
			PD,
			PF_PF,
			PF_PF_PF,
			PL_PL,
			PL_PL_PL,
			PR_PR,
			PR_PR_PR,
			PB_PB,
			PB_PB_PB,
			PD_PD,
			PD_PD_PD,
			K,
			KK,
			KKK,
			KKKF,
			KF,
			KL,
			KR,
			KB,
			KD,
			KF_KF,
			KF_KF_KF,
			KL_KL,
			KL_KL_KL,
			KR_KR,
			KR_KR_KR,
			KB_KB,
			KB_KB_KB,
			KD_KD,
			KD_KD_KD,
			PPK,
			PKK,
			PKP,
			KPK,
			KPP,
			KKP,
			PK,
			KP,
			PPKK,
			PPKKK,
			PPKKKKK,
			HP,
			HPF,
			HK,
			HKF,
			CS_P,
			CS_K,
			C_P1,
			C_P2,
			C_PF,
			C_K1,
			C_K2,
			C_KF,
			GETUP_KF,
			GETUP_KB,
			R_P,
			R_K,
			RB_P,
			RB_K,
			RL_P,
			RL_K,
			RR_P,
			RR_K,
			R_SLIDE,
			J_P,
			J_K,
			JF_P,
			JF_PB,
			JF_K,
			JF_KB,
			JB_P,
			JB_K,
			JL_P,
			JL_K,
			JR_P,
			JR_K
		}

		internal enum MeleeMovePositionType
		{
			RunForward,
			RunLeft,
			RunRight,
			RunBack,
			JumpUp,
			JumpForward,
			JumpLeft,
			JumpRight,
			JumpBack,
			StartToCrouch,
			Crouch,
			Stand,
			CloseForward,
			CloseLeft,
			CloseRight,
			CloseBack,
			RunJumpForward,
			RunJumpLeft,
			RunJumpRight,
			RunJumpBack
		}

		internal enum MeleeMoveManeuverType
		{
			Advance,
			Retreat,
			CircleLeft,
			CircleRight,
			Pause,
			Crouch,
			Jump,
			Taunt,
			RandomStop,
			GetUpForward,
			GetUpBackward,
			GetUpRollLeft,
			GetUpRollRight,
			BarabasWave
		}

		internal class MeleeMoveTypeInfo
		{
			public MeleeMoveManeuverType Type;

			public string[] ParamNames;

			public MeleeMoveTypeInfo(MeleeMoveManeuverType type, params string[] paramNames)
			{
				Type = type;
				ParamNames = paramNames;
			}
		}

		internal enum MeleeMoveEvadeType
		{
			JumpForward,
			JumpForward2,
			JumpBack,
			JumpBack2,
			JumpLeft,
			JumpLeft2,
			JumpRight,
			JumpRight2,
			RunJumpForward,
			RunJumpForward2,
			RunJumpBack,
			RunJumpBack2,
			RunJumpLeft,
			RunJumpLeft2,
			RunJumpRight,
			RunJumpRight2,
			RollForward,
			RollBackward,
			RollLeft,
			RollRight,
			SlideForward,
			SlideBack,
			SlideLeft,
			SlideRight
		}

		internal enum MeleeMoveThrowType
		{
			P_Front,
			K_Front,
			P_Behind,
			K_Behind,
			RP_Front,
			RK_Front,
			RP_Behind,
			RK_Behind,
			P_FrontDisarm,
			K_FrontDisarm,
			P_BehindDisarm,
			K_BehindDisarm,
			RP_FrontDisarm,
			RK_FrontDisarm,
			RP_BehindDisarm,
			RK_BehindDisarm,
			P_FrontRifDisarm,
			K_FrontRifDisarm,
			P_BehindRifDisarm,
			K_BehindRifDisarm,
			RP_FrontRifDisarm,
			RK_FrontRifDisarm,
			RP_BehindRifDisarm,
			RK_BehindRifDisarm,
			Tackle
		}

		[Flags]
		internal enum NeutralFlags : uint
		{
			None = 0u,
			NoResume = 1u,
			NoResumeAfterGive = 2u,
			Uninterruptible = 4u
		}

		[Flags]
		public enum NeutralItems : byte
		{
			None = 0,
			Shield = 1,
			Invisibility = 2,
			LSI = 4
		}

		[Flags]
		public enum NeutralDialogLineFlags : ushort
		{
			None = 0,
			IsPlayer = 1,
			GiveItems = 2,
			AnimOnce = 4,
			OtherAnimOnce = 8
		}

		[Flags]
		internal enum ParticleFlags : ushort
		{
			None = 0,
			NotInitiallyCreated = 2
		}

		internal enum PatrolPathPointType
		{
			MoveToFlag,
			Stop,
			Pause,
			LookAtFlag,
			LookAtPoint,
			MoveAndFaceFlag,
			Loop,
			MovementMode,
			MoveToPoint,
			LockFacing,
			MoveThroughFlag,
			MoveThroughPoint,
			StopLooking,
			FreeFacing,
			GlanceAtFlagFor,
			MoveNearFlag,
			LoopFrom,
			Scan,
			StopScanning,
			MoveToFlagLookAndWait,
			CallScript,
			ForkScript,
			IgnorePlayer,
			FaceToFlagAndFire
		}

		internal enum PatrolPathFacing
		{
			Forward = 0,
			Backward = 1,
			Left = Backward,
			Right = 2,
			Stopped = 3
		}

		internal enum PatrolPathMovementMode
		{
			ByAlertLevel,
			Stop,
			Crouch,
			Creep,
			WalkNoAim,
			Walk,
			RunNoAim,
			Run
		}

		internal enum PowerUpClass : uint
		{
			Ammo = 1112362305u,
			EnergyCell = 1162693953u,
			Hypo = 1330665800u,
			Shield = 1145849939u,
			Invisibility = 1230392905u,
			LSI = 1230195777u
		}

		internal enum SoundVolumeType
		{
			Box = 1447841093,
			Sphere = 1397770322
		}

		[Flags]
		public enum TriggerVolumeFlags : uint
		{
			None = 0u,
			OneTimeEnter = 1u,
			OneTimeInside = 2u,
			OneTimeExit = 4u,
			EnterDisabled = 8u,
			InsideDisabled = 0x10u,
			ExitDisabled = 0x20u,
			Disabled = 0x40u,
			PlayerOnly = 0x80u
		}

		[Flags]
		public enum TriggerFlags : ushort
		{
			None = 0,
			InitialActive = 8,
			ReverseAnim = 0x10,
			PingPong = 0x20
		}

		[Flags]
		internal enum TurretTargetTeams : uint
		{
			None = 0u,
			Konoko = 1u,
			TCTF = 2u,
			Syndicate = 4u,
			Neutral = 8u,
			SecurityGuard = 0x10u,
			RogueKonoko = 0x20u,
			Switzerland = 0x40u,
			SyndicateAccessory = 0x80u
		}

		[Flags]
		internal enum TurretFlags : ushort
		{
			None = 0,
			InitialActive = 2
		}

		internal enum EventType
		{
			None,
			Script,
			ActivateTurret,
			DeactivateTurret,
			ActivateConsole,
			DeactivateConsole,
			ActivateAlarm,
			DeactivateAlaram,
			ActivateTrigger,
			DeactivateTrigger,
			LockDoor,
			UnlockDoor
		}

		public static readonly MetaStruct Header = new MetaStruct("Object", new Field(MetaType.Enum<ObjectFlags>(), "Flags"), new Field(MetaType.Vector3, "Position"), new Field(MetaType.Vector3, "Rotation"));

		public static readonly MetaStruct Character = new MetaStruct("Character", new Field(MetaType.Enum<CharacterFlags>(), "Flags"), new Field(MetaType.String64, "Class"), new Field(MetaType.String32, "Name"), new Field(MetaType.String64, "Weapon"), new Field(new MetaStruct("CharacterScripts", new Field(MetaType.String32, "Spawn"), new Field(MetaType.String32, "Die"), new Field(MetaType.String32, "Combat"), new Field(MetaType.String32, "Alarm"), new Field(MetaType.String32, "Hurt"), new Field(MetaType.String32, "Defeated"), new Field(MetaType.String32, "OutOfAmmo"), new Field(MetaType.String32, "NoPath")), "Scripts"), new Field(MetaType.Int32, "AdditionalHealth"), new Field(new MetaStruct("CharacterJob", new Field(MetaType.Enum<CharacterJobType>(), "Type"), new Field(MetaType.Int16, "PatrolPathId")), "Job"), new Field(new MetaStruct("CharacterBehaviors", new Field(MetaType.Int16, "CombatId"), new Field(MetaType.Int16, "MeleeId"), new Field(MetaType.Int16, "NeutralId")), "Behaviors"), new Field(new MetaStruct("CharacterInventory", new Field(new MetaStruct("Ammo", new Field(MetaType.Int16, "Use"), new Field(MetaType.Int16, "Drop")), "Ammo"), new Field(new MetaStruct("EnergyCell", new Field(MetaType.Int16, "Use"), new Field(MetaType.Int16, "Drop")), "EnergyCell"), new Field(new MetaStruct("Hypo", new Field(MetaType.Int16, "Use"), new Field(MetaType.Int16, "Drop")), "Hypo"), new Field(new MetaStruct("Shield", new Field(MetaType.Int16, "Use"), new Field(MetaType.Int16, "Drop")), "Shield"), new Field(new MetaStruct("Invisibility", new Field(MetaType.Int16, "Use"), new Field(MetaType.Int16, "Drop")), "Invisibility"), new Field(MetaType.Padding(4))), "Inventory"), new Field(MetaType.Enum<CharacterTeam>(), "Team"), new Field(MetaType.Int32, "AmmoPercentage"), new Field(new MetaStruct("CharacterAlert", new Field(MetaType.Enum<CharacterAlertStatus>(), "Initial"), new Field(MetaType.Enum<CharacterAlertStatus>(), "Minimal"), new Field(MetaType.Enum<CharacterAlertStatus>(), "JobStart"), new Field(MetaType.Enum<CharacterAlertStatus>(), "Investigate")), "Alert"), new Field(MetaType.Int32, "AlarmGroups"), new Field(new MetaStruct("CharacterPursuit", new Field(MetaType.Enum<CharacterPursuitMode>(), "StrongUnseen"), new Field(MetaType.Enum<CharacterPursuitMode>(), "WeakUnseen"), new Field(MetaType.Enum<CharacterPursuitMode>(), "StrongSeen"), new Field(MetaType.Enum<CharacterPursuitMode>(), "WeakSeen"), new Field(MetaType.Enum<CharacterPursuitLostBehavior>(), "Lost")), "Pursuit"));

		public static readonly MetaStruct CombatProfile = new MetaStruct("CombatProfile", new Field(MetaType.String64, "Name"), new Field(MetaType.Int32, "CombatId"), new Field(new MetaStruct("CMBTBehaviors", new Field(MetaType.Enum<CombatBehaviorType>(), "LongRange"), new Field(MetaType.Enum<CombatBehaviorType>(), "MediumRange"), new Field(MetaType.Enum<CombatBehaviorType>(), "ShortRange"), new Field(MetaType.Enum<CombatBehaviorType>(), "MediumRetreat"), new Field(MetaType.Enum<CombatBehaviorType>(), "LongRetreat")), "Behaviors"), new Field(new MetaStruct("CMBTCombat", new Field(MetaType.Float, "MediumRange"), new Field(MetaType.Enum<CombatMeleeOverride>(), "MeleeOverride"), new Field(MetaType.Enum<CombatNoGunBehavior>(), "NoGunBehavior"), new Field(MetaType.Float, "ShortRange"), new Field(MetaType.Float, "PursuitDistance")), "Combat"), new Field(new MetaStruct("CMBTPanic", new Field(MetaType.Int32, "Hurt"), new Field(MetaType.Int32, "GunFire"), new Field(MetaType.Int32, "Melee"), new Field(MetaType.Int32, "Sight")), "Panic"), new Field(new MetaStruct("CMBTAlarm", new Field(MetaType.Float, "SearchDistance"), new Field(MetaType.Float, "EnemyIgnoreDistance"), new Field(MetaType.Float, "EnemyAttackDistance"), new Field(MetaType.Int32, "DamageThreshold"), new Field(MetaType.Int32, "FightTimer")), "Alarm"));

		public static readonly MetaStruct Console = new MetaStruct("Console", new Field(MetaType.String63, "Class"), new Field(MetaType.Int16, "ConsoleId"), new Field(MetaType.Enum<ConsoleFlags>(), "Flags"), new Field(MetaType.String63, "InactiveTexture"), new Field(MetaType.String63, "ActiveTexture"), new Field(MetaType.String63, "TriggeredTexture"));

		public static readonly MetaStruct Door = new MetaStruct("Door", new Field(MetaType.String63, "Class"), new Field(MetaType.Int16, "DoorId"), new Field(MetaType.Int16, "KeyId"), new Field(MetaType.Enum<DoorFlags>(), "Flags"), new Field(MetaType.Vector3, "Center"), new Field(MetaType.Float, "SquaredActivationRadius"), new Field(MetaType.String63, "Texture1"), new Field(MetaType.String63, "Texture2"));

		public static readonly MetaStruct Flag = new MetaStruct("Flag", new Field(MetaType.Color, "Color"), new Field(MetaType.Int16, "Prefix"), new Field(MetaType.Int16, "FlagId"), new Field(MetaType.String128, "Notes"));

		public static readonly MetaStruct Furniture = new MetaStruct("Furniture", new Field(MetaType.String32, "Class"), new Field(MetaType.String48, "Particle"));

		public static readonly MetaStruct MeleeProfile = new MetaStruct("MeleeProfile", new Field(MetaType.Int32, "MeleeId"), new Field(MetaType.String64, "Name"), new Field(MetaType.String64, "CharacterClass"), new Field(MetaType.Int32, "Notice"), new Field(new MetaStruct("MeleeDodge", new Field(MetaType.Int32, "Base"), new Field(MetaType.Int32, "Extra"), new Field(MetaType.Int32, "ExtraDamageThreshold")), "Dodge"), new Field(new MetaStruct("MeleeBlockSkill", new Field(MetaType.Int32, "Single"), new Field(MetaType.Int32, "Group")), "BlockSkill"), new Field(MetaType.Float, "NotBlocked"), new Field(MetaType.Float, "MustChangeStance"), new Field(MetaType.Float, "BlockedButUnblockable"), new Field(MetaType.Float, "BlockedButHasStagger"), new Field(MetaType.Float, "BlockedButHasBlockstun"), new Field(MetaType.Float, "Blocked"), new Field(MetaType.Float, "ThrowDanger"), new Field(MetaType.Int16, "DazedMinFrames"), new Field(MetaType.Int16, "DazedMaxFrames"));

		public static readonly MetaStruct MeleeTechnique = new MetaStruct("MeleeTechnique", new Field(MetaType.String64, "Name"), new Field(MetaType.Enum<MeleeTechniqueFlags>(), "Flags"), new Field(MetaType.UInt32, "Weight"), new Field(MetaType.UInt32, "Importance"), new Field(MetaType.UInt32, "RepeatDelay"));

		public static readonly MeleeMoveTypeInfo[] MeleeMoveManeuverTypeInfo = new MeleeMoveTypeInfo[14]
		{
			new MeleeMoveTypeInfo(MeleeMoveManeuverType.Advance, "Duration", "MinRange", "ThresholdRange"),
			new MeleeMoveTypeInfo(MeleeMoveManeuverType.Retreat, "Duration", "MaxRange", "ThresholdRange"),
			new MeleeMoveTypeInfo(MeleeMoveManeuverType.CircleLeft, "Duration", "MinAngle", "MaxAngle"),
			new MeleeMoveTypeInfo(MeleeMoveManeuverType.CircleRight, "Duration", "MinAngle", "MaxAngle"),
			new MeleeMoveTypeInfo(MeleeMoveManeuverType.Pause, "Duration"),
			new MeleeMoveTypeInfo(MeleeMoveManeuverType.Crouch, "Duration"),
			new MeleeMoveTypeInfo(MeleeMoveManeuverType.Jump, "Duration"),
			new MeleeMoveTypeInfo(MeleeMoveManeuverType.Taunt, "Duration"),
			new MeleeMoveTypeInfo(MeleeMoveManeuverType.RandomStop, "Chance"),
			new MeleeMoveTypeInfo(MeleeMoveManeuverType.GetUpForward, "Duration"),
			new MeleeMoveTypeInfo(MeleeMoveManeuverType.GetUpBackward, "Duration"),
			new MeleeMoveTypeInfo(MeleeMoveManeuverType.GetUpRollLeft, "Duration"),
			new MeleeMoveTypeInfo(MeleeMoveManeuverType.GetUpRollRight, "Duration"),
			new MeleeMoveTypeInfo(MeleeMoveManeuverType.BarabasWave, "MaxRange")
		};

		public static readonly MetaStruct MeleeMove = new MetaStruct("MeleeMove", new Field(MetaType.Int32, "Type"), new Field(MetaType.Float, "Param1"), new Field(MetaType.Float, "Param2"), new Field(MetaType.Float, "Param3"));

		public static readonly MetaStruct NeutralBehavior = new MetaStruct("NeutralBehavior", new Field(MetaType.String32, "Name"), new Field(MetaType.Int16, "NeutralId"));

		public static readonly MetaStruct NeutralBehaviorParams = new MetaStruct("NeutralBehaviorParams", new Field(MetaType.Enum<NeutralFlags>(), "Flags"), new Field(new MetaStruct("NeutralBehaviorRange", new Field(MetaType.Float, "Trigger"), new Field(MetaType.Float, "Talk"), new Field(MetaType.Float, "Follow"), new Field(MetaType.Float, "Enemy")), "Ranges"), new Field(new MetaStruct("NeutralehaviorSpeech", new Field(MetaType.String32, "Trigger"), new Field(MetaType.String32, "Abort"), new Field(MetaType.String32, "Enemy")), "Speech"), new Field(new MetaStruct("NeutralBehaviorScript", new Field(MetaType.String32, "AfterTalk")), "Script"), new Field(new MetaStruct("NeutralBehaviorRewards", new Field(MetaType.String32, "WeaponClass"), new Field(MetaType.Byte, "Ammo"), new Field(MetaType.Byte, "EnergyCell"), new Field(MetaType.Byte, "Hypo"), new Field(MetaType.Enum<NeutralItems>(), "Other")), "Rewards"));

		public static readonly MetaStruct NeutralBehaviorDialogLine = new MetaStruct("DialogLine", new Field(MetaType.Enum<NeutralDialogLineFlags>(), "Flags"), new Field(MetaType.Padding(2)), new Field(MetaType.Int16, "Anim"), new Field(MetaType.Int16, "OtherAnim"), new Field(MetaType.String32, "SpeechName"));

		public static readonly MetaStruct Particle = new MetaStruct("Particle", new Field(MetaType.String64, "Class"), new Field(MetaType.String48, "Tag"), new Field(MetaType.Enum<ParticleFlags>(), "Flags"), new Field(MetaType.Vector2, "DecalScale"));

		public static readonly MetaStruct PatrolPath = new MetaStruct("PatrolPath", new Field(MetaType.String32, "Name"));

		public static readonly MetaStruct PatrolPathInfo = new MetaStruct("PatrolPathInfo", new Field(MetaType.Int16, "PatrolId"), new Field(MetaType.Int16, "ReturnToNearest"));

		public static readonly MetaStruct PowerUp = new MetaStruct("PowerUp", new Field(MetaType.Enum<PowerUpClass>(), "Class"));

		public static readonly MetaStruct Sound = new MetaStruct("Sound", new Field(MetaType.String32, "Class"));

		public static readonly MetaStruct SoundSphere = new MetaStruct("SoundSphere", new Field(MetaType.Float, "MinRadius"), new Field(MetaType.Float, "MaxRadius"));

		public static readonly MetaStruct SoundParams = new MetaStruct("SoundParams", new Field(MetaType.Float, "Volume"), new Field(MetaType.Float, "Pitch"));

		public static readonly MetaStruct TriggerVolume = new MetaStruct("TriggerVolume", new Field(MetaType.String63, "Name"), new Field(new MetaStruct("TriggerVolumeScripts", new Field(MetaType.String32, "Entry"), new Field(MetaType.String32, "Inside"), new Field(MetaType.String32, "Exit")), "Scripts"), new Field(MetaType.Byte, "Teams"), new Field(MetaType.Padding(3)), new Field(MetaType.Vector3, "Size"), new Field(MetaType.Int32, "TriggerVolumeId"), new Field(MetaType.Int32, "ParentId"), new Field(MetaType.String128, "Notes"), new Field(MetaType.Enum<TriggerVolumeFlags>(), "Flags"));

		public static readonly MetaStruct Trigger = new MetaStruct("Trigger", new Field(MetaType.String63, "Class"), new Field(MetaType.Int16, "TriggerId"), new Field(MetaType.Enum<TriggerFlags>(), "Flags"), new Field(MetaType.Color, "LaserColor"), new Field(MetaType.Float, "StartPosition"), new Field(MetaType.Float, "Speed"), new Field(MetaType.Int16, "EmitterCount"), new Field(MetaType.Int16, "TimeOn"), new Field(MetaType.Int16, "TimeOff"));

		public static readonly MetaStruct Turret = new MetaStruct("Turret", new Field(MetaType.String63, "Class"), new Field(MetaType.Int16, "TurretId"), new Field(MetaType.Enum<TurretFlags>(), "Flags"), new Field(MetaType.Padding(36)), new Field(MetaType.Enum<TurretTargetTeams>(), "TargetedTeams"));

		public static readonly MetaStruct Weapon = new MetaStruct("Weapon", new Field(MetaType.String32, "Class"));

		public static int GetPatrolPathPointSize(PatrolPathPointType pointType)
		{
			switch (pointType)
			{
			case PatrolPathPointType.IgnorePlayer:
				return 1;
			case PatrolPathPointType.MoveToFlag:
			case PatrolPathPointType.LookAtFlag:
			case PatrolPathPointType.MoveAndFaceFlag:
			case PatrolPathPointType.CallScript:
			case PatrolPathPointType.ForkScript:
				return 2;
			case PatrolPathPointType.Pause:
			case PatrolPathPointType.MovementMode:
			case PatrolPathPointType.LockFacing:
			case PatrolPathPointType.LoopFrom:
				return 4;
			case PatrolPathPointType.MoveThroughFlag:
			case PatrolPathPointType.GlanceAtFlagFor:
			case PatrolPathPointType.MoveNearFlag:
			case PatrolPathPointType.Scan:
				return 6;
			case PatrolPathPointType.MoveToFlagLookAndWait:
			case PatrolPathPointType.FaceToFlagAndFire:
				return 8;
			case PatrolPathPointType.LookAtPoint:
			case PatrolPathPointType.MoveToPoint:
				return 12;
			case PatrolPathPointType.MoveThroughPoint:
				return 16;
			default:
				return 0;
			}
		}
	}
}
