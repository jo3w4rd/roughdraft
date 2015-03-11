import com.leapmotion.leap.*;

Controller controller = new Controller();
Frame lastFrame = new Frame();

void setup(){
  controller.enableGesture(Gesture.Type.TYPE_CIRCLE);
  controller.enableGesture(Gesture.Type.TYPE_KEY_TAP);
  controller.enableGesture(Gesture.Type.TYPE_SCREEN_TAP);
  controller.enableGesture(Gesture.Type.TYPE_SWIPE);
}

void draw(){
     Frame newFrame = controller.frame();
    int now = newFrame.gestures().count();
    int sinceSelf = newFrame.gestures(newFrame).count();
    int sinceLast = newFrame.gestures(lastFrame).count();

    System.out.println ("(" + now + ", " + sinceSelf + ", " + sinceLast + ") Fid: " + newFrame.id() + " : " + lastFrame.id());
    lastFrame = newFrame;
 
}
