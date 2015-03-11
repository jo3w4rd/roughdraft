using UnityEngine;
using System.Collections;
using Leap;

public class HorizontalAxis : MonoBehaviour {

	Controller leapController = new Controller();
	public bool rightHandControl = true;
 
	// Update is called once per frame
	void Update () {
		Frame frame = leapController.Frame ();

		//Find first hand of control type
		Hand controlHand = Hand.Invalid;
		foreach(Hand hand in frame.Hands){
			if ((hand.IsRight && rightHandControl) || (hand.IsLeft && !rightHandControl)){
				controlHand = hand;
				break;
			} 
		}

		//Normalize Leap coordinate to range of -1 to 1 using the InteractionBox
		Vector normalized = frame.InteractionBox.NormalizePoint(controlHand.PalmPosition, true) * 2;
		float horizontal = normalized.x - 1;
		Debug.Log ("Horizontal control: " + horizontal);
		//Your movement commands ...
	}
}
