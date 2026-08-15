using System;
using System.Collections.Generic;

namespace Oni.Motoko
{
	internal static class GeometryDatReader
	{
		public static Geometry Read(InstanceDescriptor m3gm)
		{
			if (m3gm.Template.Tag != TemplateTag.M3GM)
			{
				throw new ArgumentException(string.Format("Invalid instance type {0}", m3gm.Template.Tag), "m3gm");
			}
			InstanceDescriptor instanceDescriptor;
			InstanceDescriptor instanceDescriptor2;
			InstanceDescriptor instanceDescriptor3;
			InstanceDescriptor instanceDescriptor4;
			InstanceDescriptor instanceDescriptor5;
			InstanceDescriptor instanceDescriptor6;
			InstanceDescriptor texture;
			using (BinaryReader binaryReader = m3gm.OpenRead(4))
			{
				instanceDescriptor = binaryReader.ReadInstance();
				instanceDescriptor2 = binaryReader.ReadInstance();
				instanceDescriptor3 = binaryReader.ReadInstance();
				instanceDescriptor4 = binaryReader.ReadInstance();
				instanceDescriptor5 = binaryReader.ReadInstance();
				instanceDescriptor6 = binaryReader.ReadInstance();
				texture = binaryReader.ReadInstance();
			}
			Geometry geometry = new Geometry
			{
				Name = m3gm.FullName,
				Texture = texture
			};
			using (BinaryReader binaryReader2 = instanceDescriptor.OpenRead(52))
			{
				geometry.Points = binaryReader2.ReadVector3Array(binaryReader2.ReadInt32());
			}
			using (BinaryReader binaryReader3 = instanceDescriptor2.OpenRead(20))
			{
				geometry.Normals = binaryReader3.ReadVector3Array(binaryReader3.ReadInt32());
			}
			Vector3[] fNormals;
			using (BinaryReader binaryReader4 = instanceDescriptor3.OpenRead(20))
			{
				fNormals = binaryReader4.ReadVector3Array(binaryReader4.ReadInt32());
			}
			using (BinaryReader binaryReader5 = instanceDescriptor4.OpenRead(20))
			{
				geometry.TexCoords = binaryReader5.ReadVector2Array(binaryReader5.ReadInt32());
			}
			int[] vIndices;
			using (BinaryReader binaryReader6 = instanceDescriptor5.OpenRead(20))
			{
				vIndices = binaryReader6.ReadInt32Array(binaryReader6.ReadInt32());
			}
			int[] fIndices;
			using (BinaryReader binaryReader7 = instanceDescriptor6.OpenRead(20))
			{
				fIndices = binaryReader7.ReadInt32Array(binaryReader7.ReadInt32());
			}
			geometry.Triangles = ConvertTriangleStripToTriangleList(geometry.Points, vIndices, fNormals, fIndices);
			return geometry;
		}

		private static int[] ConvertTriangleStripToTriangleList(Vector3[] points, int[] vIndices, Vector3[] fNormals, int[] fIndices)
		{
			List<int> list = new List<int>(vIndices.Length * 2);
			int[] array = new int[3];
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < vIndices.Length; i++)
			{
				if (vIndices[i] < 0)
				{
					array[0] = vIndices[i++] & 0x7FFFFFFF;
					array[1] = vIndices[i++];
					num2 = 0;
				}
				else
				{
					array[num2] = array[2];
					num2 ^= 1;
				}
				array[2] = vIndices[i];
				Vector3 vector = points[array[0]];
				Vector3 vector2 = points[array[1]];
				Vector3 vector3 = points[array[2]];
				Vector3 v = Vector3.Normalize(fNormals[fIndices[num]]);
				Vector3 v2 = Vector3.Normalize(Vector3.Cross(vector2 - vector, vector3 - vector));
				if (Vector3.Dot(v, v2) < 0f)
				{
					list.Add(array[2]);
					list.Add(array[1]);
					list.Add(array[0]);
				}
				else
				{
					list.Add(array[0]);
					list.Add(array[1]);
					list.Add(array[2]);
				}
				num++;
			}
			return list.ToArray();
		}
	}
}
