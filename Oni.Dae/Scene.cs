namespace Oni.Dae
{
	internal class Scene : Node
	{
		private bool customAxisConversion;

		private bool sceneZUP;

		public bool CustomAxisConversion
		{
			get
			{
				return customAxisConversion;
			}
			set
			{
				customAxisConversion = value;
			}
		}

		public bool SceneZUP
		{
			get
			{
				return sceneZUP;
			}
			set
			{
				sceneZUP = value;
			}
		}
	}
}
