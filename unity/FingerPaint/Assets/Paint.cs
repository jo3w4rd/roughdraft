using UnityEngine;
using System.Collections;

public class Paint : MonoBehaviour {
	RenderTexture tex;

	// Use this for initialization
	void Start () {
		tex = gameObject.GetComponent<RenderTexture>();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
