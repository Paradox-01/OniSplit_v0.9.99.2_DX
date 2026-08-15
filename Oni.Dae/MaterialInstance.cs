using System.Collections.Generic;

namespace Oni.Dae
{
	internal class MaterialInstance : Instance<Material>
	{
		private readonly List<MaterialBinding> bindings = new List<MaterialBinding>();

		public string Symbol { get; set; }

		public List<MaterialBinding> Bindings
		{
			get
			{
				return bindings;
			}
		}

		public MaterialInstance()
		{
		}

		public MaterialInstance(string symbol, Material material)
			: base(material)
		{
			Symbol = symbol;
		}
	}
}
