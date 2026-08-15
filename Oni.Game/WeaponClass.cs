using System;

namespace Oni.Game
{
	internal class WeaponClass
	{
		private InstanceDescriptor geometry;

		public InstanceDescriptor Geometry
		{
			get
			{
				return geometry;
			}
		}

		public static WeaponClass Read(InstanceDescriptor descriptor)
		{
			if (descriptor.Template.Tag != TemplateTag.ONWC)
			{
				throw new ArgumentException(string.Format("The specified descriptor has a wrong template {0}", descriptor.Template.Tag), "descriptor");
			}
			WeaponClass weaponClass = new WeaponClass();
			using (BinaryReader binaryReader = descriptor.OpenRead(88))
			{
				weaponClass.geometry = binaryReader.ReadInstance();
				return weaponClass;
			}
		}
	}
}
