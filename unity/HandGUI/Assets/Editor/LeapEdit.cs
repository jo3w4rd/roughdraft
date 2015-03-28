using UnityEngine;
using UnityEditor;
using System.Collections;

public class LeapEdit : EditorWindow{
	[MenuItem ("Window/Leap")]
	
	public static void  ShowWindow () {
		EditorWindow.GetWindow(typeof(LeapEdit));
	}
	
	void OnGUI () {
		// The actual window code goes here
	}
}
