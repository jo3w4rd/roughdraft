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

public class TouchPoints extends PApplet {



PShape cursor;
Controller leap;
int windowWidth = 800;
int windowHeight = 800;
int cursorNormalColor = color(0, 0, 128);
int cursorHoverColor = color(0, 128, 0);
int cursorTouchColor = color(128, 0, 0);
int backgroundColor = color(250, 235, 200);

public void setup()
{
  size(windowWidth, windowHeight, P2D);
  leap = new Controller();
  cursor = createShape(ELLIPSE, 0, 0, 40, 40);
  cursor.stroke(cursorNormalColor);
  cursor.fill(colorWithAlpha(cursorNormalColor, 16));
}

public void draw() 
{
  background(backgroundColor);
  
  InteractionBox iBox = leap.frame().interactionBox();  
  PointableList pointables = leap.frame().pointables();
  for(int p = 0; p < pointables.count(); p++)
  {
    Pointable pointable = pointables.get(p);
    Vector normalizedPosition = iBox.normalizePoint(pointable.stabilizedTipPosition());
    float pixelX = normalizedPosition.getX() * windowWidth;
    float pixelY = windowHeight - normalizedPosition.getY() * windowHeight;
    int cx = (int) pixelX - 20;
    int cy = (int) pixelY - 20;
    
    Pointable.Zone fingerzone = pointable.touchZone();
    
    if(pointable.touchZone() == Pointable.Zone.ZONE_HOVERING )
    {
        cursor.stroke(cursorHoverColor);
        cursor.fill(colorWithAlpha(cursorHoverColor, (int)(64 - pointable.touchDistance() * 48)));
    }
    else if(pointable.touchZone() == Pointable.Zone.ZONE_TOUCHING)
    {
        cursor.stroke(cursorTouchColor);
        cursor.fill(colorWithAlpha(cursorTouchColor, (int)(64 - pointable.touchDistance() * 126)));
    }
    else
    {
       cursor.stroke(cursorNormalColor);
       cursor.fill(colorWithAlpha(cursorNormalColor, 16));
    }
    shape(cursor, cx, cy);
  }
}

public int colorWithAlpha(int rgb, int alpha)
{
   return color(red(rgb), green(rgb), blue(rgb), alpha); 
}

  static public void main(String[] passedArgs) {
    String[] appletArgs = new String[] { "TouchPoints" };
    if (passedArgs != null) {
      PApplet.main(concat(appletArgs, passedArgs));
    } else {
      PApplet.main(appletArgs);
    }
  }
}
