using UnityEngine;
using System.Collections;

public class ShowUI : MonoBehaviour {
	public PortUI portUI;

	void Start(){
		portUI.gameObject.SetActive(true);
		portUI.ShowWorld (false);
	}
	void Update () {
		if (Input.GetKeyDown("escape")) {
			portUI.gameObject.SetActive(true);
		}
	}
}
