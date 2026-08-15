namespace Oni.Dae
{
	internal class Camera : Entity
	{
		public CameraType Type { get; set; }

		public float XMag { get; set; }

		public float YMag { get; set; }

		public float XFov { get; set; }

		public float YFov { get; set; }

		public float AspectRatio { get; set; }

		public float ZNear { get; set; }

		public float ZFar { get; set; }
	}
}
