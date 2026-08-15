using System;
using System.IO;
using Oni.Dae;
using Oni.Motoko;

namespace Oni.Totoro
{
	internal class BodyDaeReader
	{
		private Body body;

		private float shellOffset = 0.07f;

		private bool generateNormals;

		private bool flatNormals;

		private BodyDaeReader()
		{
		}

		public static Body Read(Scene scene)
		{
			BodyDaeReader bodyDaeReader = new BodyDaeReader
			{
				body = new Body()
			};
			bodyDaeReader.ReadBodyParts(scene);
			return bodyDaeReader.body;
		}

		public static Body Read(Scene scene, bool generateNormals, bool flatNormals, float shellOffset)
		{
			BodyDaeReader bodyDaeReader = new BodyDaeReader
			{
				body = new Body(),
				flatNormals = flatNormals,
				generateNormals = generateNormals,
				shellOffset = shellOffset
			};
			bodyDaeReader.ReadBodyParts(scene);
			return bodyDaeReader.body;
		}

		private void ReadBodyParts(Scene scene)
		{
			BodyNode bodyNode = FindRootNode(scene);
			if (bodyNode == null)
			{
				throw new InvalidDataException("The scene does not contain any geometry nodes.");
			}
			bodyNode.Translation = Vector3.Zero;
			if (body.Nodes.Count != 19)
			{
				Console.Error.WriteLine("Non standard bone count: {0}", body.Nodes.Count);
			}
		}

		private BodyNode FindRootNode(Node daeNode)
		{
			if (daeNode.GeometryInstances.Any())
			{
				return ReadNode(daeNode, null);
			}
			foreach (Node node in daeNode.Nodes)
			{
				BodyNode bodyNode = FindRootNode(node);
				if (bodyNode != null)
				{
					return bodyNode;
				}
			}
			return null;
		}

		private BodyNode ReadNode(Node daeNode, BodyNode parentNode)
		{
			BodyNode bodyNode = new BodyNode
			{
				DaeNode = daeNode,
				Parent = parentNode,
				Index = body.Nodes.Count
			};
			body.Nodes.Add(bodyNode);
			foreach (GeometryInstance item in daeNode.GeometryInstances.Where((GeometryInstance n) => n.Target != null))
			{
				Oni.Dae.Geometry target = item.Target;
				if (bodyNode.Geometry != null)
				{
					Console.Error.WriteLine("The node {0} contains more than one geometry. Only the first geometry will be used.", target.Name);
				}
				bodyNode.Geometry = GeometryDaeReader.Read(target, generateNormals, flatNormals, shellOffset);
			}
			bodyNode.Translation = daeNode.Transforms.ToMatrix().Translation;
			foreach (Node node in daeNode.Nodes)
			{
				bodyNode.Nodes.Add(ReadNode(node, parentNode));
			}
			return bodyNode;
		}
	}
}
