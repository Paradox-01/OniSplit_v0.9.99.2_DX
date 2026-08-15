using System.Collections.Generic;

namespace Oni.Particles
{
	internal class Event
	{
		private readonly EventType type;

		private readonly List<EventAction> actions;

		public EventType Type
		{
			get
			{
				return type;
			}
		}

		public List<EventAction> Actions
		{
			get
			{
				return actions;
			}
		}

		public Event(EventType type)
		{
			this.type = type;
			actions = new List<EventAction>();
		}

		public Event(EventType type, EventAction[] actions, int start, int length)
			: this(type)
		{
			for (int i = start; i < start + length; i++)
			{
				this.actions.Add(actions[i]);
			}
		}
	}
}
