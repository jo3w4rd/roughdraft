import com.leapmotion.leap.*;

Controller controller = new Controller();

void draw(){
   Frame frame = controller.frame();
   Hand hand = frame.hands().frontmost(); // get a hand to use
   if(hand.isValid()){
        background(0);
        float distance = hand.palmPosition().magnitude();
        text(distance, 20, 20);
   } 
}
