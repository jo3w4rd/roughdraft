import com.leapmotion.leap.*;

Controller controller = new Controller();
PImage leftCamera;
PImage rightCamera;
int r = 0;
int g = 0;
int b = 0;

void setup(){
controller.setPolicy(Controller.PolicyFlag.POLICY_IMAGES);
size(1280,480);
}

void draw(){
   Frame frame = controller.frame();
   if(frame.isValid()){
     ImageList images = frame.images();
     Image leftImage = images.get(0);
     Image rightImage = images.get(1);
     
     //Processing PImage class
     leftCamera = createImage(leftImage.width(), leftImage.height(), RGB);
     rightCamera = createImage(rightImage.width(), rightImage.height(), RGB);
     leftCamera.loadPixels();
     rightCamera.loadPixels();
     
     //Get byte arrays containing the image data
     byte[] leftImageData = new byte[leftImage.width() * leftImage.height()];
     leftImage.data(leftImageData);
     byte[] rightImageData = new byte[rightImage.width() * rightImage.height()];
     rightImage.data(rightImageData);

     //Copy data into image object, in this case PImage defined in Processing
     //for(int i = 0; i < leftImage.width() * leftImage.height(); i++){
     for(int i = 0; i < 640; i++){
       for(int j = 239; i>= 0; i--){
         
         Vector warped = leftImage.warp(
         r = (leftImageData[i] & 0xFF) << 16; //convert to unsigned and shift into place
         g = (leftImageData[i] & 0xFF) << 8;
         b = leftImageData[i] & 0xFF;
         leftCamera.pixels[i] =  r | g | b; 

         r = (rightImageData[i] & 0xFF) << 16; //convert to unsigned and shift into place
         g = (rightImageData[i] & 0xFF) << 8;
         b = rightImageData[i] & 0xFF;
         rightCamera.pixels[i] =  r | g | b; 
       }
     }
     
     //Show the image
     leftCamera.updatePixels();
     image(leftCamera, 0, 0);

     rightCamera.updatePixels();
     image(rightCamera, leftImage.width(), 0);
   }
}


