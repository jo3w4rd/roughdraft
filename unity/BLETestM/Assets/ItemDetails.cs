using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDetails : MonoBehaviour
{
    public Text text = null;
    public ListCreator handler = null;

    public void Select()
    {
        handler.HandleEvent(text.text);
    }
}