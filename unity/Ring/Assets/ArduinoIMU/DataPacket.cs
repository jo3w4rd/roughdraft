using UnityEngine;

// Data:
// timestamp unsigned long: 4 bytes
// pose quaternion 4 floats: 4 x 4 bytes
// pose euler angles 3 floats: 3 x 4 bytes
// accel vector 3 floats: 3 x 4 bytes

public class DataPacket
{
	public readonly uint timestamp = 0; //milliseconds
	public readonly Quaternion attitude = new Quaternion();
	public readonly Vector3 eulerAngles = new Vector3();
	public readonly Vector3 acceleration = new Vector3();
	public readonly Vector3 compass = new Vector3();

	public DataPacket(){}
	public DataPacket(uint timestamp, Quaternion attitude, Vector3 eulers, Vector3 acceleration, Vector3 compass)
	{
		this.timestamp = timestamp;;
		this.attitude = attitude;
		this.eulerAngles = eulers;
		this.acceleration = acceleration;
		this.compass = compass;
	}

	public override string ToString(){
		return "Time: " + timestamp + "ms, " +
			"Q: (" + attitude.x + "," + attitude.y + "," + attitude.z + "," + attitude.w + "), " + 
			"E: (" + eulerAngles.x + "," + eulerAngles.y + "," + eulerAngles.z + ")" +
			"A: (" + acceleration.x + "," + acceleration.y + "," + acceleration.z + ")" +
			"C: (" + compass.x + "," + compass.y + "," + compass.z + ")";
	}
}