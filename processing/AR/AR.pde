import com.leapmotion.leap.*;
import processing.video.*;

ParticleSystem ps;
PVector spot;
Controller leap;
Capture cam;

void setup() {
  leap = new Controller();
  size(1280, 720);
  spot = new PVector(width/2,50);
  ps = new ParticleSystem(spot);
  
  String[] cameras = Capture.list();

  if (cameras.length == 0) {
    println("There are no cameras available for capture.");
    exit();
  } 
  else {
    println("Available cameras:");
    for (int i = 0; i < cameras.length; i++) {
      println(cameras[i]);
    }

    // The camera can be initialized directly using an element
    // from the array returned by list():
    cam = new Capture(this, cameras[0]);
    // Or, the settings can be defined based on the text in the list
    //cam = new Capture(this, 640, 480, "Built-in iSight", 30);
    
    // Start capturing the images from the camera
    cam.start();
  }  
}

void draw() {
  Pointable pointer = leap.frame().fingers().frontmost();
  Vector normalized = leap.frame().interactionBox().normalizePoint(pointer.tipPosition());
  spot.x = normalized.getX() * width;
  spot.y = height + height/4  - normalized.getY() * height;

  if (cam.available() == true) {
    cam.read();
  }
  pushMatrix();
  scale(-1,1);
  translate(-width,0);
  image(cam, 0, 0);
  popMatrix();

  ps.setLocation(spot);
  ps.addParticle();
  ps.run();
}


