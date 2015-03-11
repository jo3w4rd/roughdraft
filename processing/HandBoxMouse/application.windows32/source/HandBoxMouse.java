import processing.core.*; 
import processing.data.*; 
import processing.event.*; 
import processing.opengl.*; 

import com.leapmotion.leap.*; 

import java.util.HashMap; 
import java.util.ArrayList; 
import java.io.File; 
import java.io.BufferedReader; 
import java.io.PrintWriter; 
import java.io.InputStream; 
import java.io.OutputStream; 
import java.io.IOException; 

public class HandBoxMouse extends PApplet {



Controller controller = new Controller();
int boxSize = 300;
int boxX = 0;
int boxY = 0;
int width = 1600;
int height = 800;

public void setup()
{
     size(width, height);
     noFill();

}

public void draw()
{
  background(0xffffff);
  Frame frame = controller.frame();
  InteractionBox iBox = frame.interactionBox();
  
  Pointable pointer = frame.pointables().frontmost();
  Vector normalizedPointerPosition = iBox.normalizePoint( pointer.tipPosition(), false );
  Vector normalizedHandPosition = iBox.normalizePoint( pointer.hand().palmPosition() );
  if( pointer.isValid() && pointer.hand().isValid() )
  {
      int handX = PApplet.parseInt(width * normalizedHandPosition.getX());
      int handY = PApplet.parseInt(height - height * normalizedHandPosition.getY());
      ellipse( handX, handY, 20, 20 );
      if( (handX > boxX + boxSize) ) boxX = handX - boxSize;
      if( (handX < boxX) ) boxX = handX;
      if( (handY > boxY + boxSize) ) boxY = handY - boxSize;
      if( (handY < boxY) ) boxY = handY;
      
      int pointerX = PApplet.parseInt(boxX + boxSize * normalizedPointerPosition.getX());
      int pointerY = PApplet.parseInt(boxY + boxSize - boxSize * normalizedPointerPosition.getY());

      if(pointerX < boxX) boxX = pointerX;
      if(pointerX > boxX + boxSize) boxX += pointerX - boxX - boxSize;
      if(pointerY < boxY) boxY = pointerY;
      if(pointerY > boxY + boxSize) boxY += pointerY - boxY - boxSize;
      
      ellipse( pointerX, pointerY, 10, 10 );


  }
  rect( boxX,  boxY, boxSize, boxSize);
}

  static public void main(String[] passedArgs) {
    String[] appletArgs = new String[] { "HandBoxMouse" };
    if (passedArgs != null) {
      PApplet.main(concat(appletArgs, passedArgs));
    } else {
      PApplet.main(appletArgs);
    }
  }
}
