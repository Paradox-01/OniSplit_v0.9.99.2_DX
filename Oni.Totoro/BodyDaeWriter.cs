using Oni.Dae;
using Oni.Motoko;

namespace Oni.Totoro
{
	internal class BodyDaeWriter
	{
		private static readonly Vector3[] defaultPose = new Vector3[19]
		{
			new Vector3(0f, 90f, 90f),
			new Vector3(0f, 180f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 180f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(90f, 90f, 90f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(-90f, 0f, 0f),
			new Vector3(-90f, -90f, 90f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(90f, 0f, 0f)
		};

		private readonly GeometryDaeWriter geometryWriter;

		public BodyDaeWriter(GeometryDaeWriter geometryWriter)
		{
			this.geometryWriter = geometryWriter;
		}

		public Node Write(Body body, bool noAnimation, InstanceDescriptor[] textures)
		{
			return WriteNode(body.Root, noAnimation, textures);
		}

		private Node WriteNode(BodyNode bodyNode, bool useDefaultPose, InstanceDescriptor[] textures)
		{
			if (textures != null)
			{
				bodyNode.Geometry.Texture = textures[bodyNode.Index];
			}
			Node node = geometryWriter.WriteNode(bodyNode.Geometry, bodyNode.Name);
			node.Transforms.Translate("pos", bodyNode.Translation);
			if (useDefaultPose)
			{
				Vector3 vector = defaultPose[bodyNode.Index];
				node.Transforms.Rotate("rotX", Vector3.Right, vector.X);
				node.Transforms.Rotate("rotY", Vector3.Up, vector.Y);
				node.Transforms.Rotate("rotZ", Vector3.Backward, vector.Z);
			}
			else
			{
				node.Transforms.Rotate("rotX", Vector3.Right, 0f);
				node.Transforms.Rotate("rotY", Vector3.Up, 0f);
				node.Transforms.Rotate("rotZ", Vector3.Backward, 0f);
			}
			foreach (BodyNode node2 in bodyNode.Nodes)
			{
				node.Nodes.Add(WriteNode(node2, useDefaultPose, textures));
			}
			return node;
		}
	}
}
