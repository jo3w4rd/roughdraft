import com.leapmotion.leap.*;

Controller controller = new Controller();

void setup(){
  controller.enableGesture(Gesture.Type.TYPE_SWIPE);
}

void draw(){
        Frame frame = controller.frame();
        for (int i = 0; i < frame.gestures().count(); i++) {
            Gesture gesture = frame.gesture(i);
            //if (gesture.type() != Gesture.Type.TYPE_INVALID)
                System.out.println(gesture.type().toString());
        }  
}
