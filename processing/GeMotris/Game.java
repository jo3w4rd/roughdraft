import java.util.ArrayDeque;


class Game {   
  public Well well;
  public int score;
  public int currentX;
  public int currentY;
  public Block block;
  public ArrayDeque<Block> blockBag;
  public ArrayDeque<Integer> commandQueue;
  public int dropTime = 1000;
  public int step = 0;
  public boolean paused = false;
  
  public Game(){
     this.reset();
     this.block = getNextBlock(); 
  }
  
  public void update(int dt){
      step += dt;
      if ( step > dropTime && !paused) { 
            int patternToCheck = this.block.currentPattern;
           if( checkMove(patternToCheck, this.currentX, this.currentY + 1) ) {
              this.currentY += 1; 
           }  else {
              this.finishBlockAndStartNew();
          }     
           
           step = 0;
      }
      
      //System.out.println("X: " + this.currentX + ", Y: " + this.currentY + ", P: " + this.block.currentPattern + ", dt: " + dt + ", S: " + this.step); 
  }
  
  public void handleInput() {
    if(!commandQueue.isEmpty()){
      int command = commandQueue.pop();
      int patternToCheck;
      //System.out.println("Command: " + command); 
      switch( command ) {
         case 0: //move left
               patternToCheck = this.block.currentPattern;
               if( checkMove(patternToCheck, this.currentX - 1, this.currentY) ) {
                  this.currentX -= 1; 
               }
               break;
               
         case 1: //move right
               patternToCheck = this.block.currentPattern;
               if( checkMove(patternToCheck, this.currentX + 1, this.currentY) ) {
                  this.currentX += 1; 
               }       
               break;
               
         case 2: // rotate left
               patternToCheck = this.block.nextLeft();
               if( checkMove(patternToCheck, this.currentX, this.currentY) ) {
                  this.block.currentPattern = patternToCheck; 
               }              
               break;
               
         case 3: //rotate right
               patternToCheck = this.block.nextRight();
               if( checkMove(patternToCheck, this.currentX, this.currentY) ) {
                  this.block.currentPattern = patternToCheck; 
               }                     
               break;
               
         case 4: //drop
               patternToCheck = this.block.currentPattern;
               if( checkMove(patternToCheck, this.currentX, this.currentY + 1) ) {
                  this.currentY += 1; 
               }  else {
                 finishBlockAndStartNew();
               }     
               break;
               
         case 5: //pause
         
               break;
               
         case 6: //quit
               gameOver();
               break;
               
         default:      
      } 
    }
  }

  public void rotateTo(int orientation){
     if( checkMove(orientation, this.currentX, this.currentY) ) {
        this.block.currentPattern = orientation; 
     }                    
  }
  
  private void finishBlockAndStartNew(){
                  if( this.currentY == 0 ) { gameOver(); }
                  else {
                      this.well.addBlockToWell( this.block, this.block.currentPattern, this.currentX, this.currentY );
                      this.currentX = 3;
                      this.currentY = 0;
                      this.block = getNextBlock();
                      this.step = 0;  
                  }
  }  
  
  private boolean checkMove(int orientation, int wellX, int wellY) {
      //true if move allowed
       for( int x = 0; x < this.block.width; x++ ) {
          for( int y = 0; y < this.block.height; y++ ) {
             boolean blockCell = this.block.checkCollision(orientation, x, y);
             if( blockCell && ( wellX + x < 0 || wellX + x > this.well.width - 1 )) { return false; }
             if(blockCell && ( wellY + y > this.well.height - 1)) { return false; }
             if(blockCell &&  this.well.collisionCheck(wellX+x,wellY+y) ) { return false; }
          } 
       }
       return true;
   }
  
  public void gameOver(){
    System.out.println("Game over.");
    reset();
    pause();
  }
  
  public Block getNextBlock(){
    if(blockBag.isEmpty()) { fillBlockBag(); }
    return this.blockBag.pop();  
  }
  
  private void fillBlockBag(){
      this.blockBag.push(Block.createOBlock());
      this.blockBag.push(Block.createIBlock());
      this.blockBag.push(Block.createJBlock());
      this.blockBag.push(Block.createLBlock());
      this.blockBag.push(Block.createSBlock());
      this.blockBag.push(Block.createTBlock());
      this.blockBag.push(Block.createZBlock());
  }
  
  public void pause() {
    
  }
  
  public void reset() {
    this.well = new Well();
    this.score = 0;
    this.step = 0;
    this.dropTime = 1000;
    this.currentX = 3;
    this.currentY = 4;
    this.blockBag = new ArrayDeque<Block>();
    this.commandQueue = new ArrayDeque<Integer>();  
  }
  
  public void scoreRows(int cleared){
     score += cleared * cleared; 
  }
}
