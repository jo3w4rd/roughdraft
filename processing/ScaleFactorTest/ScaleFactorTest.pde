import com.leapmotion.leap.*;

Controller controller = new Controller();
int trackedID = 0;
Frame startingPoint = Frame.invalid();

void setup(){
  
}

void draw(){
  Frame frame = controller.frame();
  Hand hand;
  if(trackedID == 0){
    if(frame.hands().count() > 0) {
        hand = frame.hands().get(0);
      if(hand.pointables().count() == 5){
         trackedID = hand.id(); 
         startingPoint = frame;
      }
    }
  }
  else
  {
     hand = frame.hand(trackedID);
    if(!hand.equals(Hand.invalid())){
       float scale = hand.scaleFactor(startingPoint);
       println("Scale = " + scale);
    } 
    else
    {
        trackedID = 0;
    }
  }

}
