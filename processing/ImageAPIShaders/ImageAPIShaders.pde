import com.leapmotion.leap.*;

Controller controller = new Controller();
PImage leapCamera;
PImage distortionHorizontal;
PImage distortionVertical;
PShader dewarp;
PShape quad;
int r = 0;
int g = 0;
int b = 0;

void setup(){
    colorMode(RGB, 1.0);
    size(640,640,P3D);

    controller.setPolicyFlags(Controller.PolicyFlag.POLICY_IMAGES);
    dewarp = loadShader("dewarp.glsl", "passthrough.glsl");
    
    quad = createShape();
    quad.beginShape();
    quad.noStroke();
    quad.vertex(0, 0, 1, 0, 0);
    quad.vertex(0, 400, 1, 0, 1);
    quad.vertex(400, 400, 1, 1, 1);
    quad.vertex(400, 0, 1, 1, 0);
    quad.vertex(0, 0, 1, 0, 0);
    quad.fill(0,0,128);
    quad.endShape(); 
}

void draw(){
   Frame frame = controller.frame();
   if(frame.isValid()){
     ImageList images = frame.images();
     Image cameraImage = images.get(0);
     
     //Processing PImage class
     leapCamera = createImage(cameraImage.width(), cameraImage.height(), RGB);
     leapCamera.loadPixels();
     
     //Get byte arrays containing the image data
     byte[] imageData = new byte[cameraImage.width() * cameraImage.height()];
     cameraImage.data(imageData);

     //Copy data into image object, in this case PImage defined in Processing
     for(int i = 0; i < cameraImage.width() * cameraImage.height(); i++){
       r = (imageData[i] & 0xFF) << 16; //convert to unsigned and shift into place
       g = (imageData[i] & 0xFF) << 8;
       b = imageData[i] & 0xFF;
       leapCamera.pixels[i] =  r | g | b; 
     }
     leapCamera.updatePixels();     
  
     if(distortionHorizontal == null){ 
          //Get float array containing the distortion data from Image object
         float[] distortionData = cameraImage.distortion();
         distortionHorizontal = createImage(cameraImage.distortionWidth()/2, cameraImage.distortionHeight(), ARGB);
         distortionVertical = createImage(cameraImage.distortionWidth()/2, cameraImage.distortionHeight(), ARGB);
         distortionHorizontal.loadPixels();
         distortionVertical.loadPixels();
         //Extract the individual distortion values
         for(int d = 0; d < cameraImage.distortionWidth() * cameraImage.distortionHeight(); d += 2){
           float dX = distortionData[d];
           float dY = distortionData[d + 1];
           distortionHorizontal.pixels[d/2] = encodeFloatRGBA(dX);
           distortionVertical.pixels[d/2] = encodeFloatRGBA(dY); 
         }

         distortionHorizontal.updatePixels();
         distortionVertical.updatePixels();
     }

    background(1);

    shader(dewarp);
    dewarp.set("vDistortion", distortionVertical);
    dewarp.set("hDistortion", distortionHorizontal);

     
    textureMode(NORMAL);
    quad.beginShape();
    quad.texture(leapCamera);
    quad.endShape();
    shape(quad);

  }
}

color encodeFloatRGBA( float val)
{
    val = (val + 0.6)/2.3;
    float r = val;
    float g = val * 255;
    float b = val * 255 * 255;
    float a = val * 255 * 255 * 255;
    
    r = r - (float)Math.floor(r);
    g = g - (float)Math.floor(g);
    b = b - (float)Math.floor(b);
    a = a - (float)Math.floor(a);
    
    return color(r, g, b, a);
}

float decodeFloatRGBA( color argb)
{
    float r = red(argb);
    float g = green(argb);
    float b = blue(argb);
    float a = alpha(argb);
  
    g = g/255;
    b = b/(255 * 255);
    a = a/(255 * 255 * 255);
 
    return (r + g + b + a) * 2.3 - 0.6;
}

