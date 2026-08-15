using System.Collections.Generic;
using Oni.Imaging;

namespace Oni.Dae
{
	internal class Source : Entity
	{
		private float[] floatData;

		private string[] nameData;

		private int stride;

		private int count;

		public string[] NameData
		{
			get
			{
				return nameData;
			}
		}

		public float[] FloatData
		{
			get
			{
				return floatData;
			}
		}

		public int Count
		{
			get
			{
				return count;
			}
		}

		public int Stride
		{
			get
			{
				return stride;
			}
			set
			{
				stride = value;
				if (floatData != null)
				{
					count = floatData.Length / stride;
				}
				else
				{
					count = nameData.Length / stride;
				}
			}
		}

		public Source(IEnumerable<float> data, int stride)
		{
			floatData = data.ToArray();
			this.stride = stride;
			count = floatData.Length / stride;
		}

		public Source(float[] data, int stride)
		{
			floatData = (float[])data.Clone();
			this.stride = stride;
			count = data.Length / stride;
		}

		public Source(string[] data, int stride)
		{
			nameData = (string[])data.Clone();
			this.stride = stride;
			count = data.Length / stride;
		}

		public Source(IList<Vector2> data)
		{
			int num = data.Count;
			float[] array = new float[num * 2];
			for (int i = 0; i < num; i++)
			{
				array[i * 2] = data[i].X;
				array[i * 2 + 1] = 1f - data[i].Y;
			}
			floatData = array;
			count = num;
			stride = 2;
		}

		public Source(IList<Vector3> data)
		{
			int num = data.Count;
			float[] array = new float[num * 3];
			for (int i = 0; i < num; i++)
			{
				array[i * 3] = data[i].X;
				array[i * 3 + 1] = data[i].Y;
				array[i * 3 + 2] = data[i].Z;
			}
			floatData = array;
			count = num;
			stride = 3;
		}

		public static Vector2 ReaderVector2(Source source, int index)
		{
			float[] array = source.floatData;
			int num = index * source.stride;
			return new Vector2(array[num], array[num + 1]);
		}

		public static Vector2 ReadTexCoord(Source source, int index)
		{
			float[] array = source.floatData;
			int num = index * source.stride;
			return new Vector2(array[num], 1f - array[num + 1]);
		}

		public static Vector3 ReadVector3(Source source, int index)
		{
			float[] array = source.floatData;
			int num = index * source.stride;
			return new Vector3(array[num], array[num + 1], array[num + 2]);
		}

		public static Vector4 ReadVector4(Source source, int index)
		{
			float[] array = source.floatData;
			int num = index * source.stride;
			return new Vector4(array[num], array[num + 1], array[num + 2], array[num + 3]);
		}

		public static Color ReadColor(Source source, int index)
		{
			return new Color(ReadVector4(source, index));
		}
	}
}
