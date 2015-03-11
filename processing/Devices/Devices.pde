import com.leapmotion.leap.*;

Controller leap;
int deviceCount = 0;

void setup()
{
    leap = new Controller();
}

void keyPressed()
{
 println("key");
 showDevices(false); 
}

void draw()
{
 showDevices(true); 
}

void showDevices(boolean onlyIfChanged)
{
   DeviceList devices = leap.devices();
  if( devices.count() != deviceCount || !onlyIfChanged )
  {
    println(devices.count() + " devices detected");
    deviceCount = devices.count();
    for( int d = 0; d < devices.count(); d++)
    {
      Device device = devices.get(d);
      println("Device: ");
      println("  HAoV: " + device.horizontalViewAngle() * 57.295779513f + " degrees");
      println("  VAoV: " + device.verticalViewAngle() * 57.295779513f + " degrees");
      println("  Range: " + device.range());
    }
  } 
}
