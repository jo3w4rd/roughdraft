import com.leapmotion.leap.*;

Controller controller;
Boolean leapInitialized = false;
Frame lastFrame = new Frame();
int starts = 0;
int stops = 0;

void setup(){
   controller = new Controller();
}

void draw(){
  if(controller.isConnected() && !leapInitialized){
    leapInitialized = true;
    controller.enableGesture(Gesture.Type.TYPE_CIRCLE);
    controller.enableGesture(Gesture.Type.TYPE_SWIPE);
  } 
  
  
  for(int g = 0; g < controller.frame().gestures(lastFrame).count(); g++) {
    Gesture gesture = controller.frame().gestures(lastFrame).get(g);
    if(gesture.state() == Gesture.State.STATE_START){    
      starts++;
      println(gesture.id() + " type: " + gesture.type() + " state " + gesture.state() + " - " + starts + ":" + stops);
    }
    if(gesture.state() == Gesture.State.STATE_STOP)    {
      stops++;
      println(gesture.id() + " type: " + gesture.type() + " state " + gesture.state() + " - " + starts + ":" + stops);
    }
 }
 lastFrame = controller.frame();       
}

