using UnityEngine;
using System.Collections;

public class YawFollower : MonoBehaviour {
	public SerialPortConnector imuConnection = null;
	bool start = false;
	public Vector3 restForce = Vector3.zero;
	public float lastTimestamp = 0;
	public Quaternion referencePose = Quaternion.identity;
	
	// Use this for initialization
	void Start () {
		//referencePose = transform.rotation;
	}
	
	void Update () {
		if(imuConnection){
			DataPacket imudata = imuConnection.IMUData;

			transform.localRotation = Quaternion.Euler(new Vector3(0, imudata.eulerAngles.y, 0)) * referencePose;
			
			if (Input.GetKeyUp("space")){
				//referencePose = Quaternion.LookRotation(Vector3.forward, Vector3.up) * Quaternion.Inverse(imudata.attitude);
				//restForce = imudata.acceleration;
			}
		}
	}
	
}
