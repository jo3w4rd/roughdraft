import com.leapmotion.leap.*;

Controller controller = new Controller();
Frame lastFrame = Frame.invalid();
PShape plane;

void setup(){
   size( 600, 600, P3D);
   plane = createShape(RECT, 0, 0, width, width);
   perspective();
   translate(0, 0, -400);
   rotateX(PI/4);
   drawGrid();

   controller.enableGesture(Gesture.Type.TYPE_KEY_TAP); 
   controller.config().setFloat("Gesture.KeyTap.MinDownVelocity", 10.0);
   controller.config().setFloat("Gesture.KeyTap.HistorySeconds", .6);
   controller.config().setFloat("Gesture.KeyTap.MinDistance", 1.0);
   controller.config().save();
}

void draw(){
    translate(0, 0, -400);
   rotateX(PI/4);
   Frame frame = controller.frame();
   GestureList gestures = frame.gestures(lastFrame);
   lastFrame = frame;

   InteractionBox iBox = frame.interactionBox();
   
   for(int g = 0; g < gestures.count(); g++){
        KeyTapGesture tap = new KeyTapGesture(gestures.get(g));
        Vector normalized = iBox.normalizePoint(tap.position());
        ellipse(normalized.getX() * width, normalized.getZ() * height, 20, 20);
   } 
}

int gridSize = 40;
void drawGrid(){
  int hGrids = 1 + width/gridSize;
  int vGrids = 1 + height/gridSize;
  for (int h = 0; h < hGrids; h++){
    line(h * gridSize, 0, h* gridSize, height);
  }
 
  for (int v = 0; v < vGrids; v++){
    line(0, v * gridSize, width, v * gridSize);
  }
}

