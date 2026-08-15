namespace Oni.Dae
{
	internal abstract class Instance
	{
		public string Sid { get; set; }

		public string Name { get; set; }
	}
	internal abstract class Instance<T> : Instance
	{
		private T target;

		public T Target { get; set; }

		public Instance()
		{
		}

		public Instance(T target)
		{
			Target = target;
		}
	}
}
