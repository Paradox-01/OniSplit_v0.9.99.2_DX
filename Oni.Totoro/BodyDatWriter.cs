using System.Collections.Generic;
using Oni.Motoko;

namespace Oni.Totoro
{
	internal static class BodyDatWriter
	{
		private struct NodeIndices
		{
			public byte ParentIndex;

			public byte FirstChildIndex;

			public byte SiblingIndex;
		}

		public static ImporterDescriptor Write(Body body, ImporterFile importer)
		{
			ImporterDescriptor importerDescriptor = importer.CreateInstance(TemplateTag.TRCM);
			ImporterDescriptor trga = importer.CreateInstance(TemplateTag.TRGA);
			ImporterDescriptor trta = importer.CreateInstance(TemplateTag.TRTA);
			ImporterDescriptor tria = importer.CreateInstance(TemplateTag.TRIA);
			List<BodyNode> nodes = body.Nodes;
			int count = nodes.Count;
			ImporterDescriptor[] array = new ImporterDescriptor[count];
			Vector3[] array2 = new Vector3[count];
			NodeIndices[] array3 = new NodeIndices[count];
			foreach (BodyNode item in nodes)
			{
				int index = item.Index;
				array[index] = GeometryDatWriter.Write(item.Geometry, importer);
				array2[index] = item.Translation;
				int count2 = item.Nodes.Count;
				if (count2 <= 0)
				{
					continue;
				}
				array3[index].FirstChildIndex = (byte)item.Nodes[0].Index;
				int num = count2 - 1;
				for (int i = 0; i < count2; i++)
				{
					int index2 = item.Nodes[i].Index;
					if (i != num)
					{
						array3[index2].SiblingIndex = (byte)item.Nodes[i + 1].Index;
					}
					array3[index2].ParentIndex = (byte)index;
				}
			}
			WriteTRCM(importerDescriptor, trga, trta, tria, count);
			WriteTRGA(trga, array);
			WriteTRTA(trta, array2);
			WriteTRIA(tria, array3);
			return importerDescriptor;
		}

		private static void WriteTRCM(ImporterDescriptor trcm, ImporterDescriptor trga, ImporterDescriptor trta, ImporterDescriptor tria, int nodeCount)
		{
			using (BinaryWriter binaryWriter = trcm.OpenWrite(4))
			{
				binaryWriter.WriteInt16(nodeCount);
				binaryWriter.Skip(78);
				binaryWriter.Write(trga);
				binaryWriter.Write(trta);
				binaryWriter.Write(tria);
			}
		}

		private static void WriteTRGA(ImporterDescriptor trga, ImporterDescriptor[] descriptors)
		{
			using (BinaryWriter binaryWriter = trga.OpenWrite(22))
			{
				binaryWriter.WriteInt16(descriptors.Length);
				binaryWriter.Write(descriptors);
			}
		}

		private static void WriteTRTA(ImporterDescriptor trta, Vector3[] translations)
		{
			using (BinaryWriter binaryWriter = trta.OpenWrite(22))
			{
				binaryWriter.WriteInt16(translations.Length);
				binaryWriter.Write(translations);
			}
		}

		private static void WriteTRIA(ImporterDescriptor tria, NodeIndices[] indices)
		{
			using (BinaryWriter binaryWriter = tria.OpenWrite(22))
			{
				binaryWriter.WriteInt16(indices.Length);
				for (int i = 0; i < indices.Length; i++)
				{
					NodeIndices nodeIndices = indices[i];
					binaryWriter.WriteByte(nodeIndices.ParentIndex);
					binaryWriter.WriteByte(nodeIndices.FirstChildIndex);
					binaryWriter.WriteByte(nodeIndices.SiblingIndex);
					binaryWriter.WriteByte(0);
				}
			}
		}
	}
}
