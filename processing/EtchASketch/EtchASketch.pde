import com.leapmotion.leap.*;

PImage pictureFrame;

int originX = 42;
int originY = 42;
int canvasWidth = 212;
int canvasHeight = 155;
int windowWidth = 300;
int windowHeight = 247;

color canvasColor = 0xffffff;
color stylusColor = 0xff444444;
float stylusSize = 5;
float alphaVal = 10;
float stylusSpeed = 66;

float stylusX = originX + canvasWidth/2;
float stylusY = canvasHeight + originY - canvasHeight/2;

Controller leap = new Controller();
CircleGesture leftCircle = new CircleGesture();
float leftDirection = 1;
float leftProgress = 0;
float deltaLeftProgress = 0;
CircleGesture rightCircle = new CircleGesture();
float rightDirection = 1;
float rightProgress = 0;
float deltaRightProgress;
Frame lastFrame = Frame.invalid();

void setup()
{
   frameRate(30);
   size(windowWidth, windowHeight);
   background(canvasColor);
   stroke(stylusColor);
   strokeWeight(stylusSize);
   leap.enableGesture(Gesture.Type.TYPE_CIRCLE);
   leap.config().setFloat("Gesture.Circle.MinRadius", 1);
   leap.config().setFloat("Gesture.Circle.MinArc", .05f);
   Boolean success = leap.config().save();
   //pictureFrame = loadImage("Frame.png");
   //image(pictureFrame, 0, 0);
}

void updateGestures(){
  Frame frame = leap.frame();
  GestureList gestures = frame.gestures(lastFrame);
   leftCircle = new CircleGesture(frame.gesture(leftCircle.id()));
  if( leftCircle.isValid() )
  {
     deltaLeftProgress = leftProgress - leftCircle.progress();
     leftProgress = leftCircle.progress();
     if (leftCircle.pointable().direction().angleTo(leftCircle.normal()) <= Math.PI/2) {
          leftDirection = -1;
     }
     else
     {
         leftDirection = 1;
     }
     //System.out.println("Updating left " + leftCircle.id());

  }
  else
  {
     Hand leftmostHand = frame.hands().leftmost();
     for(int g = 0; g < gestures.count(); g++)
     {
        Gesture gesture = gestures.get(g);
        if( /*gesture.state() == Gesture.State.STATE_START &&*/
            leftmostHand.palmPosition().getX() < 0 &&
            gesture.hands().get(0).equals(leftmostHand)) 
            {
               leftCircle = new CircleGesture(gesture);
               leftProgress = leftCircle.progress();
               deltaLeftProgress = 0;
               if (leftCircle.pointable().direction().angleTo(leftCircle.normal()) <= Math.PI/2) {
                  leftDirection = 1;
               }
               else
               {
                  leftDirection = -1;
               }
               //System.out.println("New left " + leftCircle.id());

               break; //Use the first circle found
            }
     }
  }
    
  rightCircle = new CircleGesture(frame.gesture(rightCircle.id()));
  if( rightCircle.isValid() )
  {
     deltaRightProgress = rightProgress - rightCircle.progress();
     rightProgress = rightCircle.progress();
     //System.out.println("Updating right " + rightCircle.id());
  }
  else
  {
     Hand rightmostHand = frame.hands().rightmost();
     for(int g = 0; g < gestures.count(); g++)
     {
        Gesture gesture = gestures.get(g);
        if(/* gesture.state() == Gesture.State.STATE_START && */
            rightmostHand.palmPosition().getX() >= 0 &&
            gesture.hands().get(0).equals(rightmostHand)) 
            {
               rightCircle = new CircleGesture(gesture);
               rightProgress = rightCircle.progress();
               deltaRightProgress = 0;
               if (rightCircle.pointable().direction().angleTo(rightCircle.normal()) <= Math.PI/2) {
                  rightDirection = 1;
               }
               else
               {
                  rightDirection = -1;
               }
               //System.out.println("New right " + rightCircle.id());

               break;  //Use the first circle found
            }
     }
  }
  
  lastFrame = frame;
}

void draw()
{
   updateGestures();
   float toX = stylusX + leftDirection * deltaLeftProgress * stylusSpeed;
   if(toX < originX) toX = originX;
   if(toX > originX + canvasWidth) toX = originX + canvasWidth;
   float toY = stylusY + rightDirection * deltaRightProgress * stylusSpeed;
   if(toY < originY) toY = originY;
   if(toY > originY + canvasHeight) toY = originY + canvasHeight;
   
   line(stylusX, stylusY, toX, toY);
   //System.out.println(stylusX + ", " + stylusY + ", " + toX + ", " + toY + ", " + (deltaLeftProgress * stylusSpeed) + ", " + (deltaRightProgress * stylusSpeed));
   stylusX = toX;
   stylusY = toY;
   deltaRightProgress = 0;
   deltaLeftProgress = 0;
}

void keyPressed()
{
   background(canvasColor);
   //image(pictureFrame, 0, 0);
   
   stylusX = originX + canvasWidth/2;
   stylusY = canvasHeight + originY - canvasHeight/2;
}

static public void main(String args[]) {
    PApplet.main("EtchASketch");
}

