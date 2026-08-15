using System;
using System.Collections;
using System.Collections.Generic;

namespace Oni
{
	internal class TreeIterator<T> : IEnumerable<T>, IEnumerable
	{
		public class Enumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			public T Current
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return Current;
				}
			}

			public bool MoveNext()
			{
				throw new NotImplementedException();
			}

			public void Dispose()
			{
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}
		}

		private readonly IEnumerable<T> roots;

		private readonly Func<T, IEnumerable<T>> children;

		public TreeIterator(IEnumerable<T> roots, Func<T, IEnumerable<T>> children)
		{
			this.roots = roots;
			this.children = children;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return new Enumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator();
		}
	}
}
