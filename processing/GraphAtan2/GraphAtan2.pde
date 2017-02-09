

void setup(){
  size(640, 640);
  //translate(0, 0, -600);
  for(float i = 0; i < 2 * PI; i+=PI/320){
    for(float j = 0; j < 2 * PI; j+=PI/320){
     float tan = atan2(j, i);
     fill(color(160 * tan, 0, 0));
     point(60 * i + 320, 60 * j + 320); 
     //println(tan);
    }
  }
}


