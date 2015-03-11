import com.leapmotion.leap.*;
import java.util.ArrayList;

public class HandPose{
   public String name;
   public PImage referenceImage;
   public ArrayList<Vector> jointPositions; 
  
   public HandPose(String name, PImage img, Hand hand){
      this.name = name;
      this.referenceImage = img;
      Matrix handTransform = hand.basis();
      handTransform.setOrigin(hand.palmPosition());
      handTransform = handTransform.rigidInverse();
      for(Finger finger : hand.fingers()){
         for(Bone.Type boneType : Bone.Type.values()) {
             Bone bone = finger.bone(boneType);
             Vector normalized = handTransform.transformPoint(bone.prevJoint());
             jointPositions.add(normalized);
             if(boneType == Bone.Type.TYPE_DISTAL){ //get tip of distal phalanx
                Vector normalizedN = handTransform.transformPoint(bone.nextJoint());
                jointPositions.add(normalizedN);            
             }
         }        
      }
   }
   public HandPose(){}
    
}
