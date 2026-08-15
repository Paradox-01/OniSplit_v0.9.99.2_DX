namespace Oni.Motoko
{
	internal class GeometryDatWriter
	{
		private Geometry geometry;

		private ImporterFile importer;

		public static ImporterDescriptor Write(Geometry geometry, ImporterFile importer)
		{
			GeometryDatWriter geometryDatWriter = new GeometryDatWriter
			{
				geometry = geometry,
				importer = importer
			};
			return geometryDatWriter.WriteGeometry();
		}

		private ImporterDescriptor WriteGeometry()
		{
			int[] array = Stripify.FromTriangleList(geometry.Triangles);
			int[] array2 = Stripify.ToTriangleList(array);
			Vector3[] array3 = new Vector3[array2.Length / 3];
			int[] array4 = new int[array3.Length];
			for (int i = 0; i < array2.Length; i += 3)
			{
				Vector3 vector = geometry.Points[array2[i]];
				Vector3 vector2 = geometry.Points[array2[i + 1]];
				Vector3 vector3 = geometry.Points[array2[i + 2]];
				Vector3 vector4 = Vector3.Normalize(Vector3.Cross(vector2 - vector, vector3 - vector));
				int num = i / 3;
				array3[num] = vector4;
				array4[num] = num;
			}
			ImporterDescriptor importerDescriptor = importer.CreateInstance(TemplateTag.M3GM, geometry.Name);
			ImporterDescriptor descriptor = importer.CreateInstance(TemplateTag.PNTA);
			ImporterDescriptor descriptor2 = importer.CreateInstance(TemplateTag.VCRA);
			ImporterDescriptor descriptor3 = importer.CreateInstance(TemplateTag.VCRA);
			ImporterDescriptor descriptor4 = importer.CreateInstance(TemplateTag.TXCA);
			ImporterDescriptor descriptor5 = importer.CreateInstance(TemplateTag.IDXA);
			ImporterDescriptor descriptor6 = importer.CreateInstance(TemplateTag.IDXA);
			using (BinaryWriter binaryWriter = importerDescriptor.OpenWrite(4))
			{
				binaryWriter.Write(descriptor);
				binaryWriter.Write(descriptor2);
				binaryWriter.Write(descriptor3);
				binaryWriter.Write(descriptor4);
				binaryWriter.Write(descriptor5);
				binaryWriter.Write(descriptor6);
				if (geometry.TextureName != null)
				{
					binaryWriter.Write(importer.CreateInstance(TemplateTag.TXMP, geometry.TextureName));
				}
				else
				{
					binaryWriter.Write(0);
				}
				binaryWriter.Skip(4);
			}
			descriptor.WritePoints(geometry.Points);
			descriptor2.WriteVectors(geometry.Normals);
			descriptor3.WriteVectors(array3);
			descriptor4.WriteTexCoords(geometry.TexCoords);
			descriptor5.WriteIndices(array);
			descriptor6.WriteIndices(array4);
			return importerDescriptor;
		}
	}
}
