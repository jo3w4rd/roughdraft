import com.leapmotion.leap.*;

Controller controller = new Controller();
int boxHeight = 200;
int boxWidth = 200;
int boxX = 0;
int boxY = 0;
int width = 1600;
int height = 800;
color canvasColor = 0xffffff;
float lastX = 0;
float lastY = 0;
float offsetX = 0;
float offsetY = 0;
float dt = 0;

void setup()
{
     size(width, height);
     noFill();
     background(canvasColor);
     stroke(0x00);
}

void draw()
{
  //background(0xffffff);
  Frame frame = controller.frame();
  InteractionBox iBox = frame.interactionBox();
  dt = (frame.timestamp() - controller.frame(1).timestamp()) * 1;
  
  Pointable pointer = frame.pointables().frontmost();
  Vector normalizedPointerPosition = iBox.normalizePoint( pointer.tipPosition(), false );
  //Vector normalizedHandPosition = iBox.normalizePoint( pointer.hand().palmPosition() );
  //Vector extendedHandPosition = iBox.normalizePoint( pointer.hand().stabilizedPalmPosition().plus(pointer.hand().direction().times(pointer.length() * 1.0f)) );
  Vector  extendedHandPosition = iBox.normalizePoint( pointer.stabilizedTipPosition().minus(pointer.direction().times(pointer.length() * 1.0f)) );
  
  if( pointer.isValid() && pointer.hand().isValid() )
  {
      int handX = int(width * extendedHandPosition.getX());
      int handY = int(height - height * extendedHandPosition.getY());
      ellipse( handX, handY, 20, 20 );
      
      if( (handX > boxX + boxWidth) ) boxX = handX - boxWidth;
      if( (handX < boxX) ) boxX = handX;
      if( (handY > boxY + boxHeight) ) boxY = handY - boxHeight;
      if( (handY < boxY) ) boxY = handY;
      
      
      
      int pointerX = int(boxX + boxWidth * normalizedPointerPosition.getX());
      int pointerY = int(boxY + boxHeight - boxHeight * normalizedPointerPosition.getY());

      ellipse( pointerX, pointerY, 10, 10 );
      fingerPaint(pointerX, pointerY, 0xffff0000);

      int refX = int(width * normalizedPointerPosition.getX());
      int refY = int(height - height * normalizedPointerPosition.getY());

      ellipse( refX, refY, 5, 5 );

  }
  rect( boxX,  boxY, boxWidth, boxHeight);
}

void fingerPaint(float x, float y, color paintColor)
{
   fill(paintColor);
    float brushSize = 20;
    ellipse( x, y, brushSize, brushSize);  
   noFill(); 
}

void keyPressed()
{
   background(canvasColor);
}

