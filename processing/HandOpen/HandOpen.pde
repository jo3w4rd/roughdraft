import com.leapmotion.leap.*;

Controller controller = new Controller();
float minBarSize = 10;

void setup(){
  size( 600,300); 
}

void draw(){
  background(255);
  Frame frame = controller.frame();
  
  Hand leftHand = pickHand(frame.hands(), "left");
  float grip = leftHand.grabStrength();
  fill(colorFromGrip(grip));
  rect(0, (height *  grip - minBarSize)/2, width/2, height * (1 - grip) + minBarSize); 
  fill(0); 
  text(grip, width/4, 20);
  drawSpringV(width/4, (height *  grip - minBarSize)/2, height * (1 -grip) + minBarSize, 8, 50);
  
  Hand rightHand = pickHand(frame.hands(), "right");
  grip = rightHand.grabStrength();
  fill(colorFromGrip(grip));
  rect(width/2, (height *  grip - minBarSize)/2, width/2, height * (1 -grip) + minBarSize);  
  fill(0); 
  text(grip, 3 * width/4, 20);
  
  drawSpringV(3 * width/4, (height *  grip - minBarSize)/2, height * (1 -grip) + minBarSize, 8, 50);
}

color colorFromGrip(float grip){
  //Blend based on grip factor in between
  return color( 255 * grip, 0, 255 * (1 - grip)); 
}

//Finds first hand in list of desired type
Hand pickHand(HandList hands, String whichHand){
   for( int h = 0; h < hands.count(); h++)
    {
      Hand hand = hands.get(h);
      if(hand.isLeft() && whichHand == "left") return hand;
      if(hand.isRight() && whichHand == "right") return hand;
    } 
    return Hand.invalid();
}

void drawSpringV(float startX, float startY, float springHeight, float frequency, float amplitude)
{
  //float length = (float)Math.sqrt(Math.pow(endX - startX,2) + Math.pow(endY - startY,2));
  float cursorX = startX;
  float cursorY = startY;
  float nextX = 0;
  float nextY = 0;
  if(springHeight > 0)
  {
    for (int t = 0; t < springHeight; t++) {
      nextX = startX + amplitude * (float)Math.sin(2 * Math.PI * frequency * t/springHeight);
      nextY = startY + t;
      line(cursorX, cursorY, nextX, nextY);
      cursorX = nextX;
      cursorY = nextY;
    }
  }
}

