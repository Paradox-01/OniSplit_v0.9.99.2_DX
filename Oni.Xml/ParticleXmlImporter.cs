using System;
using System.Globalization;
using System.Xml;
using Oni.Imaging;
using Oni.Metadata;
using Oni.Particles;

namespace Oni.Xml
{
	internal class ParticleXmlImporter : ParticleXml
	{
		private XmlReader xml;

		private Particle particle;

		public ParticleXmlImporter(XmlReader xml)
		{
			this.xml = xml;
			particle = new Particle();
		}

		public static void Import(XmlReader xml, BinaryWriter writer)
		{
			int position = writer.Position;
			writer.WriteUInt16(0);
			writer.WriteUInt16(18);
			ParticleXmlImporter particleXmlImporter = new ParticleXmlImporter(xml);
			particleXmlImporter.Read();
			particleXmlImporter.particle.Write(writer);
			int value = writer.Position - position;
			writer.PushPosition(position);
			writer.WriteUInt16(value);
			writer.PopPosition();
		}

		public void Read()
		{
			ReadOptions();
			ReadProperties();
			ReadAppearance();
			ReadAttractor();
			ReadVariables();
			ReadEmitters();
			ReadEvents();
		}

		private void ReadOptions()
		{
			if (!xml.IsStartElement("Options"))
			{
				return;
			}
			xml.ReadStartElement();
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "DisableDetailLevel":
					particle.DisableDetailLevel = MetaEnum.Parse<DisableDetailLevel>(xml.ReadElementContentAsString());
					continue;
				case "Lifetime":
					particle.Lifetime = ReadValueFloat();
					continue;
				case "CollisionRadius":
					particle.CollisionRadius = ReadValueFloat();
					continue;
				case "FlyBySoundName":
					particle.FlyBySoundName = xml.ReadElementContentAsString();
					continue;
				case "AIAlertRadius":
					particle.AIAlertRadius = xml.ReadElementContentAsFloat();
					continue;
				case "AIDodgeRadius":
					particle.AIDodgeRadius = xml.ReadElementContentAsFloat();
					continue;
				}
				if (!ReadFlag1() && !ReadFlag2())
				{
					throw new FormatException(string.Format("Unknown option {0}", xml.LocalName));
				}
			}
			xml.ReadEndElement();
		}

		private void ReadProperties()
		{
			if (!xml.IsStartElement("Properties"))
			{
				return;
			}
			xml.ReadStartElement();
			while (xml.IsStartElement())
			{
				if (!ReadFlag1())
				{
					throw new FormatException(string.Format("Unknown property {0}", xml.LocalName));
				}
			}
			xml.ReadEndElement();
		}

		private void ReadAppearance()
		{
			if (!xml.IsStartElement("Appearance"))
			{
				return;
			}
			Appearance appearance = particle.Appearance;
			xml.ReadStartElement();
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "DisplayType":
				{
					string text = xml.ReadElementContentAsString();
					switch (text)
					{
					case "Geometry":
						particle.Flags1 |= ParticleFlags1.Geometry;
						break;
					case "Vector":
						particle.Flags2 |= ParticleFlags2.Vector;
						break;
					case "Decal":
						particle.Flags2 |= ParticleFlags2.Decal;
						break;
					default:
						particle.SpriteType = MetaEnum.Parse<SpriteType>(text);
						break;
					}
					break;
				}
				case "TexGeom":
					appearance.TextureName = xml.ReadElementContentAsString();
					break;
				case "Scale":
					appearance.Scale = ReadValueFloat();
					break;
				case "YScale":
					appearance.YScale = ReadValueFloat();
					break;
				case "Rotation":
					appearance.Rotation = ReadValueFloat();
					break;
				case "Alpha":
					appearance.Alpha = ReadValueFloat();
					break;
				case "XOffset":
					appearance.XOffset = ReadValueFloat();
					break;
				case "XShorten":
					appearance.XShorten = ReadValueFloat();
					break;
				case "Tint":
					appearance.Tint = ReadValueColor();
					break;
				case "EdgeFadeMin":
					appearance.EdgeFadeMin = ReadValueFloat();
					break;
				case "EdgeFadeMax":
					appearance.EdgeFadeMax = ReadValueFloat();
					break;
				case "MaxContrailDistance":
					appearance.MaxContrail = ReadValueFloat();
					break;
				case "LensFlareDistance":
					appearance.LensFlareDistance = ReadValueFloat();
					break;
				case "LensFlareFadeInFrames":
					appearance.LensFlareFadeInFrames = xml.ReadElementContentAsInt();
					break;
				case "LensFlareFadeOutFrames":
					appearance.LensFlareFadeOutFrames = xml.ReadElementContentAsInt();
					break;
				case "MaxDecals":
					appearance.MaxDecals = xml.ReadElementContentAsInt();
					break;
				case "DecalFadeFrames":
					appearance.DecalFadeFrames = xml.ReadElementContentAsInt();
					break;
				case "DecalWrapAngle":
					appearance.DecalWrapAngle = ReadValueFloat();
					break;
				default:
					if (!ReadFlag1() && !ReadFlag2())
					{
						throw new FormatException(string.Format("Unknown appearance property {0}", xml.LocalName));
					}
					break;
				}
			}
			xml.ReadEndElement();
		}

		private void ReadAttractor()
		{
			if (!xml.IsStartElement("Attractor"))
			{
				return;
			}
			xml.ReadStartElement();
			Attractor attractor = particle.Attractor;
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "Target":
					attractor.Target = (AttractorTarget)Enum.Parse(typeof(AttractorTarget), xml.ReadElementContentAsString());
					break;
				case "Selector":
					attractor.Selector = (AttractorSelector)Enum.Parse(typeof(AttractorSelector), xml.ReadElementContentAsString());
					break;
				case "Class":
					attractor.ClassName = xml.ReadElementContentAsString();
					break;
				case "MaxDistance":
					attractor.MaxDistance = ReadValueFloat();
					break;
				case "MaxAngle":
					attractor.MaxAngle = ReadValueFloat();
					break;
				case "AngleSelectMax":
					attractor.AngleSelectMax = ReadValueFloat();
					break;
				case "AngleSelectMin":
					attractor.AngleSelectMin = ReadValueFloat();
					break;
				case "AngleSelectWeight":
					attractor.AngleSelectWeight = ReadValueFloat();
					break;
				default:
					throw new FormatException(string.Format("Unknown attractor property {0}", xml.LocalName));
				}
			}
			xml.ReadEndElement();
		}

		private void ReadVariables()
		{
			if (!xml.IsStartElement("Variables"))
			{
				return;
			}
			if (xml.IsEmptyElement)
			{
				xml.ReadStartElement();
				return;
			}
			xml.ReadStartElement();
			while (xml.IsStartElement())
			{
				particle.Variables.Add(ReadVariable());
			}
			xml.ReadEndElement();
		}

		private Variable ReadVariable()
		{
			string localName = xml.LocalName;
			string attribute = xml.GetAttribute("Name");
			switch (localName)
			{
			case "Float":
				return new Variable(attribute, StorageType.Float, ReadValueFloat());
			case "Color":
				return new Variable(attribute, StorageType.Color, ReadValueColor());
			case "PingPongState":
				return new Variable(attribute, StorageType.PingPongState, ReadValueInt());
			default:
				throw new XmlException(string.Format("Unknown variable type '{0}'", localName));
			}
		}

		private void ReadEmitters()
		{
			if (!xml.IsStartElement("Emitters"))
			{
				return;
			}
			if (xml.IsEmptyElement)
			{
				xml.ReadStartElement();
				return;
			}
			xml.ReadStartElement();
			while (xml.IsStartElement())
			{
				particle.Emitters.Add(ReadEmitter());
			}
			xml.ReadEndElement();
		}

		private Emitter ReadEmitter()
		{
			xml.ReadStartElement();
			Emitter emitter = new Emitter();
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "Class":
					emitter.ParticleClass = xml.ReadElementContentAsString();
					break;
				case "Flags":
					emitter.Flags = MetaEnum.Parse<EmitterFlags>(xml.ReadElementContentAsString());
					break;
				case "TurnOffTreshold":
					emitter.TurnOffTreshold = xml.ReadElementContentAsInt();
					break;
				case "Probability":
					emitter.Probability = (int)(xml.ReadElementContentAsFloat() * 65535f);
					break;
				case "Copies":
					emitter.Copies = xml.ReadElementContentAsFloat();
					break;
				case "LinkTo":
				{
					string text = xml.ReadElementContentAsString();
					if (!string.IsNullOrEmpty(text))
					{
						if (text == "this")
						{
							emitter.LinkTo = 1;
						}
						else if (text == "link")
						{
							emitter.LinkTo = 10;
						}
						else
						{
							emitter.LinkTo = int.Parse(text, CultureInfo.InvariantCulture) + 2;
						}
					}
					break;
				}
				case "Rate":
					xml.ReadStartElement();
					ReadEmitterRate(emitter);
					xml.ReadEndElement();
					break;
				case "Position":
					xml.ReadStartElement();
					ReadEmitterPosition(emitter);
					xml.ReadEndElement();
					break;
				case "Speed":
					xml.ReadStartElement();
					ReadEmitterSpeed(emitter);
					xml.ReadEndElement();
					break;
				case "Direction":
					xml.ReadStartElement();
					ReadEmitterDirection(emitter);
					xml.ReadEndElement();
					break;
				case "Orientation":
					emitter.OrientationDir = MetaEnum.Parse<EmitterOrientation>(xml.ReadElementContentAsString());
					break;
				case "OrientationUp":
					emitter.OrientationUp = MetaEnum.Parse<EmitterOrientation>(xml.ReadElementContentAsString());
					break;
				default:
					throw new FormatException(string.Format("Unknown emitter property '{0}'", xml.LocalName));
				}
			}
			xml.ReadEndElement();
			return emitter;
		}

		private void ReadEmitterRate(Emitter emitter)
		{
			emitter.Rate = MetaEnum.Parse<EmitterRate>(xml.LocalName);
			if (xml.IsEmptyElement)
			{
				xml.ReadStartElement();
				return;
			}
			xml.ReadStartElement();
			switch (emitter.Rate)
			{
			case EmitterRate.Continous:
				emitter.Parameters[0] = ReadValueFloat("Interval");
				break;
			case EmitterRate.Random:
				emitter.Parameters[0] = ReadValueFloat("MinInterval");
				emitter.Parameters[1] = ReadValueFloat("MaxInterval");
				break;
			case EmitterRate.Distance:
				emitter.Parameters[0] = ReadValueFloat("Distance");
				break;
			case EmitterRate.Attractor:
				emitter.Parameters[0] = ReadValueFloat("RechargeTime");
				emitter.Parameters[1] = ReadValueFloat("CheckInterval");
				break;
			}
			xml.ReadEndElement();
		}

		private void ReadEmitterPosition(Emitter emitter)
		{
			emitter.Position = MetaEnum.Parse<EmitterPosition>(xml.LocalName);
			if (xml.IsEmptyElement)
			{
				xml.ReadStartElement();
				return;
			}
			xml.ReadStartElement();
			switch (emitter.Position)
			{
			case EmitterPosition.Line:
				emitter.Parameters[2] = ReadValueFloat("Radius");
				break;
			case EmitterPosition.Circle:
			case EmitterPosition.Sphere:
				emitter.Parameters[2] = ReadValueFloat("InnerRadius");
				emitter.Parameters[3] = ReadValueFloat("OuterRadius");
				break;
			case EmitterPosition.Offset:
				emitter.Parameters[2] = ReadValueFloat("X");
				emitter.Parameters[3] = ReadValueFloat("Y");
				emitter.Parameters[4] = ReadValueFloat("Z");
				break;
			case EmitterPosition.Cylinder:
				emitter.Parameters[2] = ReadValueFloat("Height");
				emitter.Parameters[3] = ReadValueFloat("InnerRadius");
				emitter.Parameters[4] = ReadValueFloat("OuterRadius");
				break;
			case EmitterPosition.BodySurface:
			case EmitterPosition.BodyBones:
				emitter.Parameters[2] = ReadValueFloat("OffsetRadius");
				break;
			}
			xml.ReadEndElement();
		}

		private void ReadEmitterDirection(Emitter emitter)
		{
			emitter.Direction = MetaEnum.Parse<EmitterDirection>(xml.LocalName);
			if (xml.IsEmptyElement)
			{
				xml.ReadStartElement();
				return;
			}
			xml.ReadStartElement();
			switch (emitter.Direction)
			{
			case EmitterDirection.Cone:
				emitter.Parameters[5] = ReadValueFloat("Angle");
				emitter.Parameters[6] = ReadValueFloat("CenterBias");
				break;
			case EmitterDirection.Ring:
				emitter.Parameters[5] = ReadValueFloat("Angle");
				emitter.Parameters[6] = ReadValueFloat("Offset");
				break;
			case EmitterDirection.Offset:
				emitter.Parameters[5] = ReadValueFloat("X");
				emitter.Parameters[6] = ReadValueFloat("Y");
				emitter.Parameters[7] = ReadValueFloat("Z");
				break;
			case EmitterDirection.Inaccurate:
				emitter.Parameters[5] = ReadValueFloat("BaseAngle");
				emitter.Parameters[6] = ReadValueFloat("Inaccuracy");
				emitter.Parameters[7] = ReadValueFloat("CenterBias");
				break;
			}
			xml.ReadEndElement();
		}

		private void ReadEmitterSpeed(Emitter emitter)
		{
			emitter.Speed = MetaEnum.Parse<EmitterSpeed>(xml.LocalName);
			if (xml.IsEmptyElement)
			{
				xml.ReadStartElement();
				return;
			}
			xml.ReadStartElement();
			switch (emitter.Speed)
			{
			case EmitterSpeed.Uniform:
				emitter.Parameters[8] = ReadValueFloat("Speed");
				break;
			case EmitterSpeed.Stratified:
				emitter.Parameters[8] = ReadValueFloat("Speed1");
				emitter.Parameters[9] = ReadValueFloat("Speed2");
				break;
			}
			xml.ReadEndElement();
		}

		private void ReadEvents()
		{
			if (!xml.IsStartElement("Events"))
			{
				return;
			}
			if (xml.IsEmptyElement)
			{
				xml.ReadStartElement();
				return;
			}
			xml.ReadStartElement();
			while (xml.IsStartElement())
			{
				ReadEvent();
			}
			xml.ReadEndElement();
		}

		private void ReadEvent()
		{
			Event obj = new Event((EventType)Enum.Parse(typeof(EventType), xml.LocalName));
			if (xml.IsEmptyElement)
			{
				xml.ReadStartElement();
			}
			else
			{
				xml.ReadStartElement();
				while (xml.IsStartElement())
				{
					obj.Actions.Add(ReadEventAction());
				}
				xml.ReadEndElement();
			}
			particle.Events.Add(obj);
		}

		private EventAction ReadEventAction()
		{
			EventAction eventAction = new EventAction((EventActionType)Enum.Parse(typeof(EventActionType), xml.LocalName));
			EventActionInfo eventActionInfo = ParticleXml.eventActionInfoTable[(int)eventAction.Type];
			if (xml.IsEmptyElement)
			{
				xml.ReadStartElement();
			}
			else
			{
				xml.ReadStartElement();
				int num = 0;
				while (xml.IsStartElement())
				{
					if (num < eventActionInfo.OutCount)
					{
						eventAction.Variables.Add(new VariableReference(xml.ReadElementContentAsString()));
					}
					else
					{
						if (num >= eventActionInfo.Parameters.Length)
						{
							throw new XmlException(string.Format("Too many arguments for action '{0}'", eventAction.Type));
						}
						switch (eventActionInfo.Parameters[num].Type)
						{
						case StorageType.Float:
						case StorageType.BlastFalloff:
							eventAction.Parameters.Add(ReadValueFloat());
							break;
						case StorageType.Color:
							eventAction.Parameters.Add(ReadValueColor());
							break;
						case StorageType.PingPongState:
						case StorageType.ActionIndex:
						case StorageType.Emitter:
						case StorageType.CoordFrame:
						case StorageType.CollisionOrient:
						case StorageType.Boolean:
						case StorageType.ImpactModifier:
						case StorageType.DamageType:
						case StorageType.Direction:
							eventAction.Parameters.Add(ReadValueInt());
							break;
						case StorageType.ImpactName:
						case StorageType.AmbientSoundName:
						case StorageType.ImpulseSoundName:
							eventAction.Parameters.Add(ReadValueInstance());
							break;
						}
					}
					num++;
				}
				xml.ReadEndElement();
			}
			return eventAction;
		}

		private Value ReadValueInstance()
		{
			return new Value(Oni.Particles.ValueType.InstanceName, xml.ReadElementContentAsString());
		}

		private Value ReadValueInt()
		{
			Value result = null;
			xml.ReadStartElement();
			if (xml.NodeType == XmlNodeType.Text)
			{
				string text = xml.ReadString();
				int result2;
				result = ((!int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out result2)) ? new Value(Oni.Particles.ValueType.Variable, text.Trim()) : new Value(result2));
			}
			xml.ReadEndElement();
			return result;
		}

		private Value ReadValueFloat(string name)
		{
			if (xml.LocalName != name)
			{
				throw new XmlException(string.Format(CultureInfo.CurrentCulture, "Unexpected '{0}' element found at line {1}", new object[2] { xml.LocalName, 0 }));
			}
			return ReadValueFloat();
		}

		private Value ReadValueFloat()
		{
			if (xml.IsEmptyElement)
			{
				xml.Read();
				return new Value(0f);
			}
			Value result = null;
			xml.ReadStartElement();
			if (xml.NodeType == XmlNodeType.Text)
			{
				string text = xml.ReadString();
				float result2;
				result = ((!float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out result2)) ? new Value(Oni.Particles.ValueType.Variable, text.Trim()) : new Value(result2));
			}
			else if (xml.NodeType == XmlNodeType.Element)
			{
				string localName = xml.LocalName;
				if (localName == "Random")
				{
					float value = float.Parse(xml.GetAttribute("Min"), CultureInfo.InvariantCulture);
					float value2 = float.Parse(xml.GetAttribute("Max"), CultureInfo.InvariantCulture);
					result = new Value(Oni.Particles.ValueType.FloatRandom, value, value2);
				}
				else
				{
					if (!(localName == "BellCurve"))
					{
						throw new XmlException(string.Format(CultureInfo.CurrentCulture, "Unknown value type '{0}'", new object[1] { localName }));
					}
					float value3 = float.Parse(xml.GetAttribute("Mean"), CultureInfo.InvariantCulture);
					float value4 = float.Parse(xml.GetAttribute("StdDev"), CultureInfo.InvariantCulture);
					result = new Value(Oni.Particles.ValueType.FloatBellCurve, value3, value4);
				}
				xml.ReadStartElement();
			}
			xml.ReadEndElement();
			return result;
		}

		private Value ReadValueColor()
		{
			Value result = null;
			xml.ReadStartElement();
			if (xml.NodeType == XmlNodeType.Text)
			{
				string text = xml.ReadString();
				Color color;
				result = ((!Color.TryParse(text, out color)) ? new Value(Oni.Particles.ValueType.Variable, text.Trim()) : new Value(color));
			}
			else if (xml.NodeType == XmlNodeType.Element)
			{
				string localName = xml.LocalName;
				if (localName == "Random")
				{
					Color color2 = Color.Parse(xml.GetAttribute("Min"));
					Color color3 = Color.Parse(xml.GetAttribute("Max"));
					result = new Value(Oni.Particles.ValueType.ColorRandom, color2, color3);
				}
				else
				{
					if (!(localName == "BellCurve"))
					{
						throw new XmlException(string.Format(CultureInfo.CurrentCulture, "Unknown value type '{0}'", new object[1] { localName }));
					}
					Color color4 = Color.Parse(xml.GetAttribute("Mean"));
					Color color5 = Color.Parse(xml.GetAttribute("StdDev"));
					result = new Value(Oni.Particles.ValueType.ColorBellCurve, color4, color5);
				}
				xml.ReadStartElement();
			}
			xml.ReadEndElement();
			return result;
		}

		private bool ReadFlag1()
		{
			ParticleFlags1 particleFlags;
			try
			{
				particleFlags = (ParticleFlags1)Enum.Parse(typeof(ParticleFlags1), xml.LocalName);
			}
			catch
			{
				return false;
			}
			if (ReadFlagValue())
			{
				particle.Flags1 |= particleFlags;
			}
			return true;
		}

		private bool ReadFlag2()
		{
			ParticleFlags2 particleFlags;
			try
			{
				particleFlags = (ParticleFlags2)Enum.Parse(typeof(ParticleFlags2), xml.LocalName);
			}
			catch
			{
				return false;
			}
			if (ReadFlagValue())
			{
				particle.Flags2 |= particleFlags;
			}
			return true;
		}

		private bool ReadFlagValue()
		{
			string text = xml.ReadElementContentAsString();
			switch (text)
			{
			case "false":
				return false;
			case "true":
				return true;
			default:
				throw new FormatException(string.Format(CultureInfo.CurrentCulture, "Unknown value '{0}'", new object[1] { text }));
			}
		}
	}
}
