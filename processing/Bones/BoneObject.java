import com.leapmotion.leap.*;

public class BoneObject{
  Matrix absoluteTransform = new Matrix();
  Matrix relativeTransform = new Matrix();
  int parent;
  int fingerIdx;
  int boneIdx;

  public BoneObject(int parent, int fingerIdx, int boneIdx, Matrix absolute, Matrix relative){
    this.absoluteTransform = absolute;
    this.relativeTransform = relative;
    this.parent = parent;
    this.fingerIdx = fingerIdx;
    this.boneIdx = boneIdx
  }

   public BoneObject(int parentIdx, int fingerIdx, Bone parent, Bone bone){
     this.parent = parentIdx;
    this.fingerIdx = fingerIdx;
    this.boneIdx = bone.type().value();
     this.absoluteTransform = bone.basis();
     this.absoluteTransform.setOrigin(bone.prevJoint());
     //Matrix temp = Matrix.identity();
     //temp.setOrigin(bone.prevJoint());
     //this.absoluteTransform = temp.times(bone.basis()); //.times(temp);
     
   }
   
      public BoneObject(int parentIdx, int fingerIdx, Matrix parentBasis, Vector parentPosition, Bone bone){
         this.fingerIdx = fingerIdx;
         this.boneIdx = bone.type().value();
       
      }
      public BoneObject(int parentIdx, Matrix parentBasis, Vector parentPosition, Matrix boneBasis, Vector bonePosition){
          this.parent = parenIdx;
          this.fingerIdx = fingerIdx;
          this.boneIdx = bone.type().value();
          Matrix parentTransform = new Matrix(parentBasis);
          parentTransform.setOrigin(parentPosition);
          Matrix boneTransform = boneBasis;
          boneTransform.setOrigin(bonePosition);
          this.relativeTransform = boneBasis.times(parentTransform.rigidInverse());
      }

}
