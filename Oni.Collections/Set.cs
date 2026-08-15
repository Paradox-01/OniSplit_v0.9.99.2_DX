using System.Collections;
using System.Collections.Generic;

namespace Oni.Collections
{
	internal class Set<T> : IEnumerable<T>, IEnumerable
	{
		private readonly Dictionary<T, int> set;

		public int Count
		{
			get
			{
				return set.Count;
			}
		}

		public Set()
		{
			set = new Dictionary<T, int>();
		}

		public Set(IEqualityComparer<T> comparer)
		{
			set = new Dictionary<T, int>(comparer);
		}

		public bool Add(T t)
		{
			if (set.ContainsKey(t))
			{
				return false;
			}
			set.Add(t, 0);
			return true;
		}

		public bool Contains(T t)
		{
			return set.ContainsKey(t);
		}

		public void UnionWith(IEnumerable<T> with)
		{
			foreach (T item in with)
			{
				set[item] = 0;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			foreach (KeyValuePair<T, int> item in set)
			{
				yield return item.Key;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
