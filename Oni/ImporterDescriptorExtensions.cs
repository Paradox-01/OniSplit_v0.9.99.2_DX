using System.Collections.Generic;

namespace Oni
{
	internal static class ImporterDescriptorExtensions
	{
		public static void WriteIndices(this ImporterDescriptor descriptor, int[] indices)
		{
			using (BinaryWriter binaryWriter = descriptor.OpenWrite(20))
			{
				binaryWriter.Write(indices.Length);
				binaryWriter.Write(indices);
			}
		}

		public static void WritePoints(this ImporterDescriptor descriptor, ICollection<Vector3> points)
		{
			BoundingBox bbox = BoundingBox.CreateFromPoints(points);
			BoundingSphere bsphere = BoundingSphere.CreateFromPoints(points);
			using (BinaryWriter binaryWriter = descriptor.OpenWrite(12))
			{
				binaryWriter.Write(bbox);
				binaryWriter.Write(bsphere);
				binaryWriter.Write(points.Count);
				binaryWriter.Write(points);
			}
		}

		public static void WriteTexCoords(this ImporterDescriptor descriptor, ICollection<Vector2> texCoords)
		{
			using (BinaryWriter binaryWriter = descriptor.OpenWrite(20))
			{
				binaryWriter.Write(texCoords.Count);
				binaryWriter.Write(texCoords);
			}
		}

		public static void WriteVectors(this ImporterDescriptor descriptor, ICollection<Vector3> vectors)
		{
			using (BinaryWriter binaryWriter = descriptor.OpenWrite(20))
			{
				binaryWriter.Write(vectors.Count);
				binaryWriter.Write(vectors);
			}
		}

		public static void WritePlanes(this ImporterDescriptor descriptor, ICollection<Plane> planes)
		{
			using (BinaryWriter binaryWriter = descriptor.OpenWrite(20))
			{
				binaryWriter.Write(planes.Count);
				binaryWriter.Write(planes);
			}
		}
	}
}
