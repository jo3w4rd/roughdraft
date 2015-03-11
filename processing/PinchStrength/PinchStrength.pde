import com.leapmotion.leap.*;

Controller controller = new Controller();
float minBarSize = 10;
String[] typeMap = {"thumb", "index", "middle", "ring", "pinky", "no"};

void setup(){
  size( 600,300); 
  textAlign(LEFT);
}

void draw(){
  background(255);
  Frame frame = controller.frame();
  
  Hand hand = frame.hands().frontmost();
  float grip = hand.pinchStrength();
  fill(colorFromGrip(grip));
  rect(0, (height *  grip - minBarSize)/2, width/2, height * (1 - grip) + minBarSize); 
  fill(0); 
  text("Pinch: " + grip, 10, 20);
  Finger pincher = pinchingFinger(hand);
  int fingerType = 5;
  if(pincher.isValid() && grip > 0) fingerType = pincher.type().ordinal(); 
  text("Thumb to " + typeMap[fingerType] + " finger", 10, 40);
  
  grip = hand.grabStrength();
  fill(colorFromGrip(grip));
  rect(width/2, (height *  grip - minBarSize)/2, width/2, height * (1 -grip) + minBarSize);  
  fill(0); 
  text("Grab: " + grip, 10 + width/2, 20);
}

color colorFromGrip(float grip){
  if(grip == 1) { //Closed is red
    return color(255,0,0);
  } else if (grip == 0) { //open is blue
     return color(0,0,255); 
  } else { //Blend based on grip factor in between
     return color( 255 * grip, 0, 255 * (1 - grip)); 
  }  
}

Finger pinchingFinger(Hand hand){
  Finger thumb = hand.fingers().fingerType(Finger.Type.TYPE_THUMB).get(0);
  Finger pincher = Finger.invalid();
  float closest = 500;
  for(int f = 1; f < 5; f++)
  {
     Finger current = hand.fingers().get(f);
     float distance = thumb.tipPosition().distanceTo(current.tipPosition());
     if(distance < closest)
     {
        closest = distance;
        pincher = current; 
     }
  } 
  return pincher;
}


void stop(){
  println("quitting");
  super.stop(); 
}
