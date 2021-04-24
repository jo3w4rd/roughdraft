using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListItemSelectHandler : MonoBehaviour
{
    public void HandleEvent(string name)
    {
        Debug.Log("Selected " + name);
    }

}
