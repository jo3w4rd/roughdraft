import com.leapmotion.leap.*;

Controller leap;

void setup()
{
  leap = new Controller();
  println("At setup, isConnected = " + leap.isConnected());  
}

void draw()
{
  println("isConnected = " + leap.isConnected());    
}


