using UnityEngine;
using System.Collections;
using Leap;

public class PerspectivePositionOverride : MonoBehaviour {
	
	void Update () {
		this.transform.position = ProjectHandPosition();
	}
	
	public Vector3 ProjectHandPosition() {
		Leap.Controller controller = new Controller();
		Leap.Hand hand = GetComponent<HandModel>().GetLeapHand();
		Leap.InteractionBox ibox = controller.Frame().InteractionBox;
		Leap.Vector handCenter = hand.PalmPosition;
		Leap.Vector normalizedPosition = ibox.NormalizePoint(handCenter, true);
		Vector3 position = new Vector3(normalizedPosition.x, normalizedPosition.y, normalizedPosition.z);
		position.z = (Camera.main.farClipPlane - Camera.main.nearClipPlane) * (1 - position.z);
		
		return Camera.main.ViewportToWorldPoint(position);
	}
}
