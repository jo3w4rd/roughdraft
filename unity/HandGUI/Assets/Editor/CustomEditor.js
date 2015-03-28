	@CustomEditor (Transform)
	class RotationHandleJS extends Editor {
		var controller = new Leap.Controller();
		var position;
		var localScale;
		var localRotation;
		var active = false;
		function OnSceneGUI () {
			e = Event.current;
         	switch (e.type) {
             	case EventType.KeyDown:
					position = target.transform.position;
					localScale = target.transform.localScale;
					localRotation = target.transform.localRotation;
					active = true;
					Debug.Log("editing");
                	break;
				case EventType.KeyUp:
					active = false;
					target.transform.position = position;
					target.transform.localScale = localScale;
					EditorUtility.SetDirty (target);
					break;
         	} 
			if(active){
				frame = controller.Frame();
				ten = controller.Frame(10);
				scale = frame.ScaleFactor(ten);
				translate = frame.Translation(ten);
				target.transform.localScale = localScale + new Vector3(scale, scale, scale);
				target.transform.position = position + new Vector3(translate.x, translate.y, translate.z);
				leapRot = frame.RotationMatrix(ten);
				quats = convertRotation(leapRot);
				target.transform.localRotation = quats;
	    	}
		}

    var LEAP_UP = new Leap.Vector(0, 1, 0);
    var LEAP_FORWARD = new Leap.Vector(0, 0, -1);
    var LEAP_ORIGIN = new Leap.Vector(0, 0, 0);

   function convertRotation(matrix:Leap.Matrix) {
      var up = matrix.TransformDirection(LEAP_UP);
      var forward = matrix.TransformDirection(LEAP_FORWARD);
	  //return Quaternion.identity;
      return Quaternion.LookRotation(new Vector3(forward.x, forward.y,forward.z), new Vector3(up.x, up.y, up.z));
    }
	}


//Set camera to back view:
//SceneView.lastActiveSceneView.LookAt(SceneView.lastActiveSceneView.pivot, Quaternion.LookRotation(Vector3.forward));