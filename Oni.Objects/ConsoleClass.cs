using System.Xml;
using Oni.Akira;
using Oni.Metadata;
using Oni.Motoko;
using Oni.Physics;
using Oni.Xml;

namespace Oni.Objects
{
	internal class ConsoleClass : GunkObjectClass
	{
		public ConsoleClassFlags Flags;

		public Vector3 ActionPoint;

		public Vector3 ActionOrientation;

		public ObjectNode Geometry;

		public Geometry ScreenGeometry;

		public GunkFlags ScreenGunkFlags;

		public string InactiveTexture;

		public string ActiveTexture;

		public string TriggeredTexture;

		public override ObjectGeometry[] GunkNodes
		{
			get
			{
				return Geometry.Geometries;
			}
		}

		public static ConsoleClass Read(InstanceDescriptor cons)
		{
			ConsoleClass consoleClass = new ConsoleClass();
			InstanceDescriptor instanceDescriptor;
			InstanceDescriptor instanceDescriptor2;
			using (BinaryReader binaryReader = cons.OpenRead())
			{
				consoleClass.Flags = (ConsoleClassFlags)binaryReader.ReadUInt32();
				consoleClass.ActionPoint = binaryReader.ReadVector3();
				consoleClass.ActionOrientation = binaryReader.ReadVector3();
				instanceDescriptor = binaryReader.ReadInstance();
				instanceDescriptor2 = binaryReader.ReadInstance();
				consoleClass.ScreenGunkFlags = (GunkFlags)binaryReader.ReadUInt32();
				consoleClass.InactiveTexture = binaryReader.ReadString(32);
				consoleClass.ActiveTexture = binaryReader.ReadString(32);
				consoleClass.TriggeredTexture = binaryReader.ReadString(32);
			}
			if (instanceDescriptor != null)
			{
				consoleClass.Geometry = ObjectDatReader.ReadObjectGeometry(instanceDescriptor);
			}
			if (instanceDescriptor2 != null)
			{
				consoleClass.ScreenGeometry = GeometryDatReader.Read(instanceDescriptor2);
			}
			return consoleClass;
		}

		public static ConsoleClass Read(XmlReader xml)
		{
			ConsoleClass consoleClass = new ConsoleClass();
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "Flags":
					consoleClass.Flags = xml.ReadElementContentAsEnum<ConsoleClassFlags>();
					break;
				case "ActionPoint":
					consoleClass.ActionPoint = xml.ReadElementContentAsVector3();
					break;
				case "ActionOrientation":
					consoleClass.ActionOrientation = xml.ReadElementContentAsVector3();
					break;
				case "InactiveTexture":
					consoleClass.InactiveTexture = xml.ReadElementContentAsString();
					break;
				case "ActiveTexture":
					consoleClass.ActiveTexture = xml.ReadElementContentAsString();
					break;
				case "TriggeredTexture":
					consoleClass.TriggeredTexture = xml.ReadElementContentAsString();
					break;
				}
			}
			return consoleClass;
		}
	}
}
