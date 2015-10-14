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
      text("w,s enable", 10, lineHeight * line++);
      text("e,d remote", 10, lineHeight * line++);
      text("r,f ws port", 10, lineHeight * line++);
      text("t,g wss port", 10, lineHeight * line++);
    
      text("websockets_enabled: " + config.getBool("websockets_enabled"), 10, lineHeight * line++);
      text("websockets_allow_remote: " + config.getBool("websockets_allow_remote"), 10, lineHeight * line++);
      text("ws_port: " + config.getInt32("ws_port"), 10, lineHeight * line++);
      text("wss_port: " + config.getInt32("wss_port"), 10, lineHeight * line++);
}

void keyPressed() {
  int currentInt = 0;
  float currentFloat = 0;
  
  switch(key){  
     case 'w':
        config.setBool("websockets_enabled",true);
        break; 
     case 's':
        config.setBool("websockets_enabled",false);
        break; 
     case 'e':
        config.setBool("websockets_allow_remote",true);
        break; 
     case 'd':
        config.setBool("websockets_allow_remote",true);
        break; 
     case 'r':
        config.setInt32("ws_port", 6437);
        break; 
     case 'f':
        config.setInt32("ws_port", 6537);
        break; 
     case 't':
        config.setInt32("wss_port",6436);
        break; 
     case 'g':
        config.setInt32("wss_port",6536);
        break; 
  }
  System.out.println("Saved: " + key + " " + config.save());
}
