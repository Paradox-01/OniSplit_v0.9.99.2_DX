using System;
using System.Collections.Generic;
using System.IO;
using Oni.Motoko;

namespace Oni.Metadata
{
	internal abstract class InstanceMetadata
	{
		[Flags]
		public enum AIFiringModeFlags : uint
		{
			None = 0u,
			NoWildShots = 1u
		}

		public enum AISACharacterTeam : ushort
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

		[Flags]
		public enum AISACharacterFlags : ushort
		{
			None = 0,
			AI = 1,
			AutoFreeze = 2,
			Neutral = 4,
			TurnGuard = 8
		}

		[Flags]
		private enum AGQGFlags : uint
		{
			None = 0u,
			DoorFrame = 1u,
			Ghost = 2u,
			StairsUp = 4u,
			StairsDown = 8u,
			Stairs = 0x10u,
			Transparent = 0x80u,
			TwoSided = 0x200u,
			NoCollision = 0x800u,
			Invisible = 0x2000u,
			NoObjectCollision = 0x4000u,
			NoCharacterCollision = 0x8000u,
			NoOcclusion = 0x10000u,
			Danger = 0x20000u,
			GridIgnore = 0x400000u,
			NoDecals = 0x800000u,
			Furniture = 0x1000000u,
			SoundTransparent = 0x8000000u,
			Impassable = 0x10000000u,
			Triangle = 0x40u,
			Horizontal = 0x80000u,
			Vertical = 0x100000u,
			ProjectionBit0 = 0x2000000u,
			ProjectionBit1 = 0x4000000u
		}

		[Flags]
		internal enum ENVPFlags : ushort
		{
			None = 0,
			NotInitiallyCreated = 2
		}

		[Flags]
		public enum CONSFlags : uint
		{
			None = 0u,
			AlarmConsole = 1u
		}

		public enum DOORSoundType
		{
			None = -1,
			Unimportant,
			Interesting,
			Danger,
			Melee,
			Gunfire
		}

		public enum DOORSoundAllow : uint
		{
			All,
			Combat,
			Gunfire,
			None
		}

		[Flags]
		private enum OBANFlags : uint
		{
			None = 0u,
			NormalLoop = 1u,
			BackToBackLoop = 2u,
			RandomStartFrame = 4u,
			Autostart = 8u,
			ZAxisUp = 0x10u
		}

		[Flags]
		public enum OBOAFlags : uint
		{
			None = 0u,
			InUse = 0x200u,
			NoCollision = 0x400u,
			NoGravity = 0x800u,
			FaceCollision = 0x1000u
		}

		public enum OBOAPhysics : uint
		{
			None,
			Static,
			Linear,
			Animated,
			Newton
		}

		[Flags]
		public enum AICharacterFlags : uint
		{
			None = 0u,
			NoStartleAnim = 1u,
			EnableMeleeFireDodge = 2u,
			ShootDodge = 4u,
			RunAwayDodge = 8u,
			NotUsed = 0x10u
		}

		public enum ONCPBodyPart : ushort
		{
			Pelvis = 0,
			LeftThigh = 1,
			LeftCalf = 2,
			LeftFoot = 3,
			RightThigh = 4,
			RightCalf = 5,
			RightFoot = 6,
			Mid = 7,
			Chest = 8,
			Neck = 9,
			Head = 10,
			LeftShoulder = 11,
			LeftArm = 12,
			LeftWrist = 13,
			LeftFist = 14,
			RightShoulder = 15,
			RightArm = 16,
			RightWrist = 17,
			RightFist = 18,
			KillImpact = 32768,
			None = ushort.MaxValue
		}

		[Flags]
		public enum FILMKeys : ulong
		{
			None = 0uL,
			Escape = 1uL,
			Console = 2uL,
			Pause = 4uL,
			Cutscene1 = 8uL,
			Cutscene2 = 0x10uL,
			F4 = 0x20uL,
			F5 = 0x40uL,
			F6 = 0x80uL,
			F7 = 0x100uL,
			F8 = 0x200uL,
			StartRecord = 0x400uL,
			StopRecord = 0x800uL,
			PlayRecord = 0x1000uL,
			F12 = 0x2000uL,
			LookMode = 0x8000uL,
			Screenshot = 0x10000uL,
			Forward = 0x200000uL,
			Backward = 0x400000uL,
			TurnLeft = 0x800000uL,
			TurnRight = 0x1000000uL,
			StepLeft = 0x2000000uL,
			StepRight = 0x4000000uL,
			Jump = 0x8000000uL,
			Crouch = 0x10000000uL,
			Punch = 0x20000000uL,
			Kick = 0x40000000uL,
			Block = 0x80000000uL,
			Walk = 0x100000000uL,
			Action = 0x200000000uL,
			Hypo = 0x400000000uL,
			Reload = 0x800000000uL,
			Swap = 0x1000000000uL,
			Drop = 0x2000000000uL,
			Fire1 = 0x4000000000uL,
			Fire2 = 0x8000000000uL,
			Fire3 = 0x10000000000uL
		}

		private enum IGPGFontStyle : uint
		{
			Normal,
			Bold,
			Italic
		}

		[Flags]
		private enum IGPGFlags : ushort
		{
			None = 0,
			Family = 1,
			Style = 2,
			Color = 4,
			Size = 8
		}

		private enum IGStFontStyle : uint
		{
			Normal,
			Bold,
			Italic
		}

		[Flags]
		private enum IGStFlags : ushort
		{
			None = 0,
			Family = 1,
			Style = 2,
			Color = 4,
			Size = 8
		}

		private enum PSpLType : uint
		{
			OutOfGameBackground = 0u,
			InGameBackground = 1u,
			SoundDebugPanelBackground = 5u
		}

		public enum TRAMType : ushort
		{
			None,
			Anything,
			Walk,
			Run,
			Slide,
			Jump,
			Stand,
			StandingTurnLeft,
			StandingTurnRight,
			RunBackwards,
			RunSidestepLeft,
			RunSidestepRight,
			Kick,
			WalkSidestepLeft,
			WalkSidestepRight,
			WalkBackwards,
			Stance,
			Crouch,
			JumpForward,
			JumpBackward,
			JumpLeft,
			JumpRight,
			Punch,
			Block,
			Land,
			Fly,
			KickForward,
			KickLeft,
			KickRight,
			KickBack,
			KickLow,
			PunchForward,
			PunchLeft,
			PunchRight,
			PunchBack,
			PunchLow,
			Kick2,
			Kick3,
			Punch2,
			Punch3,
			LandForward,
			LandRight,
			LandLeft,
			LandBack,
			PPK,
			PKK,
			PKP,
			KPK,
			KPP,
			KKP,
			PK,
			KP,
			PunchHeavy,
			KickHeavy,
			PunchForwardHeavy,
			KickForwardHeavy,
			AimingOverlay,
			HitOverlay,
			CrouchRun,
			CrouchWalk,
			CrouchRunBackwards,
			CrouchWalkBackwards,
			CrouchRunSidestepLeft,
			CrouchRunSidestepRight,
			CrouchWalkSidestepLeft,
			CrouchWalkSidestepRight,
			RunKick,
			RunPunch,
			RunBackPunch,
			RunBackKick,
			SidestepLeftKick,
			SidestepLeftPunch,
			SidestepRightKick,
			SidestepRightPunch,
			Prone,
			Flip,
			HitHead,
			HitBody,
			HitFoot,
			KnockdownHead,
			KnockdownBody,
			KnockdownFoot,
			HitCrouch,
			KnockdownCrouch,
			HitFallen,
			HitHeadBehind,
			HitBodyBehind,
			HitFootBehind,
			KnockdownHeadBehind,
			KnockdownBodyBehind,
			KnockdownFootBehind,
			HitCrouchBehind,
			KnockdownCrouchBehind,
			Idle,
			Taunt,
			Throw,
			Thrown1,
			Thrown2,
			Thrown3,
			Thrown4,
			Thrown5,
			Thrown6,
			Special1,
			Special2,
			Special3,
			Special4,
			ThrowForwardPunch,
			ThrowForwardKick,
			ThrowBackwardPunch,
			ThrowBackwardKick,
			RunThrowForwardPunch,
			RunThrowBackwardPunch,
			RunThrowForwardKick,
			RunThrowBackwardKick,
			Thrown7,
			Thrown8,
			Thrown9,
			Thrown10,
			Thrown11,
			Thrown12,
			StartleLeft,
			StartleRight,
			Sit,
			StandSpecial,
			Act,
			Kick3Fw,
			HitFootOuch,
			HitJewels,
			Thrown13,
			Thrown14,
			Thrown15,
			Thrown16,
			Thrown17,
			PPKK,
			PPKKK,
			PPKKKK,
			LandHard,
			LandHardForward,
			LandHardRight,
			LandHardLeft,
			LandHardBack,
			LandDead,
			CrouchTurnLeft,
			CrouchTurnRight,
			CrouchForward,
			CrouchBack,
			CrouchLeft,
			CrouchRight,
			GetupKickBack,
			AutopistolRecoil,
			PhaseRifleRecoil,
			PhaseStreamRecoil,
			SuperballRecoil,
			VandegrafRecoil,
			ScramCannonRecoil,
			MercuryBowRecoil,
			ScreamerRecoil,
			PickupObject,
			PickupPistol,
			PickupRifle,
			Holster,
			DrawPistol,
			DrawRifle,
			Punch4,
			ReloadPistol,
			ReloadPhaseRifle,
			ReloadPhaseStream,
			ReloadSuperball,
			ReloadVandegraf,
			ReloadScramCannon,
			ReloadMercuryBow,
			ReloadScreamer,
			PfPf,
			PfPfPf,
			PlPl,
			PlPlPl,
			PrPr,
			PrPrPr,
			PbPb,
			PbPbPb,
			PdPd,
			PdPdPd,
			KfKf,
			KfKfKf,
			KlKl,
			KlKlKl,
			KrKr,
			KrKrKr,
			KbKb,
			KbKbKb,
			KdKd,
			KdKdKd,
			StartleLt,
			StartleRt,
			StartleBk,
			StartleFw,
			Console,
			ConsoleWalk,
			Stagger,
			Watch,
			ActNo,
			ActYes,
			ActTalk,
			ActShrug,
			ActShout,
			ActGive,
			RunStop,
			WalkStop,
			RunStart,
			WalkStart,
			RunBackwardsStart,
			WalkBackwardsStart,
			Stun,
			StaggerBehind,
			Blownup,
			BlownupBehind,
			OneStepStop,
			RunSidestepLeftStart,
			RunSidestepRightStart,
			Powerup,
			FallingFlail,
			ConsolePunch,
			TeleportIn,
			TeleportOut,
			NinjaFireball,
			NinjaInvisible,
			PunchRifle,
			PickupObjectMid,
			PickupPistolMid,
			PickupRifleMid,
			Hail,
			MuroThunderbolt,
			HitOverlayAI
		}

		[Flags]
		public enum TRAMFootstepType
		{
			None = 0,
			Left = 1,
			Right = 2
		}

		[Flags]
		public enum TRAMAttackFlags
		{
			None = 0,
			Unblockable = 1,
			High = 2,
			Low = 4,
			HalfDamage = 8
		}

		[Flags]
		public enum TRAMVarient
		{
			None = 0,
			Sprint = 0x100,
			Combat = 0x200,
			RightPistol = 0x800,
			LeftPistol = 0x1000,
			RightRifle = 0x2000,
			LeftRifle = 0x4000,
			Panic = 0x8000
		}

		[Flags]
		public enum TRAMBoneFlags : uint
		{
			None = 0u,
			Pelvis = 1u,
			LeftThigh = 2u,
			LeftCalf = 4u,
			LeftFoot = 8u,
			RightThigh = 0x10u,
			RightCalf = 0x20u,
			RightFoot = 0x40u,
			Mid = 0x80u,
			Chest = 0x100u,
			Neck = 0x200u,
			Head = 0x400u,
			LeftShoulder = 0x800u,
			LeftArm = 0x1000u,
			LeftWrist = 0x2000u,
			LeftFist = 0x4000u,
			RightShoulder = 0x8000u,
			RightArm = 0x10000u,
			RightWrist = 0x20000u,
			RightFist = 0x40000u
		}

		public enum TRAMBone
		{
			Pelvis,
			LeftThigh,
			LeftCalf,
			LeftFoot,
			RightThigh,
			RightCalf,
			RightFoot,
			Mid,
			Chest,
			Neck,
			Head,
			LeftShoulder,
			LeftArm,
			LeftWrist,
			LeftFist,
			RightShoulder,
			RightArm,
			RightWrist,
			RightFist
		}

		public enum TRAMDirection
		{
			None,
			Forward,
			Backward,
			Left,
			Right
		}

		[Flags]
		public enum TRAMFlags
		{
			RuntimeLoaded = 1,
			Invulnerable = 2,
			BlockHigh = 4,
			BlockLow = 8,
			Attack = 0x10,
			DropWeapon = 0x20,
			InAir = 0x40,
			Atomic = 0x80,
			NoTurn = 0x100,
			AttackForward = 0x200,
			AttackLeft = 0x400,
			AttackRight = 0x800,
			AttackBackward = 0x1000,
			Overlay = 0x2000,
			DontInterpolateVelocity = 0x4000,
			ThrowSource = 0x8000,
			ThrowTarget = 0x10000,
			RealWorld = 0x20000,
			DoAim = 0x40000,
			DontAim = 0x80000,
			CanPickup = 0x100000,
			Aim360 = 0x200000,
			DisableShield = 0x400000,
			NoAIPickup = 0x800000
		}

		public enum TRAMState
		{
			None,
			Anything,
			RunningLeftDown,
			RunningRightDown,
			Sliding,
			WalkingLeftDown,
			WalkingRightDown,
			Standing,
			RunStart,
			RunAccel,
			RunSidestepLeft,
			RunSidestepRight,
			RunSlide,
			RunJump,
			RunJumpLand,
			RunBackStart,
			RunningBackRightDown,
			RunningBackLeftDown,
			FallenBack,
			Crouch,
			RunningUpstairRightDown,
			RunningUpstairLeftDown,
			SidestepLeftLeftDown,
			SidestepLeftRightDown,
			SidestepRightLeftDown,
			SidestepRightRightDown,
			SidestepRightJump,
			SidestepLeftJump,
			JumpForward,
			JumpUp,
			RunBackSlide,
			LieBack,
			SsLtStart,
			SsRtStart,
			WalkingSidestepLeft,
			CrouchWalk,
			WalkingSidestepRight,
			Flying,
			Falling,
			FlyingForward,
			FallingForward,
			FlyingBack,
			FallingBack,
			FlyingLeft,
			FallingLeft,
			FlyingRight,
			FallingRight,
			CrouchStart,
			WalkingBackLeftDown,
			WalkingBackRightDown,
			FallenFront,
			SidestepLeftStart,
			SidestepRightStart,
			Sit,
			PunchLow,
			StandSpecial,
			Acting,
			CrouchRunLeft,
			CrouchRunRight,
			CrouchRunBackLeft,
			CrouchRunBackRight,
			Blocking1,
			Blocking2,
			Blocking3,
			CrouchBlocking1,
			Gliding,
			WatchIdle,
			Stunned,
			Powerup,
			Thunderbolt
		}

		[Flags]
		private enum WMDDState
		{
			None = 0,
			Visible = 1,
			Disabled = 2,
			State04 = 4
		}

		[Flags]
		private enum WMDDStyle : uint
		{
			None = 0u,
			ThinBorder = 1u,
			ThickBorder = 2u,
			TitleBar = 4u,
			Title = 8u,
			CloseButton = 0x10u,
			RestoreButton = 0x20u,
			MinimizeButton = 0x40u,
			Center = 0x10000u
		}

		private enum WMDDControlFontStyle : uint
		{
			Normal,
			Bold,
			Italic
		}

		private enum WMDDControlClass : ushort
		{
			Desktop = 1,
			Title = 3,
			Button = 4,
			Checkbox = 5,
			Dialog = 6,
			Textbox = 7,
			Listbox = 8,
			MenuBar = 9,
			Menu = 10,
			Image = 11,
			Dropdown = 12,
			ProgressBar = 13,
			RadioButton = 14,
			Slider = 17,
			Label = 20
		}

		private enum WMM_MenuItemType : ushort
		{
			Separator = 1,
			Option
		}

		[Flags]
		public enum ONWCFlags : uint
		{
			None = 0u,
			NoHolster = 2u,
			UsesCells = 4u,
			TwoHanded = 8u,
			RecoilAffectsAiming = 0x10u,
			Automatic = 0x20u,
			StunSwitcher = 0x80u,
			KnockdownSwitcher = 0x100u,
			Explosive = 0x200u,
			SecondaryFire = 0x400u,
			BarabbasWeapon = 0x800u,
			Heavy = 0x1000u,
			AutoRelease = 0x2000u,
			HasReleaseDelay = 0x4000u,
			HasLaserSight = 0x8000u,
			ScaleCrosshair = 0x20000u,
			NoFade = 0x80000u,
			DrainAmmo = 0x100000u
		}

		public enum TXMPFormat : uint
		{
			BGRA4444 = 0u,
			BGR555 = 1u,
			BGRA5551 = 2u,
			RGBA = 7u,
			BGR = 8u,
			DXT1 = 9u
		}

		[Flags]
		public enum TXMPFlags : uint
		{
			None = 0u,
			HasMipMaps = 1u,
			DisableUWrap = 4u,
			DisableVWrap = 8u,
			Unknown0010 = 0x10u,
			AnimBackToBack = 0x40u,
			AnimRandom = 0x80u,
			AnimUseLocalTime = 0x100u,
			HasEnvMap = 0x200u,
			AdditiveBlend = 0x400u,
			SwapBytes = 0x1000u,
			AnimIgnoreGlobalTime = 0x4000u,
			ShieldEffect = 0x8000u,
			InvisibilityEffect = 0x10000u,
			DaodanEffect = 0x20000u
		}

		private static readonly MetaStruct aiSoundConstants = new MetaStruct("AISoundConstants", new Field(MetaType.Byte, "TauntProbability"), new Field(MetaType.Byte, "AlertProbability"), new Field(MetaType.Byte, "StartleProbability"), new Field(MetaType.Byte, "CheckBodyProbability"), new Field(MetaType.Byte, "PursueProbability"), new Field(MetaType.Byte, "CoverProbability"), new Field(MetaType.Byte, "SuperPunchProbability"), new Field(MetaType.Byte, "SuperKickProbability"), new Field(MetaType.Byte, "Super3Probability"), new Field(MetaType.Byte, "Super4Probability"), new Field(MetaType.Padding(2)), new Field(MetaType.String32, "TauntSound"), new Field(MetaType.String32, "AlertSound"), new Field(MetaType.String32, "StartleSound"), new Field(MetaType.String32, "CheckBodySound"), new Field(MetaType.String32, "PursueSound"), new Field(MetaType.String32, "CoverSound"), new Field(MetaType.String32, "SuperPunchSound"), new Field(MetaType.String32, "SuperKickSound"), new Field(MetaType.String32, "Super3Sound"), new Field(MetaType.String32, "Super4Sound"));

		private static readonly MetaStruct aiVisionConstants = new MetaStruct("AIVisionConstants", new Field(MetaType.Float, "CentralDistance"), new Field(MetaType.Float, "PeripheralDistance"), new Field(MetaType.Float, "VerticalRange"), new Field(MetaType.Float, "CentralRange"), new Field(MetaType.Float, "CentralMax"), new Field(MetaType.Float, "PeripheralRange"), new Field(MetaType.Float, "PeripheralMax"));

		private static readonly MetaStruct aiTargeting = new MetaStruct("AITargeting", new Field(MetaType.Float, "StartleMissAngle"), new Field(MetaType.Float, "StartleMissDistance"), new Field(MetaType.Float, "PredictAmount"), new Field(MetaType.Int32, "PredictPositionDelayFrames"), new Field(MetaType.Int32, "PredictDelayFrames"), new Field(MetaType.Int32, "PredictVelocityFrames"), new Field(MetaType.Int32, "PredictTrendFrames"));

		private static readonly MetaType aiWeaponSkill = new MetaStruct("AIWeaponSkill", new Field(MetaType.Float, "RecoilCompensation"), new Field(MetaType.Float, "BestAimingAngle"), new Field(MetaType.Float, "ShotGroupError"), new Field(MetaType.Float, "ShotGroupDecay"), new Field(MetaType.Float, "ShootingInaccuracyMultiplier"), new Field(MetaType.Int16, "MinShotDelay"), new Field(MetaType.Int16, "MaxShotDelay"));

		private static readonly MetaStruct aiFiringMode = new MetaStruct("AIFiringMode", new Field(MetaType.Enum<AIFiringModeFlags>(), "Flags"), new Field(MetaType.Matrix4x3, "InverseDirection"), new Field(MetaType.Vector3, "Direction"), new Field(MetaType.Vector3, "Origin"), new Field(MetaType.Float, "PredictionSpeed"), new Field(MetaType.Float, "MaxInaccuracyAngle"), new Field(MetaType.Float, "AimRadius"), new Field(MetaType.Float, "AISoundRadius"), new Field(MetaType.Float, "MinShootingDistance"), new Field(MetaType.Float, "MaxShootingDistance"), new Field(MetaType.Int16, "MaxStartleMisses"), new Field(MetaType.Int16, "SkillIndex"), new Field(MetaType.Int32, "FightTimer"), new Field(MetaType.Float, "ProjectileSpeed"), new Field(MetaType.Float, "ProjectileGravity"), new Field(MetaType.Float, "FireSpreadLength"), new Field(MetaType.Float, "FireSpreadWidth"), new Field(MetaType.Float, "FireSpreadSkew"));

		private static readonly MetaStruct aisa = new MetaStruct("AISAInstance", new Field(MetaType.Padding(22)), new Field(MetaType.ShortVarArray(new MetaStruct("AISACharacter", new Field(MetaType.String32, "Name"), new Field(MetaType.Int16, "ScriptId"), new Field(MetaType.Int16, "FlagId"), new Field(MetaType.Enum<AISACharacterFlags>(), "Flags"), new Field(MetaType.Enum<AISACharacterTeam>(), "Team"), new Field(MetaType.Pointer(TemplateTag.ONCC), "Class"), new Field(MetaType.Padding(36)), new Field(new MetaStruct("CharacterScripts", new Field(MetaType.String32, "Spawn"), new Field(MetaType.String32, "Die"), new Field(MetaType.String32, "Combat"), new Field(MetaType.String32, "Alarm"), new Field(MetaType.String32, "Hurt"), new Field(MetaType.String32, "Defeated"), new Field(MetaType.String32, "OutOfAmmo"), new Field(MetaType.String32, "NoPath")), "Scripts"), new Field(MetaType.Pointer(TemplateTag.ONWC), "WeaponClass"), new Field(MetaType.Int16, "Ammo"), new Field(MetaType.Padding(10)))), "Characters"));

		private static readonly MetaStruct akaa = new MetaStruct("AKAAInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(new MetaStruct("AKAAElement", new Field(MetaType.Int32, "Bnv"), new Field(MetaType.Int32, "Quad"), new Field(MetaType.Padding(4)))), "Elements"));

		private static readonly MetaStruct abna = new MetaStruct("ABNAInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(new MetaStruct("ABNAElement", new Field(MetaType.Int32, "Quad"), new Field(MetaType.Int32, "Plane"), new Field(MetaType.Int32, "Front"), new Field(MetaType.Int32, "Back"))), "Elements"));

		private static readonly MetaStruct akva = new MetaStruct("AKVAInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(new MetaStruct("AKVANode", new Field(MetaType.Int32, "BspTree"), new Field(MetaType.Int32, "Id"), new Field(MetaType.Int32, "FirstSide"), new Field(MetaType.Int32, "LastSide"), new Field(MetaType.Int32, "ChildBnv"), new Field(MetaType.Int32, "SiblingBnv"), new Field(MetaType.Padding(4, byte.MaxValue)), new Field(MetaType.Int32, "GridXTiles"), new Field(MetaType.Int32, "GridZTiles"), new BinaryPartField(MetaType.RawOffset, "DataOffset", "DataSize"), new Field(MetaType.Int32, "DataSize"), new Field(MetaType.Float, "TileSize"), new Field(MetaType.BoundingBox, "BoundingBox"), new Field(MetaType.Int16, "GridXOffset"), new Field(MetaType.Int16, "GridZOffset"), new Field(MetaType.Int32, "NodeId"), new Field(MetaType.Padding(12)), new Field(MetaType.Int32, "Flags"), new Field(MetaType.Plane, "Floor"), new Field(MetaType.Float, "Height"))), "Nodes"));

		private static readonly MetaStruct akba = new MetaStruct("AKBAInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(new MetaStruct("AKBASide", new Field(MetaType.Int32, "Plane"), new Field(MetaType.Int32, "FirstAdjacency"), new Field(MetaType.Int32, "LastAdjacency"), new Field(MetaType.Padding(16)))), "Sides"));

		private static readonly MetaStruct akbp = new MetaStruct("AKBPInstance", new Field(MetaType.Padding(22)), new Field(MetaType.ShortVarArray(new MetaStruct("AKBPNode", new Field(MetaType.Int32, "Plane"), new Field(MetaType.Int32, "Back"), new Field(MetaType.Int32, "Front"))), "Nodes"));

		private static readonly MetaStruct akev = new MetaStruct("AKEVInstance", new Field(MetaType.Pointer(TemplateTag.PNTA), "Points"), new Field(MetaType.Pointer(TemplateTag.PLEA), "Planes"), new Field(MetaType.Pointer(TemplateTag.TXCA), "TextureCoordinates"), new Field(MetaType.Pointer(TemplateTag.AGQG), "Quads"), new Field(MetaType.Pointer(TemplateTag.AGQR), "QuadTextures"), new Field(MetaType.Pointer(TemplateTag.AGQC), "QuadCollision"), new Field(MetaType.Pointer(TemplateTag.AGDB), "Debug"), new Field(MetaType.Pointer(TemplateTag.TXMA), "Textures"), new Field(MetaType.Pointer(TemplateTag.AKVA), "BnvNodes"), new Field(MetaType.Pointer(TemplateTag.AKBA), "BnvSides"), new Field(MetaType.Pointer(TemplateTag.IDXA), "QuadGroupList"), new Field(MetaType.Pointer(TemplateTag.IDXA), "QuadGroupId"), new Field(MetaType.Pointer(TemplateTag.AKBP), "BnvBspTree"), new Field(MetaType.Pointer(TemplateTag.ABNA), "TransparencyBspTree"), new Field(MetaType.Pointer(TemplateTag.AKOT), "Octtree"), new Field(MetaType.Pointer(TemplateTag.AKAA), "BnvAdjacency"), new Field(MetaType.Pointer(TemplateTag.AKDA), "DoorFrames"), new Field(MetaType.BoundingBox, "BoundingBox"), new Field(MetaType.Padding(24)), new Field(MetaType.Float, ""));

		private static readonly MetaStruct agqc = new MetaStruct("AGQCInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(new MetaStruct("AGQCElement", new Field(MetaType.Int32, "Plane"), new Field(MetaType.BoundingBox, "BoundingBox"))), "Elements"));

		private static readonly MetaStruct agqg = new MetaStruct("AGQGInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(new MetaStruct("AGQGQuad", new Field(MetaType.Array(4, MetaType.Int32), "Points"), new Field(MetaType.Array(4, MetaType.Int32), "TextureCoordinates"), new Field(MetaType.Array(4, MetaType.Color), "Colors"), new Field(MetaType.Enum<AGQGFlags>(), "Flags"), new Field(MetaType.Int32, "ObjectId"))), "Quads"));

		private static readonly MetaStruct agqr = new MetaStruct("AGQRInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(new MetaStruct("AGQRElement", new Field(MetaType.UInt16, "Texture"), new Field(MetaType.Padding(2)))), "Elements"));

		private static readonly MetaStruct akot = new MetaStruct("AKOTInstance", new Field(MetaType.Pointer(TemplateTag.OTIT), "Nodes"), new Field(MetaType.Pointer(TemplateTag.OTLF), "Leafs"), new Field(MetaType.Pointer(TemplateTag.QTNA), "QuadTree"), new Field(MetaType.Pointer(TemplateTag.IDXA), "GunkQuad"), new Field(MetaType.Pointer(TemplateTag.IDXA), "Bnv"));

		private static readonly MetaStruct otit = new MetaStruct("OTITInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(new MetaStruct("OTITNode", new Field(MetaType.Array(8, MetaType.Int32), "Children"))), "Nodes"));

		private static readonly MetaStruct otlf = new MetaStruct("OTLFInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(new MetaStruct("OTLFNode", new Field(MetaType.Int32, "PackedGunkQuadList"), new Field(MetaType.Array(6, MetaType.Int32), "Neighbours"), new Field(MetaType.Int32, "PackedPositionAndSize"), new Field(MetaType.Int32, "PackedBnvList"))), "Nodes"));

		private static readonly MetaStruct qtna = new MetaStruct("QTNAInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(new MetaStruct("QTNANode", new Field(MetaType.Array(4, MetaType.Int32), "Children"))), "Nodes"));

		private static readonly MetaStruct envp = new MetaStruct("ENVPInstance", new Field(MetaType.Padding(22)), new Field(MetaType.ShortVarArray(new MetaStruct("ENVPParticle", new Field(MetaType.String64, "Class"), new Field(MetaType.String48, "Tag"), new Field(MetaType.Matrix4x3, "Transform"), new Field(MetaType.Vector2, "DecalScale"), new Field(MetaType.Enum<ENVPFlags>(), "Flags"), new Field(MetaType.Padding(38)))), "Particles"));

		private static readonly MetaStruct m3gm = new MetaStruct("M3GMInstance", new Field(MetaType.Padding(4)), new Field(MetaType.Pointer(TemplateTag.PNTA), "Points"), new Field(MetaType.Pointer(TemplateTag.VCRA), "VertexNormals"), new Field(MetaType.Pointer(TemplateTag.VCRA), "FaceNormals"), new Field(MetaType.Pointer(TemplateTag.TXCA), "TextureCoordinates"), new Field(MetaType.Pointer(TemplateTag.IDXA), "TriangleStrips"), new Field(MetaType.Pointer(TemplateTag.IDXA), "FaceNormalIndices"), new Field(MetaType.Pointer(TemplateTag.TXMP), "Texture"), new Field(MetaType.Padding(4)));

		private static readonly MetaStruct m3ga = new MetaStruct("M3GAInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(MetaType.Pointer(TemplateTag.M3GM)), "Geometries"));

		private static readonly MetaStruct plea = new MetaStruct("PLEAInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(MetaType.Plane), "Planes"));

		private static readonly MetaStruct pnta = new MetaStruct("PNTAInstance", new Field(MetaType.Padding(12)), new Field(MetaType.BoundingBox, "BoundingBox"), new Field(MetaType.BoundingSphere, "BoundingSphere"), new Field(MetaType.VarArray(MetaType.Vector3), "Positions"));

		private static readonly MetaStruct txca = new MetaStruct("TXCAInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(MetaType.Vector2), "TexCoords"));

		private static readonly MetaStruct txan = new MetaStruct("TXANInstance", new Field(MetaType.Padding(12)), new Field(MetaType.Int16, "Speed"), new Field(MetaType.Int16, ""), new Field(MetaType.Padding(4)), new Field(MetaType.VarArray(MetaType.Pointer(TemplateTag.TXMP)), "Textures"));

		private static readonly MetaStruct txma = new MetaStruct("TXMAInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(MetaType.Pointer(TemplateTag.TXMP)), "Textures"));

		private static readonly MetaStruct txmb = new MetaStruct("TXMBInstance", new Field(MetaType.Padding(8)), new Field(MetaType.Int16, "Width"), new Field(MetaType.Int16, "Height"), new Field(MetaType.Padding(8)), new Field(MetaType.VarArray(MetaType.Pointer(TemplateTag.TXMP)), "Textures"));

		private static readonly MetaStruct vcra = new MetaStruct("VCRAInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(MetaType.Vector3), "Normals"));

		private static readonly MetaStruct impt = new MetaStruct("ImptInstance", new Field(MetaType.Padding(2, byte.MaxValue)), new Field(MetaType.Padding(6)), new Field(MetaType.Pointer(TemplateTag.Impt), "ParentImpact"));

		private static readonly MetaStruct mtrl = new MetaStruct("MtrlInstance", new Field(MetaType.Padding(2, byte.MaxValue)), new Field(MetaType.Padding(6)), new Field(MetaType.Pointer(TemplateTag.Mtrl), "ParentMaterial"));

		private static readonly MetaStruct cons = new MetaStruct("CONSInstance", new Field(MetaType.Enum<CONSFlags>(), "Flags"), new Field(MetaType.Vector3, "Position"), new Field(MetaType.Vector3, "Orientation"), new Field(MetaType.Pointer(TemplateTag.OFGA), "ConsoleGeometry"), new Field(MetaType.Pointer(TemplateTag.M3GM), "ScreenGeometry"), new Field(MetaType.Enum<AGQGFlags>(), "ScreenGunkFlags"), new Field(MetaType.String32, "InactiveTexture"), new Field(MetaType.String32, "ActiveTexture"), new Field(MetaType.String32, "UsedTexture"));

		private static readonly MetaStruct door = new MetaStruct("DOORInstance", new Field(MetaType.Array(2, MetaType.Pointer(TemplateTag.OFGA)), "Geometries"), new Field(MetaType.Pointer(TemplateTag.OBAN), "Animation"), new Field(MetaType.Float, "AISoundAttenuation"), new Field(MetaType.Enum<DOORSoundAllow>(), "AISoundAllow"), new Field(MetaType.Enum<DOORSoundType>(), "AISoundType"), new Field(MetaType.Float, "AISoundDistance"), new Field(MetaType.String32, "OpenSound"), new Field(MetaType.String32, "CloseSound"), new Field(MetaType.Padding(8)));

		private static readonly MetaStruct ofga = new MetaStruct("OFGAInstance", new Field(MetaType.Padding(16)), new Field(MetaType.Pointer(TemplateTag.ENVP), "EnvParticle"), new Field(MetaType.VarArray(new MetaStruct("OFGAElement", new Field(MetaType.Enum<AGQGFlags>(), "GunkFlags"), new Field(MetaType.Pointer(TemplateTag.M3GM), "Geometry"), new Field(MetaType.Padding(4)))), "Elements"));

		private static readonly MetaStruct obls = new MetaStruct("OBLSInstance", new Field(MetaType.Int32, "Type"), new Field(MetaType.Int32, "Flags"), new Field(MetaType.Vector3, "Color"), new Field(MetaType.Float, "Intensity"), new Field(MetaType.Float, "BeamAngle"), new Field(MetaType.Float, "FieldAngle"));

		private static readonly MetaStruct trig = new MetaStruct("TRIGInstance", new Field(MetaType.Color, "Color"), new Field(MetaType.UInt16, "TimeOn"), new Field(MetaType.UInt16, "TimeOff"), new Field(MetaType.Float, "StartOffset"), new Field(MetaType.Float, "AnimScale"), new Field(MetaType.Pointer(TemplateTag.M3GM), "BaseGeometry"), new Field(MetaType.Padding(4)), new Field(MetaType.Enum<AGQGFlags>(), "BaseGunkFlags"), new Field(MetaType.Pointer(TemplateTag.TRGE), "Emitter"), new Field(MetaType.Pointer(TemplateTag.OBAN), "Animation"), new Field(MetaType.String32, "ActiveSound"), new Field(MetaType.String32, "TriggerSound"), new Field(MetaType.Padding(8)));

		private static readonly MetaStruct trge = new MetaStruct("TRGEInstance", new Field(MetaType.Vector3, "Position"), new Field(MetaType.Vector3, "Direction"), new Field(MetaType.Pointer(TemplateTag.M3GM), "Geometry"), new Field(MetaType.Enum<AGQGFlags>(), "GunkFlags"));

		private static readonly MetaStruct turr = new MetaStruct("TURRInstance", new Field(MetaType.String32, "Name"), new Field(MetaType.Padding(32)), new Field(MetaType.Padding(14)), new Field(MetaType.Int16, "ParticleCount"), new Field(MetaType.Int16, ""), new Field(MetaType.Padding(6)), new Field(MetaType.Pointer(TemplateTag.M3GM), "BaseGeometry"), new Field(MetaType.Padding(4)), new Field(MetaType.Enum<AGQGFlags>(), "BaseGunkFlags"), new Field(MetaType.Pointer(TemplateTag.M3GM), "ArmGeometry"), new Field(MetaType.Enum<AGQGFlags>(), "ArmGunkFlags"), new Field(MetaType.Pointer(TemplateTag.M3GM), "WeaponGeometry"), new Field(MetaType.Enum<AGQGFlags>(), "WeaponGunkFlags"), new Field(MetaType.Vector3, "ArmTranslation"), new Field(MetaType.Vector3, "WeaponTranslation"), new Field(MetaType.Array(16, new MetaStruct("TURRParticle", new Field(MetaType.String16, "ParticleClass"), new Field(MetaType.Padding(4)), new Field(MetaType.Int16, "ShotFrequency"), new Field(MetaType.Int16, "FiringModeOwner"), new Field(MetaType.Matrix4x3, "Transform"), new Field(MetaType.Padding(4)))), "Particles"), new Field(aiFiringMode, "FiringMode"), new Field(aiTargeting, "Targeting"), new Field(aiWeaponSkill, "WeaponSkill"), new Field(MetaType.Int32, "Timeout"), new Field(MetaType.Float, "MinElevation"), new Field(MetaType.Float, "MaxElevation"), new Field(MetaType.Float, "MinAzimuth"), new Field(MetaType.Float, "MaxAzimuth"), new Field(MetaType.Float, "MaxVerticalSpeed"), new Field(MetaType.Float, "MaxHorizontalSpeed"), new Field(MetaType.String32, "ActiveSound"), new Field(MetaType.Padding(4)));

		private static readonly MetaStruct oban = new MetaStruct("OBANInstance", new Field(MetaType.Padding(12)), new Field(MetaType.Enum<OBANFlags>(), "Flags"), new Field(MetaType.Matrix4x3, "InitialTransform"), new Field(MetaType.Matrix4x3, "BaseTransform"), new Field(MetaType.Int16, "FrameLength"), new Field(MetaType.Int16, "FrameCount"), new Field(MetaType.Int16, "HalfStopFrame"), new Field(MetaType.ShortVarArray(new MetaStruct("OBANKeyFrame", new Field(MetaType.Quaternion, "Rotation"), new Field(MetaType.Vector3, "Translation"), new Field(MetaType.Int32, "Time"))), "KeyFrames"));

		private static readonly MetaStruct oboa = new MetaStruct("OBOAInstance", new Field(MetaType.Padding(22)), new Field(MetaType.ShortVarArray(new MetaStruct("OBOAObject", new Field(MetaType.Pointer(TemplateTag.M3GA), "Geometry"), new Field(MetaType.Pointer(TemplateTag.OBAN), "Animation"), new Field(MetaType.Pointer(TemplateTag.ENVP), "Particle"), new Field(MetaType.Enum<OBOAFlags>(), "Flags"), new Field(MetaType.Int32, "DoorGunkId"), new Field(MetaType.Int32, "DoorId"), new Field(MetaType.Enum<OBOAPhysics>(), "PhysicsType"), new Field(MetaType.Int32, "ScriptId"), new Field(MetaType.Vector3, "Position"), new Field(MetaType.Quaternion, "Rotation"), new Field(MetaType.Float, "Scale"), new Field(MetaType.Matrix4x3, "Transform"), new Field(MetaType.String64, "Name"), new Field(MetaType.Padding(64)))), "Objects"));

		private static readonly MetaStruct cbpi = new MetaStruct("CBPIInstance", new Field(MetaType.Array(19, MetaType.Pointer(TemplateTag.Impt)), "HitImpacts"), new Field(MetaType.Array(19, MetaType.Pointer(TemplateTag.Impt)), "BlockedImpacts"), new Field(MetaType.Array(19, MetaType.Pointer(TemplateTag.Impt)), "KilledImpacts"));

		private static readonly MetaStruct cbpm = new MetaStruct("CBPMInstance", new Field(MetaType.Array(19, MetaType.Pointer(TemplateTag.Mtrl)), "Materials"));

		private static readonly MetaStruct oncc = new MetaStruct("ONCCInstance", new Field(new MetaStruct("ONCCAirConstants", new Field(MetaType.Float, "FallGravity"), new Field(MetaType.Float, "JumpGravity"), new Field(MetaType.Float, "JumpStartVelocity"), new Field(MetaType.Float, "MaxVelocity"), new Field(MetaType.Float, "JetpackAcceleration"), new Field(MetaType.UInt16, "FramesFallGravity"), new Field(MetaType.UInt16, "JetpackTimer"), new Field(MetaType.Float, "MaxNoDamageFallingHeight"), new Field(MetaType.Float, "MaxDamageFallingHeight")), "AirConstants"), new Field(new MetaStruct("ONCCShadowConstants", new Field(MetaType.Pointer(TemplateTag.TXMP), "Texture"), new Field(MetaType.Float, "MaxHeight"), new Field(MetaType.Float, "FadeHeight"), new Field(MetaType.Float, "SizeMax"), new Field(MetaType.Float, "SizeFade"), new Field(MetaType.Float, "SizeMin"), new Field(MetaType.Int16, "AlphaMax"), new Field(MetaType.Int16, "AlphaFade")), "ShadowConstants"), new Field(new MetaStruct("ONCCJumpConstants", new Field(MetaType.Float, "JumpDistance"), new Field(MetaType.Byte, "JumpHeight"), new Field(MetaType.Byte, "JumpDistanceSquares"), new Field(MetaType.Padding(2))), "JumpConstants"), new Field(new MetaStruct("ONCCCoverConstants", new Field(MetaType.Float, "RayIncrement"), new Field(MetaType.Float, "RayMax"), new Field(MetaType.Float, "RayAngle"), new Field(MetaType.Float, "RayAngleMax")), "CoverConstants"), new Field(new MetaStruct("ONCCAutoFreezeConstants", new Field(MetaType.Float, "DistanceXZ"), new Field(MetaType.Float, "DistanceY")), "AutoFreezeConstants"), new Field(new MetaStruct("ONCCInventoryConstants", new Field(MetaType.Int16, "HypoRegenerationRate"), new Field(MetaType.Padding(2))), "InventoryConstants"), new Field(MetaType.Array(5, MetaType.Float), "LODConstants"), new Field(new MetaStruct("ONCCHurtSoundConstants", new Field(MetaType.Int16, "BasePercentage"), new Field(MetaType.Int16, "MaxPercentage"), new Field(MetaType.Int16, "PercentageThreshold"), new Field(MetaType.Int16, "Timer"), new Field(MetaType.Int16, "MinTimer"), new Field(MetaType.Int16, "MaxLight"), new Field(MetaType.Int16, "MaxMedium"), new Field(MetaType.Int16, "DeathChance"), new Field(MetaType.Int16, "VolumeTreshold"), new Field(MetaType.Int16, "MediumTreshold"), new Field(MetaType.Int16, "HeavyTreshold"), new Field(MetaType.Padding(2)), new Field(MetaType.Float, "MinVolume"), new Field(MetaType.String32, "LightSound"), new Field(MetaType.String32, "MediumSound"), new Field(MetaType.String32, "HeavySound"), new Field(MetaType.String32, "DeathSound"), new Field(MetaType.Padding(16))), "HurtSoundConstants"), new Field(new MetaStruct("ONCCAIConstants", new Field(MetaType.Enum<AICharacterFlags>(), "Flags"), new Field(MetaType.Float, "RotationSpeed"), new Field(MetaType.UInt16, "DazedMinFrames"), new Field(MetaType.UInt16, "DazedMaxFrames"), new Field(MetaType.Int32, "DodgeReactFrames"), new Field(MetaType.Float, "DodgeTimeScale"), new Field(MetaType.Float, "DodgeWeightScale"), new Field(aiTargeting, "Targeting"), new Field(MetaType.Array(13, aiWeaponSkill), "WeaponSkills"), new Field(MetaType.Int32, "DeadMakeSureDelay"), new Field(MetaType.Int32, "InvestigateBodyDelay"), new Field(MetaType.Int32, "LostContactDelay"), new Field(MetaType.Int32, "DeadTauntChance"), new Field(MetaType.Int32, "GoForGunChance"), new Field(MetaType.Int32, "RunPickupChance"), new Field(MetaType.Int16, "CombatId"), new Field(MetaType.Int16, "MeleeId"), new Field(aiSoundConstants, "SoundConstants"), new Field(aiVisionConstants, "VisionConstants"), new Field(MetaType.Int32, "HostileThreatDefiniteTimer"), new Field(MetaType.Int32, "HostileThreatStrongTimer"), new Field(MetaType.Int32, "HostileThreatWeakTimer"), new Field(MetaType.Int32, "FriendlyThreatDefiniteTimer"), new Field(MetaType.Int32, "FriendlyThreatStrongTimer"), new Field(MetaType.Int32, "FriendlyThreatWeakTimer"), new Field(MetaType.Float, "EarshotRadius")), "AIConstants"), new Field(MetaType.Pointer(TemplateTag.ONCV), "Variant"), new Field(MetaType.Pointer(TemplateTag.ONCP), "Particles"), new Field(MetaType.Pointer(TemplateTag.ONIA), "Impacts"), new Field(MetaType.Padding(4)), new Field(MetaType.String16, "ImpactModifierName"), new Field(MetaType.Array(15, new MetaStruct("ONCCImpact", new Field(MetaType.String128, "Name"), new Field(MetaType.Padding(2, byte.MaxValue)))), "Impacts"), new Field(MetaType.Padding(2)), new Field(MetaType.String64, "DeathParticle"), new Field(MetaType.Padding(8)), new Field(MetaType.Pointer(TemplateTag.TRBS), "BodySet"), new Field(MetaType.Pointer(TemplateTag.TRMA), "BodyTextures"), new Field(MetaType.Pointer(TemplateTag.CBPM), "BodyMaterials"), new Field(MetaType.Pointer(TemplateTag.CBPI), "BodyImpacts"), new Field(MetaType.Int32, "FightModeTimer"), new Field(MetaType.Int32, "IdleAnimation1Timer"), new Field(MetaType.Int32, "IdleAnimation2Timer"), new Field(MetaType.Int32, "Health"), new Field(MetaType.Enum<TRAMBoneFlags>(), "FeetBones"), new Field(MetaType.Float, "MinBodySizeFactor"), new Field(MetaType.Float, "MaxBodySizeFactor"), new Field(MetaType.Array(7, MetaType.Float), "DamageFactors"), new Field(MetaType.Float, "BossShieldProtectAmount"), new Field(MetaType.Pointer(TemplateTag.TRAC), "Animations"), new Field(MetaType.Pointer(TemplateTag.TRSC), "AimingScreens"), new Field(MetaType.UInt16, "AIRateOfFire"), new Field(MetaType.UInt16, "DeathDeleteDelay"), new Field(MetaType.Byte, "WeaponHand"), new Field(MetaType.Byte, "HasDaodanPowers"), new Field(MetaType.Byte, "HasSupershield"), new Field(MetaType.Byte, "CantTouchThis"));

		private static readonly MetaStruct onia = new MetaStruct("ONIAInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(new MetaStruct("ONIAImpact", new Field(MetaType.String16, "Name"), new Field(MetaType.String128, "Type"), new Field(MetaType.String16, "Modifier"), new Field(MetaType.Padding(2, byte.MaxValue)), new Field(MetaType.Padding(2)))), "Impacts"));

		private static readonly MetaStruct oncp = new MetaStruct("ONCPInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(new MetaStruct("ONCPParticle", new Field(MetaType.String16, "Name"), new Field(MetaType.String64, "Type"), new Field(MetaType.Enum<ONCPBodyPart>(), "BodyPart"), new Field(MetaType.Padding(1, 95)), new Field(MetaType.Padding(5)))), "Particles"));

		private static readonly MetaStruct oncv = new MetaStruct("ONCVInstance", new Field(MetaType.Pointer(TemplateTag.ONCV), "ParentVariant"), new Field(MetaType.String32, "CharacterClass"), new Field(MetaType.String32, "CharacterClassHard"));

		private static readonly MetaStruct crsa = new MetaStruct("CRSAInstance", new Field(MetaType.Padding(12)), new Field(MetaType.Int32, "FixedCount"), new Field(MetaType.Int32, "UsedCount"), new Field(MetaType.VarArray(new MetaStruct("CRSACorpse", new Field(MetaType.Padding(160)), new Field(MetaType.Pointer(TemplateTag.ONCC), "CharacterClass"), new Field(MetaType.Array(19, MetaType.Matrix4x3), "Transforms"), new Field(MetaType.BoundingBox, "BoundingBox"))), "Corpses"));

		private static readonly MetaStruct dpge = new MetaStruct("DPgeInstance", new Field(MetaType.Int16, "LevelNumber"), new Field(MetaType.Int16, "PageNumber"), new Field(MetaType.Byte, "IsLearnedMove"), new Field(MetaType.Padding(3)), new Field(MetaType.Padding(48)), new Field(MetaType.Pointer(TemplateTag.IGPG), "Page"));

		private static readonly MetaStruct film = new MetaStruct("FILMInstance", new Field(MetaType.Vector3, "Position"), new Field(MetaType.Float, "Facing"), new Field(MetaType.Float, "DesiredFacing"), new Field(MetaType.Float, "HeadFacing"), new Field(MetaType.Float, "HeadPitch"), new Field(MetaType.Int32, "FrameCount"), new Field(MetaType.Array(2, MetaType.Pointer(TemplateTag.TRAM)), "Animations"), new Field(MetaType.Padding(12)), new Field(MetaType.VarArray(new MetaStruct("Frame", new Field(MetaType.Vector2, "MouseDelta"), new Field(MetaType.Enum<FILMKeys>(), "Keys"), new Field(MetaType.Int32, "Frame"), new Field(MetaType.Padding(4)))), "Frames"));

		private static readonly MetaStruct ongs = new MetaStruct("ONGSInstance", new Field(MetaType.Float, "MaxOverhealthFactor"), new Field(MetaType.Float, "NormalHypoStrength"), new Field(MetaType.Float, "OverhealthHypoStrength"), new Field(MetaType.Float, "OverhealthMinDamage"), new Field(MetaType.Float, "OverhealthMaxDamage"), new Field(MetaType.Int32, "UsedHealthElements"), new Field(MetaType.Array(16, MetaType.Float), "HealthPercent"), new Field(MetaType.Array(16, MetaType.Color), "HealthColor"), new Field(MetaType.Array(6, MetaType.String128), "PowerupModels"), new Field(MetaType.Padding(128)), new Field(MetaType.Array(6, MetaType.String128), "PowerupGlowTextures"), new Field(MetaType.Padding(128)), new Field(MetaType.Array(6, MetaType.Vector2), "PowerupGlowSizes"), new Field(MetaType.Padding(8)), new Field(MetaType.Array(23, MetaType.String32), "Sounds"), new Field(MetaType.Array(3, MetaType.Float), "NoticeFactors"), new Field(MetaType.Array(3, MetaType.Float), "BlockChanceFactors"), new Field(MetaType.Array(3, MetaType.Float), "DodgeFactors"), new Field(MetaType.Array(3, MetaType.Float), "WeaponAccuracyFactors"), new Field(MetaType.Array(3, MetaType.Float), "EnemyHealthFactors"), new Field(MetaType.Array(3, MetaType.Float), "PlayerHealthFactors"), new Field(MetaType.Int32, "UsedAutoPrompts"), new Field(MetaType.Array(16, new MetaStruct("ONGSAutoPrompt", new Field(MetaType.String32, "Notes"), new Field(MetaType.Int16, "FirstAutopromptLevel"), new Field(MetaType.Int16, "LastAutopromptLevel"), new Field(MetaType.String32, "SubtitleName"))), "AutoPrompts"));

		private static readonly MetaStruct hpge = new MetaStruct("HPgeInstance", new Field(MetaType.Padding(4)), new Field(MetaType.Pointer(TemplateTag.IGPG), "Page"));

		private static readonly MetaStruct ighh = new MetaStruct("IGHHInstance", new Field(MetaType.Padding(28)), new Field(MetaType.Pointer(TemplateTag.TXMP), "LeftTexture"), new Field(MetaType.Pointer(TemplateTag.TXMP), "RightTexture"), new Field(MetaType.Int16, "LeftX"), new Field(MetaType.Int16, "LeftY"), new Field(MetaType.Int16, "RightX"), new Field(MetaType.Int16, "RightY"), new Field(MetaType.Int32, "LeftCount"), new Field(MetaType.Int32, "RightCount"), new Field(MetaType.VarArray(new MetaStruct("IGHHLabels", new Field(MetaType.String64, "Text"), new Field(MetaType.Int16, "X"), new Field(MetaType.Int16, "Y"))), "Labels"));

		private static readonly MetaStruct igpg = new MetaStruct("IGPGInstance", new Field(new MetaStruct("IGPGFont", new Field(MetaType.Pointer(TemplateTag.TSFF), "Family"), new Field(MetaType.Enum<IGPGFontStyle>(), "Style"), new Field(MetaType.Color, "Color"), new Field(MetaType.UInt16, "Size"), new Field(MetaType.Enum<IGPGFlags>(), "Flags")), "Font"), new Field(MetaType.Pointer(TemplateTag.NONE), "Image"), new Field(MetaType.Pointer(TemplateTag.IGSA), "Text1"), new Field(MetaType.Pointer(TemplateTag.IGSA), "Text2"));

		private static readonly MetaStruct igpa = new MetaStruct("IGPAInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(MetaType.Pointer(TemplateTag.IGPG)), "Pages"));

		private static readonly MetaStruct igst = new MetaStruct("IGStInstance", new Field(new MetaStruct("IGStFont", new Field(MetaType.Pointer(TemplateTag.TSFF), "Family"), new Field(MetaType.Enum<IGStFontStyle>(), "Style"), new Field(MetaType.Color, "Color"), new Field(MetaType.Int16, "Size"), new Field(MetaType.Enum<IGStFlags>(), "Flags")), "Font"), new Field(MetaType.String(384), "Text"));

		private static readonly MetaStruct igsa = new MetaStruct("IGSAInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(MetaType.Pointer(TemplateTag.IGSt)), "Strings"));

		private static readonly MetaStruct ipge = new MetaStruct("IPgeInstance", new Field(MetaType.Int32, "PageNumber"), new Field(MetaType.Pointer(TemplateTag.IGPG), "Page"));

		private static readonly MetaStruct keyi = new MetaStruct("KeyIInstance", new Field(MetaType.Pointer(TemplateTag.TXMP), "Punch"), new Field(MetaType.Pointer(TemplateTag.TXMP), "Kick"), new Field(MetaType.Pointer(TemplateTag.TXMP), "Forward"), new Field(MetaType.Pointer(TemplateTag.TXMP), "Backward"), new Field(MetaType.Pointer(TemplateTag.TXMP), "Left"), new Field(MetaType.Pointer(TemplateTag.TXMP), "Right"), new Field(MetaType.Pointer(TemplateTag.TXMP), "Crouch"), new Field(MetaType.Pointer(TemplateTag.TXMP), "Jump"), new Field(MetaType.Pointer(TemplateTag.TXMP), "Hold"), new Field(MetaType.Pointer(TemplateTag.TXMP), "Plus"));

		private static readonly MetaStruct onlv = new MetaStruct("ONLVInstance", new Field(MetaType.String64, "Name"), new Field(MetaType.Pointer(TemplateTag.AKEV), "Environment"), new Field(MetaType.Pointer(TemplateTag.OBOA), "Objects"), new Field(MetaType.Padding(12)), new Field(MetaType.Pointer(TemplateTag.ONSK), "SkyBox"), new Field(MetaType.Padding(4)), new Field(MetaType.Pointer(TemplateTag.AISA), "Characters"), new Field(MetaType.Padding(12)), new Field(MetaType.Pointer(TemplateTag.ONOA), "ObjectQuadMap"), new Field(MetaType.Pointer(TemplateTag.ENVP), "Particles"), new Field(MetaType.Padding(644)), new Field(MetaType.Pointer(TemplateTag.CRSA), "Corpses"));

		private static readonly MetaStruct onld = new MetaStruct("ONLDInstance", new Field(MetaType.Int16, "LevelNumber"), new Field(MetaType.Int16, "NextLevelNumber"), new Field(MetaType.String64, "DisplayName"));

		private static readonly MetaStruct onoa = new MetaStruct("ONOAInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(new MetaStruct("ONOAElement", new Field(MetaType.Int32, "ObjectId"), new Field(MetaType.Pointer(TemplateTag.IDXA), "QuadList"))), "Elements"));

		private static readonly MetaStruct opge = new MetaStruct("OPgeInstance", new Field(MetaType.Padding(2)), new Field(MetaType.UInt16, "LevelNumber"), new Field(MetaType.Pointer(TemplateTag.IGPA), "Pages"));

		private static readonly MetaStruct onsk = new MetaStruct("ONSKInstance", new Field(MetaType.Array(6, MetaType.Pointer(TemplateTag.TXMP)), "SkyboxTextures"), new Field(MetaType.Array(8, MetaType.Pointer(TemplateTag.TXMP)), "Planets"), new Field(MetaType.Pointer(TemplateTag.TXMP), "SunFlare"), new Field(MetaType.Array(5, MetaType.Pointer(TemplateTag.TXMP)), "Stars"), new Field(MetaType.Int32, "PlanetCount"), new Field(MetaType.Int32, "NoSunFlare"), new Field(MetaType.Array(8, MetaType.Float), "PlanetWidths"), new Field(MetaType.Array(8, MetaType.Float), "PlanetHeights"), new Field(MetaType.Array(8, MetaType.Float), "PlanetElevations"), new Field(MetaType.Array(8, MetaType.Float), "PlanetAzimuths"), new Field(MetaType.Float, "SunFlareSize"), new Field(MetaType.Float, "SunFlareIntensity"), new Field(MetaType.Int32, "StarCount"), new Field(MetaType.Int32, "RandomSeed"), new Field(MetaType.Int32, ""));

		private static readonly MetaStruct txtc = new MetaStruct("TxtCInstance", new Field(MetaType.Pointer(TemplateTag.IGPA), "Pages"));

		private static readonly MetaStruct onvl = new MetaStruct("ONVLInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(MetaType.Pointer(TemplateTag.ONCV)), "Variants"));

		private static readonly MetaStruct wpge = new MetaStruct("WPgeInstance", new Field(MetaType.Pointer(TemplateTag.ONWC), "WeaponClass"), new Field(MetaType.Pointer(TemplateTag.IGPG), "Page"));

		private static readonly MetaStruct pspc = new MetaStruct("PSpcInstance", new Field(MetaType.Array(9, new MetaStruct("PSpcPoint", new Field(MetaType.Int16, "X"), new Field(MetaType.Int16, "Y"))), "LeftTop"), new Field(MetaType.Array(9, new MetaStruct("PSpcPoint", new Field(MetaType.Int16, "X"), new Field(MetaType.Int16, "Y"))), "RightBottom"), new Field(MetaType.Pointer(TemplateTag.TXMP), "Texture"));

		private static readonly MetaStruct pspl = new MetaStruct("PSpLInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(new MetaStruct("PSpLElement", new Field(MetaType.Enum<PSpLType>(), "Type"), new Field(MetaType.Pointer(TemplateTag.PSpc), "Part"))), "Elements"));

		private static readonly MetaStruct psui = new MetaStruct("PSUIInstance", new Field(MetaType.Pointer(TemplateTag.PSpc), "Background"), new Field(MetaType.Pointer(TemplateTag.PSpc), "Border"), new Field(MetaType.Pointer(TemplateTag.PSpc), "Title"), new Field(MetaType.Pointer(TemplateTag.PSpc), "Grow"), new Field(MetaType.Pointer(TemplateTag.PSpc), "CloseIdle"), new Field(MetaType.Pointer(TemplateTag.PSpc), "ClosePressed"), new Field(MetaType.Pointer(TemplateTag.PSpc), "ZoomIdle"), new Field(MetaType.Pointer(TemplateTag.PSpc), "ZoomPressed"), new Field(MetaType.Pointer(TemplateTag.PSpc), "FlattenIdle"), new Field(MetaType.Pointer(TemplateTag.PSpc), "FlattenPressed"), new Field(MetaType.Pointer(TemplateTag.PSpc), "TextCaret"), new Field(MetaType.Pointer(TemplateTag.PSpc), "Outline"), new Field(MetaType.Pointer(TemplateTag.PSpc), "Button"), new Field(MetaType.Pointer(TemplateTag.PSpc), "ButtonOff"), new Field(MetaType.Pointer(TemplateTag.PSpc), "ButtonOn"), new Field(MetaType.Pointer(TemplateTag.PSpc), "CheckBoxOn"), new Field(MetaType.Pointer(TemplateTag.PSpc), "CheckBoxOff"), new Field(MetaType.Pointer(TemplateTag.PSpc), "EditField"), new Field(MetaType.Pointer(TemplateTag.PSpc), "EditFieldFocused"), new Field(MetaType.Pointer(TemplateTag.PSpc), "EditFieldHighlighted"), new Field(MetaType.Pointer(TemplateTag.PSpc), "Divider"), new Field(MetaType.Pointer(TemplateTag.PSpc), "Check"), new Field(MetaType.Pointer(TemplateTag.PSpc), "PopupMenu"), new Field(MetaType.Pointer(TemplateTag.PSpc), "ProgressBarTrack"), new Field(MetaType.Pointer(TemplateTag.PSpc), "ProgressBarFill"), new Field(MetaType.Pointer(TemplateTag.PSpc), "RadioButtonOn"), new Field(MetaType.Pointer(TemplateTag.PSpc), "RadioButtonOff"), new Field(MetaType.Pointer(TemplateTag.PSpc), "ScrollBarArrowUpIdle"), new Field(MetaType.Pointer(TemplateTag.PSpc), "ScrollBarArrowUpPressed"), new Field(MetaType.Pointer(TemplateTag.PSpc), "ScrollBarArrowDownIdle"), new Field(MetaType.Pointer(TemplateTag.PSpc), "ScrollBarArrowDownPressed"), new Field(MetaType.Pointer(TemplateTag.PSpc), "ScrollBarVerticalTrack"), new Field(MetaType.Pointer(TemplateTag.PSpc), "ScrollBarArrowLeftIdle"), new Field(MetaType.Pointer(TemplateTag.PSpc), "ScrollBarArrowLeftPressed"), new Field(MetaType.Pointer(TemplateTag.PSpc), "ScrollBarArrowRightIdle"), new Field(MetaType.Pointer(TemplateTag.PSpc), "ScrollBarArrorRightPressed"), new Field(MetaType.Pointer(TemplateTag.PSpc), "ScrollBarHorizontalTrack"), new Field(MetaType.Pointer(TemplateTag.PSpc), "ScrollBarThumb"), new Field(MetaType.Pointer(TemplateTag.PSpc), "SliderThumb"), new Field(MetaType.Pointer(TemplateTag.PSpc), "SliderTrack"), new Field(MetaType.Pointer(TemplateTag.PSpc), "Background2"), new Field(MetaType.Pointer(TemplateTag.PSpc), "Background3"), new Field(MetaType.Pointer(TemplateTag.PSpc), "File"), new Field(MetaType.Pointer(TemplateTag.PSpc), "Folder"));

		private static readonly MetaStruct subt = new MetaStruct("SUBTInstance", new Field(MetaType.Padding(16)), new BinaryPartField(MetaType.RawOffset, "DataOffset"), new Field(MetaType.VarArray(MetaType.Int32), "Elements"));

		private static readonly MetaStruct idxa = new MetaStruct("IDXAInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(MetaType.Int32), "Indices"));

		private static readonly MetaStruct tras = new MetaStruct("TRASInstance", new Field(MetaType.Pointer(TemplateTag.TRAM), "Animation"), new Field(MetaType.Float, "LeftStep"), new Field(MetaType.Float, "RightStep"), new Field(MetaType.UInt16, "LeftFrames"), new Field(MetaType.UInt16, "RightFrames"), new Field(MetaType.Float, "DownStep"), new Field(MetaType.Float, "UpStep"), new Field(MetaType.UInt16, "DownFrames"), new Field(MetaType.UInt16, "UpFrames"));

		private static readonly MetaStruct tram = new MetaStruct("TRAMInstance", new Field(MetaType.Padding(4)), new BinaryPartField(MetaType.RawOffset, "Height", "FrameCount", 4, MetaType.Float), new BinaryPartField(MetaType.RawOffset, "Velocity", "FrameCount", 8, MetaType.Vector2), new BinaryPartField(MetaType.RawOffset, "Attack", "AttackCount", 32, new MetaStruct("TRAMAttack", new Field(MetaType.Int32, "Bones"), new Field(MetaType.Float, "Unknown1"), new Field(MetaType.Int32, "Flags"), new Field(MetaType.Int16, "HitPoints"), new Field(MetaType.Int16, "StartFrame"), new Field(MetaType.Int16, "EndFrame"), new Field(MetaType.Int16, "AnimationType"), new Field(MetaType.Int16, "Unknown2"), new Field(MetaType.Int16, "BlockStun"), new Field(MetaType.Int16, "Stagger"), new Field(MetaType.Padding(6)))), new BinaryPartField(MetaType.RawOffset, "Damage", "DamageCount", 4, new MetaStruct("TRAMDamage", new Field(MetaType.Int16, "Damage"), new Field(MetaType.Int16, "Frame"))), new BinaryPartField(MetaType.RawOffset, "MotionBlur", "MotionBlurCount", 12, new MetaStruct("TRAMMotionBlur", new Field(MetaType.Int32, "Bones"), new Field(MetaType.Int16, "StartFrame"), new Field(MetaType.Int16, "EndFrame"), new Field(MetaType.Byte, "Lifetime"), new Field(MetaType.Byte, "Alpha"), new Field(MetaType.Byte, "Interval"), new Field(MetaType.Padding(1)))), new BinaryPartField(MetaType.RawOffset, "Shortcut", "ShortcutCount", 8, new MetaStruct("TRAMShortcut", new Field(MetaType.Int16, "FromState"), new Field(MetaType.Byte, "Length"), new Field(MetaType.Padding(1)), new Field(MetaType.Int32, "Flags"))), new BinaryPartField(MetaType.RawOffset, "Throw", 22, new MetaStruct("TRAMThrow", new Field(MetaType.Vector3, "PositionAdjustment"), new Field(MetaType.Float, "AngleAdjustment"), new Field(MetaType.Float, "Distance"), new Field(MetaType.Int16, "Type"))), new BinaryPartField(MetaType.RawOffset, "Footstep", "FootstepCount", 4, new MetaStruct("TRAMFootstep", new Field(MetaType.Int16, "Frame"), new Field(MetaType.Int16, "Type"))), new BinaryPartField(MetaType.RawOffset, "Particle", "ParticleCount", 24, new MetaStruct("TRAMParticle", new Field(MetaType.Int16, "StartFrame"), new Field(MetaType.Int16, "EndFrame"), new Field(MetaType.Int32, "Bone"), new Field(MetaType.String16, "Name"))), new BinaryPartField(MetaType.RawOffset, "Position", "FrameCount", 8, new MetaStruct("TRAMPosition", new Field(MetaType.Int16, "X"), new Field(MetaType.Int16, "Z"), new Field(MetaType.Int16, "Height"), new Field(MetaType.Int16, "Y"))), new BinaryPartField(MetaType.RawOffset, "Rotation"), new BinaryPartField(MetaType.RawOffset, "Sound", "SoundDataCount", 34, new MetaStruct("TRAMSound", new Field(MetaType.String32, "SoundName"), new Field(MetaType.Int16, "StartFrame"))), new Field(MetaType.Int32, "Flags"), new Field(MetaType.Array(2, MetaType.Pointer(TemplateTag.TRAM)), "DirectAnimations"), new Field(MetaType.Int32, "OverlayUsedParts"), new Field(MetaType.Int32, "OverlayReplacedParts"), new Field(MetaType.Float, "FinalRotation"), new Field(MetaType.Int16, "MoveDirection"), new Field(MetaType.Int16, "AttackSoundIndex"), new Field(new MetaStruct("TRAMExtentInfo", new Field(MetaType.Float, "MaxHorizontal"), new Field(MetaType.Float, "MinY"), new Field(MetaType.Float, "MaxY"), new Field(MetaType.Array(36, MetaType.Float), "Horizontal"), new Field(new MetaStruct("TRAMExtent", new Field(MetaType.Int16, "Frame"), new Field(MetaType.Byte, "Attack"), new Field(MetaType.Byte, "FrameOffset"), new Field(MetaType.Vector3, "Location"), new Field(MetaType.Float, "Length"), new Field(MetaType.Float, "MinY"), new Field(MetaType.Float, "MaxY"), new Field(MetaType.Float, "Angle")), "FirstExtent"), new Field(new MetaStruct("TRAMExtent", new Field(MetaType.Int16, "Frame"), new Field(MetaType.Byte, "Attack"), new Field(MetaType.Byte, "FrameOffset"), new Field(MetaType.Vector3, "Location"), new Field(MetaType.Float, "Length"), new Field(MetaType.Float, "MinY"), new Field(MetaType.Float, "MaxY"), new Field(MetaType.Float, "Angle")), "MaxExtent"), new Field(MetaType.Int32, "AlternateMoveDirection"), new Field(MetaType.Int32, "ExtentCount"), new BinaryPartField(MetaType.RawOffset, "Extents", "ExtentCount", 12, new MetaStruct("TRAMExtent", new Field(MetaType.Int16, "Frame"), new Field(MetaType.Int16, "Angle"), new Field(MetaType.Int16, "Length"), new Field(MetaType.Int16, "Offset"), new Field(MetaType.Int16, "MinY"), new Field(MetaType.Int16, "MaxY")))), "ExtentInfo"), new Field(MetaType.String16, "ImpactParticle"), new Field(MetaType.Int16, "HardPause"), new Field(MetaType.Int16, "SoftPause"), new Field(MetaType.Int32, "SoundDataCount"), new Field(MetaType.Padding(6)), new Field(MetaType.Int16, "FramesPerSecond"), new Field(MetaType.Int16, "CompressionSize"), new Field(MetaType.Int16, "Type"), new Field(MetaType.Int16, "AimingType"), new Field(MetaType.Int16, "FromState"), new Field(MetaType.Int16, "ToState"), new Field(MetaType.Int16, "BodyPartCount"), new Field(MetaType.Int16, "FrameCount"), new Field(MetaType.Int16, "Duration"), new Field(MetaType.Int16, "Varient"), new Field(MetaType.Padding(2)), new Field(MetaType.Int16, "AtomicStart"), new Field(MetaType.Int16, "AtomicEnd"), new Field(MetaType.Int16, "EndInterpolation"), new Field(MetaType.Int16, "MaxInterpolation"), new Field(MetaType.Int16, "ActionFrame"), new Field(MetaType.Int16, "FirstLevelAvailable"), new Field(MetaType.Byte, "InvulnerableStart"), new Field(MetaType.Byte, "InvulnerableEnd"), new Field(MetaType.Byte, "AttackCount"), new Field(MetaType.Byte, "DamageCount"), new Field(MetaType.Byte, "MotionBlurCount"), new Field(MetaType.Byte, "ShortcutCount"), new Field(MetaType.Byte, "FootstepCount"), new Field(MetaType.Byte, "ParticleCount"));

		private static readonly MetaStruct trac = new MetaStruct("TRACInstance", new Field(MetaType.Padding(16)), new Field(MetaType.Pointer(TemplateTag.TRAC), "ParentCollection"), new Field(MetaType.Padding(2)), new Field(MetaType.ShortVarArray(new MetaStruct("TRACAnimation", new Field(MetaType.Int16, "Weight"), new Field(MetaType.Padding(6)), new Field(MetaType.Pointer(TemplateTag.TRAM), "Animation"))), "Animations"));

		private static readonly MetaStruct trcm = new MetaStruct("TRCMInstance", new Field(MetaType.Padding(4)), new Field(MetaType.UInt16, "BodyPartCount"), new Field(MetaType.Padding(78)), new Field(MetaType.Pointer(TemplateTag.TRGA), "Geometry"), new Field(MetaType.Pointer(TemplateTag.TRTA), "Position"), new Field(MetaType.Pointer(TemplateTag.TRIA), "Hierarchy"));

		private static readonly MetaStruct trbs = new MetaStruct("TRBSInstance", new Field(MetaType.Array(5, MetaType.Pointer(TemplateTag.TRCM)), "Elements"));

		private static readonly MetaStruct trma = new MetaStruct("TRMAInstance", new Field(MetaType.Padding(22)), new Field(MetaType.ShortVarArray(MetaType.Pointer(TemplateTag.TXMP)), "Textures"));

		private static readonly MetaStruct trga = new MetaStruct("TRGAInstance", new Field(MetaType.Padding(22)), new Field(MetaType.ShortVarArray(MetaType.Pointer(TemplateTag.M3GM)), "Geometries"));

		private static readonly MetaStruct tria = new MetaStruct("TRIAInstance", new Field(MetaType.Padding(22)), new Field(MetaType.ShortVarArray(new MetaStruct("TRIAElement", new Field(MetaType.Byte, "Parent"), new Field(MetaType.Byte, "Child"), new Field(MetaType.Byte, "Sibling"), new Field(MetaType.Padding(1)))), "Elements"));

		private static readonly MetaStruct trsc = new MetaStruct("TRSCInstance", new Field(MetaType.Padding(22)), new Field(MetaType.ShortVarArray(MetaType.Pointer(TemplateTag.TRAS)), "AimingScreens"));

		private static readonly MetaStruct trta = new MetaStruct("TRTAInstance", new Field(MetaType.Padding(22)), new Field(MetaType.ShortVarArray(MetaType.Vector3), "Translations"));

		private static readonly MetaStruct tsft = new MetaStruct("TSFTInstance", new Field(MetaType.Padding(6)), new Field(MetaType.Int16, "FontSize"), new Field(MetaType.Int32, "FontStyle"), new Field(MetaType.Int16, "AscenderHeight"), new Field(MetaType.Int16, "DescenderHeight"), new Field(MetaType.Int16, "LeadingHeight"), new Field(MetaType.Int16, ""), new Field(MetaType.Array(256, MetaType.Pointer(TemplateTag.TSGA)), "Glyphs"), new Field(MetaType.VarArray(MetaType.Int32), "GlyphBitmaps"));

		private static readonly MetaStruct tsff = new MetaStruct("TSFFInstance", new Field(MetaType.Padding(16)), new Field(MetaType.Pointer(TemplateTag.TSFL), "Language"), new Field(MetaType.VarArray(MetaType.Pointer(TemplateTag.TSFT)), "Fonts"));

		private static readonly MetaStruct tsfl = new MetaStruct("TSFLInstance", new Field(MetaType.String64, ""), new Field(MetaType.String64, "Breaking"), new Field(MetaType.String64, ""), new Field(MetaType.String64, ""), new Field(MetaType.String64, ""));

		private static readonly MetaStruct tsga = new MetaStruct("TSGAInstance", new Field(MetaType.Array(256, new MetaStruct("TSGAGlyph", new Field(MetaType.Int16, "Index"), new Field(MetaType.Int16, "Width"), new Field(MetaType.Int16, "GlyphWidth"), new Field(MetaType.Int16, "GlyphHeight"), new Field(MetaType.Int16, "GlyphXOrigin"), new Field(MetaType.Int16, "GlyphYOrigin"), new Field(MetaType.Int32, "GlyphBitmapOffset"), new Field(MetaType.Padding(4)))), "Glyphs"));

		private static readonly MetaStruct wmcl = new MetaStruct("WMCLInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(new MetaStruct("WMCLCursor", new Field(MetaType.Int32, "Id"), new Field(MetaType.Pointer(TemplateTag.PSpc), "Part"))), "Cursors"));

		private static readonly MetaStruct wmdd = new MetaStruct("WMDDInstance", new Field(MetaType.String256, "Caption"), new Field(MetaType.Int16, "Id"), new Field(MetaType.Padding(2)), new Field(MetaType.Enum<WMDDState>(), "State"), new Field(MetaType.Enum<WMDDStyle>(), "Style"), new Field(MetaType.Int16, "X"), new Field(MetaType.Int16, "Y"), new Field(MetaType.Int16, "Width"), new Field(MetaType.Int16, "Height"), new Field(MetaType.VarArray(new MetaStruct("WMDDControl", new Field(MetaType.String256, "Text"), new Field(MetaType.Enum<WMDDControlClass>(), "Class"), new Field(MetaType.Int16, "Id"), new Field(MetaType.Int32, "State"), new Field(MetaType.Int32, "Style"), new Field(MetaType.Int16, "X"), new Field(MetaType.Int16, "Y"), new Field(MetaType.Int16, "Width"), new Field(MetaType.Int16, "Height"), new Field(new MetaStruct("WMDDFont", new Field(MetaType.Pointer(TemplateTag.TSFF), "Family"), new Field(MetaType.Enum<WMDDControlFontStyle>(), "Style"), new Field(MetaType.Color, "Color"), new Field(MetaType.Padding(1, 1)), new Field(MetaType.Padding(1, 0)), new Field(MetaType.Int16, "Size")), "Font"))), "Controls"));

		private static readonly MetaStruct wmmb = new MetaStruct("WMMBInstance", new Field(MetaType.Padding(18)), new Field(MetaType.Int16, "Id"), new Field(MetaType.VarArray(MetaType.Pointer(TemplateTag.WMM_)), "Items"));

		private static readonly MetaStruct wmm_ = new MetaStruct("WMM_Instance", new Field(MetaType.Padding(18)), new Field(MetaType.Int16, "Id"), new Field(MetaType.String64, "Text"), new Field(MetaType.VarArray(new MetaStruct("WMM_MenuItem", new Field(MetaType.Enum<WMM_MenuItemType>(), "Type"), new Field(MetaType.Int16, "Id"), new Field(MetaType.String64, "Text"))), "Items"));

		private static readonly MetaStruct onwc = new MetaStruct("ONWCInstance", new Field(new MetaStruct("ONWCLaserSight", new Field(MetaType.Vector3, "Origin"), new Field(MetaType.Float, "Stiffness"), new Field(MetaType.Float, "AdditionalAzimuth"), new Field(MetaType.Float, "AdditionalElevation"), new Field(MetaType.Float, "LaserMaxLength"), new Field(MetaType.Color, "LaserColor"), new Field(MetaType.Pointer(TemplateTag.TXMP), "NormalTexture"), new Field(MetaType.Color, "NormalColor"), new Field(MetaType.Float, "NormalScale"), new Field(MetaType.Pointer(TemplateTag.TXMP), "LockedTexture"), new Field(MetaType.Color, "LockedColor"), new Field(MetaType.Float, "LockedScale"), new Field(MetaType.Pointer(TemplateTag.TXMP), "TunnelTexture"), new Field(MetaType.Color, "TunnelColor"), new Field(MetaType.Float, "TunnelScale"), new Field(MetaType.Int32, "TunnelCount"), new Field(MetaType.Float, "TunnelSpacing")), "LaserSight"), new Field(new MetaStruct("ONWCAmmoMeter", new Field(MetaType.Pointer(TemplateTag.TXMP), "Icon"), new Field(MetaType.Pointer(TemplateTag.TXMP), "Empty"), new Field(MetaType.Pointer(TemplateTag.TXMP), "Fill")), "AmmoMeter"), new Field(MetaType.Pointer(TemplateTag.M3GM), "Geometry"), new Field(MetaType.String32, "Name"), new Field(MetaType.Float, "MouseSensitivity"), new Field(new MetaStruct("ONWCRecoil", new Field(MetaType.Float, "Base"), new Field(MetaType.Float, "Max"), new Field(MetaType.Float, "Factor"), new Field(MetaType.Float, "ReturnSpeed"), new Field(MetaType.Float, "FiringReturnSpeed")), "Recoil"), new Field(MetaType.Padding(36)), new Field(MetaType.Padding(2)), new Field(MetaType.Enum<TRAMType>(), "RecoilAnimationType"), new Field(MetaType.Enum<TRAMType>(), "ReloadAnimationType"), new Field(MetaType.Int16, "PauseAfterReload"), new Field(MetaType.Int16, "MaxShots"), new Field(MetaType.Int16, "ParticleCount"), new Field(MetaType.Int16, "FiringModeCount"), new Field(MetaType.Int16, "PauseBeforeReload"), new Field(MetaType.Int16, "ReleaseDelay"), new Field(MetaType.Padding(2)), new Field(MetaType.Enum<ONWCFlags>(), "Flags"), new Field(MetaType.Array(2, aiFiringMode), "FiringModes"), new Field(MetaType.Array(16, new MetaStruct("ONWCParticle", new Field(MetaType.Matrix4x3, "Transform"), new Field(MetaType.String16, "ParticleClass"), new Field(MetaType.Padding(4)), new Field(MetaType.Int16, "UsedAmmo"), new Field(MetaType.Int16, "ShotDelay"), new Field(MetaType.Int16, "RoughJusticeShotDelay"), new Field(MetaType.Int16, "ActiveFrames"), new Field(MetaType.Int16, "TriggeredBy"), new Field(MetaType.Int16, "DelayBeforeFiring"))), "Particles"), new Field(MetaType.String32, "EmptyWeaponSound"), new Field(MetaType.Padding(4)), new Field(MetaType.Pointer(TemplateTag.TXMP), "Glow"), new Field(MetaType.Pointer(TemplateTag.TXMP), "GlowAmmo"), new Field(MetaType.Vector2, "GlowTextureScale"), new Field(MetaType.Vector3, "PickupHandleOffset"), new Field(MetaType.Float, "HoveringHeight"));

		private static readonly MetaStruct aitr = new MetaStruct("AITRInstance", new Field(MetaType.Padding(22)), new Field(MetaType.ShortVarArray(new MetaStruct("AITRElement", new Field(MetaType.Int16, ""), new Field(MetaType.Int16, ""), new Field(MetaType.Int16, ""), new Field(MetaType.Int16, ""), new Field(MetaType.Int16, ""), new Field(MetaType.Int16, ""), new Field(MetaType.Int32, ""), new Field(MetaType.Int32, ""), new Field(MetaType.String64, ""))), "Elements"));

		private static readonly MetaStruct agdb = new MetaStruct("AGDBInstance", new Field(MetaType.Padding(20)), new Field(MetaType.VarArray(new MetaStruct("AGDBElement", new BinaryPartField(MetaType.RawOffset, "ObjectNameDataOffset"), new BinaryPartField(MetaType.RawOffset, "FileNameDataOffset"))), "Elements"));

		private static readonly MetaStruct akda = new MetaStruct("AKDAInstance", new Field(MetaType.Padding(20)), new Field(MetaType.Padding(4)));

		private static readonly MetaStruct obdc = new MetaStruct("OBDCInstance", new Field(MetaType.Padding(22)), new Field(MetaType.ShortVarArray(new MetaStruct("OBDCElement", new Field(MetaType.Int16, ""), new Field(MetaType.Int16, ""), new Field(MetaType.Pointer(TemplateTag.OBAN), "Animation"), new Field(MetaType.Padding(4)), new Field(MetaType.Int32, ""), new Field(MetaType.Padding(8)))), "Elements"));

		private static readonly MetaStruct onfa = new MetaStruct("ONFAInstance", new Field(MetaType.Padding(20)), new Field(MetaType.Int16, "UsedElements"), new Field(MetaType.ShortVarArray(new MetaStruct("ONFAElement", new Field(MetaType.Matrix4x3, "Transform"), new Field(MetaType.Vector3, "Position"), new Field(MetaType.Int32, ""), new Field(MetaType.Int16, "FlagId"), new Field(MetaType.Byte, ""), new Field(MetaType.Byte, ""))), "Elements"));

		private static readonly MetaStruct onma = new MetaStruct("ONMAInstance", new Field(MetaType.Padding(22)), new Field(MetaType.ShortVarArray(new MetaStruct("ONMAElement", new Field(MetaType.String64, "Name"), new Field(MetaType.Vector3, "Position"), new Field(MetaType.Vector3, "Direction"))), "Markers"));

		private static readonly MetaStruct onsa = new MetaStruct("ONSAInstance", new Field(MetaType.Padding(22)), new Field(MetaType.ShortVarArray(MetaType.Int16), "Elements"));

		private static readonly MetaStruct onta = new MetaStruct("ONTAInstance", new Field(MetaType.Padding(16)), new Field(MetaType.Int32, ""), new Field(MetaType.VarArray(new MetaStruct(new Field(MetaType.Array(8, new MetaStruct(new Field(MetaType.Int32, ""), new Field(MetaType.Int32, ""), new Field(MetaType.Int32, ""))), ""), new Field(MetaType.Array(6, new MetaStruct(new Field(MetaType.Array(4, MetaType.Int32), ""))), ""), new Field(MetaType.Array(6, new MetaStruct(new Field(MetaType.Int32, ""), new Field(MetaType.Int32, ""), new Field(MetaType.Int32, ""))), ""), new Field(MetaType.Array(6, new MetaStruct(new Field(MetaType.Int32, ""), new Field(MetaType.Int32, ""), new Field(MetaType.Int32, ""), new Field(MetaType.Int32, ""))), ""), new Field(MetaType.Array(6, MetaType.Int16), ""), new Field(MetaType.Int32, ""), new Field(MetaType.Int32, ""), new Field(MetaType.Int32, ""), new Field(MetaType.Int32, ""), new Field(MetaType.Int32, ""), new Field(MetaType.Int32, ""), new Field(MetaType.Int32, ""), new Field(MetaType.Int32, ""), new Field(MetaType.Int32, ""), new Field(MetaType.Int32, ""), new Field(MetaType.Int32, ""))), ""));

		private static readonly MetaStruct stna = new MetaStruct("StNAInstance", new Field(MetaType.Padding(22)), new Field(MetaType.ShortVarArray(MetaType.Pointer(TemplateTag.TStr)), "Names"));

		private static readonly MetaStruct tstr = new MetaStruct("TStrInstance", new Field(MetaType.String128, "Text"));

		private static InstanceMetadata pcMetadata;

		private static InstanceMetadata macMetadata;

		private Dictionary<TemplateTag, Template> templateIndex;

		protected virtual void InitializeTemplates(IList<Template> templates)
		{
			templates.Add(new Template(TemplateTag.AISA, aisa, 180964060137L, "AI Character Setup Array"));
			templates.Add(new Template(TemplateTag.AITR, aitr, 1763925L, "AI Script Trigger Array"));
			templates.Add(new Template(TemplateTag.AKAA, akaa, 1171063L, "Adjacency Array"));
			templates.Add(new Template(TemplateTag.ABNA, abna, 1207712L, "BSP Tree Node Array"));
			templates.Add(new Template(TemplateTag.AKVA, akva, 14616032L, "BNV Node Array"));
			templates.Add(new Template(TemplateTag.AKBA, akba, 3811460L, "Side Array"));
			templates.Add(new Template(TemplateTag.AKBP, akbp, 848969L, "BSP Node Array"));
			templates.Add(new Template(TemplateTag.AKDA, akda, 3036260L, "Door Frame Array"));
			templates.Add(new Template(TemplateTag.AKEV, akev, 584922226293L, "Akira Environment"));
			templates.Add(new Template(TemplateTag.AGQC, agqc, 1887121L, "Gunk Quad Collision Array"));
			templates.Add(new Template(TemplateTag.AGDB, agdb, 470551L, "Gunk Quad Debug Array"));
			templates.Add(new Template(TemplateTag.AGQG, agqg, 1835986L, "Gunk Quad General Array"));
			templates.Add(new Template(TemplateTag.AGQR, agqr, 539195L, "Gunk Quad Render Array"));
			templates.Add(new Template(TemplateTag.AKOT, akot, 76902095368L, "Oct tree"));
			templates.Add(new Template(TemplateTag.OTIT, otit, 676306L, "Oct Tree Interior Node Array"));
			templates.Add(new Template(TemplateTag.OTLF, otlf, 2010123L, "Oct Tree Leaf Node Array"));
			templates.Add(new Template(TemplateTag.QTNA, qtna, 421580L, "Quad Tree Node Array"));
			templates.Add(new Template(TemplateTag.ENVP, envp, 6799811L, "Env Particle Array"));
			templates.Add(new Template(TemplateTag.M3GM, m3gm, 170196001846L, "Geometry"));
			templates.Add(new Template(TemplateTag.M3GA, m3ga, 22018728114L, "GeometryArray"));
			templates.Add(new Template(TemplateTag.PLEA, plea, 506936L, "Plane Equation Array"));
			templates.Add(new Template(TemplateTag.PNTA, pnta, 3630956L, "3D Point Array"));
			templates.Add(new Template(TemplateTag.TXCA, txca, 594970L, "Texture Coordinate Array"));
			templates.Add(new Template(TemplateTag.TXAN, txan, 45282968455L, "Texture Map Animation"));
			templates.Add(new Template(TemplateTag.TXMA, txma, 24056332176L, "Texture map array"));
			templates.Add(new Template(TemplateTag.TXMB, txmb, 45283174994L, "Texture Map Big"));
			templates.Add(new Template(TemplateTag.VCRA, vcra, 345913L, "3D Vector Array"));
			templates.Add(new Template(TemplateTag.Impt, impt, 282390L, "Impact"));
			templates.Add(new Template(TemplateTag.Mtrl, mtrl, 167437L, "Material"));
			templates.Add(new Template(TemplateTag.CONS, cons, 85270924253L, "Console"));
			templates.Add(new Template(TemplateTag.DOOR, door, 26599423335L, "Door"));
			templates.Add(new Template(TemplateTag.OBLS, obls, 749629L, "Object LS Data"));
			templates.Add(new Template(TemplateTag.OFGA, ofga, 83566969698L, "Object Furn Geom Array"));
			templates.Add(new Template(TemplateTag.TRIG, trig, 145438592300L, "Trigger"));
			templates.Add(new Template(TemplateTag.TRGE, trge, 36266490172L, "Trigger Emitter"));
			templates.Add(new Template(TemplateTag.TURR, turr, 316893824446L, "Turret"));
			templates.Add(new Template(TemplateTag.OBAN, oban, 5114916L, "Object animation"));
			templates.Add(new Template(TemplateTag.OBDC, obdc, 33246071307L, "Door Class Array"));
			templates.Add(new Template(TemplateTag.OBOA, oboa, 82938791649L, "Starting Object Array"));
			templates.Add(new Template(TemplateTag.CBPI, cbpi, 51740530370L, "Character Body Part Impacts"));
			templates.Add(new Template(TemplateTag.CBPM, cbpm, 10395858207L, "Character Body Part Material"));
			templates.Add(new Template(TemplateTag.ONCC, oncc, 5109581306351L, "Oni Character Class"));
			templates.Add(new Template(TemplateTag.ONIA, onia, 2830234L, "Oni Character Impact Array"));
			templates.Add(new Template(TemplateTag.ONCP, oncp, 3109665L, "Oni Character Particle Array"));
			templates.Add(new Template(TemplateTag.ONCV, oncv, 170485L, "Oni Character Variant"));
			templates.Add(new Template(TemplateTag.CRSA, crsa, 51896374476L, "Corpse Array"));
			templates.Add(new Template(TemplateTag.DPge, dpge, 33194403947L, "Diary Page"));
			templates.Add(new Template(TemplateTag.FILM, film, 48102073005L, "Film"));
			templates.Add(new Template(TemplateTag.ONFA, onfa, 1772775L, "Imported Flag Node Array"));
			templates.Add(new Template(TemplateTag.ONGS, ongs, 36105142L, "Oni Game Settings"));
			templates.Add(new Template(TemplateTag.HPge, hpge, 18441269563L, "Help Page"));
			templates.Add(new Template(TemplateTag.IGHH, ighh, 38211049694L, "IGUI HUD Help"));
			templates.Add(new Template(TemplateTag.IGPG, igpg, 76477335677L, "IGUI Page"));
			templates.Add(new Template(TemplateTag.IGPA, igpa, 20900088069L, "IGUI Page Array"));
			templates.Add(new Template(TemplateTag.IGSt, igst, 11318621989L, "IGUI String"));
			templates.Add(new Template(TemplateTag.IGSA, igsa, 20900127752L, "IGUI String Array"));
			templates.Add(new Template(TemplateTag.IPge, ipge, 11064797626L, "Item Page"));
			templates.Add(new Template(TemplateTag.KeyI, keyi, 275939547053L, "Key Icons"));
			templates.Add(new Template(TemplateTag.ONLV, onlv, 539951247011L, "Oni Game Level"));
			templates.Add(new Template(TemplateTag.ONLD, onld, 266913L, "Oni Game Level Descriptor"));
			templates.Add(new Template(TemplateTag.ONMA, onma, 1197945L, "Imported Marker Node Array"));
			templates.Add(new Template(TemplateTag.ONOA, onoa, 27043257468L, "Object Gunk Array"));
			templates.Add(new Template(TemplateTag.OPge, opge, 18441354235L, "Objective Page"));
			templates.Add(new Template(TemplateTag.ONSK, onsk, 89156620391L, "Oni Sky class"));
			templates.Add(new Template(TemplateTag.ONSA, onsa, 280116L, "Imported Spawn Array"));
			templates.Add(new Template(TemplateTag.TxtC, txtc, 7376505639L, "Text Console"));
			templates.Add(new Template(TemplateTag.ONTA, onta, 10550464L, "Trigger Array"));
			templates.Add(new Template(TemplateTag.ONVL, onvl, 22619145610L, "Oni Variant List"));
			templates.Add(new Template(TemplateTag.WPge, wpge, 19047942581L, "Weapon Page"));
			templates.Add(new Template(TemplateTag.PSpc, pspc, 534088L, "Part Specification"));
			templates.Add(new Template(TemplateTag.PSpL, pspl, 838661L, "Part Specification List"));
			templates.Add(new Template(TemplateTag.PSUI, psui, 4180417615611L, "Part Specifications UI"));
			templates.Add(new Template(TemplateTag.SUBT, subt, 289896L, "Subtitle Array"));
			templates.Add(new Template(TemplateTag.IDXA, idxa, 159887L, "Index Array"));
			templates.Add(new Template(TemplateTag.TStr, tstr, 25760L, "String"));
			templates.Add(new Template(TemplateTag.StNA, stna, 24050971936L, "String Array"));
			templates.Add(new Template(TemplateTag.TRAS, tras, 8491477296L, "Totoro Aiming Screen"));
			templates.Add(new Template(TemplateTag.TRAM, tram, 70837389592L, "Totoro Animation Sequence"));
			templates.Add(new Template(TemplateTag.TRAC, trac, 65077377839L, "Animation Collection"));
			templates.Add(new Template(TemplateTag.TRCM, trcm, 152787879246L, "Totoro Quaternion Body"));
			templates.Add(new Template(TemplateTag.TRBS, trbs, 11317428793L, "Totoro Body Set"));
			templates.Add(new Template(TemplateTag.TRMA, trma, 24056327511L, "Texture Map Array"));
			templates.Add(new Template(TemplateTag.TRGA, trga, 22018728184L, "Totoro Quaternion Body Geometry Array"));
			templates.Add(new Template(TemplateTag.TRIA, tria, 705666L, "Totoro Quaternion Body Index Array"));
			templates.Add(new Template(TemplateTag.TRSC, trsc, 24049642263L, "Screen (aiming) Collection"));
			templates.Add(new Template(TemplateTag.TRTA, trta, 481768L, "Totoro Quaternion Body Translation Array"));
			templates.Add(new Template(TemplateTag.TSFT, tsft, 97619402474L, "Font"));
			templates.Add(new Template(TemplateTag.TSFF, tsff, 45272025226L, "Font Family"));
			templates.Add(new Template(TemplateTag.TSFL, tsfl, 581161L, "Font Language"));
			templates.Add(new Template(TemplateTag.TSGA, tsga, 2772632L, "Glyph Array"));
			templates.Add(new Template(TemplateTag.WMCL, wmcl, 643190L, "WM Cursor List"));
			templates.Add(new Template(TemplateTag.WMDD, wmdd, 120261047236L, "WM Dialog Data"));
			templates.Add(new Template(TemplateTag.WMMB, wmmb, 29293831991L, "WM Menu Bar"));
			templates.Add(new Template(TemplateTag.WMM_, wmm_, 793144L, "WM Menu"));
			templates.Add(new Template(TemplateTag.ONWC, onwc, 1733621247669L, "Oni Weapon Class"));
		}

		private static void GetRawAndSepPartsV32(InstanceFile file, Dictionary<int, int> rawParts, Dictionary<int, int> sepParts)
		{
			List<int> rawOffsets = new List<int>();
			using (BinaryReader binaryReader = new BinaryReader(file.FilePath))
			{
				foreach (InstanceDescriptor descriptor in file.Descriptors)
				{
					if (!descriptor.HasRawParts())
					{
						continue;
					}
					binaryReader.Position = descriptor.DataOffset;
					descriptor.Template.Type.Copy(binaryReader, null, delegate(CopyVisitor state)
					{
						BinaryPartField binaryPartField = state.Field as BinaryPartField;
						if (binaryPartField != null)
						{
							int @int = state.GetInt32();
							if (@int != 0)
							{
								rawOffsets.Add(@int);
							}
						}
					});
				}
			}
			rawOffsets.Sort();
			for (int num = 0; num < rawOffsets.Count; num++)
			{
				int num2 = rawOffsets[num];
				int num3 = ((num + 1 >= rawOffsets.Count) ? (file.Header.RawTableSize - num2) : (rawOffsets[num + 1] - num2));
				if (num3 > 0)
				{
					rawParts.Add(num2, num3);
				}
			}
		}

		public static void GetRawAndSepParts(InstanceFile file, Dictionary<int, int> rawParts, Dictionary<int, int> sepParts)
		{
			Dictionary<string, int> values = new Dictionary<string, int>();
			using (BinaryReader binaryReader = new BinaryReader(file.FilePath))
			{
				foreach (InstanceDescriptor descriptor in file.Descriptors)
				{
					if (!descriptor.HasRawParts())
					{
						continue;
					}
					values.Clear();
					binaryReader.Position = descriptor.DataOffset;
					descriptor.Template.Type.Copy(binaryReader, null, delegate(CopyVisitor state)
					{
						string currentFieldName = state.GetCurrentFieldName();
						if (!string.IsNullOrEmpty(currentFieldName))
						{
							if (state.Type == MetaType.Int32)
							{
								values[currentFieldName] = state.GetInt32();
							}
							else if (state.Type == MetaType.UInt32)
							{
								values[currentFieldName] = (int)state.GetUInt32();
							}
							else if (state.Type == MetaType.Int16)
							{
								values[currentFieldName] = state.GetInt16();
							}
							else if (state.Type == MetaType.UInt16)
							{
								values[currentFieldName] = state.GetUInt16();
							}
							else if (state.Type == MetaType.Byte)
							{
								values[currentFieldName] = state.GetByte();
							}
						}
					});
					binaryReader.Position = descriptor.DataOffset;
					descriptor.Template.Type.Copy(binaryReader, null, delegate(CopyVisitor state)
					{
						BinaryPartField binaryPartField = state.Field as BinaryPartField;
						if (binaryPartField != null)
						{
							int @int = state.GetInt32();
							if (@int != 0)
							{
								if (binaryPartField.Type == MetaType.RawOffset)
								{
									if (!rawParts.ContainsKey(@int))
									{
										rawParts.Add(@int, GetBinaryPartSize(descriptor, state, values));
									}
								}
								else if (!sepParts.ContainsKey(@int))
								{
									sepParts.Add(@int, GetBinaryPartSize(descriptor, state, values));
								}
							}
						}
					});
				}
			}
		}

		private static int GetBinaryPartSize(InstanceDescriptor descriptor, CopyVisitor state, Dictionary<string, int> values)
		{
			BinaryPartField binaryPartField = (BinaryPartField)state.Field;
			if (binaryPartField.SizeFieldName != null)
			{
				return values[state.GetParentFieldName() + "." + binaryPartField.SizeFieldName] * binaryPartField.SizeMultiplier;
			}
			if (binaryPartField.SizeMultiplier != 0)
			{
				return binaryPartField.SizeMultiplier;
			}
			return GetSpecialBinaryPartSize(descriptor, state, values);
		}

		private static int GetSpecialBinaryPartSize(InstanceDescriptor descriptor, CopyVisitor state, Dictionary<string, int> values)
		{
			switch (descriptor.Template.Tag)
			{
			case TemplateTag.AGDB:
				return GetAGDBRawDataSize(descriptor, state.GetInt32(), values);
			case TemplateTag.TRAM:
				return GetTRAMRotationsRawDataSize(descriptor, state.GetInt32(), values);
			case TemplateTag.SUBT:
				return GetSUBTRawDataSize(descriptor, state.GetInt32(), values);
			case TemplateTag.TXMP:
				return GetTXMPRawDataSize(descriptor, state.GetInt32(), values);
			default:
				throw new NotSupportedException(string.Format("Cannot get the raw data part size of type {0}", state.TopLevelType.Name));
			}
		}

		private static int GetAGDBRawDataSize(InstanceDescriptor descriptor, int rawOffset, Dictionary<string, int> values)
		{
			using (BinaryReader binaryReader = descriptor.GetRawReader(rawOffset))
			{
				int position = binaryReader.Position;
				binaryReader.SkipCString();
				return binaryReader.Position - position;
			}
		}

		private static int GetSUBTRawDataSize(InstanceDescriptor descriptor, int rawOffset, Dictionary<string, int> values)
		{
			int num = 0;
			using (BinaryReader binaryReader = descriptor.OpenRead(20))
			{
				int[] array = binaryReader.ReadInt32Array(binaryReader.ReadInt32());
				int[] array2 = array;
				foreach (int num2 in array2)
				{
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			using (BinaryReader binaryReader2 = descriptor.GetRawReader(rawOffset))
			{
				int position = binaryReader2.Position;
				binaryReader2.Position += num;
				binaryReader2.SkipCString();
				binaryReader2.SkipCString();
				return binaryReader2.Position - position;
			}
		}

		private static int GetTRAMRotationsRawDataSize(InstanceDescriptor descriptor, int rawOffset, Dictionary<string, int> values)
		{
			int num = values["TRAMInstance.BodyPartCount"];
			int length = values["TRAMInstance.CompressionSize"];
			int num2 = values["TRAMInstance.FrameCount"];
			using (BinaryReader binaryReader = descriptor.GetRawReader(rawOffset))
			{
				int position = binaryReader.Position;
				binaryReader.Skip((num - 1) * 2);
				int num3 = binaryReader.ReadInt16();
				binaryReader.Skip(num3 - num * 2);
				int num4 = 1;
				int num5 = 0;
				while (num4 > 0)
				{
					binaryReader.Skip(length);
					num4 = ((num5 < num2 - 1) ? binaryReader.ReadByte() : 0);
					num5 += num4;
				}
				return binaryReader.Position - position;
			}
		}

		private static int GetTXMPRawDataSize(InstanceDescriptor descriptor, int rawOffset, Dictionary<string, int> values)
		{
			int num = values["TXMPInstance.Width"];
			int num2 = values["TXMPInstance.Height"];
			TextureFlags textureFlags = (TextureFlags)values["TXMPInstance.Flags"];
			TXMPFormat tXMPFormat = (TXMPFormat)values["TXMPInstance.Format"];
			int num3;
			switch (tXMPFormat)
			{
			case TXMPFormat.BGRA4444:
			case TXMPFormat.BGR555:
			case TXMPFormat.BGRA5551:
				num3 = num * num2 * 2;
				break;
			case TXMPFormat.RGBA:
			case TXMPFormat.BGR:
				num3 = num * num2 * 4;
				break;
			case TXMPFormat.DXT1:
				num3 = num * num2 / 2;
				break;
			default:
				throw new NotSupportedException("Unsupported texture format");
			}
			int num4 = num3;
			if ((textureFlags & TextureFlags.HasMipMaps) != TextureFlags.None)
			{
				if (tXMPFormat == TXMPFormat.DXT1)
				{
					do
					{
						if (num > 1)
						{
							num >>= 1;
						}
						if (num2 > 1)
						{
							num2 >>= 1;
						}
						num4 += Math.Max(1, num / 4) * Math.Max(1, num2 / 4) * 8;
					}
					while (num2 > 1 || num > 1);
				}
				else
				{
					do
					{
						if (num > 1)
						{
							num >>= 1;
							num3 >>= 1;
						}
						if (num2 > 1)
						{
							num2 >>= 1;
							num3 >>= 1;
						}
						num4 += num3;
					}
					while (num2 > 1 || num > 1);
				}
			}
			return num4;
		}

		public static InstanceMetadata GetMetadata(InstanceFile instanceFile)
		{
			return GetMetadata(instanceFile.Header.TemplateChecksum);
		}

		public static InstanceMetadata GetMetadata(long templateChecksum)
		{
			switch (templateChecksum)
			{
			case 1052091763926815L:
				if (pcMetadata == null)
				{
					pcMetadata = new OniPcMetadata();
				}
				return pcMetadata;
			case 1052091493724257L:
				if (macMetadata == null)
				{
					macMetadata = new OniMacMetadata();
				}
				return macMetadata;
			default:
				throw new NotSupportedException();
			}
		}

		public Template GetTemplate(TemplateTag tag)
		{
			if (templateIndex == null)
			{
				templateIndex = new Dictionary<TemplateTag, Template>();
				List<Template> list = new List<Template>();
				InitializeTemplates(list);
				foreach (Template item in list)
				{
					templateIndex.Add(item.Tag, item);
				}
			}
			Template value;
			templateIndex.TryGetValue(tag, out value);
			return value;
		}

		public static void DumpCStructs(TextWriter writer)
		{
			OniPcMetadata oniPcMetadata = new OniPcMetadata();
			DumpVisitor dumpVisitor = new DumpVisitor(writer, oniPcMetadata);
			foreach (TemplateTag value in Enum.GetValues(typeof(TemplateTag)))
			{
				Template template = oniPcMetadata.GetTemplate(value);
				if (template != null)
				{
					dumpVisitor.VisitStruct(template.Type);
					DumpCStruct(writer, template.Type);
				}
			}
		}

		private static void DumpCStruct(TextWriter writer, MetaStruct type)
		{
			writer.WriteLine();
			writer.WriteLine("struct {0} {{", type.Name);
			string text = "\t";
			foreach (Field field in type.Fields)
			{
				if (field.Type is MetaPointer)
				{
					MetaPointer metaPointer = field.Type as MetaPointer;
					writer.WriteLine("{0}{1} *{2};", text, new OniPcMetadata().GetTemplate(metaPointer.Tag).Type.Name, field.Name);
				}
				else if (field.Type is MetaVarArray)
				{
					MetaVarArray metaVarArray = field.Type as MetaVarArray;
					writer.WriteLine("{0}{1} {2}[1];", text, metaVarArray.ElementType, field.Name);
				}
				else
				{
					writer.WriteLine("{0}{1} {2};", text, field.Type.Name, field.Name);
				}
			}
			text = text.Substring(0, text.Length - 1);
			writer.WriteLine("};");
		}
	}
}
