using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;

public class ButtonDebugger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("Button Enabled");
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnPointerClick(){

		Debug.Log ("Pointer Click");
	}

	public void OnClick(float number){
		
		Debug.Log ("Normal Click");
	}
	
	public void OnCollisionEnter(Collision collision){
		Debug.Log ("Button collision");
	}
	void OnTriggerEnter(Collider other) {
		Debug.Log ("Button trigger");
	}
}
