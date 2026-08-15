using System;
using System.Collections.Generic;
using System.Threading;

namespace Oni
{
	internal static class Parallel
	{
		public static void ForEach<T>(IEnumerable<T> items, Action<T> action)
		{
			T[] array = items.ToArray();
			if (array.Length == 0)
			{
				return;
			}
			if (array.Length == 1)
			{
				action(array[0]);
				return;
			}
			int processorCount = Environment.ProcessorCount;
			if (processorCount == 1)
			{
				T[] array2 = array;
				foreach (T arg in array2)
				{
					action(arg);
				}
				return;
			}
			Thread thread = new Thread((ThreadStart)delegate
			{
				for (int j = array.Length / 2; j < array.Length; j++)
				{
					action(array[j]);
				}
			});
			thread.Start();
			for (int num = 0; num < array.Length / 2; num++)
			{
				action(array[num]);
			}
			thread.Join();
		}
	}
}
