namespace Oni.Dae
{
	internal class EffectTexture
	{
		public string TexCoordSemantic { get; set; }

		public EffectTextureChannel Channel { get; set; }

		public EffectSampler Sampler { get; set; }

		public EffectTexture()
		{
		}

		public EffectTexture(EffectSampler sampler, string texCoordSemantic)
		{
			TexCoordSemantic = texCoordSemantic;
			Sampler = sampler;
		}
	}
}
