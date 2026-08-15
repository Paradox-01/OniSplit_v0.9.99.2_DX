using System.Collections.Generic;
using System.IO;

namespace Oni.Particles
{
	internal class Particle
	{
		private struct ActionRange
		{
			public bool IsEmpty
			{
				get
				{
					return First == Last;
				}
			}

			public int First { get; set; }

			public int Last { get; set; }

			internal ActionRange(BinaryReader reader)
			{
				First = reader.ReadUInt16();
				Last = reader.ReadUInt16();
			}
		}

		private ParticleFlags1 flags1;

		private ParticleFlags2 flags2;

		private SpriteType spriteType;

		private DisableDetailLevel disableDetailLevel;

		private Value lifetime;

		private Value collisionRadius;

		private float aiDodgeRadius;

		private float aiAlertRadius;

		private string flybySoundName;

		private Appearance appearance;

		private Attractor attractor;

		private List<Variable> variables;

		private List<Emitter> emitters;

		private List<Event> events;

		public ParticleFlags1 Flags1
		{
			get
			{
				return flags1 & ~ParticleFlags1.SpriteModeMask;
			}
			set
			{
				flags1 = value;
			}
		}

		public ParticleFlags2 Flags2
		{
			get
			{
				return (ParticleFlags2)((uint)flags2 & 0xFFFFFF9Fu);
			}
			set
			{
				flags2 = value;
			}
		}

		public SpriteType SpriteType
		{
			get
			{
				return spriteType;
			}
			set
			{
				spriteType = value;
			}
		}

		public DisableDetailLevel DisableDetailLevel
		{
			get
			{
				return disableDetailLevel;
			}
			set
			{
				disableDetailLevel = value;
			}
		}

		public string FlyBySoundName
		{
			get
			{
				return flybySoundName;
			}
			set
			{
				flybySoundName = value;
			}
		}

		public Value Lifetime
		{
			get
			{
				return lifetime;
			}
			set
			{
				lifetime = value;
			}
		}

		public Value CollisionRadius
		{
			get
			{
				return collisionRadius;
			}
			set
			{
				collisionRadius = value;
			}
		}

		public float AIDodgeRadius
		{
			get
			{
				return aiDodgeRadius;
			}
			set
			{
				aiDodgeRadius = value;
			}
		}

		public float AIAlertRadius
		{
			get
			{
				return aiAlertRadius;
			}
			set
			{
				aiAlertRadius = value;
			}
		}

		public Appearance Appearance
		{
			get
			{
				return appearance;
			}
		}

		public Attractor Attractor
		{
			get
			{
				return attractor;
			}
		}

		public List<Variable> Variables
		{
			get
			{
				return variables;
			}
		}

		public List<Emitter> Emitters
		{
			get
			{
				return emitters;
			}
		}

		public List<Event> Events
		{
			get
			{
				return events;
			}
		}

		public Particle()
		{
			lifetime = Value.FloatZero;
			collisionRadius = Value.FloatZero;
			appearance = new Appearance();
			attractor = new Attractor();
			variables = new List<Variable>();
			emitters = new List<Emitter>();
			events = new List<Event>();
		}

		public Particle(BinaryReader reader)
			: this()
		{
			appearance = new Appearance();
			attractor = new Attractor();
			reader.Skip(8);
			flags1 = (ParticleFlags1)reader.ReadInt32();
			flags2 = (ParticleFlags2)reader.ReadInt32();
			spriteType = (SpriteType)((int)(flags1 & ParticleFlags1.SpriteModeMask) >> 5);
			disableDetailLevel = (DisableDetailLevel)((int)(flags2 & ParticleFlags2.DisableLevelMask) >> 5);
			reader.Skip(4);
			int num = reader.ReadUInt16();
			int num2 = reader.ReadUInt16();
			int num3 = reader.ReadUInt16();
			reader.Skip(2);
			variables = new List<Variable>(num);
			emitters = new List<Emitter>(num2);
			events = new List<Event>(num3);
			ActionRange[] array = new ActionRange[16];
			for (int i = 0; i < 16; i++)
			{
				array[i] = new ActionRange(reader);
			}
			lifetime = Value.Read(reader);
			collisionRadius = Value.Read(reader);
			aiDodgeRadius = reader.ReadSingle();
			aiAlertRadius = reader.ReadSingle();
			flybySoundName = reader.ReadString(16);
			appearance = new Appearance(reader);
			attractor = new Attractor(reader);
			reader.Skip(12);
			for (int j = 0; j < num; j++)
			{
				variables.Add(new Variable(reader));
			}
			EventAction[] array2 = new EventAction[num2];
			for (int k = 0; k < num2; k++)
			{
				array2[k] = new EventAction(reader);
			}
			try
			{
				for (int l = 0; l < num3; l++)
				{
					emitters.Add(new Emitter(reader));
				}
			}
			catch (EndOfStreamException)
			{
			}
			for (int m = 0; m < array.Length; m++)
			{
				ActionRange actionRange = array[m];
				if (!actionRange.IsEmpty)
				{
					events.Add(new Event((EventType)m, array2, actionRange.First, actionRange.Last - actionRange.First));
				}
			}
		}

		public void Write(BinaryWriter writer)
		{
			List<EventAction> list = new List<EventAction>();
			ActionRange[] array = new ActionRange[16];
			int i;
			for (i = 0; i < array.Length; i++)
			{
				Event obj = events.Find((Event x) => x.Type == (EventType)i);
				ActionRange actionRange = new ActionRange
				{
					First = list.Count,
					Last = list.Count + ((obj != null) ? obj.Actions.Count : 0)
				};
				array[i] = actionRange;
				if (obj != null)
				{
					list.AddRange(obj.Actions);
				}
			}
			writer.Write((int)flags1 | ((int)spriteType << 5));
			writer.Write((int)flags2 | ((int)disableDetailLevel << 5));
			writer.Skip(4);
			writer.WriteUInt16(variables.Count);
			writer.WriteUInt16(list.Count);
			writer.WriteUInt16(emitters.Count);
			writer.WriteUInt16(256);
			for (int num = 0; num < array.Length; num++)
			{
				writer.WriteUInt16(array[num].First);
				writer.WriteUInt16(array[num].Last);
			}
			lifetime.Write(writer);
			collisionRadius.Write(writer);
			writer.Write(aiDodgeRadius);
			writer.Write(aiAlertRadius);
			writer.Write(flybySoundName, 16);
			appearance.Write(writer);
			attractor.Write(writer);
			writer.Skip(12);
			foreach (Variable variable in variables)
			{
				variable.Write(writer);
			}
			foreach (EventAction item in list)
			{
				item.Write(writer);
			}
			foreach (Emitter emitter in emitters)
			{
				emitter.Write(writer);
			}
		}
	}
}
