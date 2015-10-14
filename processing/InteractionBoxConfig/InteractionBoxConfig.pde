import com.leapmotion.leap.*;

Controller controller = new Controller();
Config config;

int lineHeight = 20;

void setup(){
  size(400, 600);
  config = controller.config();
}

void draw(){
  background(0);
  int line = 1;
      text("w,s auto", 10, lineHeight * line++);
      text("e,d height", 10, lineHeight * line++);
      text("r,f scale", 10, lineHeight * line++);
      text("t,g center x", 10, lineHeight * line++);
      text("y,h center y", 10, lineHeight * line++);
      text("u,j center z", 10, lineHeight * line++);
    
      text("interaction_box_auto: " + config.getBool("interaction_box_auto"), 10, lineHeight * line++);
      text("interaction_box_height: " + config.getInt32("interaction_box_height"), 10, lineHeight * line++);
      text("interaction_box_scale: " + config.getFloat("interaction_box_scale"), 10, lineHeight * line++);
      text("interaction_center_x: " + config.getInt32("interaction_center_x"), 10, lineHeight * line++);
      text("interaction_center_y: " + config.getInt32("interaction_center_y"), 10, lineHeight * line++);
      text("interaction_center_z: " + config.getInt32("interaction_center_z"), 10, lineHeight * line++);
      
      Frame frame = controller.frame();
      InteractionBox box = frame.interactionBox();
      text("IB center: " + box.center(), 10, lineHeight * line++);
      text("IB width: " + box.width(), 10, lineHeight * line++);
      text("IB height: " + box.height(), 10, lineHeight * line++);
      text("IB depth: " + box.depth(), 10, lineHeight * line++);
      text("denorm(.5,.5,.5): " + box.denormalizePoint(new Vector(.5,.5,.5)), 10, lineHeight * line++);
      text("Norm center: " + box.normalizePoint(box.center(),false), 10, lineHeight * line++);
      text("Norm (50, 50, 50: " + box.normalizePoint(new Vector(50,50,50),false), 10, lineHeight * line++);
      text("Frame isValid: " + frame.isValid(), 10, lineHeight * line++);
}

void keyPressed() {
  int currentInt = 0;
  float currentFloat = 0;
  
  switch(key){  
     case 'w':
        config.setBool("interaction_box_auto",true);
        break; 
     case 's':
        config.setBool("interaction_box_auto",false);
        break; 
     case 'e':
        currentInt = config.getInt32("interaction_box_height");
        config.setInt32("interaction_box_height", currentInt + 20);
        break; 
     case 'd':
        currentInt = config.getInt32("interaction_box_height");
        config.setInt32("interaction_box_height",currentInt - 20);
        break; 
     case 'r':
        currentFloat = config.getFloat("interaction_box_scale");
        config.setFloat("interaction_box_scale",currentFloat + .1);
        break; 
     case 'f':
        currentFloat = config.getFloat("interaction_box_scale");
        config.setFloat("interaction_box_scale",currentFloat - .1);
        break; 
     case 't':
        currentInt = config.getInt32("interaction_center_x");
        config.setInt32("interaction_center_x",currentInt + 20);
        break; 
     case 'g':
        currentInt = config.getInt32("interaction_center_x");
        config.setInt32("interaction_center_x",currentInt - 20);
        break; 
     case 'y':
        currentInt = config.getInt32("interaction_center_y");
        config.setInt32("interaction_center_y",currentInt + 20);
        break; 
     case 'h':
        currentInt = config.getInt32("interaction_center_y");
        config.setInt32("interaction_center_y",currentInt - 20);
        break; 
     case 'u':
        currentInt = config.getInt32("interaction_center_z");
        config.setInt32("interaction_center_z",currentInt + 20);
        break; 
     case 'j':
        currentInt = config.getInt32("interaction_center_z");
        config.setInt32("interaction_center_z",currentInt - 20);
        break; 
  }
  System.out.println("Saved: " + key + " " + config.save());
}
