import com.leapmotion.leap.*;
import java.util.Hashtable;

Hashtable<String, ConfigItem> configs = new Hashtable<String, ConfigItem>();
//Controller controller = new Contrtroller();

void setup(){
  configs.put("t", new ConfigItem('t', "tracking_tool_enabled", "false", "true", 0));
  configs.put("h", new ConfigItem('h', "tracking_hand_enabled", "false", "true", 0));
  configs.put("a", new ConfigItem('a', "tracking_enabled", "false", "true", 0));
  
  configs.put("q", new ConfigItem('q', "tracking_quad_enabled", "false", "true", 0));
  configs.put("p", new ConfigItem('p', "skeleton_palm_filtering", "0.5", "0.0", 2));
  configs.put("v", new ConfigItem('v', "tracking_version", "v1", "v2", 3));
  configs.put("f", new ConfigItem('f', "skeleton_finger_filtering", "0.6", "0.0", 2));
  if(controller.isConnected()){
    for (ConfigItem item : configs.values()) {
    println(item);   
  }
  }
}

void draw(){
  background(color(255,255,255));
  Frame frame = controller.frame();
  InteractionBox box = frame.interactionBox();
  for(Finger finger : frame.fingers()){
    Vector norm = box.normalizePoint(finger.tipPosition());
    ellipse( norm.getX() * width, height * (1 - norm.getY()), 5, 5);
  }
}

void keyPressed() {
  if (configs.containsKey(String.valueOf(key))) {
    ConfigItem item = configs.get(String.valueOf(key));
    if(!item.isSet){
      item.setValue(item.onValue);
    } else {
      item.setValue(item.offValue);
    }
    item.isSet = !item.isSet;
  }
  for (ConfigItem item : configs.values()) {
    println(item);   
  }
}
