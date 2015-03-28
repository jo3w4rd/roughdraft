using UnityEngine;
using System.Collections;
using Leap;

public class CollisionID : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Collision Trigger Event " + other);
		FingerModel finger = other.GetComponentInParent<FingerModel>();
		if(finger){
			Debug.Log ("Finger " + finger.fingerType);
		}
	}
}
