using UnityEditor;
using UnityEngine;
using System.Collections;
using Leap;

//[CustomEditor(typeof(GameObject))]
public class CustomGameObjectTransformEditor : Editor {
	Controller controller = new Controller();
	public override void OnInspectorGUI(){
		//if(GUI.changed) EditorUtility.SetDirty();
	}

	public void OnSceneGUI(){
		Frame frame = controller.Frame ();
		
	}
}
