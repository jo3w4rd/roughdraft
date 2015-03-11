import com.leapmotion.leap.*;
import com.leapmotion.leap.Gesture.State;
import com.leapmotion.leap.Gesture.Type;

import java.io.File;

PImage[] images = new PImage[24];
Controller leap = new Controller();

void setup()
{
  frameRate(60);
   size(527,333);
   try{
     File dir = new File(sketchPath + "/Bison");
     File[] files = dir.listFiles();
     for (Integer i = 0; i < files.length; i++) {
        if(!files[i].isHidden()){
          System.out.println(i + " " + files[i].toString());
          images[i-1] = loadImage(files[i].getPath());
        }
     }
   }
   catch( Exception e) { System.out.println(e);}
}

float position = 0;
int cell = 0;
int direction = 1;
void draw(){
  System.out.println(position + ", " +  cell + ", " + frameRate);
  cell += Math.round(position * direction * .8);
  position = position % 1;
   if( cell >= images.length - 2 ) cell = 0;
  if( cell < 0 ) cell = images.length - 2;

  //System.out.println(cell + ": " + images[cell].width + ", " + images[cell].height);
  if( images[cell].width > 0 ) background(images[cell]);
  if( leap.isConnected())
  {
        if(!leap.isGestureEnabled(Type.TYPE_CIRCLE)) leap.enableGesture(Type.TYPE_CIRCLE);
        if(!leap.isGestureEnabled(Type.TYPE_SWIPE)) leap.enableGesture(Type.TYPE_SWIPE);
        Frame frame = leap.frame();
        GestureList gestures = frame.gestures();
        for (int i = 0; i < gestures.count(); i++) {
            Gesture gesture = gestures.get(i);

            switch (gesture.type()) {
                case TYPE_CIRCLE:
                    CircleGesture circle = new CircleGesture(gesture);

                    // Calculate clock direction using the angle between circle normal and pointable
                    if (circle.pointable().direction().angleTo(circle.normal()) <= Math.PI/4) {
                        // Clockwise if angle is less than 90 degrees
                        direction = 1;
                    } else {
                        direction = -1;
                    }

                    // Calculate angle swept since last frame
                    double sweptAngle = 0;
                    if (circle.state() != State.STATE_START) {
                        CircleGesture previousUpdate = new CircleGesture(leap.frame(1).gesture(circle.id()));
                        sweptAngle = (circle.progress() - previousUpdate.progress()) * 20;
                        //sweptAngle = (sweptAngle > 1) ? 1 : sweptAngle;
                        position += circle.progress();
                    }


                    break;
                case TYPE_SWIPE:
                    SwipeGesture swipe = new SwipeGesture(gesture);
                    if( Math.abs(swipe.direction().getX()) > 0.5 ) {
                       if( swipe.direction().getX() > 0 ) {
                          direction = 1;
                       } else {
                         direction = -1;
                       }
                       
                       position = 1;
                    }

                    break;
                case TYPE_SCREEN_TAP:
                    ScreenTapGesture screenTap = new ScreenTapGesture(gesture);
                    System.out.println("Screen Tap id: " + screenTap.id()
                               + ", " + screenTap.state()
                               + ", position: " + screenTap.position()
                               + ", direction: " + screenTap.direction());
                    break;
                case TYPE_KEY_TAP:
                    KeyTapGesture keyTap = new KeyTapGesture(gesture);
                    System.out.println("Key Tap id: " + keyTap.id()
                               + ", " + keyTap.state()
                               + ", position: " + keyTap.position()
                               + ", direction: " + keyTap.direction());
                    break;
                default:
                    System.out.println("Unknown gesture type.");
                    break;
            }
        }

  }
}
