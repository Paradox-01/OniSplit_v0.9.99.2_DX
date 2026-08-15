namespace Oni.Dae
{
	internal class EffectSampler
	{
		public EffectParameter Owner { get; set; }

		public EffectSurface Surface { get; set; }

		public EffectSamplerWrap WrapS { get; set; } = EffectSamplerWrap.Wrap;

		public EffectSamplerWrap WrapT { get; set; } = EffectSamplerWrap.Wrap;

		public EffectSamplerFilter MinFilter { get; set; }

		public EffectSamplerFilter MagFilter { get; set; }

		public EffectSamplerFilter MipFilter { get; set; }

		public EffectSampler()
		{
		}

		public EffectSampler(EffectSurface surface)
		{
			Surface = surface;
		}
	}
}
