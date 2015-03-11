import processing.core.*; 
import processing.data.*; 
import processing.event.*; 
import processing.opengl.*; 

import com.leapmotion.leap.*; 

import java.util.HashMap; 
import java.util.ArrayList; 
import java.io.BufferedReader; 
import java.io.PrintWriter; 
import java.io.InputStream; 
import java.io.OutputStream; 
import java.io.IOException; 

public class InteractionBoxPainter extends PApplet {



int width = 800;
int height = 600;
int maxBrushSize = 120;
int canvasColor = 0xffffff;
float alphaVal = 10;

Controller leap = new Controller();

public void setup()
{
   frameRate(120);
   size(width, height);
   background(canvasColor);
   stroke(0x00ffffff);
}


public void draw(){
  Frame frame = leap.frame();
  Pointable pointer = frame.pointables().frontmost();
  if( pointer.isValid() )
  {
    
    int frontColor = color( 255, 0, 0, alphaVal );
    int backColor  = color( 0, 0, 255, alphaVal );
    
    InteractionBox iBox = frame.interactionBox();
    Vector stabilizedTip = iBox.normalizePoint(pointer.stabilizedTipPosition());
    fingerPaint(stabilizedTip, backColor);
    Vector tip = iBox.normalizePoint(pointer.tipPosition());
    fingerPaint(tip, frontColor);
  }
}

public void fingerPaint(Vector tip, int paintColor)
{
   fill(paintColor);
    float x = tip.getX() * width;
    float y = height - tip.getY() * height;
    float brushSize = maxBrushSize - maxBrushSize * tip.getZ();
    ellipse( x, y, brushSize, brushSize);   
}

public void keyPressed()
{
   background(canvasColor);
}


  static public void main(String[] passedArgs) {
    String[] appletArgs = new String[] { "InteractionBoxPainter" };
    if (passedArgs != null) {
      PApplet.main(concat(appletArgs, passedArgs));
    } else {
      PApplet.main(appletArgs);
    }
  }
}
