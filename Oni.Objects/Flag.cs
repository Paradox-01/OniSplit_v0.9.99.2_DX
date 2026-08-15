using System;
using System.Xml;
using Oni.Imaging;
using Oni.Xml;

namespace Oni.Objects
{
	internal class Flag : ObjectBase
	{
		public Color Color;

		public string Prefix;

		public int ScriptId;

		public string Notes;

		public Flag()
		{
			base.TypeId = ObjectType.Flag;
		}

		protected override void WriteOsd(BinaryWriter writer)
		{
			writer.Write(Color);
			writer.Write(Prefix, 2);
			writer.WriteInt16(ScriptId);
			writer.Write(Notes, 128);
		}

		protected override void ReadOsd(BinaryReader reader)
		{
			Color = reader.ReadColor();
			Prefix = reader.ReadString(2);
			ScriptId = reader.ReadInt16();
			Notes = reader.ReadString(128);
			Prefix = new string(new char[2]
			{
				Prefix[1],
				Prefix[0]
			});
		}

		protected override void WriteOsd(XmlWriter xml)
		{
			throw new NotImplementedException();
		}

		protected override void ReadOsd(XmlReader xml, ObjectLoadContext context)
		{
			while (xml.IsStartElement())
			{
				switch (xml.LocalName)
				{
				case "Color":
				{
					byte[] array = xml.ReadElementContentAsArray(XmlConvert.ToByte);
					if (array.Length > 3)
					{
						Color = new Color(array[0], array[1], array[2], array[3]);
					}
					else
					{
						Color = new Color(array[0], array[1], array[2]);
					}
					break;
				}
				case "Prefix":
				{
					string text = xml.ReadElementContentAsString();
					if (text.Length > 2)
					{
						int num = int.Parse(text);
						text = new string(new char[2]
						{
							(char)((num >> 8) & 0xFF),
							(char)(num & 0xFF)
						});
					}
					Prefix = text;
					break;
				}
				case "FlagId":
					ScriptId = xml.ReadElementContentAsInt();
					break;
				case "Note":
					Notes = xml.ReadElementContentAsString();
					break;
				default:
					xml.Skip();
					break;
				}
			}
		}
	}
}
