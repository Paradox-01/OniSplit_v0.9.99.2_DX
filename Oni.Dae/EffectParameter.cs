namespace Oni.Dae
{
	internal class EffectParameter
	{
		private object value;

		private string reference;

		public string Sid { get; set; }

		public string Semantic { get; set; }

		public object Value
		{
			get
			{
				return value;
			}
			set
			{
				SetValueOwner(null);
				this.value = value;
				if (value != null)
				{
					reference = null;
				}
				SetValueOwner(this);
			}
		}

		public string Reference
		{
			get
			{
				return reference;
			}
			set
			{
				reference = value;
				if (reference != null)
				{
					this.value = null;
				}
			}
		}

		public EffectParameter()
		{
		}

		public EffectParameter(string sid, object value)
		{
			Sid = sid;
			this.value = value;
			SetValueOwner(this);
		}

		public EffectParameter(string sid, object value, Effect parent)
		{
			Sid = sid;
			this.value = value;
		}

		private void SetValueOwner(EffectParameter owner)
		{
			EffectSampler effectSampler = value as EffectSampler;
			if (effectSampler != null)
			{
				effectSampler.Owner = owner;
				return;
			}
			EffectSurface effectSurface = value as EffectSurface;
			if (effectSurface != null)
			{
				effectSurface.DeclaringParameter = owner;
			}
		}
	}
}
