import com.leapmotion.leap.*;

Controller leap = new Controller();
int width = 650;
int height = 650;
int baseSize = 100;
float scale = 1;
float zRotation = 0;
float x = (width-baseSize)/2;
float y = (height-baseSize)/2;

float cursorX = 0;
float cursorY = 0;

boolean selected = false;
boolean hovering = false;

Frame startFrame = Frame.invalid();

void setup(){
   size(650, 650); 
}

void draw(){
   processFrame(leap.frame());
   background(255,255,255);
   drawObject();
   drawCursor();
}

void drawObject(){
   if(selected){
      fill(255,0,0); 
   }
   if(hovering){
      fill(128,0,0); 
   }
   
   pushMatrix();
   rotate(zRotation);
   translate(x,y);
   rect(0, 0, baseSize * scale, baseSize * scale);
   popMatrix();
}

void drawCursor(){
   ellipse(cursorX, cursorY, 5, 5); 
}

void processFrame(Frame frame){
  updateCursor(frame);
  triggerWithTouchZone(frame);
}

float selectZ = 0;
void triggerWithTouchZone(Frame frame){
     //Look for at least two touching fingers
     int touchCount = 0;
     int hoverCount = 0;
     for(int p = 0; p < frame.pointables().count();p++){
        if(frame.pointables().get(p).touchZone() == Pointable.Zone.ZONE_TOUCHING){
           touchCount++;
        } 
        if(frame.pointables().get(p).touchZone() == Pointable.Zone.ZONE_HOVERING){
           hoverCount++;
        } 
     }
     if( touchCount > 0 ){
         selected = true;
         boolean doTranslation = true;
         boolean doScale = true;
         boolean doRotation = true;
         
         float transProbability = frame.translationProbability(leap.frame(1));
         if(transProbability > .35){
           // doScale = false;
            //doRotation = false; 
         }
         float scaleProbability = frame.scaleProbability(leap.frame(1));
         if(scaleProbability > .35){
             //doTranslation = false;
             //doRotation = false;
         }
         float rotationProbability = frame.rotationProbability(leap.frame(1));
         if(rotationProbability > .35){
             //doTranslation = false;
             //doScale = false;
         }

        if(doTranslation){
             Vector translation = frame.translation(leap.frame(1));
             x += translation.getX();
             y -= translation.getY();
         }
         if(doScale){
           scale -= 1-frame.scaleFactor(leap.frame(1));
         }
         if(doRotation){
            zRotation = frame.rotationAngle(leap.frame(1),Vector.zAxis());
         }
         hovering = false;
     } else if( hoverCount > 0 ){
        selected = false;
        hovering = true; 
     } else {
        selected = false;
        hovering = false; 
     }
}

int trackedHandID = 0;
void updateCursor(Frame frame){
   if(!frame.hands().isEmpty()){
      Hand hand = frame.hand(trackedHandID);
      if(!hand.isValid()){
         hand = frame.hands().get(0);
         trackedHandID = hand.id(); 
      }
      
      Vector normalizedPosition = frame.interactionBox().normalizePoint(hand.palmPosition());
      cursorX = normalizedPosition.getX() * width;
      cursorY = height - normalizedPosition.getY() * height;
   } 
}
