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
 rawImage(); 
 distortion();
 imageFingerTipsRaw();
}
void rawImage(){
// !!!Image_raw
 Frame frame = controller.frame();
 if(frame.isValid()){
   ImageList images = frame.images();
   for(Image image : images)
   {
     //Processing PImage class
     PImage camera = cameras[image.id()];
     camera = createImage(image.width(), image.height(), RGB);
     camera.loadPixels();
     
     //Get byte array containing the image data from Image object
     byte[] imageData = image.data();

     //Copy image data into display object, in this case PImage defined in Processing
     for(int i = 0; i < image.width() * image.height(); i++){
       r = (imageData[i] & 0xFF) << 16; //convert to unsigned and shift into place
       g = (imageData[i] & 0xFF) << 8;
       b = imageData[i] & 0xFF;
       camera.pixels[i] =  r | g | b; 
     }
     
     //Show the image
     camera.updatePixels();
     image(camera, 640 * image.id(), 0);  
   }
} 
// !!!END
}

void distortion(){
 Frame frame = controller.frame();
 if(frame.isValid()){
   ImageList images = frame.images();
   for(Image image : images)
   {
     
 // !!!Image_distortion
 //Get float array containing the distortion data from Image object
 float[] distortionData = image.distortion();

//Extract the individual distortion values
for(int d = 0; d < image.distortionWidth() * image.distortionHeight(); d += 2){
   float dX = distortionData[d];
   float dY = distortionData[d + 1];
 
   //Check if the values are valid (in the range [0..1])
   if(!((dX < 0) || (dX > 1)) && !((dY < 0) || (dY > 1))) {
      // Use the data... 
      // In this case, we use the Processing API to draw 
      // a colored ellipse over the raw image
      fill(color(dX * 255, 255, dY * 255));
      int originX = 640 * image.id();
      ellipse(originX + dX * image.width(), dY * image.height(), 5, 5);
   }
}
// !!!END
  }
 }
}

void imageFingerTipsRaw(){
// !!!Image_fingertips_raw
float cameraOffset = 20; //Physical x-axis offset of cameras in millimeters
Frame frame = controller.frame();
if(frame.isValid()){
    ImageList images = frame.images();
    for(Image image : images)
    {
        FingerList fingers = frame.fingers();
        for(Finger finger : fingers){
            Vector tip = finger.tipPosition();
            
            // Calculate the slope of the tip position
            float hSlope = -(tip.getX() + cameraOffset * (2 * image.id() - 1))/tip.getY();
            float vSlope = tip.getZ()/tip.getY();
            
            // Get the pixel location using the tip slope
            Vector pixelLocation = image.warp(new Vector(hSlope, vSlope, 0f));
            
            //Draw ellipse at the specified pixel over the raw image (uses Processing API)
            fill(color(127, 0, 255));
            int originX = 640 * image.id();
            ellipse(pixelLocation.getX() + originX, pixelLocation.getY(), 10, 10);
        }

    }
  }
// !!!END
}

void oneLineExamples(Frame frame){
Image image = frame.images().get(0);
// !!!Image_warp_1
float horizontal_slope = tan(65 * 180/PI);
float vertical_slope = tan(15 * 180/PI);
Vector pixel = image.warp(new Vector(horizontal_slope, vertical_slope, 0));
int data_index = floor(pixel.getY()) * image.width() + floor(pixel.getX());
byte brightness = image.data()[data_index]; //Note that in Java bytes are signed [-127..127]
// !!!END
// !!!Image_rectify_1
Vector feature = new Vector(127, 68, 0);
Vector slopes = image.rectify(feature);
// !!!END
// !!!Image_data_1
byte[] image_buffer = image.data();
// !!!END
// !!!Image_distortion_1
float[] distortion_buffer = image.distortion();
// !!!END
// !!!Image_image_width_1
int width = image.width();
// !!!END
// !!!Image_image_height_1
int height = image.height();
// !!!END
// !!!Image_distortion_width_1
int correctionGridWidth = image.distortionWidth();
// !!!END
// !!!Image_distortion_height_1
int correctionGridHeight = image.distortionHeight();
// !!!END
// !!!Image_ray_factors_1
Vector raySlopes = new Vector(-3.25, 1.75, 0);
Vector normRay =
    new Vector(raySlopes.getX() * image.rayScaleX() + image.rayOffsetX(),
           raySlopes.getY() * image.rayScaleY() + image.rayOffsetY(), 0);
// !!!END
// !!!Image_ray_factors_2
Vector normSlopes = new Vector(.25, .98, 0);
Vector slope = new Vector((normSlopes.getX() - image.rayOffsetX())/image.rayScaleX(),
             (normSlopes.getY() - image.rayOffsetY())/image.rayScaleY(), 0);
// !!!END

}

void rectify(Image image){
// !!!Image_rectify
for (int horizontal = 0; horizontal < image.width(); horizontal++) {
    for (int vertical = 0; vertical < image.height(); vertical++ ){
        Vector slopePair = image.rectify(new Vector(horizontal, vertical, 0));
        //Use the slope values ...
    }
}
// !!!END  
}
