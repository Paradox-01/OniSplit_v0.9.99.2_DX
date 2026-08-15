using System;
using Oni.Game;
using Oni.Motoko;

namespace Oni.Totoro
{
	internal static class BodyDatReader
	{
		private struct NodeIndices
		{
			public byte ParentIndex;

			public byte FirstChildIndex;

			public byte SiblingIndex;
		}

		public static Body Read(InstanceDescriptor source)
		{
			InstanceDescriptor instanceDescriptor = ReadTRCM(source);
			InstanceDescriptor trga;
			InstanceDescriptor trta;
			InstanceDescriptor tria;
			using (BinaryReader binaryReader = instanceDescriptor.OpenRead(84))
			{
				trga = binaryReader.ReadInstance();
				trta = binaryReader.ReadInstance();
				tria = binaryReader.ReadInstance();
			}
			Geometry[] array = ReadTRGA(trga);
			Vector3[] array2 = ReadTRTA(trta);
			NodeIndices[] array3 = ReadTRIA(tria);
			BodyNode[] array4 = new BodyNode[array.Length];
			for (int i = 0; i < array4.Length; i++)
			{
				array4[i] = new BodyNode
				{
					Name = BodyNode.Names[i],
					Index = i,
					Geometry = array[i],
					Translation = array2[i]
				};
			}
			for (int j = 0; j < array4.Length; j++)
			{
				BodyNode bodyNode = array4[j];
				for (int num = array3[j].FirstChildIndex; num != 0; num = array3[num].SiblingIndex)
				{
					array4[num].Parent = bodyNode;
					bodyNode.Nodes.Add(array4[num]);
				}
			}
			Body body = new Body();
			body.Nodes.AddRange(array4);
			return body;
		}

		private static InstanceDescriptor ReadTRCM(InstanceDescriptor source)
		{
			if (source.Template.Tag == TemplateTag.TRCM)
			{
				return source;
			}
			if (source.Template.Tag == TemplateTag.ONCC)
			{
				source = CharacterClass.Read(source).Body;
			}
			if (source.Template.Tag != TemplateTag.TRBS)
			{
				throw new InvalidOperationException(string.Format("Invalid body source type {0}", source.Template.Tag));
			}
			return ReadTRBS(source).Last();
		}

		private static InstanceDescriptor[] ReadTRBS(InstanceDescriptor trbs)
		{
			using (BinaryReader binaryReader = trbs.OpenRead())
			{
				return binaryReader.ReadInstanceArray(5);
			}
		}

		private static Geometry[] ReadTRGA(InstanceDescriptor trga)
		{
			InstanceDescriptor[] array;
			using (BinaryReader binaryReader = trga.OpenRead(22))
			{
				array = binaryReader.ReadInstanceArray(binaryReader.ReadInt16());
			}
			Geometry[] array2 = new Geometry[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = GeometryDatReader.Read(array[i]);
			}
			return array2;
		}

		private static Vector3[] ReadTRTA(InstanceDescriptor trta)
		{
			using (BinaryReader binaryReader = trta.OpenRead(22))
			{
				return binaryReader.ReadVector3Array(binaryReader.ReadInt16());
			}
		}

		private static NodeIndices[] ReadTRIA(InstanceDescriptor tria)
		{
			using (BinaryReader binaryReader = tria.OpenRead(22))
			{
				NodeIndices[] array = new NodeIndices[binaryReader.ReadInt16()];
				for (int i = 0; i < array.Length; i++)
				{
					array[i].ParentIndex = binaryReader.ReadByte();
					array[i].FirstChildIndex = binaryReader.ReadByte();
					array[i].SiblingIndex = binaryReader.ReadByte();
					binaryReader.Skip(1);
				}
				return array;
			}
		}
	}
}
