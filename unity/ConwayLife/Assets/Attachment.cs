using UnityEngine;
using System.Collections;

public class Attachment : ScriptableObject {
    const int xPlanarity = 0;
    const int yPlanarity = 1;
    const int zPlanarity = 2;

    public Vector3 Location = Vector3.zero;
    public GameObject attached = null;
    public bool active = true;
    public int planarity = 0;

    private SpringJoint joint = null;

    public Attachment(Vector3 position){
        Location = position;
    }

    public void Attach(Rigidbody rigidbody){
        joint = new SpringJoint ();
        joint.connectedBody = rigidbody;
        attached = rigidbody.gameObject;
    }

    public void Detach(){
        attached = null;
        joint = null;
    }
}
