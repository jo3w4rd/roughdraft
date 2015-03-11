import com.leapmotion.leap.*;

Controller controller = new Controller();
PImage leftCamera;
PImage rightCamera;
PImage[] cameras = new PImage[2];
int r = 0;
int g = 0;
int b = 0;

void setup(){
// !!!Image_policy
controller.setPolicyFlags(Controller.PolicyFlag.POLICY_IMAGES);
// !!!END
  size(1280,480);
  noStroke();

}

void draw() {
 correctedImage(); 
 imageFingerTipsCorrected();
}
void correctedImage(){
// !!!Image_warp
//Draw the undistorted image using the warp() function
int targetWidth = 200;
int targetHeight = 200;

 Frame frame = controller.frame();
 if(frame.isValid()){
   ImageList images = frame.images();
   for(Image image : images)
   {
     //Processing PImage class
     PImage camera = cameras[image.id()];
     camera = createImage(targetWidth, targetHeight, RGB);
     camera.loadPixels();
     
     int[] brightness = {0,0,0}; //An array to hold the rgba color components
     
     //For each pixel in the target image...
     for(float y = 0; y < targetWidth; y++){
       for(float x = 0; x < targetHeight; x++){
         //Normalized slope for this pixel
         Vector input = new Vector(x/targetWidth, y/targetHeight,0);
         //Convert from normalized [0..1] to slope [-4..4]
         input.setX((input.getX() - image.rayOffsetX()) / image.rayScaleX());
         input.setY((input.getY() - image.rayOffsetY()) / image.rayScaleY());

         //Look up the pixel coordinates in the raw image corresponding to the slope values 
         Vector pixel = image.warp(input);
         
         //Check that the coordinates are valid (i.e. within the camera image)
         if(pixel.getX() >= 0 && pixel.getX() < image.width() && pixel.getY() >= 0 && pixel.getY() < image.height()) {
             int data_index = floor(pixel.getY()) * image.width() + floor(pixel.getX()); //xy to buffer index
             brightness[0] = image.data()[data_index] & 0xff; //Look up brightness value
             brightness[2] = brightness[1] = brightness[0]; //Set the color components equal (greyscale)
         } else {
             brightness[0] = 255; //Display invalid pixels as red
             brightness[2] = brightness[1] = 0;
         }

         //Set the pixel of the display object, in this case PImage defined in Processing
         camera.pixels[floor(y * targetWidth + x)] =  color(brightness[0], brightness[1], brightness[2]); 
       }
     }
     
     //Show the image
     camera.updatePixels();
     image(camera, 640 * image.id(), 0);  
   }
} 
// !!!END
}

void imageFingerTipsCorrected(){
// !!!Image_fingertips_corrected
int targetWidth = 200;
int targetHeight = 200;
Frame frame = controller.frame();
if(frame.isValid()){
    ImageList images = frame.images();
    for(Image image : images)
    {
        FingerList fingers = frame.fingers();
        for(Finger finger : fingers){
            Vector tip = finger.tipPosition();
            float hSlope = -(tip.getX() + 20 * (2 * image.id() - 1))/tip.getY();
            float vSlope = tip.getZ()/tip.getY();

            //Normalize ray from [-4..4] to [0..1] (the inverse of how the undistorted image was drawn earlier)
            Vector ray = new Vector(hSlope * image.rayScaleX() + image.rayOffsetX(),
                                    vSlope * image.rayScaleY() + image.rayOffsetY(), 0);

            //Pixel coordinates from [0..1] to [0..width/height]
            Vector pixelLocation = new Vector(ray.getX() * targetWidth, ray.getY() * targetHeight, 0);
            
            //Draw ellipse at the specified pixel over the raw image (uses Processing API)
            fill(color(127, 0, 255));
            int originX = 640 * image.id();
            ellipse(pixelLocation.getX() + originX, pixelLocation.getY(), 10, 10);
        }
    }
}
// !!!END
}

