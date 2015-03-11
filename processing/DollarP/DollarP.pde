import com.leapmotion.leap.*;

PDollarRecognizer recognizer = new PDollarRecognizer();

ArrayList<Point> sample;
int id = 1;

void setup(){
   size(500, 500); 
   sample = new ArrayList<Point>();
}

void draw(){}

void mousePressed(){
  sample.add(new Point(mouseX, mouseY, id));

}
void mouseDragged(){
  sample.add(new Point(mouseX, mouseY, id));
}
void mouseReleased(){
  sample.add(new Point(mouseX, mouseY, id++));  
}

void keyPressed(){
  println("Sample pts: " + sample.size());
  RecognizerResults result = recognizer.Recognize(sample);
  println(result.mName + " " + result.mScore);
  sample = new ArrayList<Point>();

}
