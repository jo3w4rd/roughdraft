using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArduinoBluetoothAPI;
using System;

public class BLEManager : MonoBehaviour
{
    BluetoothHelper bluetoothHelper;
    public ListCreator DeviceList;
    public ListCreator ServiceList;

    void Start()
    {
        BluetoothHelper.BLE = true;  //use Bluetooth Low Energy Technology
        bluetoothHelper = BluetoothHelper.GetInstance();
        bluetoothHelper.OnConnected += OnConnected;
        bluetoothHelper.OnConnectionFailed += OnConnectionFailed;
        bluetoothHelper.OnScanEnded += OnScanEnded;
        //bluetoothHelper
        DeviceList.ConnectCommand.AddListener(Connect);
    }

    private void OnConnectionFailed(BluetoothHelper helper)
    {
        Debug.Log("Connection Failed.");
    }

    public void Scan()
    {
        bluetoothHelper.ScanNearbyDevices();
        DeviceList.Clear();
    }

    public void Connect(string deviceName)
    {
        Debug.Log("Connect to device " + deviceName);
        bluetoothHelper.setDeviceName(deviceName);
        bluetoothHelper.Connect();
    }

    public void QueryServices()
    {
        bluetoothHelper.getGattServices();
        ServiceList.Clear();
    }

    private void OnScanEnded(BluetoothHelper helper, LinkedList<BluetoothDevice> nearbyDevices)
    {
        Debug.Log($"Scan found {nearbyDevices.Count} devices.");
        foreach(var device in nearbyDevices)
        {
            Debug.Log($"{device.DeviceName}: {device.DeviceAddress}: {device.Rssi}");
            DeviceList.AddItem(device.DeviceName);
        }
    }

    private void OnConnected(BluetoothHelper helper)
    {
        Debug.Log("Connected to " + helper.getBluetoothDevice().DeviceName);
        try
        {
            List<BluetoothHelperService> services = helper.getGattServices();
            foreach (BluetoothHelperService s in services)
            {
                Debug.Log("Service : " + s.getName());
                foreach (BluetoothHelperCharacteristic item in s.getCharacteristics())
                {
                    Debug.Log(item.getName());
                }
            }
            helper.StartListening();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
