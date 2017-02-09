using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PortUI : MonoBehaviour {

	string[] portNames;
	int current = 0;
	public InputField portDisplay = null;
	public Button openButton = null;
	public Button closeButton = null;
	public Text status = null;
	public GameObject[] enableList;

	public SerialPortConnector serialPort;

	void Start () {
		Refresh ();
	}

	public void OpenPort(){
		if(portNames.Length < 1)
			return;

		string message = serialPort.OpenPort(portNames[current]);

		if(message == "true"){
			Debug.Log ("It worked");
			gameObject.SetActive(false);
			openButton.interactable = false;
			closeButton.interactable = true;
			message = "Connection successful";
			ShowWorld(true);
		} 
		status.text = message;
	}

	public void ClosePort(){
		serialPort.ClosePort();
		openButton.interactable = true;
		closeButton.interactable = false;
		status.text = "";
	}

	public void Refresh(){
		portNames = SerialPortConnector.getPortNames();
		current = portNames.Length - 1;
		SetDisplayText(current);
	}

	void SetDisplayText(int index){
		if (index < 0 || index >= portNames.Length)
			return;
		Debug.Log ("Port: " + portNames[index] + " at " + current);
		portDisplay.text = portNames[index];
		current = index;
	}

	public void Next(){
		SetDisplayText(current + 1);
	}

	public void Previous(){
		SetDisplayText(current - 1);
	}

	public void ShowWorld(bool isShown) {
		foreach(GameObject obj in enableList){
			obj.SetActive(isShown);
		}
	}

}
