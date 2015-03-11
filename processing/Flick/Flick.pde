import com.leapmotion.leap.*;

int width = 800;
int height = 600;
Controller leap = new Controller();
int time = 0;
int state = 0;

void setup(){
    frameRate(60);
    size(width, height, P3D);
    background(255,255,255);

}

void draw(){
  Frame frame = leap.frame();
  int dt = frame.timestamp() - time;
  
  if( state == 0 && !frame.pointables.isEmpty() )
  {
    //Ready
    
  }
  if( state == 1 && !frame.pointables.isEmpty() )
  {
    //Draw 
  }
  
  if( state == 1 && dt > 1000 )
  {
    //Finish
    
  }
  
  if(state == 2)
  {
    //Reset to start state
    
  }
}
