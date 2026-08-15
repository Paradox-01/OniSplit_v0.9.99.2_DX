using System.IO;
using System.Xml;
using Oni.Metadata;

namespace Oni.Xml
{
	internal class FilmToXmlConverter : RawXmlExporter
	{
		private static readonly MetaType filmHeader = new MetaStruct("FilmHeader", new Field(MetaType.Vector3, "Position"), new Field(MetaType.Float, "Facing"), new Field(MetaType.Float, "DesiredFacing"), new Field(MetaType.Float, "HeadFacing"), new Field(MetaType.Float, "HeadPitch"), new Field(MetaType.Int32, "FrameCount"), new Field(MetaType.Padding(28)));

		private static readonly MetaType filmAnimations = new MetaStruct("FilmAnimations", new Field(MetaType.Array(2, MetaType.String128), "Animations"));

		private static readonly MetaType filmFrames = new MetaStruct("FilmFrames", new Field(MetaType.VarArray(new MetaStruct("Frame", new Field(MetaType.Vector2, "MouseDelta"), new Field(MetaType.Enum<InstanceMetadata.FILMKeys>(), "Keys"), new Field(MetaType.Int32, "Frame"), new Field(MetaType.Padding(4)))), "Frames"));

		public FilmToXmlConverter(BinaryReader reader, XmlWriter writer)
			: base(reader, writer)
		{
		}

		public static void Convert(string filePath, string outputDirPath)
		{
			using (BinaryReader binaryReader = new BinaryReader(filePath, true))
			{
				using (XmlWriter xmlWriter = CreateXmlWriter(Path.Combine(outputDirPath, Path.GetFileNameWithoutExtension(filePath) + ".xml")))
				{
					xmlWriter.WriteStartElement("Instance");
					xmlWriter.WriteAttributeString("id", "0");
					xmlWriter.WriteAttributeString("type", "FILM");
					FilmToXmlConverter visitor = new FilmToXmlConverter(binaryReader, xmlWriter);
					binaryReader.Position = filmAnimations.Size;
					filmHeader.Accept(visitor);
					binaryReader.Position = 0;
					filmAnimations.Accept(visitor);
					binaryReader.Position = filmAnimations.Size + filmHeader.Size;
					filmFrames.Accept(visitor);
					xmlWriter.WriteEndElement();
				}
			}
		}

		private static XmlWriter CreateXmlWriter(string filePath)
		{
			XmlWriterSettings settings = new XmlWriterSettings
			{
				CloseOutput = true,
				Indent = true,
				IndentChars = "    "
			};
			FileStream output = File.Create(filePath);
			XmlWriter xmlWriter = XmlWriter.Create(output, settings);
			try
			{
				xmlWriter.WriteStartElement("Oni");
				return xmlWriter;
			}
			catch
			{
				xmlWriter.Close();
				throw;
			}
		}
	}
}
