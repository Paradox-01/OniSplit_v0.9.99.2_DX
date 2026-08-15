using System;
using System.Collections.Generic;

namespace Oni.Motoko
{
	internal class Stripify
	{
		private struct Edge : IEquatable<Edge>
		{
			public readonly int V1;

			public readonly int V2;

			public Edge(int V1, int V2)
			{
				this.V1 = V1;
				this.V2 = V2;
			}

			public static bool operator ==(Edge e1, Edge e2)
			{
				if (e1.V1 == e2.V1)
				{
					return e1.V2 == e2.V2;
				}
				return false;
			}

			public static bool operator !=(Edge e1, Edge e2)
			{
				if (e1.V1 == e2.V1)
				{
					return e1.V2 != e2.V2;
				}
				return true;
			}

			public bool Equals(Edge edge)
			{
				if (V1 == edge.V1)
				{
					return V2 == edge.V2;
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				if (obj is Edge)
				{
					return Equals((Edge)obj);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return V1 ^ V2;
			}
		}

		private const int BeginStrip = int.MinValue;

		private int[] tlist;

		private int[] adjacency;

		private int[] degree;

		private List<int> strips;

		private bool[] used;

		public static int[] FromTriangleList(int[] triangleList)
		{
			Stripify stripify = new Stripify(triangleList);
			return stripify.Run();
		}

		public static int[] ToTriangleList(int[] triangleStrips)
		{
			int num = 0;
			for (int i = 0; i < triangleStrips.Length; i++)
			{
				num++;
				if (triangleStrips[i] < 0)
				{
					num -= 2;
				}
			}
			int[] array = new int[num * 3];
			int num2 = 0;
			int[] array2 = new int[3];
			int num3 = 0;
			for (int j = 0; j < triangleStrips.Length; j++)
			{
				if (triangleStrips[j] < 0)
				{
					array2[0] = triangleStrips[j] & 0x7FFFFFFF;
					j++;
					array2[1] = triangleStrips[j];
					j++;
					num3 = 0;
				}
				else
				{
					array2[num3] = array2[2];
					num3 = (num3 + 1) % 2;
				}
				array2[2] = triangleStrips[j];
				Array.Copy(array2, 0, array, num2, 3);
				num2 += 3;
			}
			return array;
		}

		private Stripify(int[] triangleList)
		{
			tlist = triangleList;
		}

		private int[] Run()
		{
			strips = new List<int>();
			GenerateAdjacency();
			while (GenerateStrip())
			{
			}
			for (int i = 0; i < degree.Length; i++)
			{
				if (!used[i])
				{
					int num = i * 3;
					strips.Add(tlist[num] | int.MinValue);
					strips.Add(tlist[num + 1]);
					strips.Add(tlist[num + 2]);
					used[i] = true;
				}
			}
			return strips.ToArray();
		}

		private bool GenerateStrip()
		{
			int num = -1;
			int num2 = 4;
			int num3 = 4;
			for (int i = 0; i < degree.Length; i++)
			{
				if (used[i] || degree[i] == 0)
				{
					continue;
				}
				if (degree[i] < num2)
				{
					num2 = degree[i];
					num3 = 4;
					num = i;
				}
				else
				{
					if (degree[i] != num2)
					{
						continue;
					}
					for (int j = 0; j < 3; j++)
					{
						int num4 = adjacency[i * 3 + j];
						if (num4 != -1 && !used[num4] && degree[num4] != 0 && degree[num4] < num3)
						{
							num3 = degree[num4];
							num = i;
						}
					}
				}
			}
			if (num == -1)
			{
				return false;
			}
			UseTriangle(num);
			int num5 = -1;
			int num6 = 0;
			num2 = 4;
			for (int k = 0; k < 3; k++)
			{
				int num7 = adjacency[num * 3 + k];
				if (num7 != -1 && !used[num7] && degree[num7] < num2)
				{
					num2 = degree[num7];
					num5 = num7;
					num6 = k;
				}
			}
			int[] array = new int[3]
			{
				tlist[num * 3 + (num6 + 2) % 3],
				tlist[num * 3 + num6 % 3],
				tlist[num * 3 + (num6 + 1) % 3]
			};
			strips.Add(array[0] | int.MinValue);
			strips.Add(array[1]);
			strips.Add(array[2]);
			int num8 = 0;
			while (num5 != -1)
			{
				UseTriangle(num5);
				array[0] = array[1 + num8];
				for (int l = 0; l < 3; l++)
				{
					int num9 = num5 * 3;
					if (tlist[num9 + l] == array[(2 + num8) % 3] && tlist[num9 + (l + 1) % 3] == array[num8])
					{
						num6 = (l + 2 - num8) % 3;
						array[1 + num8] = tlist[num9 + (l + 2) % 3];
						break;
					}
				}
				strips.Add(array[1 + num8]);
				num = num5;
				num5 = adjacency[num * 3 + num6];
				if (num5 == -1 || used[num5])
				{
					break;
				}
				UseTriangle(num5);
				num8 = (num8 + 1) % 2;
			}
			return true;
		}

		private void UseTriangle(int t)
		{
			degree[t] = 0;
			used[t] = true;
			for (int i = 0; i < 3; i++)
			{
				int num = adjacency[t * 3 + i];
				if (num != -1 && degree[num] > 0)
				{
					degree[num]--;
				}
			}
		}

		private void GenerateAdjacency()
		{
			adjacency = new int[tlist.Length];
			degree = new int[tlist.Length / 3];
			used = new bool[tlist.Length / 3];
			for (int i = 0; i < adjacency.Length; i++)
			{
				adjacency[i] = -1;
			}
			Dictionary<Edge, int> dictionary = new Dictionary<Edge, int>();
			for (int j = 0; j < tlist.Length; j += 3)
			{
				for (int k = 0; k < 3; k++)
				{
					Edge key = new Edge(tlist[j + k], tlist[j + (k + 1) % 3]);
					dictionary[key] = j / 3;
				}
			}
			for (int l = 0; l < tlist.Length; l += 3)
			{
				for (int m = 0; m < 3; m++)
				{
					if (adjacency[l + m] == -1)
					{
						Edge key2 = new Edge(tlist[l + (m + 1) % 3], tlist[l + m]);
						int value;
						if (dictionary.TryGetValue(key2, out value) && value != l / 3)
						{
							adjacency[l + m] = value;
							degree[l / 3]++;
						}
					}
				}
			}
		}
	}
}
