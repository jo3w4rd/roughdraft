import com.leapmotion.leap.*;
import java.util.ArrayList;

public class HandPose{
   public String name;
   public PImage referenceImage;
   public ArrayList<Vector> jointPositions = new ArrayList<Vector>(); 

   public float displaySize = 5;
   float top = 600;
   float bottom = 0;
   float left = 600;
   float right = 0;
   
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
   
   public void drawPose(){
      for(Vector joint : jointPositions){
         ellipse(joint.getX(), joint.getZ(), displaySize, displaySize);
         //bounding box
         if(joint.getX() < left) left = joint.getX(); 
         if(joint.getX() > right) right = joint.getX(); 
         if(joint.getZ() < top) top = joint.getZ(); 
         if(joint.getZ() < bottom) bottom = joint.getZ(); 
         image(referenceImage, left, bottom);
      } 
   }
}
