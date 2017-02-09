using UnityEngine;
using System.Collections;

public class PitchFollower : MonoBehaviour {
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
			Debug.Log (imudata);
			//Debug.Log(transform.rotation);
			
			transform.localRotation = Quaternion.Euler(new Vector3(imudata.eulerAngles.x, 0, 0)) * referencePose;
			
			if (Input.GetKeyUp("space")){
				//referencePose = Quaternion.LookRotation(Vector3.forward, Vector3.up) * Quaternion.Inverse(imudata.attitude);
				//restForce = imudata.acceleration;
			}
		}
	}

}
