import com.leapmotion.leap.*;

int width = 800;
int height = 600;
color canvasColor = 0xffffff;
float turns = radians(360);
float xRotation = 0;
float yRotation = 0;
Controller leap = new Controller();

void setup()
{
   frameRate(120);
   size(width, height, P3D);
   background(canvasColor);
}

void draw(){
  Frame frame = leap.frame();
  Hand hand = frame.hands().frontmost();
  if( hand.isValid() )
  {
    InteractionBox box = frame.interactionBox();
    Vector controlPosition = hand.palmPosition();
    Vector normalizedPosition = box.normalizePoint(controlPosition, false);
    yRotation = turns * (1 - normalizedPosition.getY());
    xRotation = turns * normalizedPosition.getX();
  }
  background(canvasColor);
  translate(width/2, height/2);
  rotateXYZ(yRotation, 0.0, xRotation);
  box(200);

}

void rotateXYZ(float xx, float yy, float zz) {
  float cx, cy, cz, sx, sy, sz;
  
  cx = cos(xx);
  cy = cos(yy);
  cz = cos(zz);
  sx = sin(xx);
  sy = sin(yy);
  sz = sin(zz);
  
  applyMatrix(cy*cz, (cz*sx*sy)-(cx*sz), (cx*cz*sy)+(sx*sz), 0.0,
  cy*sz, (cx*cz)+(sx*sy*sz), (-cz*sx)+(cx*sy*sz), 0.0,
  -sy, cy*sx, cx*cy, 0.0,
  0.0, 0.0, 0.0, 1.0);
}
