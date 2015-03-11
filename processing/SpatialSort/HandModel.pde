import com.leapmotion.leap.*;

class HandModel
{
    int leapID = 0;
    FingerModel[] fingers;

   HandModel(int leapID, int fingerCount)
  {
     this.leapID = leapID;
     this.fingers = new FingerModel[fingerCount];
  }

  FingerModel[] fingersLeftToRight()
  {
      FingerModel[] xSort = new FingerModel[fingers.length];
      for( int f = 0; f < fingers.length; f++ )
      {
        int place = 0;
        for (int g = 0; g < fingers.length; g++ )
        {
          if( f != g )
          {
            if( fingers[f].tx >= fingers[g].tx ) place++;
          } 
        }
        xSort[place] = fingers[f];
      }
      return xSort;
  }
  
    FingerModel[] fingersBackToFront()
  {
      FingerModel[] zSort = new FingerModel[fingers.length];
      for( int f = 0; f < fingers.length; f++ )
      {
        int place = 0;
        for (int g = 0; g < fingers.length; g++ )
        {
          if( f != g )
          {
            if( fingers[f].tz >= fingers[g].tz ) place++;
          } 
        }
        zSort[place] = fingers[f];
      }
      return zSort;
  }
  
    FingerModel[] fingersBottomToTop()
  {
      FingerModel[] ySort = new FingerModel[fingers.length];
      for( int f = 0; f < fingers.length; f++ )
      {
        int place = 0;
        for (int g = 0; g < fingers.length; g++ )
        {
          if( f != g )
          {
            if( fingers[f].ty >= fingers[g].ty ) place++;
          } 
        }
        ySort[place] = fingers[f];
      }
      return ySort;
  }
  
  void classifyFingers(Controller leap)
  {
     if( fingers.length == 0 ) return;
     
     Hand leapHand = leap.frame().hand( this.leapID );
     FingerModel[] orderedFingers = this.fingersLeftToRight();
     if( orderedFingers.length == 5 ) //If there are 5 fingers, just use x axis sort order
     {
        int d = 1;
        for ( FingerModel finger : orderedFingers )
       {
          finger.digitID = d++;
       } 
     }
     else // Guess fingers by offset from 0 based on width
     {
       float fingerWidth = 0;
       int count = 0;
       for ( FingerModel finger : orderedFingers )
       {
          fingerWidth += finger.fingerWidth;
          count++;
       }
       fingerWidth = fingerWidth/count;
       
       for ( FingerModel finger : orderedFingers )
       {
          if( finger.tx < -3 * fingerWidth ) finger.digitID = 1;
          else if( finger.bx < 0 ) finger.digitID = 2;
          else if( finger.bx < 1 * fingerWidth ) finger.digitID = 3;
          else if( finger.bx > 2.2 * fingerWidth ) finger.digitID = 5;
          else if( finger.bx > .9 * fingerWidth ) finger.digitID = 4;

       } 
       java.util.Arrays.sort(fingers); // However, we can have duplicates so sort and adjust
       int maxID = 0;
        for ( int f = 0; f < fingers.length - 1; f++ )
       {
         if( fingers[f].digitID == fingers[f+1].digitID )
         {
            if(fingers[f].digitID == 2)
            {
               if(fingers[f].bz > fingers[f+1].bz) 
               {
                 fingers[f].digitID = 1;
               }
            }
            else
            {
              fingers[f+1].digitID++;
              f = 0;
            }
         }
       }
            
     }
  }
}
