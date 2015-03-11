import com.leapmotion.leap.*;

int width = 800;
int height = 600;

Controller leap = new Controller();
HandModel[] hands;
float avgFingerWidth;

void setup()
{
  frameRate(60);
   size(width, height, P3D);
   camera( width/2, -height/2, 100, width/2, height/2, 200, 0.0, 0.0, 1.0 );
   textSize(50);
}


void draw(){
  background(255,255,255);
  fill(50);

  translate(width/2, height/2,200);
  Frame frame = leap.frame();
  hands = new HandModel[ frame.hands().count() ];
  for( int h = 0; h < frame.hands().count(); h++ )
  {
      Hand leapHand = frame.hands().get(h);
      hands[h] = new HandModel( leapHand.id(), leapHand.fingers().count() );
      
      Matrix handTransform = leapHand.basis();
      handTransform.setOrigin(leapHand.palmPosition()); 
      handTransform = handTransform.rigidInverse();

      pushMatrix();
      translate(0, 0, 0);
      stroke(50,200,50);
      sphere(10);
      //rotateX(leapHand.direction().getX());
      //rotateY(leapHand.direction().getY());
      //rotateZ(leapHand.direction().getZ());
      box(3,3,80);
      popMatrix();
      for( int f = 0; f < leapHand.fingers().count(); f++ )
      {
        Finger leapFinger = leapHand.fingers().get(f);
         Vector tPosition = handTransform.transformPoint( leapFinger.tipPosition() );
         FingerModel finger = new FingerModel( leapFinger.id() );
         finger.tx = tPosition.getX();
         finger.ty = tPosition.getY();
         finger.tz = tPosition.getZ();
         finger.fingerWidth = leapFinger.width();
         finger.length = leapFinger.length();
         Vector basePosition =  handTransform.transformDirection( leapFinger.direction() ).times(finger.length);
         finger.bx = finger.tx - basePosition.getX();
         finger.by = finger.ty - basePosition.getY();
         finger.bz = finger.tz - basePosition.getZ();
         
         pushMatrix();
         stroke(0,0,0);
         translate(finger.tx, -finger.ty,finger.tz);
         sphere(10);
         pushMatrix();
         rotateX(PI/2);
         fill(50);
         //text(finger.leapID, -10, -10);
         fill(200);
         //text(finger.digitID, 20, -10);
         popMatrix();

         popMatrix();
         
         pushMatrix();
         translate(finger.bx, -finger.by,finger.bz);
         stroke(200,50,50);
         sphere(5);
         popMatrix();

         hands[h].fingers[f] = finger;
      }
      hands[h].classifyFingers(leap);
      
      for( int hi = 0; hi < hands.length; hi++ )
      {
        for( int f = 0; f < hands[hi].fingers.length; f++ )
        {
            FingerModel finger = hands[hi].fingers[f];
            pushMatrix();
              stroke(0,0,0);
              translate(finger.tx, -finger.ty,finger.tz);
              pushMatrix();
                rotateX(PI/2);
                fill(50);
                //text(finger.leapID, -10, -10);
                fill(201);
                text(finger.ty, 10, 40);
              popMatrix();
            popMatrix();
        }
      }
      
     //keyPressed();
  }
  
}

void keyPressed()
{
   Frame frame = leap.frame();                 
   for( HandModel hand : hands )
  {
    Hand leapHand = frame.hand( hand.leapID ); 
    if( leapHand.fingers().count() == 0) break;
    println("Printing left to right fingers");
    FingerModel[] fingers = hand.fingersLeftToRight();
    println( " Hand: (" + leapHand.palmPosition().getX() + ", " + leapHand.palmPosition().getY() + ", " + leapHand.palmPosition().getZ() + ")");
    for( FingerModel finger : fingers )
    { 
       Vector leapPos = frame.finger( finger.leapID ).tipPosition();
       //println( finger.digitID + " : " + finger.leapID  + " HFR: (" + finger.tx + ", " + finger.ty + ", " + finger.tz + ") " + finger.fingerWidth + ", " + finger.length + " LeapFR: (" + leapPos.getX() + ", " + leapPos.getY() + ", " + leapPos.getZ() + ")");
       print( finger.digitID + " : " + finger.leapID );
       print( " HFR: (" + finger.tx + ", " + finger.ty + ", " + finger.tz + ") " + finger.fingerWidth + ", " + finger.length);
       print( " Base: (" + finger.bx + ", " + finger.by + ", " + finger.bz + ") ");
       println( " LeapFR: (" + leapPos.getX() + ", " + leapPos.getY() + ", " + leapPos.getZ() + ")");

    }
    //println("Printing bottom to top fingers");
    fingers = hand.fingersBottomToTop();
    for( FingerModel finger : fingers )
    { 
       Vector leapPos = frame.finger( finger.leapID ).tipPosition();
      //println(finger.leapID  + " HFR: (" + finger.tx + ", " + finger.ty + ", " + finger.tz + ") LeapFR: (" + leapPos.getX() + ", " + leapPos.getY() + ", " + leapPos.getZ() + ")");
    }  
    
    //println("Printing back to front fingers");
    fingers = hand.fingersBackToFront();
    for( FingerModel finger : fingers )
    { 
       Vector leapPos = frame.finger( finger.leapID ).tipPosition();
      // println(finger.leapID  + " HFR: (" + finger.tx + ", " + finger.ty + ", " + finger.tz + ") LeapFR: (" + leapPos.getX() + ", " + leapPos.getY() + ", " + leapPos.getZ() + ")");
    }  
  
  }
}
