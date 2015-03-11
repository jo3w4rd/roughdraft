import com.leapmotion.leap.*;

int width = 640;
int height = 480;

Controller leap = new Controller();

void setup()
{
  frameRate(60);
   size(width, height);
}

float currentFrameScale = 1;
float currentLeftHandScale = 1;
float currentRightHandScale = 1;

void draw(){
  Frame frame = leap.frame();
  background(102);
  //translate(width/2, height/2);
  pushMatrix();
    translate(width/2, height/2);

   currentFrameScale += frame.scaleFactor(leap.frame(1)) - 1;   
   currentFrameScale = (currentFrameScale < .25) ? .25 : currentFrameScale; 
   currentFrameScale = (currentFrameScale > 2) ? 2 : currentFrameScale; 
   scale(currentFrameScale);
   rect(-width/4, -height/4, width/2, height/2);
   popMatrix();
   
   currentLeftHandScale += frame.hands().get(0).scaleFactor(leap.frame(1)) - 1;
   currentLeftHandScale = (currentLeftHandScale < .05) ? .05 : currentLeftHandScale; 
   currentLeftHandScale = (currentLeftHandScale > 4) ? 4 : currentLeftHandScale; 

   pushMatrix();
   translate(width/4,  height/4);
   scale(currentLeftHandScale);
   rect(-20, -20, 40, 40);
   popMatrix();

   currentRightHandScale += frame.hands().get(1).scaleFactor(leap.frame(1)) - 1;
   currentRightHandScale = (currentRightHandScale < .05) ? .05 : currentRightHandScale; 
   currentRightHandScale = (currentRightHandScale > 4) ? 4 : currentRightHandScale; 
   
   pushMatrix();
   translate(3*width/4, 3*height/4);
    scale(currentRightHandScale);
   rect(-20, -20, 40, 40);
   popMatrix();
  
}
