using UnityEngine;
using System.Collections;
using Leap;

public class ArmAttacher : MonoBehaviour {

	Controller controller = new Controller();

	public HandController handcontroller = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(handcontroller){
			HandModel[] models = handcontroller.GetAllGraphicsHands();

			foreach(HandModel hand in models){
				Vector3 elbow = hand.GetElbowPosition();
				hand.transform.position = this.transform.position + hand.GetPalmPosition() + elbow;
				Debug.Log ("Update attachment " + this.transform.position);
				//hand.transform.position = new Vector3(0,0,0);
			}
		}
		
	}
	
	void LateUpdateFoo () {
		if(handcontroller){
				Hand left = findLeftHand ();
				if(left.IsValid){
					Arm arm = left.Arm;
					Vector elbow = arm.ElbowPosition;
					Vector3 unityElbow = elbow.ToUnityScaled();
					Vector3 scaled = new Vector3(unityElbow.x * handcontroller.transform.localScale.x,
				                                 unityElbow.y * handcontroller.transform.localScale.y,
				                                 unityElbow.z * handcontroller.transform.localScale.z);

				Vector palm = left.PalmPosition;
				Vector3 unityPalm = palm.ToUnityScaled();
				Vector3 scaledPalm = new Vector3(unityPalm.x * handcontroller.transform.localScale.x,
				                                 unityPalm.y * handcontroller.transform.localScale.y,
				                                 unityPalm.z * handcontroller.transform.localScale.z);
				
				handcontroller.transform.position = this.transform.position + new Vector3(0, 12, 0);// + scaled - scaledPalm;
				
				handcontroller.transform.Translate(-scaledPalm);
				handcontroller.transform.Translate(-scaled);
				Debug.Log ("Update attachment " + this.transform.position + ", " + handcontroller.transform.position + ", " + scaled + ", " + scaledPalm);
				}
		}
	
	}

	Hand findLeftHand(){
		Frame frame = controller.Frame ();
		foreach(Hand hand in frame.Hands){
			if(hand.IsLeft) return hand;
		}
		return Hand.Invalid;
	}
}
