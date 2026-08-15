using System.Collections.Generic;

namespace Oni.Metadata
{
	internal class OniMacMetadata : InstanceMetadata
	{
		private static MetaStruct bina = new MetaStruct("BINAInstance", new Field(MetaType.Int32, "DataSize"), new BinaryPartField(MetaType.SepOffset, "DataOffset", "DataSize"));

		private static MetaStruct txmp = new MetaStruct("TXMPInstance", new Field(MetaType.Padding(128)), new Field(MetaType.Enum<TXMPFlags>(), "Flags"), new Field(MetaType.Int16, "Width"), new Field(MetaType.Int16, "Height"), new Field(MetaType.Enum<TXMPFormat>(), "Format"), new Field(MetaType.Pointer(TemplateTag.TXAN), "Animation"), new Field(MetaType.Pointer(TemplateTag.TXMP), "EnvMap"), new Field(MetaType.Padding(4)), new BinaryPartField(MetaType.SepOffset, "DataOffset"), new Field(MetaType.Padding(8)));

		private static MetaStruct osbd = new MetaStruct("OSBDInstance", new Field(MetaType.Int32, "DataSize"), new BinaryPartField(MetaType.SepOffset, "DataOffset", "DataSize"));

		private static MetaStruct sndd = new MetaStruct("SNDDInstance", new Field(MetaType.Int32, "Flags"), new Field(MetaType.Int32, "Duration"), new Field(MetaType.Int32, "DataSize"), new BinaryPartField(MetaType.RawOffset, "DataOffset", "DataSize"));

		protected override void InitializeTemplates(IList<Template> templates)
		{
			base.InitializeTemplates(templates);
			templates.Add(new Template(TemplateTag.BINA, bina, 89617L, "Binary Data"));
			templates.Add(new Template(TemplateTag.OSBD, osbd, 89660L, "Oni Sound Binary Data"));
			templates.Add(new Template(TemplateTag.TXMP, txmp, 36794461023L, "Texture Map"));
			templates.Add(new Template(TemplateTag.SNDD, sndd, 266731L, "Sound Data"));
		}
	}
}
