
/**
 * Space Junk  
 * by Ira Greenberg (zoom suggestion by Danny Greenberg).
 * 
 * Rotating cubes in space using a custom Cube class. 
 * Color controlled by light sources. Move the mouse left
 * and right to zoom.
 */

import com.leapmotion.leap.*;

// Used for oveall rotation
float ang;

// Cube count-lower/raise to test performance
int limit = 500;

// Array for all cubes
Cube[]cubes = new Cube[limit];
Controller leap = new Controller();

void setup() {
  size(1280, 720, P3D); 
  background(0); 
  noStroke();

  // Instantiate cubes, passing in random vals for size and postion
  for (int i = 0; i< cubes.length; i++){
    cubes[i] = new Cube(int(random(-10, 10)), int(random(-10, 10)), 
                        int(random(-10, 10)), int(random(-140, 140)), 
                        int(random(-140, 140)), int(random(-140, 140)));
  }
}

Frame startFrame = new Frame();
void draw(){
  background(0); 
  fill(200);
  Vector translation = Vector.zero();
  Vector rotation = Vector.zero();
 if( leap.isConnected())
  {
        Frame frame = leap.frame();
        if(!startFrame.isValid()) {
          startFrame = frame;
        } else {
          translation = frame.translation(startFrame);
          //rotation = frame.rotationAxis(startFrame).times( frame.rotationAngle(startFrame));
        }
  }
  // Set up some different colored lights
  pointLight(51, 102, 255, 65, 60, 100); 
  pointLight(200, 40, 60, -65, -60, -150);

  // Raise overall light in scene 
  ambientLight(70, 70, 10); 

  // Center geometry in display windwow.
  // you can change 3rd argument ('0')
  // to move block group closer(+)/further(-)
  translate(width/2 + translation.getX() * 10, height/2 - translation.getY() * 10, -200 - translation.getZ() * 10);

  // Rotate around y and x axes
  rotateY(rotation.getX());
  rotateX(rotation.getY());
  rotateX(rotation.getZ());

  // Draw cubes
  for (int i = 0; i < cubes.length; i++){
    cubes[i].drawCube();
  }
  
  // Used in rotate function calls above
  ang += 0.2;
}


