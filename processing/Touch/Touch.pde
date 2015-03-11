import com.leapmotion.leap.*;

PShape target;
PShape cursor;
Controller leap;
int width = 600;
int height = 600;
int tx = 100;
int ty = 100;
int tw = 150;
int th = 150;
int cx = 15;
int cy = 15;
int cw = 30;
int ch = 30;
int offsetX = 0;
int offsetY = 0;
color targetNormalColor = color(64, 0, 0);
color targetHoverColor = color(128, 0, 0);
color targetSelectedColor = color(255, 0, 0);
color cursorNormalColor = color(0, 0, 0);
color cursorHoverColor = color(0, 128, 0);
color cursorTouchColor = color(0, 255, 0);
color backgroundColor = color(255, 255, 255);
boolean isDragging = false;
boolean isHovering = false;
boolean isOver = false;

void setup()
{
  size(width, height, P2D);
  leap = new Controller();
  target = createShape(RECT, 0, 0, tw, th);
  target.fill(targetNormalColor);
  cursor = createShape(ELLIPSE, 0, 0, cw, ch);
  cursor.stroke(cursorNormalColor);
  cursor.fill(255, 255, 255, 64);
}

void draw() 
{
  background(backgroundColor);
  
  Frame frame = leap.frame();
  InteractionBox iBox = frame.interactionBox();
  Finger finger = frame.fingers().frontmost();
  if(finger.isValid())
  {
     //is finger over the target?
    Vector normalizedPosition = iBox.normalizePoint(finger.tipPosition());
    float pixelX = normalizedPosition.getX() * width;
    float pixelY = height - normalizedPosition.getY() * height;
    cx = (int) pixelX - cw/2;
    cy = (int) pixelY - ch/2;
    isOver =  pixelX > tx && pixelX < tx + tw && pixelY > ty && pixelY < ty + tw;
    
    if(finger.touchZone() == Pointable.Zone.ZONE_HOVERING )
      {
          cursor.stroke(cursorHoverColor);
          target.fill(targetNormalColor);
          if(isOver)
          {
            target.fill(targetHoverColor);
            isHovering = true;
            isDragging = false;
          }
      }
      else if(finger.touchZone() == Pointable.Zone.ZONE_TOUCHING)
      {
          cursor.stroke(cursorTouchColor);
          if(isOver)
          {
            if(!isDragging)
            {
               if(isHovering)
               {
                 isHovering = false;
                 isDragging = true;
                 target.fill(targetSelectedColor);
               }
               offsetX = tx - (cx + cw/2);
               offsetY = ty - (cy + ch/2); 
            }
            tx = (int)pixelX + offsetX;
            ty =(int) pixelY + offsetY;
          }
      }
      else
      {
         target.fill(targetNormalColor);
         cursor.stroke(cursorNormalColor);
         isHovering = false;
         isDragging = false; 
      }

    shape(target, tx, ty); 
    shape(cursor, cx, cy);
  }
  else
  {
    shape(target, tx, ty);     
  }
}
