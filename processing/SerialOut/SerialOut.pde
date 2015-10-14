import com.leapmotion.leap.*;
import processing.serial.*;

Serial port;

void setup(){
  port = new Serial(this, "/dev/cu.usbmodem1411", 115200);
  println(Serial.list());
}

void draw(){
  if(port.available() > 0){
    println(port.readBytes());
  }
}
