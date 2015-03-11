import com.leapmotion.leap.*;

Controller controller = new Controller();

void setup(){

}

void draw(){
  background(255);
  Frame frame = controller.frame();
    for (Finger f : frame.fingers())
    {
        switch (f.type()) 
        {
        case TYPE_THUMB :
             if(f.hand().isRight())
             {
                 System.out.println("Fid: " + frame.id() + " finger id: " + f.id() + " position : (x:" + f.tipPosition().getX() + " , y:" + f.tipPosition().getY() +")");
             }
        }
    } 
}
