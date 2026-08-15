namespace Oni.Akira
{
	internal abstract class BspNode<T> where T : BspNode<T>
	{
		public readonly Plane Plane;

		public readonly T BackChild;

		public readonly T FrontChild;

		protected BspNode(Plane plane, T backChild, T frontChild)
		{
			Plane = plane;
			BackChild = backChild;
			FrontChild = frontChild;
		}
	}
}
