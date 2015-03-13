using UnityEngine;
using System.Collections;
using Leap;

public class CamSwitcher : MonoBehaviour {

	public Camera cam1;
	public Camera cam2;

	void Start () {
		cam1.enabled = true; 
		cam2.enabled = false; 
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.C)) {
			cam1.enabled = !cam1.enabled;
			cam2.enabled = !cam2.enabled;
			Bone bone = Bone.Invalid;
			Debug.Log (bone.IsValid);
		}
	}
}
