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
  Vector metacarpalDirection = finger.jointPosition(Finger.Joint.JOINT_MCP).minus(finger.jointPosition(Finger.Joint.JOINT_PIP));
  Vector distalPhalangeDirection = finger.jointPosition(Finger.Joint.JOINT_DIP).minus(finger.jointPosition(Finger.Joint.JOINT_TIP));
  float angle = metacarpalDirection.angleTo(distalPhalangeDirection) * 180/PI;
  return angle;  
}

