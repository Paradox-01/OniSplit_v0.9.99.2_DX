namespace Oni.Dae
{
	internal abstract class Transform
	{
		private string sid;

		private readonly float[] values;

		private Sampler[] animations;

		public string Sid
		{
			get
			{
				return sid;
			}
			set
			{
				sid = value;
			}
		}

		public float[] Values
		{
			get
			{
				return values;
			}
		}

		public bool HasAnimations
		{
			get
			{
				return animations != null;
			}
		}

		public Sampler[] Animations
		{
			get
			{
				if (animations == null)
				{
					animations = new Sampler[values.Length];
				}
				return animations;
			}
		}

		protected Transform(int valueCount)
		{
			values = new float[valueCount];
		}

		protected Transform(string sid, int valueCount)
		{
			this.sid = sid;
			values = new float[valueCount];
		}

		protected Sampler GetAnimation(int index)
		{
			if (animations == null)
			{
				return null;
			}
			return animations[index];
		}

		public void BindAnimation(string valueName, Sampler animation)
		{
			if (string.IsNullOrEmpty(valueName))
			{
				for (int i = 0; i < values.Length; i++)
				{
					BindAnimation(i, animation);
				}
				return;
			}
			int num = ParseValueIndex(valueName);
			if (num != -1)
			{
				BindAnimation(num, animation);
			}
		}

		private void BindAnimation(int index, Sampler animation)
		{
			if (animation.Inputs.Count == 0 || animation.Inputs[0].Source.Count == 0)
			{
				animation = null;
			}
			if (animation != null || HasAnimations)
			{
				Animations[index] = animation;
			}
		}

		private int ParseValueIndex(string name)
		{
			if (name[0] == '(')
			{
				int num = name.IndexOf(')', 1);
				if (num == -1)
				{
					return -1;
				}
				return int.Parse(name.Substring(1, num - 1).Trim());
			}
			return ValueNameToValueIndex(name);
		}

		public abstract int ValueNameToValueIndex(string name);

		public abstract string ValueIndexToValueName(int index);

		public abstract Matrix ToMatrix();
	}
}
