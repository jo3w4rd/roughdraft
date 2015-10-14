import com.leapmotion.leap.*;

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
        controller.config().setFloat(configKey, value ? 1.0 : 0.0);
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

