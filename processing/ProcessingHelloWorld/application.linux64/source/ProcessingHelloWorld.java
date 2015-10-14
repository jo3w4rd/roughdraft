import processing.core.*; 
import processing.data.*; 
import processing.event.*; 
import processing.opengl.*; 

import com.leapmotion.leap.*; 

import java.util.HashMap; 
import java.util.ArrayList; 
import java.io.File; 
import java.io.BufferedReader; 
import java.io.PrintWriter; 
import java.io.InputStream; 
import java.io.OutputStream; 
import java.io.IOException; 

public class ProcessingHelloWorld extends PApplet {



Controller controller = new Controller();

public void setup(){
  size( 600, 400 );
}

public void draw(){
  background(0);
  Frame frame = controller.frame();
  text(frame.hands().count() + " Hands", 100, 100);
  text(frame.fingers().count() + " Fingers", 100, 150);
}
  static public void main(String[] passedArgs) {
    String[] appletArgs = new String[] { "ProcessingHelloWorld" };
    if (passedArgs != null) {
      PApplet.main(concat(appletArgs, passedArgs));
    } else {
      PApplet.main(appletArgs);
    }
  }
}
