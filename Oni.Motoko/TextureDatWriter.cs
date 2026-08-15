using Oni.Imaging;

namespace Oni.Motoko
{
	internal class TextureDatWriter
	{
		private readonly Importer importer;

		public static void Write(Texture texture, string outputDirPath)
		{
			DatWriter datWriter = new DatWriter();
			Write(texture, datWriter);
			datWriter.Write(outputDirPath);
		}

		public static void Write(Texture texture, Importer importer)
		{
			TextureDatWriter textureDatWriter = new TextureDatWriter(importer);
			textureDatWriter.Write(texture);
		}

		private TextureDatWriter(Importer importer)
		{
			this.importer = importer;
		}

		private void Write(Texture texture)
		{
			ImporterDescriptor importerDescriptor = importer.CreateInstance(TemplateTag.TXMP, texture.Name);
			int value = importer.RawWriter.Align32();
			TextureFlags textureFlags = texture.Flags;
			ImporterDescriptor descriptor = null;
			if (texture.EnvMap != null)
			{
				descriptor = importer.CreateInstance(TemplateTag.TXMP, texture.EnvMap.Name);
				textureFlags |= TextureFlags.HasEnvMap;
			}
			if (texture.Surfaces.Count > 1)
			{
				textureFlags |= TextureFlags.HasMipMaps;
			}
			using (BinaryWriter binaryWriter = importerDescriptor.OpenWrite(128))
			{
				binaryWriter.Write((int)textureFlags);
				binaryWriter.WriteInt16(texture.Width);
				binaryWriter.WriteInt16(texture.Height);
				binaryWriter.Write((int)texture.Format);
				binaryWriter.Write(0);
				binaryWriter.Write(descriptor);
				binaryWriter.Write(value);
				binaryWriter.Skip(12);
			}
			foreach (Surface surface in texture.Surfaces)
			{
				importer.RawWriter.Write(surface.Data);
			}
		}
	}
}
