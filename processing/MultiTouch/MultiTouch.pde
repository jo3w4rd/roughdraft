import com.leapmotion.leap.*;

PShape cursor;
Controller leap;
int width = 600;
int height = 600;
int cx = 15;
int cy = 15;
int cw = 40;
int ch = 40;
color cursorNormalColor = color(0, 0, 128);
color cursorHoverColor = color(0, 128, 0);
color cursorTouchColor = color(128, 0, 0);
color backgroundColor = color(250, 235, 200);

void setup()
{
  size(width, height, P2D);
  leap = new Controller();
  cursor = createShape(ELLIPSE, 0, 0, cw, ch);
  cursor.stroke(cursorNormalColor);
  cursor.fill(colorWithAlpha(cursorNormalColor, 16));
}

void draw() 
{
  background(backgroundColor);
  Frame frame = leap.frame();
  InteractionBox iBox = frame.interactionBox();
  
  FingerList fingers = frame.fingers();
  for(int f = 0; f < fingers.count(); f++)
  {
    Finger finger = fingers.get(f);
    Vector normalizedPosition = iBox.normalizePoint(finger.stabilizedTipPosition());
    float pixelX = normalizedPosition.getX() * width;
    float pixelY = height - normalizedPosition.getY() * height;
    cx = (int) pixelX - cw/2;
    cy = (int) pixelY - ch/2;
    
    if(finger.touchZone() == Pointable.Zone.ZONE_HOVERING )
    {
        cursor.stroke(cursorHoverColor);
        cursor.fill(colorWithAlpha(cursorHoverColor, (int)(64 - finger.touchDistance() * 48)));
    }
    else if(finger.touchZone() == Pointable.Zone.ZONE_TOUCHING)
    {
        cursor.stroke(cursorTouchColor);
        cursor.fill(colorWithAlpha(cursorTouchColor, (int)(64 - finger.touchDistance() * 126)));
    }
    else
    {
       cursor.stroke(cursorNormalColor);
       cursor.fill(colorWithAlpha(cursorNormalColor, 16));
    }
    shape(cursor, cx, cy);
  }
}

color colorWithAlpha(color rgb, int alpha)
{
   return color(red(rgb), green(rgb), blue(rgb), alpha); 
}

