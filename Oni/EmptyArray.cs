using System.Runtime.InteropServices;

namespace Oni
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	internal struct EmptyArray<T>
	{
		public static readonly T[] Value = new T[0];
	}
}
