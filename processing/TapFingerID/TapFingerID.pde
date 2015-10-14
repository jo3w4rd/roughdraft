import com.leapmotion.leap.*;

Controller controller = new Controller();
Frame lastFrame = new Frame();

void setup(){
  controller.enableGesture(Gesture.Type.TYPE_KEY_TAP);
}

void draw(){
     Frame newFrame = controller.frame();
    int now = newFrame.gestures().count();
    int sinceSelf = newFrame.gestures(newFrame).count();
    int sinceLast = newFrame.gestures(lastFrame).count();
    
    GestureList gestures = newFrame.gestures(lastFrame);
    for(Gesture gesture : gestures){
      if(gesture.type() == KeyTapGesture.classType()){
        KeyTapGesture keytap = new KeyTapGesture(gesture);
        Pointable tappingPointable = keytap.pointable();
        if(tappingPointable.isFinger()){
          Finger tappingFinger = new Finger(tappingPointable);
          println("Tapper: " + tappingFinger.type());
        }
      }
    }
    
    System.out.println ("(" + now + ", " + sinceSelf + ", " + sinceLast + ") Fid: " + newFrame.id() + " : " + lastFrame.id());
    lastFrame = newFrame;
 
}
