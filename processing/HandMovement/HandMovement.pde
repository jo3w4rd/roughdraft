import com.leapmotion.leap.*;

    Controller controller = new Controller();
    float x, y, z;
    
       void setup(){
          size( 400, 400, P3D); 
          x = width/2;
          y = height/2;
          z = 0;
       }
       void draw(){
            background(255);
            translate(x, y, z);
            Frame frame = controller.frame();
            if(frame.id() > 0){
                if(frame.hands().count() > 0){
                    Hand hand = frame.hands().get(0);

                    //Method 1. using palm velocity
                    Vector velocity = hand.palmVelocity();
                    stroke(#0000ff);
                    strokeWeight(2);
                    line(0, 0, 0, velocity.get(0), velocity.get(1), velocity.get(2));
                    //Method 2. using 2 hand positions
                    Frame earlierFrame = controller.frame(59); //60 frames earlier than current
                    Hand earlierHand = earlierFrame.hand(hand.id()); //look up the hand by its ID
                    if(earlierHand.isValid()){ //then we found the same hand
                        Vector displacement = hand.palmPosition().minus(earlierHand.palmPosition());
                        float dt = frame.timestamp() - earlierFrame.timestamp(); //microseconds
                        println("dt mico " + dt);
                        dt = dt * .000001; //seconds
                        println("dt seconds " + dt + " " + displacement + " " + velocity);
                        Vector velocity2 = displacement.times(1/dt);//mm per second
                        translate(5, 0, 0);
                        stroke(#ff0000);
                        strokeWeight(2);
                        line(0, 0, 0, velocity2.get(0), velocity2.get(1), velocity2.get(2));                        
                    }
            }
       }
    }

