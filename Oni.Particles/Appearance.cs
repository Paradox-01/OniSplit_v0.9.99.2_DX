using Oni.Imaging;

namespace Oni.Particles
{
	internal class Appearance
	{
		private Value scale;

		private Value yScale;

		private Value rotation;

		private Value alpha;

		private Value xOffset;

		private Value xShorten;

		private Value tint;

		private Value edgeFadeMin;

		private Value edgeFadeMax;

		private Value maxContrail;

		private Value lensFlareDistance;

		private int fadeInFrames;

		private int fadeOutFrames;

		private int maxDecals;

		private int decalFadeFrames;

		private Value decalWrapAngle;

		private string textureName;

		public string TextureName
		{
			get
			{
				return textureName;
			}
			set
			{
				textureName = value;
			}
		}

		public Value Scale
		{
			get
			{
				return scale;
			}
			set
			{
				scale = value;
			}
		}

		public Value YScale
		{
			get
			{
				return yScale;
			}
			set
			{
				yScale = value;
			}
		}

		public Value Rotation
		{
			get
			{
				return rotation;
			}
			set
			{
				rotation = value;
			}
		}

		public Value Alpha
		{
			get
			{
				return alpha;
			}
			set
			{
				alpha = value;
			}
		}

		public Value XOffset
		{
			get
			{
				return xOffset;
			}
			set
			{
				xOffset = value;
			}
		}

		public Value XShorten
		{
			get
			{
				return xShorten;
			}
			set
			{
				xShorten = value;
			}
		}

		public Value Tint
		{
			get
			{
				return tint;
			}
			set
			{
				tint = value;
			}
		}

		public Value EdgeFadeMin
		{
			get
			{
				return edgeFadeMin;
			}
			set
			{
				edgeFadeMin = value;
			}
		}

		public Value EdgeFadeMax
		{
			get
			{
				return edgeFadeMax;
			}
			set
			{
				edgeFadeMax = value;
			}
		}

		public Value MaxContrail
		{
			get
			{
				return maxContrail;
			}
			set
			{
				maxContrail = value;
			}
		}

		public Value LensFlareDistance
		{
			get
			{
				return lensFlareDistance;
			}
			set
			{
				lensFlareDistance = value;
			}
		}

		public int LensFlareFadeInFrames
		{
			get
			{
				return fadeInFrames;
			}
			set
			{
				fadeInFrames = value;
			}
		}

		public int LensFlareFadeOutFrames
		{
			get
			{
				return fadeOutFrames;
			}
			set
			{
				fadeOutFrames = value;
			}
		}

		public int MaxDecals
		{
			get
			{
				return maxDecals;
			}
			set
			{
				maxDecals = value;
			}
		}

		public int DecalFadeFrames
		{
			get
			{
				return decalFadeFrames;
			}
			set
			{
				decalFadeFrames = value;
			}
		}

		public Value DecalWrapAngle
		{
			get
			{
				return decalWrapAngle;
			}
			set
			{
				decalWrapAngle = value;
			}
		}

		public Appearance()
		{
			scale = Value.FloatOne;
			yScale = Value.FloatOne;
			rotation = Value.FloatZero;
			alpha = Value.FloatOne;
			textureName = "notfoundtex";
			xOffset = Value.FloatZero;
			xShorten = Value.FloatZero;
			tint = new Value(new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue));
			edgeFadeMin = Value.FloatZero;
			edgeFadeMax = Value.FloatZero;
			maxContrail = Value.FloatZero;
			lensFlareDistance = Value.FloatZero;
			maxDecals = 100;
			decalFadeFrames = 60;
			decalWrapAngle = new Value(60f);
		}

		public Appearance(BinaryReader reader)
		{
			scale = Value.Read(reader);
			yScale = Value.Read(reader);
			rotation = Value.Read(reader);
			alpha = Value.Read(reader);
			textureName = reader.ReadString(32);
			xOffset = Value.Read(reader);
			xShorten = Value.Read(reader);
			tint = Value.Read(reader);
			edgeFadeMin = Value.Read(reader);
			edgeFadeMax = Value.Read(reader);
			maxContrail = Value.Read(reader);
			lensFlareDistance = Value.Read(reader);
			fadeInFrames = reader.ReadInt16();
			fadeOutFrames = reader.ReadInt16();
			maxDecals = reader.ReadInt16();
			decalFadeFrames = reader.ReadInt16();
			decalWrapAngle = Value.Read(reader);
		}

		public void Write(BinaryWriter writer)
		{
			scale.Write(writer);
			yScale.Write(writer);
			rotation.Write(writer);
			alpha.Write(writer);
			writer.Write(textureName, 32);
			xOffset.Write(writer);
			xShorten.Write(writer);
			tint.Write(writer);
			edgeFadeMin.Write(writer);
			edgeFadeMax.Write(writer);
			maxContrail.Write(writer);
			lensFlareDistance.Write(writer);
			writer.WriteInt16(fadeInFrames);
			writer.WriteInt16(fadeOutFrames);
			writer.WriteInt16(maxDecals);
			writer.WriteInt16(decalFadeFrames);
			decalWrapAngle.Write(writer);
		}
	}
}
