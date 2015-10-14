import processing.core.*; 
import processing.data.*; 
import processing.event.*; 
import processing.opengl.*; 

import com.leapmotion.leap.*; 
import java.util.Hashtable; 
import com.leapmotion.leap.*; 

import java.util.HashMap; 
import java.util.ArrayList; 
import java.io.File; 
import java.io.BufferedReader; 
import java.io.PrintWriter; 
import java.io.InputStream; 
import java.io.OutputStream; 
import java.io.IOException; 

public class ConfigSettings extends PApplet {




Hashtable<String, ConfigItem> configs = new Hashtable<String, ConfigItem>();
//Controller controller = new Contrtroller();

public void setup(){
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

public void draw(){
  background(color(255,255,255));
  Frame frame = controller.frame();
  InteractionBox box = frame.interactionBox();
  for(Finger finger : frame.fingers()){
    Vector norm = box.normalizePoint(finger.tipPosition());
    ellipse( norm.getX() * width, height * (1 - norm.getY()), 5, 5);
  }
}

public void keyPressed() {
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


private static Controller controller = new Controller();

public class ConfigItem{
  public char commandKey = '1';
  public String configKey = "config";
  public String onValue = "1";
  public String offValue = "0";
  public int type = 0; //0: boolean, 1: int, 2: float 3: string
  public boolean isSet = false;
  
  public ConfigItem(char command, String key, String on, String off, int type){
    this.commandKey = command;
    this.configKey = key;
    this.onValue = on;
    this.offValue = off;
    this.type = type;
  }
  
  public String toString(){
    switch(this.type){
      case 0:
        return commandKey + ": " + configKey + " is " + (this.getBoolValue() ? "on." : "off.");
      case 1:
        return commandKey + ": " + configKey + " is " + this.getIntValue();
      case 2:
        return commandKey + ": " + configKey + " is " + this.getFloatValue();
      case 3:
        return commandKey + ": " + configKey + " is " + this.getStringValue();
     }
     return commandKey + ": " + configKey;
  }
  
  public void setValue(boolean value){
    switch(this.type){
      case 0:
        controller.config().setBool(configKey, value);
        break;
      case 1:
        controller.config().setInt32(configKey, value ? 1 : 0);
        break;
      case 2:
        controller.config().setFloat(configKey, value ? 1.0f : 0.0f);
        break;
      case 3:
        controller.config().setString(configKey, value ? "true" : "false");
        break;
    }
    controller.config().save();
  }
  
  public boolean getBoolValue(){
    return controller.config().getBool(configKey);
  }

  public void setValue(String value){
    switch(this.type){
      case 0:
        setValue(Boolean.parseBoolean(value));
        break;
      case 1:
        setValue(Integer.parseInt(value));
        break;
      case 2:
        setValue(Float.parseFloat(value));
        break;
      case 3:
        setValue(value);
        break;
    }
    controller.config().save();
  }

  public void setValue(float value){
    switch(this.type){
      case 0:
        controller.config().setBool(configKey, value == 0 ? false : true);
        break;
      case 1:
        controller.config().setInt32(configKey, (int)value);
        break;
      case 2:
        controller.config().setFloat(configKey, value);
        break;
      case 3:
        controller.config().setString(configKey, "" + value);
        break;
    }
    controller.config().save();
  }
  
  public float getFloatValue(){
    return controller.config().getFloat(configKey);
  }

  public void setValue(int value){
    switch(this.type){
      case 0:
        controller.config().setBool(configKey, value == 0 ? false : true);
        break;
      case 1:
        controller.config().setInt32(configKey, value);
        break;
      case 2:
        controller.config().setFloat(configKey, (float)value);
        break;
      case 3:
        controller.config().setFloat(configKey, value);
        break;
    }
    controller.config().save();
  }
  
  public int getIntValue(){
    return controller.config().getInt32(configKey);
  }
  public String getStringValue(){
    return controller.config().getString(configKey);
  }

}

  static public void main(String[] passedArgs) {
    String[] appletArgs = new String[] { "ConfigSettings" };
    if (passedArgs != null) {
      PApplet.main(concat(appletArgs, passedArgs));
    } else {
      PApplet.main(appletArgs);
    }
  }
}
