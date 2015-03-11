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
      handTransform.origin = hand.palmPosition();
      handTransform = handTransform.rigidInverse();
      for(Finger finger : hand.fingers()){
        for(int b = 0; b < 4; b++){
            Bone bone = finger.bone(b);
            Vector normalized = handTransform.transformPoint(bone.previousJoint());
            jointPositions.add(normalized);
            if(b == 3){ //get tip of distal phalank
              Vector normalized = handBasis.handTransform(bone.nextJoint());
              jointPositions.add(normalized);              
            }
        }
        
      }
   }
   public HandPose(){}
   
   static public ByteArray serialize(HandPose pose){return new ByteArray();}
   static public HandPose deserialize(ByteArray serializedPoze){return new HandPose();}
      
 
}
