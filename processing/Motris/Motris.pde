import com.leapmotion.leap.*;

Controller leap = new Controller();
Frame lastInputFrame = Frame.invalid();
float cumulativeRotation = 0;
float cumulativeXTranslation = 0;
float cumulativeYTranslation = 0;

Game motrisGame = new Game();
int now = 0;
int last = 0;
int startX = 100;
int startY = 60;
int squareSize = 20;

void setup(){
  size( 400, 600 );
  now = millis();
  last = now; 
}

void draw(){
  processLeapInput();
  now = millis();
  motrisGame.handleInput();
  motrisGame.update(now-last);
  int clearedRows = motrisGame.well.clearFilledRows();
  motrisGame.scoreRows(clearedRows);
  //System.out.println("Score: " + motrisGame.score);
  last = now;
  
  background(255,255,255);
  for( int w = 0; w < motrisGame.well.width; w++) {
     for ( int h = motrisGame.well.hidden; h < motrisGame.well.height; h++ ) {
         boolean filled = motrisGame.well.collisionCheck(w, h);
         if(filled) {
            fill(0,0,0); 
         } else {
            noFill(); 
         }
         rect((w * squareSize) + startX, (h * squareSize) + startY, squareSize, squareSize);
     } 
  }
  
  for( int w = 0; w < motrisGame.block.width; w++) {
     for ( int h = 0; h < motrisGame.block.height; h++ ) {
         if( w + motrisGame.currentX < motrisGame.well.width 
             && w + motrisGame.currentX >= 0
             && h + motrisGame.currentY < motrisGame.well.height ) {
             boolean filled = motrisGame.block.checkCollision(motrisGame.block.currentPattern, w, h);
             if(filled) {
                fill(255,0,0); 
             } else {
                noFill(); 
             }
             rect(((w + motrisGame.currentX) * squareSize) + startX, ((h + motrisGame.currentY) * squareSize) + startY, squareSize, squareSize);
         } 
     }
  }
  
  
}

void processLeapInput(){
  relativeMotions();
  //absoluteMotions();
}

Frame lastRotationframe = Frame.invalid();
Frame lastXTranslationFrame = Frame.invalid();
Frame lastYTranslationFrame = Frame.invalid();

void absoluteMotions(){
   if(lastRotationframe.isValid() && !leap.frame().hands().isEmpty()){
     
      //Rotation
      float rotation = leap.frame().rotationAngle(lastRotationframe, Vector.zAxis());
      System.out.println("R: " + rotation);
      if( (rotation > -.3) && (rotation < .3)){
        motrisGame.rotateTo(0);
      } else if ((rotation > .3) && (rotation < .6)) {
        motrisGame.rotateTo(1);         
      } else if ((rotation < -.3) && (rotation > -.6)) {
        motrisGame.rotateTo(3);                 
      } else {
        motrisGame.rotateTo(2);         
      }

   } else {
      lastRotationframe = leap.frame(); 
   }
   
   //Movement along X axis
   if(lastXTranslationFrame.isValid() && !leap.frame().hands().isEmpty()){
   
      float offsetX = 25;
      Vector translation = leap.frame().translation(lastXTranslationFrame);
      System.out.println("TX: " + translation.getX());

      if(translation.getX() > offsetX){
         motrisGame.commandQueue.push(1);
        lastXTranslationFrame = leap.frame(); 
      } else if(translation.getX() < -offsetX) {
          motrisGame.commandQueue.push(0);
          lastXTranslationFrame = leap.frame();
      } 
   } else {
      lastXTranslationFrame = leap.frame(); 
   }      

   //Movement along Y axis
   if(lastYTranslationFrame.isValid() && !leap.frame().hands().isEmpty()){
   
      float offsetY = 7;
      Vector translation = leap.frame().translation(lastYTranslationFrame);
      System.out.println("TY: " + translation.getY());

      if(translation.getY() < -offsetY){
        motrisGame.commandQueue.push(4);
        lastYTranslationFrame = leap.frame(); 

      }
   } else {
      lastYTranslationFrame = leap.frame(); 
   }      
   
}

void relativeMotions(){
  if(lastInputFrame.isValid()){
      cumulativeYTranslation += leap.frame().translation(lastInputFrame).getY();
      if( cumulativeYTranslation > 30 ) { //up overrides other motions
          cumulativeYTranslation = 0;
          cumulativeXTranslation = 0;
          cumulativeRotation = 0;
          lastInputFrame = leap.frame();
      } else {
          if( cumulativeYTranslation < -40 ) {
             motrisGame.commandQueue.push(4);
             cumulativeYTranslation = 0;
             lastInputFrame = leap.frame(); 
          } else {
          
              float rotation = leap.frame().rotationAngle(lastInputFrame, Vector.zAxis());
              if( .1 < Math.abs(rotation) ) { 
                cumulativeRotation += rotation;
              } 
              System.out.println("Rotation: " + cumulativeRotation);
              if( cumulativeRotation > Math.PI/2 ) {
                 motrisGame.commandQueue.push(2);
                 cumulativeRotation = 0;
                lastInputFrame = leap.frame(); 
              } else if( cumulativeRotation < -Math.PI/2 ) {
                 motrisGame.commandQueue.push(3);
                 cumulativeRotation = 0;
                lastInputFrame = leap.frame();
              }
              cumulativeXTranslation += leap.frame().translation(lastInputFrame).getX();
              if( cumulativeXTranslation > 80 ) {
                 motrisGame.commandQueue.push(1);
                cumulativeXTranslation = 0;
               lastInputFrame = leap.frame(); 
              }
              if( cumulativeXTranslation < -80 ) {
                 motrisGame.commandQueue.push(0);
                cumulativeXTranslation = 0;
               lastInputFrame = leap.frame(); 
              }
          }
      }

  } else {
       lastInputFrame = leap.frame();    
  }
  
}

void keyPressed() {
    switch(keyCode){
       case UP:
          motrisGame.commandQueue.push(2);
          break;
       case LEFT:
          motrisGame.commandQueue.push(0);
          break;
       case RIGHT:
          motrisGame.commandQueue.push(1);
          break;
       case DOWN:
          motrisGame.commandQueue.push(4);
          break;   
       default:     
    }
}



void printBlock(Block block) {
  for( int p = 0; p< 4; p++ ) {
     for( int w = 0; w<4; w++ ){
        for( int h = 0; h<4; h++ ) {
           System.out.print( block.checkCollision(p,w,h) + " , "); 
        }
        System.out.println();
     } 
     System.out.println();   
  }
}
