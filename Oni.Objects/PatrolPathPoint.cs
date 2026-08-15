using System.Collections.Generic;

namespace Oni.Objects
{
	internal class PatrolPathPoint
	{
		public PatrolPathPointType Type;

		public readonly Dictionary<string, object> Attributes = new Dictionary<string, object>();

		public PatrolPathPoint(PatrolPathPointType type)
		{
			Type = type;
		}
	}
}
