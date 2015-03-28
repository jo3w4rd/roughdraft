import com.leapmotion.leap.*;

Controller controller = new Controller();
String[] typeMap = {"thumb", "index", "middle", "ring", "pinky", "none"};

void setup(){
  size( 300,300); 
  fill(0);
  textAlign(LEFT);
}

void draw(){
  background(255);
  Frame frame = controller.frame();
  Hand hand = frame.hands().frontmost();
  int fingerType = 5;
  for(int f = 0; f < 5; f++){
     Finger finger = hand.fingers().get(f);
     float curl = fingerCurl(finger);  
     if(finger.isValid()) fingerType = finger.type().ordinal(); 
     text(typeMap[f] + " curl = " + curl, 40, 20 + f * 20);
  }
  

}

float fingerCurl(Finger finger){
  Vector metacarpalDirection;
  if(finger.type() == Finger.Type.TYPE_THUMB) { //Thumb is special
      metacarpalDirection = finger.bone(Bone.Type.TYPE_INTERMEDIATE).basis().getZBasis().opposite();
  } else {
      metacarpalDirection = finger.bone(Bone.Type.TYPE_METACARPAL).basis().getZBasis().opposite();    
  }
  Vector distalPhalangeDirection = finger.bone(Bone.Type.TYPE_DISTAL).basis().getZBasis().opposite(); 
  float rawangle = metacarpalDirection.angleTo(distalPhalangeDirection) * 180/PI;
  Vector crossBones = metacarpalDirection.cross(distalPhalangeDirection);
  Vector boneXBasis = finger.bone(Bone.Type.TYPE_METACARPAL).basis().getXBasis(); //lateral basis vector
  if(finger.hand().isLeft()) boneXBasis = boneXBasis.opposite(); //Left hand uses a left-hand basis
  int sign = (crossBones.dot(boneXBasis) >= 0) ? 1 : -1;
  return sign * rawangle;  
}

