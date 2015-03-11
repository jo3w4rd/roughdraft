import com.leapmotion.leap.*;

Controller leap = new Controller();

float dx = 0;
float dy = 0;
float dz = 0;
float cdx = 0;
float cdy = 0;
float cdz = 0;
float adx = 0;
float ady = 0;
float adz = 0;

Hand lastHandOne = Hand.invalid();
Hand lastHandTwo = Hand.invalid();

void setup()
{
 colorMode(RGB,1); 
 size(600,600);
}
void draw()
{
  background(1,1,1);
  Frame frame = leap.frame();
  InteractionBox iBox = frame.interactionBox();
  Hand handOne = frame.hand(lastHandOne.id());
  if(handOne.isValid())
  {
    if(lastHandOne.isValid())
    {
       dx = handOne.palmPosition().getX() - lastHandOne.palmPosition().getX(); 
       dy = handOne.palmPosition().getY() - lastHandOne.palmPosition().getY(); 
       dz = handOne.palmPosition().getZ() - lastHandOne.palmPosition().getZ();
       cdx += dx;
       cdy += dy;
       cdz += dz;
       adx += abs(dx);
       ady += abs(dy);
       adz += abs(dz);
       print("(" + dx + ", " + dy + ", " + dz + ")"); 
       print(" : (" + cdx + ", " + cdy + ", " + cdz + ")"); 
       println(" : (" + adx + ", " + ady + ", " + adz + ")"); 
    }
    Vector p = iBox.normalizePoint(handOne.palmPosition());
    fill(1,0,0);
    ellipse( width * p.getX(), height - height * p.getY(), 50, 50);
  }
  else
  {
     if(frame.hands().count() > 0) handOne = frame.hands().get(0); 
  }
  lastHandOne = handOne;
  println("Color " + get(width/2, height/2))    ;
}

void keyPressed()
{
 dx = dy = dz = 0; 
 cdx = cdy = cdz = 0; 
 adx = ady = adz = 0; 
}
