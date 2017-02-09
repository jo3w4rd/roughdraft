using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {
    private int age = 0;
    public bool isLiving = true;
    public Vector3 target = Vector3.zero;
    public float speed = 100f;
    public Vector3 shrivelRate = new Vector3(1.02f, 1.07f, 1.03f);
    public int deathCycles = 900;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float dt = Time.deltaTime;

        transform.position -= (transform.position - target) * dt;
        if (Vector3.Distance (transform.position, target) < .1) {
            target = transform.position;
        }
        if (!isLiving) {
            transform.localScale = new Vector3( transform.localScale.x/shrivelRate.x,transform.localScale.y/shrivelRate.y,transform.localScale.z/shrivelRate.z);
            deathCycles--;
            Rigidbody body = transform.gameObject.AddComponent<Rigidbody>();
            body.useGravity = true;
        }
        if(deathCycles < 0) Object.Destroy (this.gameObject);

	}
}
