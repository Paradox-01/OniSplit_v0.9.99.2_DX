namespace Oni.Dae
{
	internal class Light : Entity
	{
		public LightType Type { get; set; }

		public Vector3 Color { get; set; }

		public float ConstantAttenuation { get; set; }

		public float LinearAttenuation { get; set; }

		public float QuadraticAttenuation { get; set; }

		public float FalloffAngle { get; set; }

		public float FalloffExponent { get; set; }

		public float ZFar { get; set; }
	}
}
