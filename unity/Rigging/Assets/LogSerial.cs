using UnityEngine;
using System.Collections;
using Leap;

public class LogSerial : MonoBehaviour {
	Leap.Controller controller = new Controller();

	// Use this for initialization
	void Start () {
		Debug.Log (controller.Devices[0].SerialNumber);
		Debug.Log (controller.Devices[0].ToString ());	
	}
	
	// Update is called once per frame
	void Update () {

	}
}
