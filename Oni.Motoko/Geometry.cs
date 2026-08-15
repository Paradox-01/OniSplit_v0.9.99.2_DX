namespace Oni.Motoko
{
	internal class Geometry
	{
		private InstanceDescriptor texture;

		public string Name;

		public Vector3[] Points;

		public Vector3[] Normals;

		public Vector2[] TexCoords;

		public int[] Triangles;

		public string TextureName;

		public bool HasTransform;

		public Matrix Transform;

		public InstanceDescriptor Texture
		{
			get
			{
				return texture;
			}
			set
			{
				texture = value;
				TextureName = ((value == null) ? null : value.Name);
			}
		}
	}
}
