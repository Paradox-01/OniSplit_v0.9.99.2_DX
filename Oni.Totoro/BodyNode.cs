using System.Collections.Generic;
using Oni.Dae;
using Oni.Motoko;

namespace Oni.Totoro
{
	internal class BodyNode
	{
		public static readonly string[] Names = new string[19]
		{
			"pelvis", "left_thigh", "left_calf", "left_foot", "right_thigh", "right_calf", "right_foot", "mid", "chest", "neck",
			"head", "left_shoulder", "left_biceps", "left_wrist", "left_handfist", "right_shoulder", "right_biceps", "right_wrist", "right_handfist"
		};

		public string Name;

		public int Index;

		public BodyNode Parent;

		public readonly List<BodyNode> Nodes = new List<BodyNode>();

		public Vector3 Translation;

		public Oni.Motoko.Geometry Geometry;

		public Node DaeNode;
	}
}
