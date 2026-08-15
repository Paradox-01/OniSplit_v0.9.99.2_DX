using System.Collections.Generic;

namespace Oni.Physics
{
	internal class ObjectAnimation
	{
		public string Name;

		public ObjectAnimationFlags Flags;

		public int Length;

		public int Stop;

		public ObjectAnimationKey[] Keys;

		public List<ObjectAnimationKey> Interpolate()
		{
			List<ObjectAnimationKey> list = new List<ObjectAnimationKey>(Length);
			for (int i = 1; i < Keys.Length; i++)
			{
				ObjectAnimationKey objectAnimationKey = Keys[i - 1];
				ObjectAnimationKey objectAnimationKey2 = Keys[i];
				list.Add(objectAnimationKey);
				for (int j = objectAnimationKey.Time + 1; j < objectAnimationKey2.Time; j++)
				{
					float amount = (float)(j - objectAnimationKey.Time) / (float)(objectAnimationKey2.Time - objectAnimationKey.Time);
					list.Add(new ObjectAnimationKey
					{
						Time = j,
						Translation = Vector3.Lerp(objectAnimationKey.Translation, objectAnimationKey2.Translation, amount),
						Rotation = Quaternion.Lerp(objectAnimationKey.Rotation, objectAnimationKey2.Rotation, amount),
						Scale = Vector3.Lerp(objectAnimationKey.Scale, objectAnimationKey2.Scale, amount)
					});
				}
			}
			list.Add(Keys.Last());
			return list;
		}
	}
}
