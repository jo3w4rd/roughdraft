/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

// The finger model for our skeletal hand made out of various non-deforming models.
public class PerspectiveFinger : FingerModel {
	
	public Transform[] bones = new Transform[NUM_BONES];
	public Transform[] joints = new Transform[NUM_BONES - 1];
	
	Camera cam;

	public override void InitFinger() {
		cam = Camera.main;
		SetPositions();
	}
	
	public override void UpdateFinger() {
		SetPositions();
	}
	
	protected void SetPositions() {
		
		for (int i = 0; i < bones.Length; ++i) {
			if (bones[i] != null) {
				bones[i].transform.position = ProjectedPalmPosition() + bones[i].transform.localPosition;
				bones[i].transform.rotation = GetBoneRotation(i);
			}
		}
		
		for (int i = 0; i < joints.Length; ++i) {
			if (joints[i] != null) {
				joints[i].transform.position = GetJointPosition(i + 1) - controller_.transform.position + ProjectedPalmPosition();
				joints[i].transform.rotation = GetBoneRotation(i + 1);
			}
		}
	}

	public Vector3 AcquireBoneCenter(int bone_type) {
		Bone bone = finger_.Bone((Bone.BoneType)(bone_type));
		Leap.InteractionBox ibox = controller_.GetFrame().InteractionBox;
		Leap.Vector leapPosition = bone.Center;
		Leap.Vector normalizedPosition = ibox.NormalizePoint(leapPosition, true);
		Vector3 position = new Vector3(normalizedPosition.x, normalizedPosition.y, normalizedPosition.z);
		position.z = (cam.farClipPlane - cam.nearClipPlane) - position.z * (cam.farClipPlane - cam.nearClipPlane);
		
		return cam.ViewportToWorldPoint(position);

	}
	
	public Vector3 ProjectedPalmPosition() {	
		Leap.InteractionBox ibox = controller_.GetFrame().InteractionBox;
		Leap.Vector leapPalmPosition = hand_.PalmPosition;
		Leap.Vector normalizedPalmPosition = ibox.NormalizePoint(leapPalmPosition, true);
		Vector3 position = new Vector3(normalizedPalmPosition.x, normalizedPalmPosition.y, normalizedPalmPosition.z);
		position.z = (cam.farClipPlane - cam.nearClipPlane) - position.z * (cam.farClipPlane - cam.nearClipPlane);
		
		return cam.ViewportToWorldPoint(position);
	}
}
