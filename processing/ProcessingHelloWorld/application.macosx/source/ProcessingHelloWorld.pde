import com.leapmotion.leap.*;

Controller controller = new Controller();

void setup(){
  size( 600, 400 );
}

void draw(){
  background(0);
  Frame frame = controller.frame();
  text(frame.hands().count() + " Hands", 100, 100);
  text(frame.fingers().count() + " Fingers", 100, 150);
}
