
int simtime = 0;
int time;
int pwmPeriod = 40;
int pwmDutyCycle = 50;
int cycleCursor = 0;
float amplitude = 1;
int cycles = 0;

void setup(){
   size(300, 200); 
   translate(100,100); 
   ellipse(0, amplitude * 50, 1, 1);
   background(255); 

}

void draw(){
  runSim();
  
  if(time % pwmPeriod == 0) {
    cycleCursor = 0;
    cycles++;
  }
  float on = pwmPeriod * pwmDutyCycle / 100;
  if( on < cycleCursor ) {
     amplitude = 1.0;
  } else {
     amplitude = 0; 
  }
  
  //translate(1, 0);
  ellipse(time, amplitude * 50, 1, 1);
  cycleCursor++;
  if(cycles > 15){
     time = 0;
     cycles = 0;
     background(255); 
  }
  
  int millis(){
  
  }
  
  void runSim(){
    time++;
  }
  
}

