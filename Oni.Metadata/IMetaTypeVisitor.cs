namespace Oni.Metadata
{
	internal interface IMetaTypeVisitor
	{
		void VisitStruct(MetaStruct type);

		void VisitArray(MetaArray type);

		void VisitVarArray(MetaVarArray type);

		void VisitEnum(MetaEnum type);

		void VisitByte(MetaByte type);

		void VisitInt16(MetaInt16 type);

		void VisitUInt16(MetaUInt16 type);

		void VisitInt32(MetaInt32 type);

		void VisitUInt32(MetaUInt32 type);

		void VisitInt64(MetaInt64 type);

		void VisitUInt64(MetaUInt64 type);

		void VisitFloat(MetaFloat type);

		void VisitString(MetaString type);

		void VisitColor(MetaColor type);

		void VisitVector2(MetaVector2 type);

		void VisitVector3(MetaVector3 type);

		void VisitQuaternion(MetaQuaternion type);

		void VisitMatrix4x3(MetaMatrix4x3 type);

		void VisitPlane(MetaPlane type);

		void VisitBoundingSphere(MetaBoundingSphere type);

		void VisitBoundingBox(MetaBoundingBox type);

		void VisitPointer(MetaPointer type);

		void VisitRawOffset(MetaRawOffset type);

		void VisitSepOffset(MetaSepOffset type);

		void VisitPadding(MetaPadding type);
	}
}
