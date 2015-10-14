import com.leapmotion.leap.*;

Controller controller = new Controller();

void setup(){
 //if needed...
}

void draw(){
  background(255);
  Frame frame = controller.frame();
  FingerList extendedFingers = frame.fingers().extended();
  int fingerCount = extendedFingers.count();
}
