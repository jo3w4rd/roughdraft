using UnityEngine;
using System.Collections;
using Leap.Unity;

[ExecuteInEditMode]
public class LeapBoneVisualizer : MonoBehaviour {
 private HandModel model;
  Color xBasisColor = new Color(.8f, 0f, 0f, 1f);
  Color yBasisColor = new Color(0f, 0.8f, 0f, 1f);
  Color zBasisColor = new Color(0f, 0f, 0.8f, 1f);
  Color boneColor = new Color(0.8f, 0f, 0.8f, 1f);

//palm
//wristJoint
//elbowJoint
//forearm
//fingers array
  //bones array
    void OnDrawGizmos()
    {
      if(model == null)
        model = gameObject.GetComponent<HandModel>();

      Draw(null, model.palm, Color.white);
      if(model.forearm != null)
        Draw(model.wristJoint, model.forearm, Color.white);
      foreach(FingerModel finger in model.fingers){
        Draw(model.wristJoint, finger.bones[0], boneColor);
        for(int b = 1; b < 4; b++)
          Draw(finger.bones[b-1], finger.bones[b], boneColor);
      }
    }

  public void Draw(Transform parent, Transform self, Color color){
    Gizmos.color = boneColor;
    if(self != null){
      if(parent != null)
        Gizmos.DrawLine(self.position, parent.position);
      //Gizmos.DrawCube(self.position, new Vector3(.01f, .01f, .01f));
      DrawBasis(self.position, self.forward, self.up, self.right, .01f);
    }
 }
    public void DrawBasis(Vector3 position, Vector3 forward, Vector3 up, Vector3 right, float scale){
      Vector3 origin = position;
      Debug.DrawLine(origin, origin + right * scale, xBasisColor);
      Debug.DrawLine(origin, origin + up * scale, yBasisColor);
      Debug.DrawLine(origin, origin + forward * scale, zBasisColor);
    }

}
