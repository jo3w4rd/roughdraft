import com.leapmotion.leap.*;

Controller controller = new Controller();
String[] fingernames = {"Thumb", "Index", "Middle", "Ring", "Pinky"};
String[] bonenames = {"Metacarpal", "Proximal", "Intermediate", "Distal"};
void setup(){
  
}

void draw(){
  Frame frame = controller.frame();
  for(Hand hand : frame.hands())
  {
    String handType = hand.isLeft() ? "Left hand" : "Right Hand";
    System.out.println(handType + " " + hand.palmPosition());
    
     for(Finger finger : hand.fingers())
     {
        System.out.println("    " + fingernames[finger.type().ordinal()] + " " + finger.tipPosition());
        for(Bone.Type bones : Bone.Type.values())
        {
           Bone bone = finger.bone(bones);
           System.out.println("        " + bonenames[bone.type().ordinal()] + " " + bone.basis().getOrigin());
        } 
     }
  }
}
