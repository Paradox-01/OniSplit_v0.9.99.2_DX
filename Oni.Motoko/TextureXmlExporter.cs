using System;
using System.IO;
using System.Xml;
using Oni.Imaging;
using Oni.Metadata;
using Oni.Xml;

namespace Oni.Motoko
{
	internal sealed class TextureXmlExporter : RawXmlExporter
	{
		private InstanceDescriptor txmp;

		private string outputDirPath;

		private string baseFileName;

		private TextureXmlExporter(BinaryReader reader, XmlWriter writer)
			: base(reader, writer)
		{
		}

		public static void Export(InstanceDescriptor txmp, XmlWriter writer, string outputDirPath, string baseFileName)
		{
			using (BinaryReader binaryReader = txmp.OpenRead(128))
			{
				TextureXmlExporter textureXmlExporter = new TextureXmlExporter(binaryReader, writer)
				{
					txmp = txmp,
					outputDirPath = outputDirPath,
					baseFileName = baseFileName
				};
				textureXmlExporter.Export();
			}
		}

		private void Export()
		{
			InstanceMetadata.TXMPFlags tXMPFlags = (InstanceMetadata.TXMPFlags)base.Reader.ReadInt32();
			int num = base.Reader.ReadInt16();
			int num2 = base.Reader.ReadInt16();
			InstanceMetadata.TXMPFormat tXMPFormat = (InstanceMetadata.TXMPFormat)base.Reader.ReadInt32();
			InstanceDescriptor instanceDescriptor = base.Reader.ReadInstance();
			InstanceDescriptor instanceDescriptor2 = base.Reader.ReadInstance();
			int num3 = base.Reader.ReadInt32();
			tXMPFlags = (InstanceMetadata.TXMPFlags)((uint)tXMPFlags & 0xFFFFEDEFu);
			base.Xml.WriteStartElement("Texture");
			string fullName = txmp.FullName;
			if (fullName.StartsWith("TXMP", StringComparison.Ordinal))
			{
				fullName = fullName.Substring(4);
			}
			base.Xml.WriteElementString("Flags", tXMPFlags.ToString().Replace(",", " "));
			base.Xml.WriteElementString("Format", tXMPFormat.ToString());
			if (instanceDescriptor2 != null)
			{
				base.Xml.WriteElementString("EnvMap", instanceDescriptor2.FullName);
			}
			if (instanceDescriptor == null)
			{
				string text = baseFileName + ".tga";
				TgaWriter.Write(TextureDatReader.Read(txmp).Surfaces[0], Path.Combine(outputDirPath, text));
				base.Xml.WriteElementString("Image", text);
			}
			else
			{
				WriteAnimationFrames2(instanceDescriptor);
			}
			base.Xml.WriteEndElement();
		}

		private void WriteAnimationFrames2(InstanceDescriptor txan)
		{
			using (BinaryReader binaryReader = txan.OpenRead(12))
			{
				int num = binaryReader.ReadInt16();
				binaryReader.Skip(6);
				int num2 = binaryReader.ReadInt32();
				base.Xml.WriteElementString("Speed", XmlConvert.ToString(num));
				for (int i = 0; i < num2; i++)
				{
					InstanceDescriptor instanceDescriptor;
					if (i == 0)
					{
						binaryReader.Skip(4);
						instanceDescriptor = txmp;
					}
					else
					{
						instanceDescriptor = binaryReader.ReadInstance();
					}
					string text = string.Format("{0}_{1:d3}.tga", baseFileName, i);
					TgaWriter.Write(TextureDatReader.Read(instanceDescriptor).Surfaces[0], Path.Combine(outputDirPath, text));
					base.Xml.WriteElementString("Image", text);
				}
			}
		}
	}
}
