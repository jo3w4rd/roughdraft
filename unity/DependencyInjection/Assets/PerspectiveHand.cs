using UnityEngine;
using System.Collections;
using Leap;

// The model for our skeletal hand made out of various polyhedra.
public class PerspectiveHand : HandModel {
	
	protected const float PALM_CENTER_OFFSET = 0.0150f;
	
	public GameObject palm;
	public GameObject forearm;
	public GameObject wristJoint;

	Camera cam;
	Vector3 nCenter; //center of window at closest rendered point.
	float zNear, zFar, zRange;
	
	Matrix4x4 projection;
	
	void Start() {
		// Ignore collisions with self.
		Leap.Utils.IgnoreCollisions(gameObject, gameObject);
		cam = Camera.main;
	}
	
	public override void InitHand() {
		cam = Camera.main;
		SetPositions();
	}
	
	public override void UpdateHand() {
		SetPositions();
	}
	
	public Vector3 GetPalmCenter() {
		Vector3 offset = PALM_CENTER_OFFSET * Vector3.Scale(GetPalmDirection(), transform.localScale);
		return AcquirePalmPosition();// - offset;
	}
	
	protected void SetPositions() {
		for (int f = 0; f < fingers.Length; ++f) {
			if (fingers[f] != null)
				fingers[f].InitFinger();
		}
		
		if (palm != null) {
			palm.transform.position = GetPalmCenter();
			palm.transform.rotation = GetPalmRotation();
		}
		
		if (wristJoint != null) {
			wristJoint.transform.position = GetWristPosition();
			wristJoint.transform.rotation = GetPalmRotation();
		}
		
		if (forearm != null) {
			forearm.transform.position = GetArmCenter();
			forearm.transform.rotation = GetArmRotation();
		}
	}

	
	public Vector3 AcquirePalmPosition() {
		
		Leap.InteractionBox ibox = controller_.GetFrame().InteractionBox;
		Leap.Vector leapPalmPosition = hand_.PalmPosition;
		Leap.Vector normalizedPalmPosition = ibox.NormalizePoint(leapPalmPosition, true);
		Vector3 position = new Vector3(normalizedPalmPosition.x, normalizedPalmPosition.y, normalizedPalmPosition.z);
		position.z = (cam.farClipPlane - cam.nearClipPlane) - position.z * (cam.farClipPlane - cam.nearClipPlane);
		
		return cam.ViewportToWorldPoint(position);
	}


}
