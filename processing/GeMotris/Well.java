
class Well {   
   public int width = 10;
   public int height = 24;
   public int hidden = 4;
   public int[][] well;
   
   public Well(){
     well = new int[width][height + hidden];
   }
   
   public boolean collisionCheck(int w, int h){
     if( well[w][h] > 0 ) {return true; }
     return false;
   }
   
   public void addBlockToWell(Block block, int orientation, int w, int h){
       for( int x = 0; x < block.width; x++ ) {
          for( int y = 0; y < block.height; y++ ) {
            if( block.checkCollision(orientation, x, y)){
               well[w+x][h+y] = 1;
            }
          } 
       }
       //clearFilledRows();
   }
   
   int clearFilledRows(){
     int cleared = 0;
     boolean filled = true;
     for( int h = this.height + hidden - 1; h > 0; h--){
       for(int w = 0; w < this.width; w++){
         if( well[w][h] == 0 ) {
           filled = false;
           break;
         }
       }
       if(filled) {
          //System.out.println("Row complete: " + h);
          cleared++;
          clearRow(h);
          shiftDown(h,1);
          h++;
       }
       filled = true; 
     }
     return cleared;
   }
   
   void clearRow(int row){
       for( int i = 0; i < this.width; i++  ){
           well[i][row] = 0;
       }
   }
   
   void shiftDown(int startRow, int count) {
     for( int r = startRow; r > 0; r--) {
        for( int w = 0; w < this.width; w ++ ){
          if(r - count >= 0 ){
             well[w][r] = well[w][r-count];
          } else {
            well[w][r] = 0;
          }
        } 
     }
     
   }
 }
