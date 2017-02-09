using UnityEngine;
using System.Collections;

public class IMUFollower : MonoBehaviour {
	public SerialPortConnector imuConnection = null;
	public Vector3 restForce = Vector3.zero;

	void Start () {
	}
	
	void Update () {
		if(imuConnection){
			DataPacket imudata = imuConnection.IMUData;
			Debug.Log (imudata);

			transform.localRotation = Quaternion.Euler(imudata.eulerAngles);
			
			if (Input.GetKeyUp("space")){
			}
		}
	}
}
