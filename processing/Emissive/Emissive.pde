import com.leapmotion.leap.*;

Controller controller = new Controller();

void setup() {
  size(displayWidth, displayHeight, P3D);
  noStroke();
  colorMode(RGB, 1);
  fill(0.4);
}

void draw() {
  
  Frame frame = controller.frame();
  Hand hand = frame.hands().frontmost(); // get a hand to use
  
  background(0);
  lightSpecular(1, 1, 1);
  directionalLight(0.8, 0.8, 0.8, 0, 0, -1);
    
  for (int x = 0; x <= width; x += 30) {
   for (int y = 0; y <= height; y += 30) {
    pushMatrix();
    translate(x, y);
    sphere(10);
    popMatrix();
  
    if(hand.isValid()){
      float distance = hand.palmPosition().magnitude();
      println(distance);
        float s = distance/500;
        //specular(s, s, s);
        emissive(s ,s, s);
    } else { 
      //specular(0, 0, 0);
        emissive(0, 0, 0);
   }
  }
 }
}

