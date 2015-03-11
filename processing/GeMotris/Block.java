class Block {   
   public int width = 4;
   public int height = 4;
   public int[][][] patterns;
   public int currentPattern = 0;
   
   
   public int nextLeft() {
      if( currentPattern == 0 ) { return 3; }
      return currentPattern - 1; 
   }

   public int nextRight(){
      if( currentPattern == 3 ) { return 0; }
      return currentPattern + 1; 
   }
   
   public void setPattern( int newPattern) {
      currentPattern = newPattern; 
   }
   
   public boolean checkCollision( int orientation, int w, int h ) {
      if(patterns[orientation][w][h] > 0 ) { return true; }
     return false; 
   }
    
   public static Block createIBlock() {
       Block iBlock = new Block();
       int[][][] pattern  = { { { 0, 1, 0, 0 }, //Upright
                             { 0, 1, 0, 0 }, 
                             { 0, 1, 0, 0 },
                             { 0, 1, 0, 0 }
                           },
                           { { 0, 0, 0, 0 }, //right
                             { 1, 1, 1, 1 }, 
                             { 0, 0, 0, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 0, 0, 1, 0 }, //downward
                             { 0, 0, 1, 0 }, 
                             { 0, 0, 1, 0 },
                             { 0, 0, 1, 0 }
                           },
                           { { 0, 0, 0, 0 }, //left
                             { 0, 0, 0, 0 }, 
                             { 1, 1, 1, 1 },
                             { 0, 0, 0, 0 }
                           }
                         };
                         
       iBlock.patterns = pattern;
                         
       return iBlock;
   }
      public static Block createJBlock() {
       Block iBlock = new Block();
       int[][][] pattern = {{{ 0, 1, 0, 0 }, //Upright
                             { 0, 1, 0, 0 }, 
                             { 1, 1, 0, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 1, 0, 0, 0 }, //right
                             { 1, 1, 1, 0 }, 
                             { 0, 0, 0, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 0, 1, 1, 0 }, //downward
                             { 0, 1, 0, 0 }, 
                             { 0, 1, 0, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 0, 0, 0, 0 }, //left
                             { 1, 1, 1, 0 }, 
                             { 0, 0, 1, 0 },
                             { 0, 0, 0, 0 }
                           }
                         };
                         
       iBlock.patterns = pattern;
                         
       return iBlock;
   }
   public static Block createLBlock() {
       Block iBlock = new Block();
       int[][][] pattern = {{{ 0, 1, 0, 0 }, //Upright
                             { 0, 1, 0, 0 }, 
                             { 0, 1, 1, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 0, 0, 0, 0 }, //right
                             { 1, 1, 1, 0 }, 
                             { 1, 0, 0, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 1, 1, 0, 0 }, //downward
                             { 0, 1, 0, 0 }, 
                             { 0, 1, 0, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 0, 0, 1, 0 }, //left
                             { 1, 1, 1, 0 }, 
                             { 0, 0, 0, 0 },
                             { 0, 0, 0, 0 }
                           }
                         };
                         
       iBlock.patterns = pattern;
                         
       return iBlock;
   }
   public static Block createOBlock() {
       Block iBlock = new Block();
       int[][][] pattern = {{{ 1, 1, 0, 0 }, //Upright
                             { 1, 1, 0, 0 }, 
                             { 0, 0, 0, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 1, 1, 0, 0 }, //right
                             { 1, 1, 0, 0 }, 
                             { 0, 0, 0, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 1, 1, 0, 0 }, //downward
                             { 1, 1, 0, 0 }, 
                             { 0, 0, 0, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 1, 1, 0, 0 }, //left
                             { 1, 1, 0, 0 }, 
                             { 0, 0, 0, 0 },
                             { 0, 0, 0, 0 }
                           }
                         };
                         
       iBlock.patterns = pattern;
                         
       return iBlock;
   }
   public static Block createSBlock() {
       Block iBlock = new Block();
       int[][][] pattern = {{{ 0, 0, 0, 0 }, //Upright
                             { 0, 1, 1, 0 }, 
                             { 1, 1, 0, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 1, 0, 0, 0 }, //right
                             { 1, 1, 0, 0 }, 
                             { 0, 1, 0, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 0, 1, 1, 0 }, //downward
                             { 1, 1, 0, 0 }, 
                             { 0, 0, 0, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 0, 1, 0, 0 }, //left
                             { 0, 1, 1, 0 }, 
                             { 0, 0, 1, 0 },
                             { 0, 0, 0, 0 }
                           }
                         };
                         
       iBlock.patterns = pattern;
                         
       return iBlock;
   }
   public static Block createTBlock() {
       Block iBlock = new Block();
       int[][][] pattern = {{{ 0, 0, 0, 0 }, //Upright
                             { 1, 1, 1, 0 }, 
                             { 0, 1, 0, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 0, 1, 0, 0 }, //right
                             { 1, 1, 0, 0 }, 
                             { 0, 1, 0, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 0, 1, 0, 0 }, //downward
                             { 1, 1, 1, 0 }, 
                             { 0, 0, 0, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 0, 1, 0, 0 }, //left
                             { 0, 1, 1, 0 }, 
                             { 0, 1, 0, 0 },
                             { 0, 0, 0, 0 }
                           }
                         };
                         
       iBlock.patterns = pattern;
                         
       return iBlock;
   }
   public static Block createZBlock() {
       Block iBlock = new Block();
       int[][][] pattern = {{{ 0, 0, 0, 0 }, //Upright
                             { 1, 1, 0, 0 }, 
                             { 0, 1, 1, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 0, 1, 0, 0 }, //right
                             { 1, 1, 0, 0 }, 
                             { 1, 0, 0, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 1, 1, 0, 0 }, //downward
                             { 0, 1, 1, 0 }, 
                             { 0, 0, 0, 0 },
                             { 0, 0, 0, 0 }
                           },
                           { { 0, 0, 1, 0 }, //left
                             { 0, 1, 1, 0 }, 
                             { 0, 1, 0, 0 },
                             { 0, 0, 0, 0 }
                           }
                         };
                         
       iBlock.patterns = pattern;
                         
       return iBlock;
   }

}
