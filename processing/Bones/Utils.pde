float[] MatrixToFloat(Matrix matrix){
  Vector xbasis = matrix.getXBasis();
  Vector ybasis = matrix.getYBasis();
  Vector zbasis = matrix.getZBasis();
  Vector origin = matrix.getOrigin();

  return new float[] {xbasis.getX(), xbasis.getY(), xbasis.getZ(), origin.getX(),
          ybasis.getX(), ybasis.getY(), ybasis.getZ(), origin.getY(),
          zbasis.getX(), zbasis.getY(), zbasis.getZ(), origin.getZ(),
          0, 0, 0, 1};
}

void ApplyLeapMatrix(Matrix matrix){
  Vector xbasis = matrix.getXBasis();
  Vector ybasis = matrix.getYBasis();
  Vector zbasis = matrix.getZBasis();
  Vector origin = matrix.getOrigin();
  System.out.println(origin);
  applyMatrix(xbasis.getX(), xbasis.getY(), xbasis.getZ(), origin.getX(),
          ybasis.getX(), ybasis.getY(), ybasis.getZ(), origin.getY(),
          zbasis.getX(), zbasis.getY(), zbasis.getZ(), origin.getZ(),
          0, 0, 0, 1);
}

Bone.Type intToBoneType(int boneIdx){
   switch(boneIdx){
      case 0:
        return Bone.Type.TYPE_METACARPAL;
      case 1:
        return Bone.Type.TYPE_PROXIMAL;
      case 2:
        return Bone.Type.TYPE_INTERMEDIATE;
      case 3:
        return Bone.Type.TYPE_DISTAL;
       default:
        return Bone.Type.TYPE_METACARPAL;
   } 
}
