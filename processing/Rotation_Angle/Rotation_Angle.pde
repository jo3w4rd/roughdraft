import com.leapmotion.leap.*;

PShape pointer;
Controller controller = new Controller();
ArrayList<Vector> tapPoints = new ArrayList<Vector>();
ArrayList<Vector> startPoints = new ArrayList<Vector>();

ArrayList<Target> targets = new ArrayList<Target>();

void setup()
{
  frameRate(60);
   size(2500,1200,P3D);
   background(255);
   ambientLight(51, 102, 126);
   controller.enableGesture(Gesture.Type.TYPE_SCREEN_TAP);
   controller.config().setFloat("Gesture.ScreenTap.MinForwardVelocity", 10.0f);
   controller.config().setFloat("Gesture.ScreenTap.HistorySeconds", .5f);
   controller.config().setFloat("Gesture.ScreenTap.MinDistance", 4.0f);
   controller.config().save();
  setupTargets();   
}

void setupTargets()
{
   targets.add(new Target(20, 20, 200, 20)); 
   targets.add(new Target(20, 60, 200, 40)); 
   targets.add(new Target(20, 120, 200, 60)); 
   targets.add(new Target(20, 200, 200, 80)); 
   targets.add(new Target(20, 300, 200, 100)); 
   targets.add(new Target(20, 420, 200, 10)); 
   drawTargets();
}

Frame lastFrame = Frame.invalid();
void draw(){
  background(255); //clear

  Frame frame = controller.frame();
  if(frame != lastFrame)
  {
    InteractionBox iBox = frame.interactionBox(); 
    if(frame.fingers().count() > 0) 
    {
        Vector normTip = iBox.normalizePoint(frame.fingers().get(0).stabilizedTipPosition(), true);
        drawNormalizedPoint(normTip, color(0,128,0));
    }
    GestureList gesturesSince = frame.gestures(lastFrame);
    for(int g = 0; g < gesturesSince.count(); g++)
    {
        ScreenTapGesture screenTap = new ScreenTapGesture(gesturesSince.get(g));
        Vector normTapPoint = iBox.normalizePoint(screenTap.position(), true);
        tapPoints.add(normTapPoint);
        
        int history = 1;
        Frame pastFrame = controller.frame(history++);
        do{
          pastFrame = controller.frame(history++);
        } while( pastFrame.timestamp() < frame.timestamp() - screenTap.duration());
        
        startPoints.add(iBox.normalizePoint(pastFrame.pointable(screenTap.pointable().id()).stabilizedTipPosition(),true));
    }
  }
  lastFrame = frame;
  for( Vector point : tapPoints ) drawNormalizedPoint(point, color(255, 0, 0)); 
  for( Vector point : startPoints ) drawNormalizedPoint(point, color(0, 0, 255)); 

  drawTargets(); 
}

void drawNormalizedPoint(Vector point, int pointColor)
{
       stroke(pointColor);
       pushMatrix();
       translate(point.getX() * 500, 500 * (1 - point.getY()), point.getZ());
       sphere(1);
       popMatrix();  
}

void drawTargets()
{
  for(Target target : targets)
  {
       stroke(0);
       pushMatrix();
       translate(target.x, target.y, 0);
       rect(0, 0, target.width, target.height);
       popMatrix();  
  }
}

void durationAndVelocity()
{
  /*
        Vector pV = screenTap.pointable().tipVelocity();
        System.out.print(pV.toString()); 

        Vector startPoint = pV.times(screenTap.durationSeconds());
        System.out.print(" * " + screenTap.durationSeconds() + " = " + startPoint.toString() + " + " + screenTap.position().toString() + " = "); 
      
        startPoint = screenTap.position().minus(startPoint);
        System.out.println(startPoint.toString() + " norm: " + iBox.normalizePoint(startPoint,true).toString()); 
        startPoints.add(iBox.normalizePoint(startPoint,true));
 */ 
}
