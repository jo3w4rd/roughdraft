import com.leapmotion.leap.*;

int width = 800;
int height = 600;
int maxBrushSize = 120;
color canvasColor = 0xffffff;
float alphaVal = 10;

Controller leap = new Controller();

void setup()
{
   frameRate(120);
   size(width, height);
   background(canvasColor);
   stroke(0x00ffffff);
}


void draw(){
  Frame frame = leap.frame();
  Pointable pointer = frame.pointables().frontmost();
  if( pointer.isValid() )
  {
    
    color frontColor = color( 255, 0, 0, alphaVal );
    color backColor  = color( 0, 0, 255, alphaVal );
    
    InteractionBox iBox = frame.interactionBox();
    Vector stabilizedTip = iBox.normalizePoint(pointer.stabilizedTipPosition());
    fingerPaint(stabilizedTip, backColor);
    Vector tip = iBox.normalizePoint(pointer.tipPosition());
    fingerPaint(tip, frontColor);
  }
}

void fingerPaint(Vector tip, color paintColor)
{
   fill(paintColor);
    float x = tip.getX() * width;
    float y = height - tip.getY() * height;
    float brushSize = maxBrushSize - maxBrushSize * tip.getZ();
    ellipse( x, y, brushSize, brushSize);   
}

void keyPressed()
{
   background(canvasColor);
}


