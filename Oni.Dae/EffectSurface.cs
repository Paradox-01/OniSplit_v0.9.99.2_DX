namespace Oni.Dae
{
	internal class EffectSurface
	{
		public EffectParameter DeclaringParameter { get; set; }

		public Image InitFrom { get; set; }

		public EffectSurface()
		{
		}

		public EffectSurface(Image initFrom)
		{
			InitFrom = initFrom;
		}
	}
}
