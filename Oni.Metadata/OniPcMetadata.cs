using System.Collections.Generic;

namespace Oni.Metadata
{
	internal class OniPcMetadata : InstanceMetadata
	{
		private static MetaStruct bina = new MetaStruct("BINAInstance", new Field(MetaType.Int32, "DataSize"), new BinaryPartField(MetaType.RawOffset, "DataOffset", "DataSize"));

		private static MetaStruct osbd = new MetaStruct("OSBDInstance", new Field(MetaType.Int32, "DataSize"), new BinaryPartField(MetaType.RawOffset, "DataOffset", "DataSize"));

		private static MetaStruct txmp = new MetaStruct("TXMPInstance", new Field(MetaType.Padding(128)), new Field(MetaType.Enum<TXMPFlags>(), "Flags"), new Field(MetaType.UInt16, "Width"), new Field(MetaType.UInt16, "Height"), new Field(MetaType.Enum<TXMPFormat>(), "Format"), new Field(MetaType.Pointer(TemplateTag.TXAN), "Animation"), new Field(MetaType.Pointer(TemplateTag.TXMP), "EnvMap"), new BinaryPartField(MetaType.RawOffset, "DataOffset"), new Field(MetaType.Padding(12)));

		private static MetaStruct sndd = new MetaStruct("SNDDInstance", new Field(MetaType.Int32, "WaveHeaderSize"), new Field(MetaType.Int16, "Format"), new Field(MetaType.Int16, "ChannelCount"), new Field(MetaType.Int32, "SamplesPerSecond"), new Field(MetaType.Int32, "BytesPerSecond"), new Field(MetaType.Int16, "BlockAlignment"), new Field(MetaType.Int16, "BitsPerSample"), new Field(MetaType.Int16, "AdpcmHeaderSize"), new Field(MetaType.Int16, "SamplesPerBlock"), new Field(MetaType.Int16, "CoefficientCount"), new Field(MetaType.Array(7, new MetaStruct("ADPCMCoefficient", new Field(MetaType.Int16, "Coefficient1"), new Field(MetaType.Int16, "Coefficient2"))), "Coefficients"), new Field(MetaType.Int16, "Duration"), new Field(MetaType.Int32, "DataSize"), new BinaryPartField(MetaType.RawOffset, "DataOffset", "DataSize"));

		protected override void InitializeTemplates(IList<Template> templates)
		{
			base.InitializeTemplates(templates);
			templates.Add(new Template(TemplateTag.BINA, bina, 56129L, "Binary Data"));
			templates.Add(new Template(TemplateTag.OSBD, osbd, 56172L, "Oni Sound Binary Data"));
			templates.Add(new Template(TemplateTag.TXMP, txmp, 36794037633L, "Texture Map"));
			templates.Add(new Template(TemplateTag.SNDD, sndd, 3605880L, "Sound Data"));
		}
	}
}
