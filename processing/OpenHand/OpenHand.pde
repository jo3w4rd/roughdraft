import com.leapmotion.leap.*;

Controller controller = new Controller();

void setup(){
  size(400,400);
  fill(color(0,0,255));
}

void draw(){
  Frame frame = controller.frame();
  Hand hand = frame.hands().frontmost();
  println("Hand: " + hand);
  if(hand.isValid()){
    int count = hand.fingers().extended().count();
    if( count < 5 )
    {
      fill(color(255,0,0));
    } else {
      fill(color(0,255,0));    
    }
    rect(0,0,400,400);
  }
}
