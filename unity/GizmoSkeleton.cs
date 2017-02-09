using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;
 
[ExecuteInEditMode]
public class GizmoSkeleton : MonoBehaviour 
{
  Color xBasisColor = new Color(.8f, 0f, 0f, 1f);
  Color yBasisColor = new Color(0f, 0.8f, 0f, 1f);
  Color zBasisColor = new Color(0f, 0f, 0.8f, 1f);
  Color boneColor = new Color(0.8f, 0f, 0.8f, 1f);

  public Transform root;
  public List<Transform> children = new List<Transform>();
 
  void Awake(){
    if (root != null) {
          children.Clear();
          children.AddRange(root.GetComponentsInChildren<Transform>());
    }
  }
     void OnDrawGizmos()
     {
             foreach (Transform child in children)
             {
                 
                 if (child == root)
                 {
                     Gizmos.color = boneColor;
                     //Gizmos.DrawCube(child.position, new Vector3(.01f, .01f, .01f));
                     DrawBasis(root.position, root.forward, root.up, root.right, .01f);
                 }
                 else
                 {
                     Gizmos.color = boneColor;
                     Gizmos.DrawLine(child.position, child.parent.position);
                     //Gizmos.DrawCube(child.position, new Vector3(.01f, .01f, .01f));
                     DrawBasis(child.position, child.forward, child.up, child.right, .01f);

                 }
             }
     }
 
    public void DrawBasis(Vector3 position, Vector3 forward, Vector3 up, Vector3 right, float scale){
      Vector3 origin = position;
      Debug.DrawLine(origin, origin + right * scale, xBasisColor);
      Debug.DrawLine(origin, origin + up * scale, yBasisColor);
      Debug.DrawLine(origin, origin + forward * scale, zBasisColor);
    }
 }