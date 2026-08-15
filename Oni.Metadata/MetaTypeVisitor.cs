namespace Oni.Metadata
{
	internal abstract class MetaTypeVisitor : IMetaTypeVisitor
	{
		public virtual void VisitStruct(MetaStruct type)
		{
		}

		public virtual void VisitArray(MetaArray type)
		{
		}

		public virtual void VisitVarArray(MetaVarArray type)
		{
		}

		public virtual void VisitEnum(MetaEnum type)
		{
			type.BaseType.Accept(this);
		}

		public virtual void VisitByte(MetaByte type)
		{
		}

		public virtual void VisitInt16(MetaInt16 type)
		{
		}

		public virtual void VisitUInt16(MetaUInt16 type)
		{
		}

		public virtual void VisitInt32(MetaInt32 type)
		{
		}

		public virtual void VisitUInt32(MetaUInt32 type)
		{
		}

		public virtual void VisitInt64(MetaInt64 type)
		{
		}

		public virtual void VisitUInt64(MetaUInt64 type)
		{
		}

		public virtual void VisitFloat(MetaFloat type)
		{
		}

		public virtual void VisitString(MetaString type)
		{
		}

		public virtual void VisitColor(MetaColor type)
		{
		}

		public virtual void VisitVector2(MetaVector2 type)
		{
			VisitStruct(type);
		}

		public virtual void VisitVector3(MetaVector3 type)
		{
			VisitStruct(type);
		}

		public virtual void VisitQuaternion(MetaQuaternion type)
		{
			VisitStruct(type);
		}

		public virtual void VisitMatrix4x3(MetaMatrix4x3 type)
		{
			VisitStruct(type);
		}

		public virtual void VisitPlane(MetaPlane type)
		{
			VisitStruct(type);
		}

		public virtual void VisitBoundingSphere(MetaBoundingSphere type)
		{
			VisitStruct(type);
		}

		public virtual void VisitBoundingBox(MetaBoundingBox type)
		{
			VisitStruct(type);
		}

		public virtual void VisitPointer(MetaPointer type)
		{
		}

		public virtual void VisitRawOffset(MetaRawOffset type)
		{
		}

		public virtual void VisitSepOffset(MetaSepOffset type)
		{
		}

		public virtual void VisitPadding(MetaPadding type)
		{
		}
	}
}
