using System;

namespace Oni.Motoko
{
	[Flags]
	internal enum TextureFlags
	{
		None = 0,
		HasMipMaps = 1,
		NoUWrap = 4,
		NoVWrap = 8,
		AnimPingPong = 0x40,
		AnimRandom = 0x80,
		AnimGlobalTime = 0x100,
		HasEnvMap = 0x200,
		AdditiveBlend = 0x400,
		SwapBytes = 0x1000,
		AnimLoop = 0x4000,
		Shield = 0x8000,
		Invisibility = 0x10000,
		Daodan = 0x20000
	}
}
