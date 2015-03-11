	using UnityEngine;
	using System.Collections;
	using Leap;
	
	public class CanvasPointer : MonoBehaviour {
		
		Controller leapController = new Controller();
		public bool rightHandControl = true;
		public GameObject pointer;
		public Canvas parentCanvas;
	
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
			
	        // Normalize the Leap coordinates
			Vector normalized = frame.InteractionBox.NormalizePoint(controlHand.Fingers[1].TipPosition, true);
			float xtran = normalized.x;
			float ytran = normalized.y;
			float ztran = parentCanvas.planeDistance * (1.5f - normalized.z); // Might want to change this to find the "best" z value or range
	
			// Normalized Leap/Viewport coordinates to world space
			pointer.transform.position = parentCanvas.worldCamera.ViewportToWorldPoint(new Vector3(xtran, ytran, ztran));
		}
	}
