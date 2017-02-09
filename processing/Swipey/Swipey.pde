    import com.leapmotion.leap.*;
    
    Controller controller = new Controller();
    Frame lastFrame = new Frame();
    Boolean isSwipingLeft = false;
    Boolean isSwipingRight = false;
    int swipingHandID = -1;
    int currentSwipeID = -1;
    
    void setup(){
      controller.enableGesture(Gesture.Type.TYPE_SWIPE);
    }
    
    void draw(){
        Frame newFrame = controller.frame();
        GestureList currentGestures = newFrame.gestures(lastFrame);
        lastFrame = newFrame;
     
        if(currentGestures.count() == 0){
          if(isSwipingLeft || isSwipingRight){
           EndSwipe(); 
           isSwipingLeft = false;
           isSwipingRight = false;
          }
        } else {
          if(isSwipingLeft || isSwipingRight){
            //Check continuing swipe
            for(int g = 0; g < currentGestures.count(); g++){
               //println("Continue " + currentGestures.get(g).id());
               if(currentGestures.get(g).id() == currentSwipeID){
                  SwipeGesture swipe = new SwipeGesture(currentGestures.get(g));
                  ContinueSwipe(swipe);
                  break;
               } 
            }
          } else {
            // It must be a new swipe
            for(int g = 0; g < currentGestures.count(); g++){
               SwipeGesture swipe = new SwipeGesture(currentGestures.get(g));
               if(swipe.direction().angleTo(Vector.left()) < Math.toRadians(30)){
                 //Swiping left
                 isSwipingLeft = true;
                 currentSwipeID = swipe.id();
                 StartSwipe(swipe);
                 break; 
               }
               if(swipe.direction().angleTo(Vector.right()) < Math.toRadians(30)){
                 //Swiping right
                 isSwipingRight = true;
                 currentSwipeID = swipe.id();
                 StartSwipe(swipe);
                 break; 
               }
            }
          }
               
        } 
    }
    
    void StartSwipe(SwipeGesture swipe){
      println("Started swiping " + (isSwipingLeft ? "left." : "right."));
    }
    
    void ContinueSwipe(SwipeGesture swipe){
      println("Continued swiping " + (isSwipingLeft ? "left." : "right."));
      
    }
    
    void EndSwipe(){
      println("Ended swiping.");
    }
