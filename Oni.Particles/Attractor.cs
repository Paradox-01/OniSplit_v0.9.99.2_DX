namespace Oni.Particles
{
	internal class Attractor
	{
		private AttractorTarget target;

		private AttractorSelector selector;

		private string className;

		private Value maxDistance;

		private Value maxAngle;

		private Value angleSelectMin;

		private Value angleSelectMax;

		private Value angleSelectWeight;

		public AttractorTarget Target
		{
			get
			{
				return target;
			}
			set
			{
				target = value;
			}
		}

		public AttractorSelector Selector
		{
			get
			{
				return selector;
			}
			set
			{
				selector = value;
			}
		}

		public string ClassName
		{
			get
			{
				return className;
			}
			set
			{
				className = value;
			}
		}

		public Value MaxDistance
		{
			get
			{
				return maxDistance;
			}
			set
			{
				maxDistance = value;
			}
		}

		public Value MaxAngle
		{
			get
			{
				return maxAngle;
			}
			set
			{
				maxAngle = value;
			}
		}

		public Value AngleSelectMin
		{
			get
			{
				return angleSelectMin;
			}
			set
			{
				angleSelectMin = value;
			}
		}

		public Value AngleSelectMax
		{
			get
			{
				return angleSelectMax;
			}
			set
			{
				angleSelectMax = value;
			}
		}

		public Value AngleSelectWeight
		{
			get
			{
				return angleSelectWeight;
			}
			set
			{
				angleSelectWeight = value;
			}
		}

		public Attractor()
		{
			target = AttractorTarget.None;
			selector = AttractorSelector.Distance;
			maxDistance = new Value(150f);
			maxAngle = new Value(30f);
			angleSelectMin = new Value(3f);
			angleSelectMax = new Value(3f);
			angleSelectWeight = new Value(3f);
		}

		public Attractor(BinaryReader reader)
		{
			target = (AttractorTarget)reader.ReadInt32();
			selector = (AttractorSelector)reader.ReadInt32();
			reader.Skip(4);
			className = reader.ReadString(64);
			maxDistance = Value.Read(reader);
			maxAngle = Value.Read(reader);
			angleSelectMin = Value.Read(reader);
			angleSelectMax = Value.Read(reader);
			angleSelectWeight = Value.Read(reader);
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write((int)target);
			writer.Write((int)selector);
			writer.Skip(4);
			writer.Write(className, 64);
			maxDistance.Write(writer);
			maxAngle.Write(writer);
			angleSelectMin.Write(writer);
			angleSelectMax.Write(writer);
			angleSelectWeight.Write(writer);
		}
	}
}
