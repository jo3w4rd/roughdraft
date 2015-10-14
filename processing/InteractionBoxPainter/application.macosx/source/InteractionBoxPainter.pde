import com.leapmotion.leap.*;

int width = 800;
int height = 600;
int maxBrushSize = 120;
color canvasColor = 0xffffff;
float alphaVal = 10;
PGraphics canvas;
Boolean isDrawing = true;

Controller leap = new Controller();

void setup()
{
   frameRate(120);
   size(width, height);
   canvas = createGraphics(width, height);
   background(canvasColor);
   stroke(0x00ffffff);
}


void draw(){
  Frame frame = leap.frame();
  Pointable pointer = frame.pointables().frontmost();
  if( pointer.isValid() )
  {
    background(canvasColor);

    color frontColor = color( 255, 0, 0, alphaVal );
    color backColor  = color( 0, 0, 255, alphaVal );
    
    InteractionBox iBox = frame.interactionBox();
    Vector tip = iBox.normalizePoint(pointer.tipPosition());
    float x = tip.getX() * width;
    float y = height - tip.getY() * height;
    float brushSize = maxBrushSize - maxBrushSize * tip.getZ();
    float xBrushSize = maxBrushSize - (float)Math.sin(pointer.direction().yaw()) * brushSize;
    float yBrushSize = maxBrushSize - (float)Math.sin(pointer.direction().pitch()) * brushSize;

    if(isDrawing){
      canvas.beginDraw();
      canvas.fill(frontColor);
      canvas.noStroke();
      canvas.ellipse( x, y, xBrushSize, yBrushSize);
      canvas.endDraw();
    }
    image(canvas, 0, 0); //Draw canvas to screen
    
    noFill();
    stroke(0, 0, 0);
    ellipse( x, y, xBrushSize, yBrushSize); // draw cursor
  }
}

void keyPressed()
{
   isDrawing = !isDrawing;
}


