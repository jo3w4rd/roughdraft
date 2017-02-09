import com.leapmotion.leap.*;
//import java.util.ArrayList;

//Bone vertices
PVector A = new PVector(-1, 1, 0);
PVector B = new PVector(1, 1, 0);
PVector C = new PVector(1, -1, 0);
PVector D = new PVector(-1, -1, 0);
PVector E = new PVector(0, 0, 1);

ArrayList<BoneObject> skeleton = new ArrayList<BoneObject>();
Controller controller = new Controller();

float scale = 10;
float distance = 170;

void setup(){
  size(640, 360, P3D);
  float fov = PI/3.0;
  float cameraZ = (height/2.0) / tan(fov/2.0);
  perspective(fov, float(width)/float(height), 
            1.0, cameraZ*10.0);
}

void draw(){
  background(255);
  translate(320, 180, distance);
  mouseViewControl();

  Frame frame = controller.frame();
  if(frame.isValid()){
    Hand hand = frame.hands().frontmost();
    BoneObject palm = skeleton.get(0);
    palm.absoluteTransform = hand.basis();
    palm.relativeTransform = hand.basis();
    drawBone(palm, hand.palmWidth());
    for(Finger finger : hand.fingers()){
      for(Bone.Type boneType : Bone.Type.values()) {
        Bone bone = finger.bone(boneType);
        Matrix boneBasis = bone.basis();
        float lne = bone.length();
        //drawBone(new BoneObject(0, bone), lne );
      }
    }
  }

}

void drawBone(BoneObject bone, float length){
  pushMatrix();
    ApplyLeapMatrix(bone.absoluteTransform);
    drawBoneShape(length);
  popMatrix();
}

void drawBoneShape(float length){
  beginShape(TRIANGLES);
    vertex(A.x, A.y, A.z);
    vertex(E.x, E.y, E.z * length);
    vertex(D.x, D.y, D.z);
    
    vertex(A.x, A.y, A.z);
    vertex(E.x, E.y, E.z * length);
    vertex(B.x, B.y, B.z);

    vertex(A.x, A.y, A.z);
    vertex(D.x, D.y, D.z);
    vertex(B.x, B.y, B.z);

    vertex(C.x, C.y, C.z);
    vertex(E.x, E.y, E.z * length);
    vertex(B.x, B.y, B.z);

    vertex(C.x, C.y, C.z);
    vertex(D.x, D.y, D.z);
    vertex(E.x, E.y, E.z * length);

    vertex(C.x, C.y, C.z);
    vertex(D.x, D.y, D.z);
    vertex(B.x, B.y, B.z);
  endShape();
}

void keyPressed() {
  Frame frame = controller.frame();
  setBindPose(frame);
}

void setBindPose(Frame frame){
  if(frame.isValid()){
    Hand hand = frame.hands().frontmost();
    BoneObject palm = skeleton.get(0);
    palm.absoluteTransform = hand.basis();
    palm.relativeTransform = hand.basis();
    //root
    skeleton.add(new BoneObject(-1, hand.basis(), hand.palmPosition(), hand.basis(), hand.palmPosition()));
    for(int f = 0; f < 5; f++){
      Finger finger = hand.fingers().get(f);
      Bone prevBone = finger.bone(Bone.Type.TYPE_METACARPAL);
      skeleton.add(new BoneObject(0, hand.basis(), hand.palmPosition(), prevBone.basis(), prevBone.prevJoint()));
      int parentIndex = skeleton.size();
      for( int b = 1; b < 4; b++){
        Bone bone = finger.bone(intToBoneType(b));
        skeleton.add(new BoneObject(parentIndex + b - 1, prevBone.basis(), prevBone.prevJoint(), bone.basis(), bone.prevJoint()));
      }
    }
  }
}

void updateBones(Hand hand){

}

void mouseViewControl(){
    //scale(scale);
    rotateX(map(mouseX, 0, width, 2 * PI, 0));
    rotateY(map(mouseY, 0, height, 2 * PI, 0));
}
void mouseWheel(MouseEvent event) {
  float e = event.getCount();
  scale += e;
  distance += e * 3;
}
