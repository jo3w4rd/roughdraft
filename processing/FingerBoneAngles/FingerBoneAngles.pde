import com.leapmotion.leap.*;

Controller controller = new Controller();

Finger.Type fingerTypeA = Finger.Type.TYPE_INDEX;
Finger.Type fingerTypeB = Finger.Type.TYPE_MIDDLE;

Bone.Type boneType = Bone.Type.TYPE_PROXIMAL;

void draw(){
   Frame frame = controller.frame();
  if(frame.isValid()){
    Finger fingerA = frame.hands().get(0).fingers().fingerType(fingerTypeA).get(0);
    Finger fingerB = frame.hands().get(0).fingers().fingerType(fingerTypeB).get(0);
    Bone boneA = fingerA.bone(boneType);
    Bone boneB = fingerB.bone(boneType);
    float angle = boneA.direction().angleTo(boneB.direction());
    System.out.println("Angle: " + angle * RAD_TO_DEG);
  } 
}

