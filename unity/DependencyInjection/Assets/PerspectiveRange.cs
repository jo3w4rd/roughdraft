	using UnityEngine;
	using System.Collections;
	using Leap;
	
	public class PerspectiveRange : MonoBehaviour {
	
		public HandController handController;
		public bool useLeft = true;

		void Update () {
			if(handController.GetLeapController().Frame().Hands.Count > 0) 
			{
				this.transform.position = ProjectHandPosition();
				this.transform.rotation = Camera.main.transform.rotation;
			}
		}
	
		public Vector3 ProjectHandPosition() {
			Leap.Hand hand = GetMatchingLeapHand(handController.GetLeapController().Frame().Hands);
			Leap.InteractionBox ibox = handController.GetLeapController().Frame().InteractionBox;
			//Leap.Vector handCenter = center(handController.GetLeapController().Frame().Hands);
			Leap.Vector handCenter = hand.PalmPosition;
			Leap.Vector normalizedPosition = ibox.NormalizePoint(handCenter, true);
			Vector3 position = new Vector3(normalizedPosition.x, normalizedPosition.y, normalizedPosition.z) * 1.1f;

			if(hand.IsLeft) position.x += .1f;
			if(hand.IsRight) position.x -= .1f;
			//Set z as world position/depth
		position.z = Camera.main.nearClipPlane + (Camera.main.farClipPlane - Camera.main.nearClipPlane) * Mathf.Pow((1.0f - position.z), 6);
		
			return Camera.main.ViewportToWorldPoint(position);
		}

		Leap.Hand GetMatchingLeapHand(HandList hands){
			foreach(Hand hand in hands){
				if(useLeft && hand.IsLeft) return hand;
				if(!useLeft && hand.IsRight) return hand;
			}
			return new Hand();
		}

		Leap.Vector center(Leap.HandList hands){
			Leap.Vector center = new Leap.Vector(0, 0, 0);
			foreach( Hand hand in hands){
				center += hand.PalmPosition;
			}
			return center/hands.Count;
		}
	}
