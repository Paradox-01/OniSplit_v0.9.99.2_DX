using System;
using System.Collections;
using System.Collections.Generic;

namespace Oni
{
	internal static class Enumerable
	{
		public static bool Any<T>(this IEnumerable<T> source)
		{
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					T current = enumerator.Current;
					return true;
				}
			}
			return false;
		}

		public static bool Any<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			foreach (T item in source)
			{
				if (predicate(item))
				{
					return true;
				}
			}
			return false;
		}

		public static bool All<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			foreach (T item in source)
			{
				if (!predicate(item))
				{
					return false;
				}
			}
			return true;
		}

		public static IEnumerable<T> OfType<T>(this IEnumerable source) where T : class
		{
			foreach (object item in source)
			{
				T val = item as T;
				if (val != null)
				{
					yield return val;
				}
			}
		}

		public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source) where T : class
		{
			Dictionary<T, bool> set = new Dictionary<T, bool>();
			bool hasNull = false;
			foreach (T item in source)
			{
				if (item == null)
				{
					if (!hasNull)
					{
						hasNull = true;
						yield return null;
					}
				}
				else if (!set.ContainsKey(item))
				{
					set.Add(item, true);
					yield return item;
				}
			}
		}

		public static int Count<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			int num = 0;
			foreach (T item in source)
			{
				if (predicate(item))
				{
					num++;
				}
			}
			return num;
		}

		public static IEnumerable<T> Concatenate<T>(this IEnumerable<T> first, IEnumerable<T> second)
		{
			foreach (T item in first)
			{
				yield return item;
			}
			foreach (T item2 in second)
			{
				yield return item2;
			}
		}

		public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			foreach (T item in source)
			{
				if (predicate(item))
				{
					yield return item;
				}
			}
		}

		public static bool IsEmpty<T>(this IEnumerable<T> source)
		{
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					T current = enumerator.Current;
					return true;
				}
			}
			return false;
		}

		public static T First<T>(this IEnumerable<T> source)
		{
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			throw new InvalidOperationException();
		}

		public static T First<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			foreach (T item in source)
			{
				if (predicate(item))
				{
					return item;
				}
			}
			throw new InvalidOperationException();
		}

		public static T FirstOrDefault<T>(this IEnumerable<T> source)
		{
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return default(T);
		}

		public static T FirstOrDefault<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			foreach (T item in source)
			{
				if (predicate(item))
				{
					return item;
				}
			}
			return default(T);
		}

		public static IEnumerable<TOut> Select<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, TOut> selector)
		{
			foreach (TIn item in source)
			{
				yield return selector(item);
			}
		}

		public static IEnumerable<TOut> SelectMany<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, IEnumerable<TOut>> selector)
		{
			foreach (TIn item in source)
			{
				foreach (TOut item2 in selector(item))
				{
					yield return item2;
				}
			}
		}

		public static float Max(this IEnumerable<float> source)
		{
			float num = float.MinValue;
			foreach (float item in source)
			{
				num = Math.Max(num, item);
			}
			return num;
		}

		public static float Min<T>(this IEnumerable<T> source, Func<T, float> selector)
		{
			float num = float.MaxValue;
			foreach (T item in source)
			{
				num = Math.Min(num, selector(item));
			}
			return num;
		}

		public static int Min<T>(this IEnumerable<T> source, Func<T, int> selector)
		{
			int num = int.MaxValue;
			foreach (T item in source)
			{
				num = Math.Min(num, selector(item));
			}
			return num;
		}

		public static float Min(this IEnumerable<float> source)
		{
			float num = float.MaxValue;
			foreach (float item in source)
			{
				num = Math.Min(num, item);
			}
			return num;
		}

		public static float Max<T>(this IEnumerable<T> source, Func<T, float> selector)
		{
			float num = float.MinValue;
			foreach (T item in source)
			{
				num = Math.Max(num, selector(item));
			}
			return num;
		}

		public static int Max(this IEnumerable<int> source)
		{
			int num = int.MinValue;
			foreach (int item in source)
			{
				if (item > num)
				{
					num = item;
				}
			}
			return num;
		}

		public static int Max<T>(this IEnumerable<T> source, Func<T, int> selector)
		{
			int num = int.MinValue;
			foreach (T item in source)
			{
				int num2 = selector(item);
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}

		public static TOutput[] ConvertAll<TInput, TOutput>(this TInput[] input, Func<TInput, TOutput> converter)
		{
			TOutput[] array = new TOutput[input.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = converter(input[i]);
			}
			return array;
		}

		public static int Sum<T>(this IEnumerable<T> source, Func<T, int> selector)
		{
			int num = 0;
			foreach (T item in source)
			{
				num += selector(item);
			}
			return num;
		}

		public static IEnumerable<T> Repeat<T>(T value, int count)
		{
			for (int i = 0; i < count; i++)
			{
				yield return value;
			}
		}

		public static T[] ToArray<T>(this IEnumerable<T> source)
		{
			ICollection<T> collection = source as ICollection<T>;
			if (collection != null)
			{
				T[] array = new T[collection.Count];
				collection.CopyTo(array, 0);
				return array;
			}
			return new List<T>(source).ToArray();
		}

		public static List<T> ToList<T>(this IEnumerable<T> source)
		{
			return new List<T>(source);
		}

		public static IEnumerable<T> Ring<T>(this IEnumerable<T> source)
		{
			foreach (T item in source)
			{
				yield return item;
			}
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					yield return enumerator.Current;
				}
			}
		}

		public static IEnumerable<T> Skip<T>(this IEnumerable<T> source, int count)
		{
			foreach (T item in source)
			{
				if (count <= 0)
				{
					yield return item;
				}
				count--;
			}
		}
	}
}
