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

}

void draw(){
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
     byte[] imageData = new byte[image.width() * image.height()];
     image.data(imageData);

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

void oneLineExamples(Frame frame){
Image image = frame.images().get(0);
// !!!Image_warp_1
float horizontal_slope = tan(65 * 180/PI);
float vertical_slope = tan(15 * 180/PI);
Vector pixel = image.warp(new Vector(horizontal_slope, vertical_slope, 0.0f));
int data_index = floor(pixel.getY()) * image.width() + floor(pixel.getX());
int brightness = image.data()[data_index] & 0xFF;
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

