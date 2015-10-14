import com.leapmotion.leap.*;

Controller controller = new Controller();

void setup(){
  size( 200, 200 );
}

void draw(){
  background(0);
  Frame frame = controller.frame();
  text(frame.hands().count() + " Hands", 50, 50);
  text(frame.fingers().count() + " Fingers", 50, 100);
}
