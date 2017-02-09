using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
//using Leap;

public class SerialPortConnector : MonoBehaviour {
	// Data:
	// timestamp unsigned long: 1 x 4 bytes
	// pose quaternion 4 floats: 4 x 4 bytes
	// pose euler angles 3 floats: 3 x 4 bytes
	// accel vector 3 floats: 3 x 4 bytes
	// compass vector 3 floats: 3 x 4 bytes
	// 56 bytes
	public const int serial_buffer_size = 56;

	public DataPacket IMUData = new DataPacket();

	SerialPort port;
	byte[] buffer = new byte[serial_buffer_size];
	int axisOrder = 0;

	void Awake(){
		//string[] ports = getPortNames ();
		//OpenPort (ports[ports.Length - 1]);
		port = new SerialPort();
	}

	public string OpenPort (string name) {
		try{
			port = new SerialPort(name);
			port.Open ();
			return "true";
		} catch (Exception e){
			Debug.Log(e);
			return e.Message;
		}
	}

	public void ClosePort(){
		port.Close();
	}

	// Update is called once per frame
	void Update () {


		if (Input.GetKeyUp("a")){
			if (axisOrder < 5){
				axisOrder++;
			} else {axisOrder = 0;} 
		}
		
		while(port.IsOpen && port.BytesToRead >= serial_buffer_size){
			port.Read(buffer, 0, serial_buffer_size);
			IMUData = parseData(buffer);
			//Debug.Log (IMUData);
		}
	}

	DataPacket parseData(byte[] data){
		int offset = 0;

		uint timestamp = BitConverter.ToUInt32(data, offset);
		offset += sizeof(UInt32);

		float qx = BitConverter.ToSingle(data, offset);
		offset += sizeof(float);
		float qy = BitConverter.ToSingle(data, offset);
		offset += sizeof(float);
		float qz = BitConverter.ToSingle(data, offset);
		offset += sizeof(float);
		float qw = BitConverter.ToSingle(data, offset);
		offset += sizeof(float);
		Quaternion quat = makeQuaternion(qx, qy, qz, qw);

		float ex = BitConverter.ToSingle(data, offset);
		offset += sizeof(float);
		float ey = BitConverter.ToSingle(data, offset);
		offset += sizeof(float);
		float ez = BitConverter.ToSingle(data, offset);
		offset += sizeof(float);
		Vector3 eulers = MakeUnityPoseVector(ex, ey, ez);

		float ax = BitConverter.ToSingle(data, offset);
		offset += sizeof(float);
		float ay = BitConverter.ToSingle(data, offset);
		offset += sizeof(float);
		float az = BitConverter.ToSingle(data, offset);
		Vector3 accel = MakeUnityAccelerationVector(ax, ay, az);

		float cx = BitConverter.ToSingle(data, offset);
		offset += sizeof(float);
		float cy = BitConverter.ToSingle(data, offset);
		offset += sizeof(float);
		float cz = BitConverter.ToSingle(data, offset);
		Vector3 compass = MakeUnityCompassVector(cx, cy, cz);

		return new DataPacket(timestamp, quat, eulers, accel, compass);
	}

	Quaternion makeQuaternion(float qx, float qy, float qz, float qw){
		Quaternion quat;
		switch(axisOrder){
			case 0:
				quat = new Quaternion(qx, qy, qz, qw);
				break;
			case 1:
				quat = new Quaternion(qx, qz, qy, qw);
				break;
			case 2:
				quat = new Quaternion(qy, qx, qz, qw);
				break;
			case 3:
				quat = new Quaternion(qy, qz, qx, qw);
				break;
			case 4:
				quat = new Quaternion(qz, qy, qx, qw);
				break;
			case 5:
				quat = new Quaternion(qz, qx, qy, qw);
				break;
			default:
				quat = Quaternion.identity;
				break;
		}
		return quat;
	}

	Vector3 MakeUnityPoseVector(float imu_x, float imu_y, float imu_z){
		return new Vector3(imu_x, imu_z, imu_y) * 180/Mathf.PI;
	}
	Vector3 MakeUnityAccelerationVector(float imu_x, float imu_y, float imu_z){
		return new Vector3(imu_x, imu_z, imu_y);
	}
	Vector3 MakeUnityCompassVector(float imu_x, float imu_y, float imu_z){
		return new Vector3(0, imu_x, 0);
	}

	public static string[] getPortNames ()
	{
		int platform = (int)Environment.OSVersion.Platform;
		//List<string> serial_ports = new List<string> ();

		string[] serial_ports;
		// Are we on Unix?
		if (platform == 4 || platform == 128 || platform == 6) {
			serial_ports = Directory.GetFiles ("/dev/", "tty.*");
			}
	    else {
			serial_ports = SerialPort.GetPortNames();
		}
		return serial_ports;
	}
}
