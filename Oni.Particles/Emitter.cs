namespace Oni.Particles
{
	internal class Emitter
	{
		private string particleClass;

		private EmitterFlags flags;

		private int turnOffTreshold;

		private int probability;

		private float copies;

		private int linkTo;

		private EmitterRate rate;

		private EmitterPosition position;

		private EmitterDirection direction;

		private EmitterSpeed speed;

		private EmitterOrientation orientationDir;

		private EmitterOrientation orientationUp;

		private Value[] parameters;

		public EmitterFlags Flags
		{
			get
			{
				return flags;
			}
			set
			{
				flags = value;
			}
		}

		public int Probability
		{
			get
			{
				return probability;
			}
			set
			{
				probability = value;
			}
		}

		public int TurnOffTreshold
		{
			get
			{
				return turnOffTreshold;
			}
			set
			{
				turnOffTreshold = value;
			}
		}

		public int LinkTo
		{
			get
			{
				return linkTo;
			}
			set
			{
				linkTo = value;
			}
		}

		public string ParticleClass
		{
			get
			{
				return particleClass;
			}
			set
			{
				particleClass = value;
			}
		}

		public float Copies
		{
			get
			{
				return copies;
			}
			set
			{
				copies = value;
			}
		}

		public EmitterRate Rate
		{
			get
			{
				return rate;
			}
			set
			{
				rate = value;
			}
		}

		public EmitterPosition Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
			}
		}

		public EmitterDirection Direction
		{
			get
			{
				return direction;
			}
			set
			{
				direction = value;
			}
		}

		public EmitterSpeed Speed
		{
			get
			{
				return speed;
			}
			set
			{
				speed = value;
			}
		}

		public EmitterOrientation OrientationDir
		{
			get
			{
				return orientationDir;
			}
			set
			{
				orientationDir = value;
			}
		}

		public EmitterOrientation OrientationUp
		{
			get
			{
				return orientationUp;
			}
			set
			{
				orientationUp = value;
			}
		}

		public Value[] Parameters
		{
			get
			{
				return parameters;
			}
		}

		public Emitter()
		{
			parameters = new Value[12];
		}

		public Emitter(BinaryReader reader)
		{
			particleClass = reader.ReadString(64);
			reader.Skip(4);
			flags = (EmitterFlags)reader.ReadInt32();
			turnOffTreshold = reader.ReadInt16();
			probability = reader.ReadUInt16();
			copies = reader.ReadSingle();
			linkTo = reader.ReadInt32();
			rate = (EmitterRate)reader.ReadInt32();
			position = (EmitterPosition)reader.ReadInt32();
			direction = (EmitterDirection)reader.ReadInt32();
			speed = (EmitterSpeed)reader.ReadInt32();
			orientationDir = (EmitterOrientation)reader.ReadInt32();
			orientationUp = (EmitterOrientation)reader.ReadInt32();
			parameters = new Value[12];
			for (int i = 0; i < parameters.Length; i++)
			{
				parameters[i] = Value.Read(reader);
			}
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write(particleClass, 64);
			writer.Skip(4);
			writer.Write((int)flags);
			writer.WriteInt16(turnOffTreshold);
			writer.WriteUInt16(probability);
			writer.Write(copies);
			writer.Write(linkTo);
			writer.Write((int)rate);
			writer.Write((int)position);
			writer.Write((int)direction);
			writer.Write((int)speed);
			writer.Write((int)orientationDir);
			writer.Write((int)orientationUp);
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i] != null)
				{
					parameters[i].Write(writer);
				}
				else
				{
					Value.Empty.Write(writer);
				}
			}
		}
	}
}
