import com.leapmotion.leap.*;

int width = 800;
int height = 600;

Controller leap = new Controller();

void setup()
{
  frameRate(60);
   size(width, height);
}


void draw(){
  background(255,255,255);
  fill(50);

  Frame frame = leap.frame();
  InteractionBox iBox = frame.interactionBox();
  for( int h = 0; h < frame.hands().count(); h++ )
  {
      Hand hand = frame.hands().get(h);
      Vector handPosition = iBox.normalizePoint(hand.palmPosition());
      translate(handPosition.getX() * width, handPosition.getZ() * height);
      rotate(hand.direction().yaw());
      rect(-2,-40,4,80);
      
  }

}
