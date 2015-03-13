using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;

public class ButtonDebugger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("Click");
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnPointerClick(){

		Debug.Log ("Click");
	}

	public void OnClick(float number){
		
		Debug.Log ("Click");
	}
	
}
