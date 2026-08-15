using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using Oni.Imaging;
using Oni.Metadata;
using Oni.Xml;

namespace Oni.Motoko
{
	internal class TextureXmlImporter
	{
		private readonly XmlImporter importer;

		private readonly XmlReader xml;

		private readonly string filePath;

		public TextureXmlImporter(XmlImporter importer, XmlReader xml, string filePath)
		{
			this.importer = importer;
			this.xml = xml;
			this.filePath = filePath;
		}

		public void Import()
		{
			xml.ReadStartElement();
			string name = Importer.DecodeFileName(Path.GetFileNameWithoutExtension(filePath));
			InstanceMetadata.TXMPFlags tXMPFlags = MetaEnum.Parse<InstanceMetadata.TXMPFlags>(xml.ReadElementContentAsString("Flags", ""));
			InstanceMetadata.TXMPFormat tXMPFormat = MetaEnum.Parse<InstanceMetadata.TXMPFormat>(xml.ReadElementContentAsString("Format", ""));
			int num = 0;
			int num2 = 0;
			int value = 1;
			if (xml.IsStartElement("Width"))
			{
				num = xml.ReadElementContentAsInt();
			}
			if (xml.IsStartElement("Height"))
			{
				num2 = xml.ReadElementContentAsInt();
			}
			string text = null;
			if (xml.IsStartElement("EnvMap"))
			{
				text = xml.ReadElementContentAsString();
				if (text != null && text.Length == 0)
				{
					text = null;
				}
			}
			if (xml.IsStartElement("Speed"))
			{
				value = xml.ReadElementContentAsInt();
			}
			List<string> list = new List<string>();
			string directoryName = Path.GetDirectoryName(filePath);
			while (xml.IsStartElement("Image"))
			{
				string text2 = xml.ReadElementContentAsString();
				if (!Path.IsPathRooted(text2))
				{
					text2 = Path.Combine(directoryName, text2);
				}
				if (!File.Exists(text2))
				{
					throw new IOException(string.Format("Could not find image file '{0}'", text2));
				}
				list.Add(text2);
			}
			xml.ReadEndElement();
			List<Surface> list2 = new List<Surface>();
			foreach (string item in list)
			{
				list2.Add(TgaReader.Read(item));
			}
			if (list2.Count == 0)
			{
				throw new InvalidDataException("No images found. A texture must have at least one image.");
			}
			int num3 = 0;
			int num4 = 0;
			foreach (Surface item2 in list2)
			{
				if (num3 == 0)
				{
					num3 = item2.Width;
				}
				else if (num3 != item2.Width)
				{
					throw new NotSupportedException("All animation frames must have the same size.");
				}
				if (num4 == 0)
				{
					num4 = item2.Height;
				}
				else if (num4 != item2.Height)
				{
					throw new NotSupportedException("All animation frames must have the same size.");
				}
			}
			if (num == 0)
			{
				num = num3;
			}
			else if (num > num3)
			{
				throw new NotSupportedException("Cannot upscale images.");
			}
			if (num2 == 0)
			{
				num2 = num4;
			}
			else if (num2 > num4)
			{
				throw new NotSupportedException("Cannot upscale images.");
			}
			if (num != num3 || num2 != num4)
			{
				for (int i = 0; i < list2.Count; i++)
				{
					list2[i] = list2[i].Resize(num, num2);
				}
			}
			tXMPFlags |= InstanceMetadata.TXMPFlags.SwapBytes;
			if (text != null)
			{
				tXMPFlags |= InstanceMetadata.TXMPFlags.HasEnvMap;
			}
			for (int j = 0; j < list2.Count; j++)
			{
				BinaryWriter binaryWriter = ((j != 0) ? importer.BeginXmlInstance(TemplateTag.TXMP, null, j.ToString(CultureInfo.InvariantCulture)) : importer.BeginXmlInstance(TemplateTag.TXMP, name, j.ToString(CultureInfo.InvariantCulture)));
				binaryWriter.Skip(128);
				binaryWriter.Write((int)tXMPFlags);
				binaryWriter.WriteUInt16(num);
				binaryWriter.WriteUInt16(num2);
				binaryWriter.Write((int)tXMPFormat);
				if (j == 0 && list2.Count > 1)
				{
					binaryWriter.WriteInstanceId(list2.Count);
				}
				else
				{
					binaryWriter.Write(0);
				}
				if (text != null)
				{
					binaryWriter.WriteInstanceId(list2.Count + ((list2.Count > 1) ? 1 : 0));
				}
				else
				{
					binaryWriter.Write(0);
				}
				binaryWriter.Write(importer.RawWriter.Align32());
				binaryWriter.Skip(12);
				Surface surface = list2[j];
				List<Surface> list3 = new List<Surface> { surface };
				if ((tXMPFlags & InstanceMetadata.TXMPFlags.HasMipMaps) != InstanceMetadata.TXMPFlags.None)
				{
					int num5 = num;
					int num6 = num2;
					while (num5 > 1 || num6 > 1)
					{
						num5 = Math.Max(num5 >> 1, 1);
						num6 = Math.Max(num6 >> 1, 1);
						list3.Add(surface.Resize(num5, num6));
					}
				}
				foreach (Surface item3 in list3)
				{
					Surface surface2 = item3.Convert(TextureFormatToSurfaceFormat(tXMPFormat));
					importer.RawWriter.Write(surface2.Data);
				}
				importer.EndXmlInstance();
			}
			if (list2.Count > 1)
			{
				ImporterDescriptor importerDescriptor = importer.CreateInstance(TemplateTag.TXAN);
				using (BinaryWriter binaryWriter2 = importerDescriptor.OpenWrite(12))
				{
					binaryWriter2.WriteInt16(value);
					binaryWriter2.WriteInt16(value);
					binaryWriter2.Write(0);
					binaryWriter2.Write(list2.Count);
					binaryWriter2.Write(0);
					for (int k = 1; k < list2.Count; k++)
					{
						binaryWriter2.WriteInstanceId(k);
					}
				}
			}
			if (text != null)
			{
				importer.CreateInstance(TemplateTag.TXMP, text);
			}
		}

		private static SurfaceFormat TextureFormatToSurfaceFormat(InstanceMetadata.TXMPFormat format)
		{
			switch (format)
			{
			case InstanceMetadata.TXMPFormat.BGRA4444:
				return SurfaceFormat.BGRA4444;
			case InstanceMetadata.TXMPFormat.BGR555:
				return SurfaceFormat.BGRX5551;
			case InstanceMetadata.TXMPFormat.BGRA5551:
				return SurfaceFormat.BGRA5551;
			case InstanceMetadata.TXMPFormat.RGBA:
				return SurfaceFormat.RGBA;
			case InstanceMetadata.TXMPFormat.BGR:
				return SurfaceFormat.BGRX;
			case InstanceMetadata.TXMPFormat.DXT1:
				return SurfaceFormat.DXT1;
			default:
				throw new NotSupportedException(string.Format("Texture format {0} is not supported", format));
			}
		}
	}
}
