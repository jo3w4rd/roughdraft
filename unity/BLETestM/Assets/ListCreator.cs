using System;
using UnityEngine;
using UnityEngine.Events;

public class ListCreator : MonoBehaviour
{

    [SerializeField]
    private Transform SpawnPoint = null;

    [SerializeField]
    private GameObject item = null;

    [SerializeField]
    private RectTransform content = null;

    [SerializeField]
    private int itemHeight = 30;

    private int numberOfItems = 0;

    public StringEvent ConnectCommand;

    // Use this for initialization
    void Start()
    {
        if (ConnectCommand == null)
            ConnectCommand = new StringEvent();
    }

    public void AddItem(string label)
    {
        numberOfItems++;
        content.sizeDelta = new Vector2(0, numberOfItems * itemHeight);
        float spawnY = numberOfItems * itemHeight;
        Vector3 pos = new Vector3(SpawnPoint.position.x, -spawnY, SpawnPoint.position.z);
        GameObject SpawnedItem = Instantiate(item, pos, SpawnPoint.rotation);
        SpawnedItem.transform.SetParent(SpawnPoint, false);
        ItemDetails itemDetails = SpawnedItem.GetComponent<ItemDetails>();
        itemDetails.text.text = label;
        itemDetails.handler = this;
    }

    public void Clear()
    {
        for(int i = 0; i < SpawnPoint.childCount; i++)
        {
            Destroy(SpawnPoint.GetChild(i).gameObject);
        }
        numberOfItems = 0;
        content.sizeDelta = Vector2.zero;
    }

    public void HandleEvent(string deviceName)
    {
        Debug.Log("Selected " + deviceName);
        ConnectCommand.Invoke(deviceName);
    }

    private void AddTestItems()
    {
        for (int i = 0; i < 12; i++)
        {
            AddItem("Item " + i);
        }
    }
}

[Serializable]
public class StringEvent : UnityEvent<string> { }