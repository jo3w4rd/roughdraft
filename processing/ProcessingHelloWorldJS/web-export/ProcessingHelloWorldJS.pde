    var controller = new Leap.Controller();

    void setup(){
      size(200, 200);
      controller.connect();
    }
    void draw(){
      background(0);
      var frame = controller.frame();
      text(frame.hands.length + " hands.", 50, 50);
      text(frame.fingers.length + " hands.", 50, 100);
    }


