using UnityEngine;
using System.Collections;
using Leap;

public class GestureCounts : MonoBehaviour {

	Frame lastFrame = new Frame();
	Controller controller;

	// Use this for initialization
	void Start () {
		controller = new Controller();
		controller.EnableGesture (Gesture.GestureType.TYPE_CIRCLE);
		controller.EnableGesture (Gesture.GestureType.TYPE_SWIPE);
		controller.EnableGesture (Gesture.GestureType.TYPE_KEY_TAP);
		controller.EnableGesture (Gesture.GestureType.TYPE_SCREEN_TAP);
		
	}
	
	// Update is called once per frame
	void Update () {
		Frame newFrame = controller.Frame();
		int now = newFrame.Gestures().Count;
		int sinceSelf = newFrame.Gestures(newFrame).Count;
		int sinceLast = newFrame.Gestures(lastFrame).Count;

		Debug.Log ("(" + now + ", " + sinceSelf + ", " + sinceLast + ") Fid: " + newFrame.Id + " : " + lastFrame.Id);
		lastFrame = newFrame;
	}
}
