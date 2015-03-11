import com.leapmotion.leap.*;

int width = 800;
int height = 600;
int maxBrushSize = 10;
color canvasColor = 0xffffff;
float alphaVal = 100;
PGraphics canvas;
Boolean isDrawing = true;
float maxVelocity = 10;
float cx = width/2;
float cy = height/2;
float scale = 1;

Controller leap = new Controller();

void setup()
{
   colorMode(HSB, 10, 10, 10, 100);
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
    float velocity = pointer.tipVelocity().magnitude();
    color frontColor = color( (1 - velocity/maxVelocity) * 100, 100, velocity/maxVelocity * 50, alphaVal );
    color backColor  = color( 0, 0, 255, alphaVal );
    
    InteractionBox iBox = frame.interactionBox();
    Vector tip = iBox.normalizePoint(pointer.tipPosition());
    
    if( pointer.tipPosition().getY() < 8000){
      cx = cx + pointer.tipVelocity().getX()/maxVelocity * Math.abs(tip.getX()/scale);
      cy = cy - pointer.tipVelocity().getY()/maxVelocity * tip.getY()/scale;
    }
    System.out.println("(" + cx + ", " + cy +")");
    if(isDrawing){
      canvas.beginDraw();
      canvas.fill(frontColor);
      canvas.noStroke();
      canvas.ellipse( cx, cy, maxBrushSize, maxBrushSize);
      canvas.endDraw();
    }
    image(canvas, 0, 0); //Draw canvas to screen
    
    noFill();
    stroke(0, 0, 0);
    ellipse( cx, cy, maxBrushSize, maxBrushSize); // draw cursor
  }
}

void keyPressed()
{
   isDrawing = !isDrawing;
}


