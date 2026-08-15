using System.Collections.Generic;

namespace Oni.Particles
{
	internal class EventAction
	{
		private readonly List<Value> parameters;

		private readonly List<VariableReference> variables;

		private readonly EventActionType type;

		public EventActionType Type
		{
			get
			{
				return type;
			}
		}

		public List<Value> Parameters
		{
			get
			{
				return parameters;
			}
		}

		public List<VariableReference> Variables
		{
			get
			{
				return variables;
			}
		}

		private EventAction()
		{
			parameters = new List<Value>();
			variables = new List<VariableReference>();
		}

		public EventAction(EventActionType type)
			: this()
		{
			this.type = type;
		}

		public EventAction(BinaryReader reader)
			: this()
		{
			type = (EventActionType)reader.ReadInt32();
			reader.ReadInt32();
			for (int i = 0; i < 8; i++)
			{
				VariableReference variableReference = new VariableReference(reader);
				if (variableReference.IsDefined)
				{
					variables.Add(variableReference);
				}
			}
			for (int j = 0; j < 8; j++)
			{
				Value value = Value.Read(reader);
				if (value != null)
				{
					parameters.Add(value);
				}
			}
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write((int)type);
			writer.Write(0);
			foreach (VariableReference variable in variables)
			{
				variable.Write(writer);
			}
			for (int i = variables.Count; i < 8; i++)
			{
				VariableReference.Empty.Write(writer);
			}
			foreach (Value parameter in parameters)
			{
				parameter.Write(writer);
			}
			for (int j = parameters.Count; j < 8; j++)
			{
				Value.Empty.Write(writer);
			}
		}
	}
}
