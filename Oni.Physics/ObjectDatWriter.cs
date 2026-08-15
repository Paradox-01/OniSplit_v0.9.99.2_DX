namespace Oni.Physics
{
	internal class ObjectDatWriter
	{
		internal static ImporterDescriptor WriteAnimation(ObjectAnimation animation, Importer importer)
		{
			ObjectAnimationKey objectAnimationKey = animation.Keys[0];
			Matrix matrix = Matrix.CreateScale(objectAnimationKey.Scale);
			Matrix m = matrix * Matrix.CreateFromQuaternion(objectAnimationKey.Rotation) * Matrix.CreateTranslation(objectAnimationKey.Translation);
			ImporterDescriptor importerDescriptor = importer.CreateInstance(TemplateTag.OBAN, animation.Name);
			using (BinaryWriter binaryWriter = importerDescriptor.OpenWrite(12))
			{
				binaryWriter.Write((int)animation.Flags);
				binaryWriter.WriteMatrix4x3(m);
				binaryWriter.WriteMatrix4x3(matrix);
				binaryWriter.WriteInt16(1);
				binaryWriter.WriteUInt16(animation.Length);
				binaryWriter.WriteInt16(animation.Stop);
				binaryWriter.WriteUInt16(animation.Keys.Length);
				ObjectAnimationKey[] keys = animation.Keys;
				foreach (ObjectAnimationKey objectAnimationKey2 in keys)
				{
					binaryWriter.Write(objectAnimationKey2.Rotation);
					binaryWriter.Write(objectAnimationKey2.Translation);
					binaryWriter.Write(objectAnimationKey2.Time);
				}
				return importerDescriptor;
			}
		}
	}
}
