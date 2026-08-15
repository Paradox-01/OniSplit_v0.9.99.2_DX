using System;
using System.Collections.Generic;
using System.IO;
using Oni.Dae;
using Oni.Imaging;

namespace Oni.Motoko
{
	internal class TextureDaeWriter
	{
		private readonly string outputDirPath;

		private readonly Dictionary<InstanceDescriptor, Material> materials = new Dictionary<InstanceDescriptor, Material>();

		public TextureDaeWriter(string outputDirPath)
		{
			this.outputDirPath = outputDirPath;
		}

		public Material WriteMaterial(InstanceDescriptor txmp)
		{
			Material value;
			if (!materials.TryGetValue(txmp, out value))
			{
				value = CreateMaterial(txmp);
				materials.Add(txmp, value);
			}
			return value;
		}

		private Material CreateMaterial(InstanceDescriptor txmp)
		{
			Texture texture = TextureDatReader.Read(txmp);
			string path = Utils.CleanupTextureName(txmp.Name) + ".tga";
			path = Path.Combine("images", path);
			TgaWriter.Write(texture.Surfaces[0], Path.Combine(outputDirPath, path));
			string name = TextureNameToId(txmp);
			Image initFrom = new Image
			{
				FilePath = "./" + path.Replace('\\', '/'),
				Name = name
			};
			EffectSurface effectSurface = new EffectSurface(initFrom);
			EffectSampler effectSampler = new EffectSampler(effectSurface)
			{
				WrapS = (texture.WrapU ? EffectSamplerWrap.Wrap : EffectSamplerWrap.None),
				WrapT = (texture.WrapV ? EffectSamplerWrap.Wrap : EffectSamplerWrap.None)
			};
			EffectTexture effectTexture = new EffectTexture(effectSampler, "diffuse_TEXCOORD");
			Effect effect = new Effect
			{
				Name = name,
				DiffuseValue = effectTexture,
				TransparentValue = (texture.HasAlpha ? effectTexture : null),
				Parameters = 
				{
					new EffectParameter("surface", effectSurface),
					new EffectParameter("sampler", effectSampler)
				}
			};
			return new Material
			{
				Name = name,
				Effect = effect
			};
		}

		private static string TextureNameToId(InstanceDescriptor txmp)
		{
			string text = Utils.CleanupTextureName(txmp.Name);
			if (text.StartsWith("Iteration", StringComparison.Ordinal))
			{
				text = text.Substring(9);
				if (char.IsDigit(text[0]) && char.IsDigit(text[1]) && char.IsDigit(text[2]) && text[3] == '_')
				{
					text = text.Substring(4);
				}
			}
			return text;
		}
	}
}
