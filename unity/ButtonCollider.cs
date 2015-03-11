using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonCollider : MonoBehaviour {

	public Button button;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		Debug.Log("Collision in collider *******************");
	}
}
