public class LeapListener extends Listener{
   int changeCount = 0; 
   public void  onDeviceChange (Controller controller){
      System.out.println("Device changed " + ++changeCount);        
  } 
}
