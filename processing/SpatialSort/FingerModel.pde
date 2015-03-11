import com.leapmotion.leap.*;

class FingerModel implements Comparable<FingerModel>
{
  int leapID = 0;
  int digitID = 0;
  float tx = 0;
  float ty = 0;
  float tz = 0;
  float bx = 0;
  float by = 0;
  float bz = 0;  
  float fingerWidth;
  float length;
  
  FingerModel( int leapID )
  {
     this.leapID = leapID; 
  }
  
  public int compareTo(FingerModel other)
  {
    if( this.digitID < other.digitID ) return -1;
    if( this.digitID > other.digitID ) return 1;
    if( this.bx < other.bx ) return -1;
    if( this.bx > other.bx ) return 1;
    return 0;
  }
}
