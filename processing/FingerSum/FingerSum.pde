import com.leapmotion.leap.*;

String report = "Calculating";
Controller controller = new Controller();

void setup(){
 size(500, 500);  
}

void draw(){
  background(0);
  Frame frame = controller.frame();
  if(frame.isValid()){
      float total = 0;
      for(Finger finger : frame.fingers()){
         total += finger.length();  
      }
      float palmWidth = 0;
      if(frame.hands().count() > 0) palmWidth = frame.hands().get(0).palmWidth();
      report = String.format("Total length = %f%n Palm width = %f", total, palmWidth);
  }
  text(report,8,25);
}


