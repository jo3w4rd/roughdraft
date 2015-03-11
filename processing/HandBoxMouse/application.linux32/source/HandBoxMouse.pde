import com.leapmotion.leap.*;

Controller controller = new Controller();
int boxSize = 300;
int boxX = 0;
int boxY = 0;
int width = 1600;
int height = 800;

void setup()
{
     size(width, height);
     noFill();

}

void draw()
{
  background(0xffffff);
  Frame frame = controller.frame();
  InteractionBox iBox = frame.interactionBox();
  
  Pointable pointer = frame.pointables().frontmost();
  Vector normalizedPointerPosition = iBox.normalizePoint( pointer.tipPosition(), false );
  Vector normalizedHandPosition = iBox.normalizePoint( pointer.hand().palmPosition() );
  if( pointer.isValid() && pointer.hand().isValid() )
  {
      int handX = int(width * normalizedHandPosition.getX());
      int handY = int(height - height * normalizedHandPosition.getY());
      ellipse( handX, handY, 20, 20 );
      if( (handX > boxX + boxSize) ) boxX = handX - boxSize;
      if( (handX < boxX) ) boxX = handX;
      if( (handY > boxY + boxSize) ) boxY = handY - boxSize;
      if( (handY < boxY) ) boxY = handY;
      
      int pointerX = int(boxX + boxSize * normalizedPointerPosition.getX());
      int pointerY = int(boxY + boxSize - boxSize * normalizedPointerPosition.getY());

      if(pointerX < boxX) boxX = pointerX;
      if(pointerX > boxX + boxSize) boxX += pointerX - boxX - boxSize;
      if(pointerY < boxY) boxY = pointerY;
      if(pointerY > boxY + boxSize) boxY += pointerY - boxY - boxSize;
      
      ellipse( pointerX, pointerY, 10, 10 );


  }
  rect( boxX,  boxY, boxSize, boxSize);
}

