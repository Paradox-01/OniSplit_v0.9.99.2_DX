using System;
using System.Globalization;
using System.Xml;
using Oni.Imaging;
using Oni.Particles;

namespace Oni.Xml
{
	internal class ParticleXmlExporter : ParticleXml
	{
		private Particle particle;

		private XmlWriter xml;

		private ParticleXmlExporter(Particle particle, XmlWriter writer)
		{
			this.particle = particle;
			xml = writer;
		}

		public static void Export(string name, BinaryReader rawReader, XmlWriter xml)
		{
			Particle particle = new Particle(rawReader);
			xml.WriteStartElement("Particle");
			xml.WriteAttributeString("Name", name);
			ParticleXmlExporter particleXmlExporter = new ParticleXmlExporter(particle, xml);
			particleXmlExporter.Write();
			xml.WriteEndElement();
		}

		public void Write()
		{
			WriteOptions();
			WriteProperties();
			WriteAppearance();
			WriteAttractor();
			WriteVariables();
			WriteEmitters();
			WriteEvents();
		}

		private void WriteProperties()
		{
			xml.WriteStartElement("Properties");
			int num = 4096;
			int num2 = 0;
			while (num2 < 13)
			{
				WriteFlag1((ParticleFlags1)num);
				num2++;
				num <<= 1;
			}
			xml.WriteEndElement();
		}

		private void WriteOptions()
		{
			xml.WriteStartElement("Options");
			WriteValue(particle.Lifetime, "Lifetime");
			xml.WriteElementString("DisableDetailLevel", particle.DisableDetailLevel.ToString());
			ParticleFlags1[] array = ParticleXml.optionFlags1;
			foreach (ParticleFlags1 flag in array)
			{
				WriteFlag1(flag);
			}
			ParticleFlags2[] array2 = ParticleXml.optionFlags2;
			foreach (ParticleFlags2 flag2 in array2)
			{
				WriteFlag2(flag2);
			}
			WriteValue(particle.CollisionRadius, "CollisionRadius");
			xml.WriteElementString("AIDodgeRadius", XmlConvert.ToString(particle.AIDodgeRadius));
			xml.WriteElementString("AIAlertRadius", XmlConvert.ToString(particle.AIAlertRadius));
			xml.WriteElementString("FlyBySoundName", particle.FlyBySoundName);
			xml.WriteEndElement();
		}

		private void WriteAttractor()
		{
			Attractor attractor = particle.Attractor;
			xml.WriteStartElement("Attractor");
			xml.WriteElementString("Target", attractor.Target.ToString());
			xml.WriteElementString("Selector", attractor.Selector.ToString());
			xml.WriteElementString("Class", attractor.ClassName);
			WriteValue(attractor.MaxDistance, "MaxDistance");
			WriteValue(attractor.MaxAngle, "MaxAngle");
			WriteValue(attractor.AngleSelectMin, "AngleSelectMin");
			WriteValue(attractor.AngleSelectMax, "AngleSelectMax");
			WriteValue(attractor.AngleSelectWeight, "AngleSelectWeight");
			xml.WriteEndElement();
		}

		private void WriteAppearance()
		{
			Appearance appearance = particle.Appearance;
			xml.WriteStartElement("Appearance");
			if ((particle.Flags1 & ParticleFlags1.Geometry) != ParticleFlags1.None)
			{
				xml.WriteElementString("DisplayType", "Geometry");
				xml.WriteElementString("TexGeom", appearance.TextureName);
			}
			else if ((particle.Flags2 & ParticleFlags2.Vector) != ParticleFlags2.None)
			{
				xml.WriteElementString("DisplayType", "Vector");
			}
			else if ((particle.Flags2 & ParticleFlags2.Decal) != ParticleFlags2.None)
			{
				xml.WriteElementString("DisplayType", "Decal");
				xml.WriteElementString("TexGeom", appearance.TextureName);
			}
			else
			{
				xml.WriteElementString("DisplayType", particle.SpriteType.ToString());
				xml.WriteElementString("TexGeom", appearance.TextureName);
			}
			ParticleFlags1[] array = ParticleXml.appearanceFlags1;
			foreach (ParticleFlags1 flag in array)
			{
				WriteFlag1(flag);
			}
			ParticleFlags2[] array2 = ParticleXml.appearanceFlags2;
			foreach (ParticleFlags2 flag2 in array2)
			{
				WriteFlag2(flag2);
			}
			WriteFlag1(ParticleFlags1.ScaleToVelocity);
			WriteValue(appearance.Scale, "Scale");
			WriteFlag1(ParticleFlags1.UseSeparateYScale);
			WriteValue(appearance.YScale, "YScale");
			WriteValue(appearance.Rotation, "Rotation");
			WriteValue(appearance.Alpha, "Alpha");
			WriteValue(appearance.XOffset, "XOffset");
			WriteValue(appearance.XShorten, "XShorten");
			WriteFlag2(ParticleFlags2.UseSpecialTint);
			WriteValue(appearance.Tint, "Tint");
			WriteFlag2(ParticleFlags2.FadeOutOnEdge);
			WriteFlag2(ParticleFlags2.OneSidedEdgeFade);
			WriteValue(appearance.EdgeFadeMin, "EdgeFadeMin");
			WriteValue(appearance.EdgeFadeMax, "EdgeFadeMax");
			WriteValue(appearance.MaxContrail, "MaxContrailDistance");
			WriteFlag2(ParticleFlags2.LensFlare);
			WriteValue(appearance.LensFlareDistance, "LensFlareDistance");
			xml.WriteElementString("LensFlareFadeInFrames", XmlConvert.ToString(appearance.LensFlareFadeInFrames));
			xml.WriteElementString("LensFlareFadeOutFrames", XmlConvert.ToString(appearance.LensFlareFadeOutFrames));
			WriteFlag2(ParticleFlags2.DecalFullBrightness);
			xml.WriteElementString("MaxDecals", XmlConvert.ToString(appearance.MaxDecals));
			xml.WriteElementString("DecalFadeFrames", XmlConvert.ToString(appearance.DecalFadeFrames));
			WriteValue(appearance.DecalWrapAngle, "DecalWrapAngle");
			xml.WriteEndElement();
		}

		private void WriteFlag1(ParticleFlags1 flag)
		{
			xml.WriteElementString(flag.ToString(), ((particle.Flags1 & flag) != ParticleFlags1.None) ? "true" : "false");
		}

		private void WriteFlag2(ParticleFlags2 flag)
		{
			xml.WriteElementString(flag.ToString(), ((particle.Flags2 & flag) != ParticleFlags2.None) ? "true" : "false");
		}

		private void WriteVariables()
		{
			xml.WriteStartElement("Variables");
			foreach (Variable variable in particle.Variables)
			{
				WriteVariable(variable);
			}
			xml.WriteEndElement();
		}

		private void WriteVariable(Variable variable)
		{
			switch (variable.StorageType)
			{
			case StorageType.Float:
				xml.WriteStartElement("Float");
				break;
			case StorageType.Color:
				xml.WriteStartElement("Color");
				break;
			case StorageType.PingPongState:
				xml.WriteStartElement("PingPongState");
				break;
			}
			xml.WriteAttributeString("Name", variable.Name);
			WriteValue(variable.Value);
			xml.WriteEndElement();
		}

		private void WriteEmitters()
		{
			xml.WriteStartElement("Emitters");
			foreach (Emitter emitter in particle.Emitters)
			{
				WriteEmitter(emitter);
			}
			xml.WriteEndElement();
		}

		private void WriteEmitter(Emitter emitter)
		{
			xml.WriteStartElement("Emitter");
			xml.WriteElementString("Class", emitter.ParticleClass);
			xml.WriteElementString("Flags", (emitter.Flags == EmitterFlags.None) ? string.Empty : emitter.Flags.ToString().Replace(",", string.Empty));
			xml.WriteElementString("TurnOffTreshold", XmlConvert.ToString(emitter.TurnOffTreshold));
			xml.WriteElementString("Probability", XmlConvert.ToString((float)emitter.Probability / 65535f));
			xml.WriteElementString("Copies", XmlConvert.ToString(emitter.Copies));
			switch (emitter.LinkTo)
			{
			case 0:
				xml.WriteElementString("LinkTo", string.Empty);
				break;
			case 1:
				xml.WriteElementString("LinkTo", "this");
				break;
			case 10:
				xml.WriteElementString("LinkTo", "link");
				break;
			default:
				xml.WriteElementString("LinkTo", XmlConvert.ToString(emitter.LinkTo - 2));
				break;
			}
			WriteEmitterRate(emitter);
			WriteEmitterPosition(emitter);
			WriteEmitterDirection(emitter);
			WriteEmitterSpeed(emitter);
			xml.WriteElementString("Orientation", emitter.OrientationDir.ToString());
			xml.WriteElementString("OrientationUp", emitter.OrientationUp.ToString());
			xml.WriteEndElement();
		}

		private void WriteEmitterRate(Emitter emitter)
		{
			xml.WriteStartElement("Rate");
			xml.WriteStartElement(emitter.Rate.ToString());
			switch (emitter.Rate)
			{
			case EmitterRate.Continous:
				WriteValue(emitter.Parameters[0], "Interval");
				break;
			case EmitterRate.Random:
				WriteValue(emitter.Parameters[0], "MinInterval");
				WriteValue(emitter.Parameters[1], "MaxInterval");
				break;
			case EmitterRate.Distance:
				WriteValue(emitter.Parameters[0], "Distance");
				break;
			case EmitterRate.Attractor:
				WriteValue(emitter.Parameters[0], "RechargeTime");
				WriteValue(emitter.Parameters[1], "CheckInterval");
				break;
			}
			xml.WriteEndElement();
			xml.WriteEndElement();
		}

		private void WriteEmitterPosition(Emitter emitter)
		{
			xml.WriteStartElement("Position");
			xml.WriteStartElement(emitter.Position.ToString());
			switch (emitter.Position)
			{
			case EmitterPosition.Line:
				WriteValue(emitter.Parameters[2], "Radius");
				break;
			case EmitterPosition.Circle:
			case EmitterPosition.Sphere:
				WriteValue(emitter.Parameters[2], "InnerRadius");
				WriteValue(emitter.Parameters[3], "OuterRadius");
				break;
			case EmitterPosition.Offset:
				WriteValue(emitter.Parameters[2], "X");
				WriteValue(emitter.Parameters[3], "Y");
				WriteValue(emitter.Parameters[4], "Z");
				break;
			case EmitterPosition.Cylinder:
				WriteValue(emitter.Parameters[2], "Height");
				WriteValue(emitter.Parameters[3], "InnerRadius");
				WriteValue(emitter.Parameters[4], "OuterRadius");
				break;
			case EmitterPosition.BodySurface:
			case EmitterPosition.BodyBones:
				WriteValue(emitter.Parameters[2], "OffsetRadius");
				break;
			}
			xml.WriteEndElement();
			xml.WriteEndElement();
		}

		private void WriteEmitterDirection(Emitter emitter)
		{
			xml.WriteStartElement("Direction");
			xml.WriteStartElement(emitter.Direction.ToString());
			switch (emitter.Direction)
			{
			case EmitterDirection.Cone:
				WriteValue(emitter.Parameters[5], "Angle");
				WriteValue(emitter.Parameters[6], "CenterBias");
				break;
			case EmitterDirection.Ring:
				WriteValue(emitter.Parameters[5], "Angle");
				WriteValue(emitter.Parameters[6], "Offset");
				break;
			case EmitterDirection.Offset:
				WriteValue(emitter.Parameters[5], "X");
				WriteValue(emitter.Parameters[6], "Y");
				WriteValue(emitter.Parameters[7], "Z");
				break;
			case EmitterDirection.Inaccurate:
				WriteValue(emitter.Parameters[5], "BaseAngle");
				WriteValue(emitter.Parameters[6], "Inaccuracy");
				WriteValue(emitter.Parameters[7], "CenterBias");
				break;
			}
			xml.WriteEndElement();
			xml.WriteEndElement();
		}

		private void WriteEmitterSpeed(Emitter emitter)
		{
			xml.WriteStartElement("Speed");
			xml.WriteStartElement(emitter.Speed.ToString());
			switch (emitter.Speed)
			{
			case EmitterSpeed.Uniform:
				WriteValue(emitter.Parameters[8], "Speed");
				break;
			case EmitterSpeed.Stratified:
				WriteValue(emitter.Parameters[8], "Speed1");
				WriteValue(emitter.Parameters[9], "Speed2");
				break;
			}
			xml.WriteEndElement();
			xml.WriteEndElement();
		}

		private void WriteEvents()
		{
			xml.WriteStartElement("Events");
			foreach (Event @event in particle.Events)
			{
				WriteEvent(@event);
			}
			xml.WriteEndElement();
		}

		private void WriteEvent(Event e)
		{
			if (e.Actions.Count == 0)
			{
				return;
			}
			xml.WriteStartElement(e.Type.ToString());
			foreach (EventAction action in e.Actions)
			{
				WriteAction(action);
			}
			xml.WriteEndElement();
		}

		private void WriteAction(EventAction action)
		{
			int type = (int)action.Type;
			if (type >= ParticleXml.eventActionInfoTable.Length || ParticleXml.eventActionInfoTable[type] == null)
			{
				Console.Error.WriteLine("ParticleXmlExporter: Unknown event action type {0}, ignoring", type);
				return;
			}
			xml.WriteStartElement(action.Type.ToString());
			EventActionInfo eventActionInfo = ParticleXml.eventActionInfoTable[type];
			int num = 0;
			foreach (VariableReference variable in action.Variables)
			{
				if (num < eventActionInfo.Parameters.Length)
				{
					xml.WriteElementString(eventActionInfo.Parameters[num++].Name, variable.Name);
				}
			}
			foreach (Value parameter in action.Parameters)
			{
				if (num < eventActionInfo.Parameters.Length)
				{
					xml.WriteStartElement(eventActionInfo.Parameters[num++].Name);
					WriteValue(parameter);
					xml.WriteEndElement();
				}
			}
			xml.WriteEndElement();
		}

		private void WriteValue(Value value, string name)
		{
			xml.WriteStartElement(name);
			WriteValue(value);
			xml.WriteEndElement();
		}

		private void WriteValue(Value value)
		{
			if (value != null)
			{
				switch (value.Type)
				{
				case Oni.Particles.ValueType.Variable:
				case Oni.Particles.ValueType.InstanceName:
					xml.WriteString(value.Name);
					break;
				case Oni.Particles.ValueType.Float:
					xml.WriteString(XmlConvert.ToString(value.Float1));
					break;
				case Oni.Particles.ValueType.FloatRandom:
					xml.WriteStartElement("Random");
					xml.WriteAttributeString("Min", XmlConvert.ToString(value.Float1));
					xml.WriteAttributeString("Max", XmlConvert.ToString(value.Float2));
					xml.WriteEndElement();
					break;
				case Oni.Particles.ValueType.FloatBellCurve:
					xml.WriteStartElement("BellCurve");
					xml.WriteAttributeString("Mean", XmlConvert.ToString(value.Float1));
					xml.WriteAttributeString("StdDev", XmlConvert.ToString(value.Float2));
					xml.WriteEndElement();
					break;
				case Oni.Particles.ValueType.Color:
					WriteColor(value.Color1);
					break;
				case Oni.Particles.ValueType.ColorRandom:
					xml.WriteStartElement("Random");
					WriteColorAttribute("Min", value.Color1);
					WriteColorAttribute("Max", value.Color2);
					xml.WriteEndElement();
					break;
				case Oni.Particles.ValueType.ColorBellCurve:
					xml.WriteStartElement("BellCurve");
					WriteColorAttribute("Mean", value.Color1);
					WriteColorAttribute("StdDev", value.Color2);
					xml.WriteEndElement();
					break;
				case Oni.Particles.ValueType.Int32:
					xml.WriteString(XmlConvert.ToString(value.Int));
					break;
				case Oni.Particles.ValueType.TimeCycle:
					xml.WriteStartElement("TimeCycle");
					xml.WriteAttributeString("Length", XmlConvert.ToString(value.Float1));
					xml.WriteAttributeString("Scale", XmlConvert.ToString(value.Float2));
					xml.WriteEndElement();
					break;
				case Oni.Particles.ValueType.Int16:
				case Oni.Particles.ValueType.Int16Random:
					break;
				}
			}
		}

		private void WriteColor(Color color)
		{
			if (color.A == byte.MaxValue)
			{
				xml.WriteValue(string.Format(CultureInfo.InvariantCulture, "{0} {1} {2}", new object[3] { color.R, color.G, color.B }));
			}
			else
			{
				xml.WriteValue(string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3}", color.R, color.G, color.B, color.A));
			}
		}

		private void WriteColorAttribute(string name, Color color)
		{
			if (color.A == byte.MaxValue)
			{
				xml.WriteAttributeString(name, string.Format(CultureInfo.InvariantCulture, "{0} {1} {2}", new object[3] { color.R, color.G, color.B }));
			}
			else
			{
				xml.WriteAttributeString(name, string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3}", color.R, color.G, color.B, color.A));
			}
		}
	}
}
