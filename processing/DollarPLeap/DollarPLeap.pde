import com.leapmotion.leap.*;

Controller controller = new Controller();
PImage leftCamera;
HandPoseLibrary library = new HandPoseLibrary();
HandPoseRecognizer recognizer = new HandPoseRecognizer();

void setup(){
   size(500, 500); 
   recognizer.library = library;
   controller.setPolicy(Controller.PolicyFlag.POLICY_IMAGES);
}

void draw(){
  background(255, 255, 255);
  Frame frame = controller.frame();
  pushMatrix();
  //translate(width/2, height * 3/2, 1);
  Hand hand = frame.hands().get(0);
  Matrix handTransform = hand.basis();
  handTransform.setOrigin(hand.palmPosition());
  handTransform = handTransform.rigidInverse();
  for(Finger finger : hand.fingers()){
     for(Bone.Type boneType : Bone.Type.values()) {
         Bone bone = finger.bone(boneType);
         Vector transformed = handTransform.transformPoint(bone.prevJoint());
         Vector normalized = normalize(transformed);
         //translate(width * normalized.getX(), height - ( 1 * normalized.getY()));
         ellipse(normalized.getX(), normalized.getZ(),5,5);
         if(boneType == Bone.Type.TYPE_DISTAL){ //get tip of distal phalanx
            Vector transformedN = handTransform.transformPoint(bone.nextJoint());
            Vector normalizedN = normalize(transformedN);
            //translate(width * normalized.getX(), height - ( 1 * normalized.getY()));
            ellipse(normalizedN.getX(), normalizedN.getZ(),5,5);
            if(finger.type() == Finger.Type.TYPE_INDEX){
                println("x: " + normalizedN.getX() + " y: " + normalizedN.getZ() );
            }            
         }
     }        
  }
  popMatrix();
  
  ImageList images = frame.images();
  Image leftImage = images.get(0);
  //Processing PImage class
  leftCamera = createImage(leftImage.width(), leftImage.height(), RGB);
  leftCamera.loadPixels();
       //Get byte arrays containing the image data
  byte[] leftImageData = new byte[leftImage.width() * leftImage.height()];
  leftImage.data(leftImageData);

  int r, g, b = 0;
  //Copy data into image object, in this case PImage defined in Processing
  for(int i = 0; i < leftImage.width() * leftImage.height(); i++){
     r = (leftImageData[i] & 0xFF) << 16; //convert to unsigned and shift into place
     g = (leftImageData[i] & 0xFF) << 8;
     b = leftImageData[i] & 0xFF;
     leftCamera.pixels[i] =  r | g | b; 
  }
     
  //Show the image
  leftCamera.updatePixels();
  image(leftCamera, 0, height/2);

}

void mousePressed(){

}
void mouseDragged(){
}
void mouseReleased(){
}

void keyPressed(){
  if(key == 'a'){
     //Add pose to library 
  }
  
  if(key == 'c'){
    //Check pose against library
  }
}

Vector normalize(Vector point){ 
  float x = point.getX() + 120;  
  float y = point.getY() + 120; 
  float z = point.getZ() + 120; 

  return new Vector(x, y, z);    
}
