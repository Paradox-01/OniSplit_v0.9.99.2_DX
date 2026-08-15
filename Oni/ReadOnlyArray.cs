namespace Oni
{
	internal struct ReadOnlyArray<T>
	{
		private readonly T[] array;

		public int Length
		{
			get
			{
				return array.Length;
			}
		}

		public T this[int index]
		{
			get
			{
				return array[index];
			}
		}

		public ReadOnlyArray(T[] array)
		{
			this.array = array;
		}
	}
}
