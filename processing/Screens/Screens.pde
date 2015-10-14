import com.leapmotion.leap.*;

LeapListener listener = new LeapListener();
Controller controller = new Controller(listener);
Vector[] corners = new Vector[4];
boolean initialized = false;
int line = 1;
int lineHeight = 20;

void setup(){
  size(displayWidth, displayHeight);
  Config config = controller.config();
  config.setString("screen_calibration0", "{\"screen_calibration0\" : [10.0, 10.0, 10.0, 20.0, 50.0, 20.0, 200.0, 30.0, 30.0]}");
  config.setBool("screen_detected",true);
  System.out.println(config.save());
}

void draw(){
  line = 1;
  if(!(initialized && controller.isConnected())){
    Config config = controller.config();
    corners[0] = new Vector(config.getFloat("screen_point0_x"), config.getFloat("screen_point0_y"), config.getFloat("screen_point0_z"));
    corners[1] = new Vector(config.getFloat("screen_point1_x"), config.getFloat("screen_point1_y"), config.getFloat("screen_point1_z"));
    corners[2] = new Vector(config.getFloat("screen_point3_x"), config.getFloat("screen_point3_y"), config.getFloat("screen_point3_z"));
    corners[3] = new Vector(config.getFloat("screen_point5_x"), config.getFloat("screen_point5_y"), config.getFloat("screen_point5_z"));
    initialized = true;
  }
  background(255);
  Frame frame = controller.frame();
  if(frame.isValid()){
    Config config = controller.config();
    Vector position = frame.pointables().frontmost().tipPosition();
    Vector normPosition = frame.interactionBox().normalizePoint(position, true);
    
    fill(color(0,255,0));
    ellipse(normPosition.getX() * displayWidth, displayHeight * (1 - normPosition.getY()), 20, 20);
    
    System.out.println("Screens: " + controller.locatedScreens().count());
    Screen screen = controller.locatedScreens().get(0);
    Vector intersection = screen.intersect(frame.pointables().frontmost(), true);
    fill(color(255,0,0));
    ellipse(intersection.getX() * displayWidth, displayHeight * (1 - intersection.getY()), 20, 20);

    Vector projection = screen.project(frame.pointables().frontmost().tipPosition(), true);
    fill(color(255,0,255));
    ellipse(projection.getX() * displayWidth, displayHeight * (1 - projection.getY()), 20, 20);

    Vector blc = frame.interactionBox().normalizePoint(screen.bottomLeftCorner(), false);
    fill(color(0,0,0));
    ellipse(blc.getX() * displayWidth, displayHeight * (1 - blc.getY()), 20, 20);
    
    
    text("Norm IBox (green): " + normPosition, 10, lineHeight * line++);
    text("Proj (purple): " + projection, 10, lineHeight * line++);
    text("Intersect (red): " + intersection, 10, lineHeight * line++);
    text("BLC: " + screen.bottomLeftCorner(), 10, lineHeight * line++);
    text("VertAxe: " + screen.verticalAxis(), 10, lineHeight * line++);
    text("HorAxe: " + screen.horizontalAxis(), 10, lineHeight * line++);
    text("Screen dim: " + screen.widthPixels() + ", " + screen.heightPixels(), 10, lineHeight * line++);
    text("Display dim: " + displayWidth + ", " + displayHeight, 10, lineHeight * line++);

    fill(color(0,0,255));
    for(int c = 0; c < 4; c++){
      //Vector norm = frame.interactionBox().normalizePoint(corners[c], true);
      //ellipse(norm.getX() * displayWidth, displayHeight * (1 - norm.getY()), 10, 10);
      int[] screenPos = VectorToScreen(corners[c], screen);
      ellipse(screenPos[0], screenPos[1], 10, 10);
       text("corner: " + c + " ( " + screenPos[0] + ", " + screenPos[1] + ") " + corners[c], 10, lineHeight * line++);
    }    
  }
}

int[] VectorToScreen(Vector vector, Screen screen){
  Vector shifted = vector; //.minus(screen.bottomLeftCorner());
  float hProjection = shifted.dot(screen.horizontalAxis().normalized());
  float vProjection = shifted.dot(screen.verticalAxis().normalized());
  int[] result = new int[2];
  result[0] = (int)(vProjection * screen.widthPixels());
  result[1] = (int)(vProjection * screen.heightPixels());
  return result;
} 


void keyPressed(){
  Frame frame = controller.frame();
  if(frame.isValid()){
    Config config = controller.config();
    Vector position = frame.pointables().frontmost().tipPosition();
    switch (key){
      case '1':
        println("1: " + position);
        corners[0] = position;
        config.setFloat("screen_point0_x", position.getX());
        config.setFloat("screen_point0_y", position.getY());
        config.setFloat("screen_point0_z", position.getZ());
        config.setFloat("screen_point7_x", position.getX());
        config.setFloat("screen_point7_y", position.getY());
        config.setFloat("screen_point7_z", position.getZ());

        config.setFloat("screen_point0_x0", position.getX());
        config.setFloat("screen_point0_y0", position.getY());
        config.setFloat("screen_point0_z0", position.getZ());
        config.setFloat("screen_point7_x0", position.getX());
        config.setFloat("screen_point7_y0", position.getY());
        config.setFloat("screen_point7_z0", position.getZ());
        break;
      case '2':
        println("2 " + position);
        corners[1] = position;
        config.setFloat("screen_point1_x", position.getX());
        config.setFloat("screen_point1_y", position.getY());
        config.setFloat("screen_point1_z", position.getZ());
        config.setFloat("screen_point2_x", position.getX());
        config.setFloat("screen_point2_y", position.getY());
        config.setFloat("screen_point2_z", position.getZ());
        break;
      case '3':
        println("3 " + position);
        corners[2] = position;
        config.setFloat("screen_point3_x", position.getX());
        config.setFloat("screen_point3_y", position.getY());
        config.setFloat("screen_point3_z", position.getZ());
        config.setFloat("screen_point4_x", position.getX());
        config.setFloat("screen_point4_y", position.getY());
        config.setFloat("screen_point4_z", position.getZ());
        break;
      case '4':
        println("4 " + position);
        corners[3] = position;
        config.setFloat("screen_point5_x", position.getX());
        config.setFloat("screen_point5_y", position.getY());
        config.setFloat("screen_point5_z", position.getZ());
        config.setFloat("screen_point6_x", position.getX());
        config.setFloat("screen_point6_y", position.getY());
        config.setFloat("screen_point6_z", position.getZ());
        config.setBool("screen_detected",true);
        break;
    }
     
    System.out.println(config.save());
 }
}
