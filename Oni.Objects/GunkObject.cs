namespace Oni.Objects
{
	internal abstract class GunkObject : ObjectBase
	{
		private GunkObjectClass gunkClass;

		private string className;

		public GunkObjectClass GunkClass
		{
			get
			{
				return gunkClass;
			}
			protected set
			{
				gunkClass = value;
				if (value != null)
				{
					className = value.Name;
				}
			}
		}

		public string ClassName
		{
			get
			{
				return className;
			}
			protected set
			{
				className = value;
			}
		}

		public int GunkId
		{
			get
			{
				return ((int)base.TypeId << 24) | base.ObjectId;
			}
		}
	}
}
