using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GATTCodes
{
    private static Dictionary<string, string> names;
    private static Dictionary<string, string> ids;

    public static string GetName(string code)
    {
        if (String.IsNullOrEmpty(code))
            return "";
        
        if (names == null)
            Initialize();

        if(names.TryGetValue(code, out var name))
            return name;

        return code;
    }
    public static string GetID(string name)
    {
        if (String.IsNullOrEmpty(name))
            return "";
        
        if (ids == null)
            Initialize();

        if(ids.TryGetValue(name, out var id))
            return id;
        
        return name;
    }

    private static void Initialize()
    {
        names = new Dictionary<string, string>();
        names.Add("{00002900-0000-1000-8000-00805f9b34fb}", "Characteristic Extended Properties");
        names.Add("{00002901-0000-1000-8000-00805f9b34fb}", "Characteristic User Description");
        names.Add("{00002902-0000-1000-8000-00805f9b34fb}", "Client Characteristic Configuration");
        names.Add("{00002903-0000-1000-8000-00805f9b34fb}", "Server Characteristic Configuration");
        names.Add("{00002904-0000-1000-8000-00805f9b34fb}", "Characteristic Presentation Format");
        names.Add("{00002905-0000-1000-8000-00805f9b34fb}", "Characteristic Aggregate Format");
        names.Add("{00002906-0000-1000-8000-00805f9b34fb}", "Valid Range");
        names.Add("{00002907-0000-1000-8000-00805f9b34fb}", "External Report Reference");
        names.Add("{00002908-0000-1000-8000-00805f9b34fb}", "Report Reference");
        names.Add("{00002909-0000-1000-8000-00805f9b34fb}", "Number of Digitals");
        names.Add("{0000290a-0000-1000-8000-00805f9b34fb}", "Value Trigger Setting");
        names.Add("{0000290b-0000-1000-8000-00805f9b34fb}", "Environmental Sensing Configuration");
        names.Add("{0000290c-0000-1000-8000-00805f9b34fb}", "Environmental Sensing Measurement");
        names.Add("{0000290d-0000-1000-8000-00805f9b34fb}", "Environmental Sensing Trigger Setting");
        names.Add("{0000290e-0000-1000-8000-00805f9b34fb}", "Time Trigger Setting");
        names.Add("{00001800-0000-1000-8000-00805f9b34fb}", "Generic Access");
        names.Add("{00001801-0000-1000-8000-00805f9b34fb}", "Generic Attribute");
        names.Add("{00001802-0000-1000-8000-00805f9b34fb}", "Immediate Alert");
        names.Add("{00001803-0000-1000-8000-00805f9b34fb}", "Link Loss");
        names.Add("{00001804-0000-1000-8000-00805f9b34fb}", "Tx Power");
        names.Add("{00001805-0000-1000-8000-00805f9b34fb}", "Current Time Service");
        names.Add("{00001806-0000-1000-8000-00805f9b34fb}", "Reference Time Update Service");
        names.Add("{00001807-0000-1000-8000-00805f9b34fb}", "Next DST Change Service");
        names.Add("{00001808-0000-1000-8000-00805f9b34fb}", "Glucose");
        names.Add("{00001809-0000-1000-8000-00805f9b34fb}", "Health Thermometer");
        names.Add("{0000180a-0000-1000-8000-00805f9b34fb}", "Device Information");
        names.Add("{0000180d-0000-1000-8000-00805f9b34fb}", "Heart Rate");
        names.Add("{0000180e-0000-1000-8000-00805f9b34fb}", "Phone Alert Status Service");
        names.Add("{0000180f-0000-1000-8000-00805f9b34fb}", "Battery Service");
        names.Add("{00001810-0000-1000-8000-00805f9b34fb}", "Blood Pressure");
        names.Add("{00001811-0000-1000-8000-00805f9b34fb}", "Alert Notification Service");
        names.Add("{00001812-0000-1000-8000-00805f9b34fb}", "Human Interface Device");
        names.Add("{00001813-0000-1000-8000-00805f9b34fb}", "Scan Parameters");
        names.Add("{00001814-0000-1000-8000-00805f9b34fb}", "Running Speed and Cadence");
        names.Add("{00001815-0000-1000-8000-00805f9b34fb}", "Automation IO");
        names.Add("{00001816-0000-1000-8000-00805f9b34fb}", "Cycling Speed and Cadence");
        names.Add("{00001818-0000-1000-8000-00805f9b34fb}", "Cycling Power");
        names.Add("{00001819-0000-1000-8000-00805f9b34fb}", "Location and Navigation");
        names.Add("{0000181a-0000-1000-8000-00805f9b34fb}", "Environmental Sensing");
        names.Add("{0000181b-0000-1000-8000-00805f9b34fb}", "Body Composition");
        names.Add("{0000181c-0000-1000-8000-00805f9b34fb}", "User Data");
        names.Add("{0000181d-0000-1000-8000-00805f9b34fb}", "Weight Scale");
        names.Add("{0000181e-0000-1000-8000-00805f9b34fb}", "Bond Management Service");
        names.Add("{0000181f-0000-1000-8000-00805f9b34fb}", "Continuous Glucose Monitoring");
        names.Add("{00001820-0000-1000-8000-00805f9b34fb}", "Internet Protocol Support Service");
        names.Add("{00001821-0000-1000-8000-00805f9b34fb}", "Indoor Positioning");
        names.Add("{00001822-0000-1000-8000-00805f9b34fb}", "Pulse Oximeter Service");
        names.Add("{00001823-0000-1000-8000-00805f9b34fb}", "HTTP Proxy");
        names.Add("{00001824-0000-1000-8000-00805f9b34fb}", "Transport Discovery");
        names.Add("{00001825-0000-1000-8000-00805f9b34fb}", "Object Transfer Service");
        names.Add("{00001826-0000-1000-8000-00805f9b34fb}", "Fitness Machine");
        names.Add("{00001827-0000-1000-8000-00805f9b34fb}", "Mesh Provisioning Service");
        names.Add("{00001828-0000-1000-8000-00805f9b34fb}", "Mesh Proxy Service");
        names.Add("{00001829-0000-1000-8000-00805f9b34fb}", "Reconnection Configuration");
        names.Add("{00002a00-0000-1000-8000-00805f9b34fb}", "Device Name");
        names.Add("{00002a01-0000-1000-8000-00805f9b34fb}", "Appearance");
        names.Add("{00002a02-0000-1000-8000-00805f9b34fb}", "Peripheral Privacy Flag");
        names.Add("{00002a03-0000-1000-8000-00805f9b34fb}", "Reconnection Address");
        names.Add("{00002a04-0000-1000-8000-00805f9b34fb}", "Peripheral Preferred Connection Parameters");
        names.Add("{00002a05-0000-1000-8000-00805f9b34fb}", "Service Changed");
        names.Add("{00002a06-0000-1000-8000-00805f9b34fb}", "Alert Level");
        names.Add("{00002a07-0000-1000-8000-00805f9b34fb}", "Tx Power Level");
        names.Add("{00002a08-0000-1000-8000-00805f9b34fb}", "Date Time");
        names.Add("{00002a09-0000-1000-8000-00805f9b34fb}", "Day of Week");
        names.Add("{00002a0a-0000-1000-8000-00805f9b34fb}", "Day Date Time");
        names.Add("{00002a0b-0000-1000-8000-00805f9b34fb}", "Exact Time 100");
        names.Add("{00002a0c-0000-1000-8000-00805f9b34fb}", "Exact Time 256");
        names.Add("{00002a0d-0000-1000-8000-00805f9b34fb}", "DST Offset");
        names.Add("{00002a0e-0000-1000-8000-00805f9b34fb}", "Time Zone");
        names.Add("{00002a0f-0000-1000-8000-00805f9b34fb}", "Local Time Information");
        names.Add("{00002a10-0000-1000-8000-00805f9b34fb}", "Secondary Time Zone");
        names.Add("{00002a11-0000-1000-8000-00805f9b34fb}", "Time with DST");
        names.Add("{00002a12-0000-1000-8000-00805f9b34fb}", "Time Accuracy");
        names.Add("{00002a13-0000-1000-8000-00805f9b34fb}", "Time Source");
        names.Add("{00002a14-0000-1000-8000-00805f9b34fb}", "Reference Time Information");
        names.Add("{00002a15-0000-1000-8000-00805f9b34fb}", "Time Broadcast");
        names.Add("{00002a16-0000-1000-8000-00805f9b34fb}", "Time Update Control Point");
        names.Add("{00002a17-0000-1000-8000-00805f9b34fb}", "Time Update State");
        names.Add("{00002a18-0000-1000-8000-00805f9b34fb}", "Glucose Measurement");
        names.Add("{00002a19-0000-1000-8000-00805f9b34fb}", "Battery Level");
        names.Add("{00002a1a-0000-1000-8000-00805f9b34fb}", "Battery Power State");
        names.Add("{00002a1b-0000-1000-8000-00805f9b34fb}", "Battery Level State");
        names.Add("{00002a1c-0000-1000-8000-00805f9b34fb}", "Temperature Measurement");
        names.Add("{00002a1d-0000-1000-8000-00805f9b34fb}", "Temperature Type");
        names.Add("{00002a1e-0000-1000-8000-00805f9b34fb}", "Intermediate Temperature");
        names.Add("{00002a1f-0000-1000-8000-00805f9b34fb}", "Temperature Celsius");
        names.Add("{00002a20-0000-1000-8000-00805f9b34fb}", "Temperature Fahrenheit");
        names.Add("{00002a21-0000-1000-8000-00805f9b34fb}", "Measurement Interval");
        names.Add("{00002a22-0000-1000-8000-00805f9b34fb}", "Boot Keyboard Input Report");
        names.Add("{00002a23-0000-1000-8000-00805f9b34fb}", "System ID");
        names.Add("{00002a24-0000-1000-8000-00805f9b34fb}", "Model Number String");
        names.Add("{00002a25-0000-1000-8000-00805f9b34fb}", "Serial Number String");
        names.Add("{00002a26-0000-1000-8000-00805f9b34fb}", "Firmware Revision String");
        names.Add("{00002a27-0000-1000-8000-00805f9b34fb}", "Hardware Revision String");
        names.Add("{00002a28-0000-1000-8000-00805f9b34fb}", "Software Revision String");
        names.Add("{00002a29-0000-1000-8000-00805f9b34fb}", "Manufacturer Name String");
        names.Add("{00002a2a-0000-1000-8000-00805f9b34fb}", "IEEE 11073-20601 Regulatory Certification Data List");
        names.Add("{00002a2b-0000-1000-8000-00805f9b34fb}", "Current Time");
        names.Add("{00002a2c-0000-1000-8000-00805f9b34fb}", "Magnetic Declination");
        names.Add("{00002a2f-0000-1000-8000-00805f9b34fb}", "Position 2D");
        names.Add("{00002a30-0000-1000-8000-00805f9b34fb}", "Position 3D");
        names.Add("{00002a31-0000-1000-8000-00805f9b34fb}", "Scan Refresh");
        names.Add("{00002a32-0000-1000-8000-00805f9b34fb}", "Boot Keyboard Output Report");
        names.Add("{00002a33-0000-1000-8000-00805f9b34fb}", "Boot Mouse Input Report");
        names.Add("{00002a34-0000-1000-8000-00805f9b34fb}", "Glucose Measurement Context");
        names.Add("{00002a35-0000-1000-8000-00805f9b34fb}", "Blood Pressure Measurement");
        names.Add("{00002a36-0000-1000-8000-00805f9b34fb}", "Intermediate Cuff Pressure");
        names.Add("{00002a37-0000-1000-8000-00805f9b34fb}", "Heart Rate Measurement");
        names.Add("{00002a38-0000-1000-8000-00805f9b34fb}", "Body Sensor Location");
        names.Add("{00002a39-0000-1000-8000-00805f9b34fb}", "Heart Rate Control Point");
        names.Add("{00002a3a-0000-1000-8000-00805f9b34fb}", "Removable");
        names.Add("{00002a3b-0000-1000-8000-00805f9b34fb}", "Service Required");
        names.Add("{00002a3c-0000-1000-8000-00805f9b34fb}", "Scientific Temperature Celsius");
        names.Add("{00002a3d-0000-1000-8000-00805f9b34fb}", "String");
        names.Add("{00002a3e-0000-1000-8000-00805f9b34fb}", "Network Availability");
        names.Add("{00002a3f-0000-1000-8000-00805f9b34fb}", "Alert Status");
        names.Add("{00002a40-0000-1000-8000-00805f9b34fb}", "Ringer Control point");
        names.Add("{00002a41-0000-1000-8000-00805f9b34fb}", "Ringer Setting");
        names.Add("{00002a42-0000-1000-8000-00805f9b34fb}", "Alert Category ID Bit Mask");
        names.Add("{00002a43-0000-1000-8000-00805f9b34fb}", "Alert Category ID");
        names.Add("{00002a44-0000-1000-8000-00805f9b34fb}", "Alert Notification Control Point");
        names.Add("{00002a45-0000-1000-8000-00805f9b34fb}", "Unread Alert Status");
        names.Add("{00002a46-0000-1000-8000-00805f9b34fb}", "New Alert");
        names.Add("{00002a47-0000-1000-8000-00805f9b34fb}", "Supported New Alert Category");
        names.Add("{00002a48-0000-1000-8000-00805f9b34fb}", "Supported Unread Alert Category");
        names.Add("{00002a49-0000-1000-8000-00805f9b34fb}", "Blood Pressure Feature");
        names.Add("{00002a4a-0000-1000-8000-00805f9b34fb}", "HID Information");
        names.Add("{00002a4b-0000-1000-8000-00805f9b34fb}", "Report Map");
        names.Add("{00002a4c-0000-1000-8000-00805f9b34fb}", "HID Control Point");
        names.Add("{00002a4d-0000-1000-8000-00805f9b34fb}", "Report");
        names.Add("{00002a4e-0000-1000-8000-00805f9b34fb}", "Protocol Mode");
        names.Add("{00002a4f-0000-1000-8000-00805f9b34fb}", "Scan Interval Window");
        names.Add("{00002a50-0000-1000-8000-00805f9b34fb}", "PnP ID");
        names.Add("{00002a51-0000-1000-8000-00805f9b34fb}", "Glucose Feature");
        names.Add("{00002a52-0000-1000-8000-00805f9b34fb}", "Record Access Control Point");
        names.Add("{00002a53-0000-1000-8000-00805f9b34fb}", "RSC Measurement");
        names.Add("{00002a54-0000-1000-8000-00805f9b34fb}", "RSC Feature");
        names.Add("{00002a55-0000-1000-8000-00805f9b34fb}", "SC Control Point");
        names.Add("{00002a56-0000-1000-8000-00805f9b34fb}", "Digital");
        names.Add("{00002a57-0000-1000-8000-00805f9b34fb}", "Digital Output");
        names.Add("{00002a58-0000-1000-8000-00805f9b34fb}", "Analog");
        names.Add("{00002a59-0000-1000-8000-00805f9b34fb}", "Analog Output");
        names.Add("{00002a5a-0000-1000-8000-00805f9b34fb}", "Aggregate");
        names.Add("{00002a5b-0000-1000-8000-00805f9b34fb}", "CSC Measurement");
        names.Add("{00002a5c-0000-1000-8000-00805f9b34fb}", "CSC Feature");
        names.Add("{00002a5d-0000-1000-8000-00805f9b34fb}", "Sensor Location");
        names.Add("{00002a5e-0000-1000-8000-00805f9b34fb}", "PLX Spot-Check Measurement");
        names.Add("{00002a5f-0000-1000-8000-00805f9b34fb}", "PLX Continuous Measurement Characteristic");
        names.Add("{00002a60-0000-1000-8000-00805f9b34fb}", "PLX Features");
        names.Add("{00002a62-0000-1000-8000-00805f9b34fb}", "Pulse Oximetry Control Point");
        names.Add("{00002a63-0000-1000-8000-00805f9b34fb}", "Cycling Power Measurement");
        names.Add("{00002a64-0000-1000-8000-00805f9b34fb}", "Cycling Power Vector");
        names.Add("{00002a65-0000-1000-8000-00805f9b34fb}", "Cycling Power Feature");
        names.Add("{00002a66-0000-1000-8000-00805f9b34fb}", "Cycling Power Control Point");
        names.Add("{00002a67-0000-1000-8000-00805f9b34fb}", "Location and Speed Characteristic");
        names.Add("{00002a68-0000-1000-8000-00805f9b34fb}", "Navigation");
        names.Add("{00002a69-0000-1000-8000-00805f9b34fb}", "Position Quality");
        names.Add("{00002a6a-0000-1000-8000-00805f9b34fb}", "LN Feature");
        names.Add("{00002a6b-0000-1000-8000-00805f9b34fb}", "LN Control Point");
        names.Add("{00002a6c-0000-1000-8000-00805f9b34fb}", "Elevation");
        names.Add("{00002a6d-0000-1000-8000-00805f9b34fb}", "Pressure");
        names.Add("{00002a6e-0000-1000-8000-00805f9b34fb}", "Temperature");
        names.Add("{00002a6f-0000-1000-8000-00805f9b34fb}", "Humidity");
        names.Add("{00002a70-0000-1000-8000-00805f9b34fb}", "True Wind Speed");
        names.Add("{00002a71-0000-1000-8000-00805f9b34fb}", "True Wind Direction");
        names.Add("{00002a72-0000-1000-8000-00805f9b34fb}", "Apparent Wind Speed");
        names.Add("{00002a73-0000-1000-8000-00805f9b34fb}", "Apparent Wind Direction");
        names.Add("{00002a74-0000-1000-8000-00805f9b34fb}", "Gust Factor");
        names.Add("{00002a75-0000-1000-8000-00805f9b34fb}", "Pollen Concentration");
        names.Add("{00002a76-0000-1000-8000-00805f9b34fb}", "UV Index");
        names.Add("{00002a77-0000-1000-8000-00805f9b34fb}", "Irradiance");
        names.Add("{00002a78-0000-1000-8000-00805f9b34fb}", "Rainfall");
        names.Add("{00002a79-0000-1000-8000-00805f9b34fb}", "Wind Chill");
        names.Add("{00002a7a-0000-1000-8000-00805f9b34fb}", "Heat Index");
        names.Add("{00002a7b-0000-1000-8000-00805f9b34fb}", "Dew Point");
        names.Add("{00002a7d-0000-1000-8000-00805f9b34fb}", "Descriptor Value Changed");
        names.Add("{00002a7e-0000-1000-8000-00805f9b34fb}", "Aerobic Heart Rate Lower Limit");
        names.Add("{00002a7f-0000-1000-8000-00805f9b34fb}", "Aerobic Threshold");
        names.Add("{00002a80-0000-1000-8000-00805f9b34fb}", "Age");
        names.Add("{00002a81-0000-1000-8000-00805f9b34fb}", "Anaerobic Heart Rate Lower Limit");
        names.Add("{00002a82-0000-1000-8000-00805f9b34fb}", "Anaerobic Heart Rate Upper Limit");
        names.Add("{00002a83-0000-1000-8000-00805f9b34fb}", "Anaerobic Threshold");
        names.Add("{00002a84-0000-1000-8000-00805f9b34fb}", "Aerobic Heart Rate Upper Limit");
        names.Add("{00002a85-0000-1000-8000-00805f9b34fb}", "Date of Birth");
        names.Add("{00002a86-0000-1000-8000-00805f9b34fb}", "Date of Threshold Assessment");
        names.Add("{00002a87-0000-1000-8000-00805f9b34fb}", "Email Address");
        names.Add("{00002a88-0000-1000-8000-00805f9b34fb}", "Fat Burn Heart Rate Lower Limit");
        names.Add("{00002a89-0000-1000-8000-00805f9b34fb}", "Fat Burn Heart Rate Upper Limit");
        names.Add("{00002a8a-0000-1000-8000-00805f9b34fb}", "First Name");
        names.Add("{00002a8b-0000-1000-8000-00805f9b34fb}", "Five Zone Heart Rate Limits");
        names.Add("{00002a8c-0000-1000-8000-00805f9b34fb}", "Gender");
        names.Add("{00002a8d-0000-1000-8000-00805f9b34fb}", "Heart Rate Max");
        names.Add("{00002a8e-0000-1000-8000-00805f9b34fb}", "Height");
        names.Add("{00002a8f-0000-1000-8000-00805f9b34fb}", "Hip Circumference");
        names.Add("{00002a90-0000-1000-8000-00805f9b34fb}", "Last Name");
        names.Add("{00002a91-0000-1000-8000-00805f9b34fb}", "Maximum Recommended Heart Rate");
        names.Add("{00002a92-0000-1000-8000-00805f9b34fb}", "Resting Heart Rate");
        names.Add("{00002a93-0000-1000-8000-00805f9b34fb}", "Sport Type for Aerobic and Anaerobic Thresholds");
        names.Add("{00002a94-0000-1000-8000-00805f9b34fb}", "Three Zone Heart Rate Limits");
        names.Add("{00002a95-0000-1000-8000-00805f9b34fb}", "Two Zone Heart Rate Limit");
        names.Add("{00002a96-0000-1000-8000-00805f9b34fb}", "VO2 Max");
        names.Add("{00002a97-0000-1000-8000-00805f9b34fb}", "Waist Circumference");
        names.Add("{00002a98-0000-1000-8000-00805f9b34fb}", "Weight");
        names.Add("{00002a99-0000-1000-8000-00805f9b34fb}", "Database Change Increment");
        names.Add("{00002a9a-0000-1000-8000-00805f9b34fb}", "User Index");
        names.Add("{00002a9b-0000-1000-8000-00805f9b34fb}", "Body Composition Feature");
        names.Add("{00002a9c-0000-1000-8000-00805f9b34fb}", "Body Composition Measurement");
        names.Add("{00002a9d-0000-1000-8000-00805f9b34fb}", "Weight Measurement");
        names.Add("{00002a9e-0000-1000-8000-00805f9b34fb}", "Weight Scale Feature");
        names.Add("{00002a9f-0000-1000-8000-00805f9b34fb}", "User Control Point");
        names.Add("{00002aa0-0000-1000-8000-00805f9b34fb}", "Magnetic Flux Density - 2D");
        names.Add("{00002aa1-0000-1000-8000-00805f9b34fb}", "Magnetic Flux Density - 3D");
        names.Add("{00002aa2-0000-1000-8000-00805f9b34fb}", "Language");
        names.Add("{00002aa3-0000-1000-8000-00805f9b34fb}", "Barometric Pressure Trend");
        names.Add("{00002aa4-0000-1000-8000-00805f9b34fb}", "Bond Management Control Point");
        names.Add("{00002aa5-0000-1000-8000-00805f9b34fb}", "Bond Management Features");
        names.Add("{00002aa6-0000-1000-8000-00805f9b34fb}", "Central Address Resolution");
        names.Add("{00002aa7-0000-1000-8000-00805f9b34fb}", "CGM Measurement");
        names.Add("{00002aa8-0000-1000-8000-00805f9b34fb}", "CGM Feature");
        names.Add("{00002aa9-0000-1000-8000-00805f9b34fb}", "CGM Status");
        names.Add("{00002aaa-0000-1000-8000-00805f9b34fb}", "CGM Session Start Time");
        names.Add("{00002aab-0000-1000-8000-00805f9b34fb}", "CGM Session Run Time");
        names.Add("{00002aac-0000-1000-8000-00805f9b34fb}", "CGM Specific Ops Control Point");
        names.Add("{00002aad-0000-1000-8000-00805f9b34fb}", "Indoor Positioning Configuration");
        names.Add("{00002aae-0000-1000-8000-00805f9b34fb}", "Latitude");
        names.Add("{00002aaf-0000-1000-8000-00805f9b34fb}", "Longitude");
        names.Add("{00002ab0-0000-1000-8000-00805f9b34fb}", "Local North Coordinate");
        names.Add("{00002ab1-0000-1000-8000-00805f9b34fb}", "Local East Coordinate");
        names.Add("{00002ab2-0000-1000-8000-00805f9b34fb}", "Floor Number");
        names.Add("{00002ab3-0000-1000-8000-00805f9b34fb}", "Altitude");
        names.Add("{00002ab4-0000-1000-8000-00805f9b34fb}", "Uncertainty");
        names.Add("{00002ab5-0000-1000-8000-00805f9b34fb}", "Location Name");
        names.Add("{00002ab6-0000-1000-8000-00805f9b34fb}", "URI");
        names.Add("{00002ab7-0000-1000-8000-00805f9b34fb}", "HTTP Headers");
        names.Add("{00002ab8-0000-1000-8000-00805f9b34fb}", "HTTP Status Code");
        names.Add("{00002ab9-0000-1000-8000-00805f9b34fb}", "HTTP Entity Body");
        names.Add("{00002aba-0000-1000-8000-00805f9b34fb}", "HTTP Control Point");
        names.Add("{00002abb-0000-1000-8000-00805f9b34fb}", "HTTPS Security");
        names.Add("{00002abc-0000-1000-8000-00805f9b34fb}", "TDS Control Point");
        names.Add("{00002abd-0000-1000-8000-00805f9b34fb}", "OTS Feature");
        names.Add("{00002abe-0000-1000-8000-00805f9b34fb}", "Object Name");
        names.Add("{00002abf-0000-1000-8000-00805f9b34fb}", "Object Type");
        names.Add("{00002ac0-0000-1000-8000-00805f9b34fb}", "Object Size");
        names.Add("{00002ac1-0000-1000-8000-00805f9b34fb}", "Object First-Created");
        names.Add("{00002ac2-0000-1000-8000-00805f9b34fb}", "Object Last-Modified");
        names.Add("{00002ac3-0000-1000-8000-00805f9b34fb}", "Object ID");
        names.Add("{00002ac4-0000-1000-8000-00805f9b34fb}", "Object Properties");
        names.Add("{00002ac5-0000-1000-8000-00805f9b34fb}", "Object Action Control Point");
        names.Add("{00002ac6-0000-1000-8000-00805f9b34fb}", "Object List Control Point");
        names.Add("{00002ac7-0000-1000-8000-00805f9b34fb}", "Object List Filter");
        names.Add("{00002ac8-0000-1000-8000-00805f9b34fb}", "Object Changed");
        names.Add("{00002ac9-0000-1000-8000-00805f9b34fb}", "Resolvable Private Address Only");
        names.Add("{00002acc-0000-1000-8000-00805f9b34fb}", "Fitness Machine Feature");
        names.Add("{00002acd-0000-1000-8000-00805f9b34fb}", "Treadmill Data");
        names.Add("{00002ace-0000-1000-8000-00805f9b34fb}", "Cross Trainer Data");
        names.Add("{00002acf-0000-1000-8000-00805f9b34fb}", "Step Climber Data");
        names.Add("{00002ad0-0000-1000-8000-00805f9b34fb}", "Stair Climber Data");
        names.Add("{00002ad1-0000-1000-8000-00805f9b34fb}", "Rower Data");
        names.Add("{00002ad2-0000-1000-8000-00805f9b34fb}", "Indoor Bike Data");
        names.Add("{00002ad3-0000-1000-8000-00805f9b34fb}", "Training Status");
        names.Add("{00002ad4-0000-1000-8000-00805f9b34fb}", "Supported Speed Range");
        names.Add("{00002ad5-0000-1000-8000-00805f9b34fb}", "Supported Inclination Range");
        names.Add("{00002ad6-0000-1000-8000-00805f9b34fb}", "Supported Resistance Level Range");
        names.Add("{00002ad7-0000-1000-8000-00805f9b34fb}", "Supported Heart Rate Range");
        names.Add("{00002ad8-0000-1000-8000-00805f9b34fb}", "Supported Power Range");
        names.Add("{00002ad9-0000-1000-8000-00805f9b34fb}", "Fitness Machine Control Point");
        names.Add("{00002ada-0000-1000-8000-00805f9b34fb}", "Fitness Machine Status");
        names.Add("{00002aed-0000-1000-8000-00805f9b34fb}", "Date UTC");
        names.Add("{00002b1d-0000-1000-8000-00805f9b34fb}", "RC Feature");
        names.Add("{00002b1e-0000-1000-8000-00805f9b34fb}", "RC Settings");
        names.Add("{00002b1f-0000-1000-8000-00805f9b34fb}", "Reconnection Configuration Control Point");
        names.Add("{ce061800-43e5-11e4-916c-0800200c9a66}", "GAP Primary service");
        names.Add("{ce062A00-43e5-11e4-916c-0800200c9a66}", "GAP device name");
        names.Add("{ce062A01-43e5-11e4-916c-0800200c9a66}", "GAP appearance");
        names.Add("{ce062A02-43e5-11e4-916c-0800200c9a66}", "GAP Peripheral privacy");
        names.Add("{ce062A03-43e5-11e4-916c-0800200c9a66}", "GAP reconnect address");
        names.Add("{ce062A04-43e5-11e4-916c-0800200c9a66}", "Peripheral preffered connection parameters");
        names.Add("{ce061801-43e5-11e4-916c-0800200c9a66}", "GATT primary service");
        names.Add("{ce062A05-43e5-11e4-916c-0800200c9a66}", "Service changed");
        names.Add("{ce062902-43e5-11e4-916c-0800200c9a66}", "GATT client configuration");
        names.Add("{ce060010-43e5-11e4-916c-0800200c9a66}", "C2 device information service");
        names.Add("{ce060011-43e5-11e4-916c-0800200c9a66}", "C2 module number");
        names.Add("{ce060012-43e5-11e4-916c-0800200c9a66}", "C2 serial number");
        names.Add("{ce060013-43e5-11e4-916c-0800200c9a66}", "C2 harware revision");
        names.Add("{ce060014-43e5-11e4-916c-0800200c9a66}", "C2 firmware revision");
        names.Add("{ce060015-43e5-11e4-916c-0800200c9a66}", "C2 manufacture name");
        names.Add("{ce060016-43e5-11e4-916c-0800200c9a66}", "Erg machine type");
        names.Add("{ce060020-43e5-11e4-916c-0800200c9a66}", "PM control primary service");
        names.Add("{ce060021-43e5-11e4-916c-0800200c9a66}", "PM receive");
        names.Add("{ce060022-43e5-11e4-916c-0800200c9a66}", "PM transmit");
        names.Add("{ce060030-43e5-11e4-916c-0800200c9a66}", "C2 rowing primary service");
        names.Add("{ce060031-43e5-11e4-916c-0800200c9a66}", "C2 rowing general status");
        names.Add("{ce060032-43e5-11e4-916c-0800200c9a66}", "C2 rowing additional status 1");
        names.Add("{ce060033-43e5-11e4-916c-0800200c9a66}", "C2 rowing additional status 2");
        names.Add("{ce060034-43e5-11e4-916c-0800200c9a66}", "C2 rowing general status and additional status sample rate");
        names.Add("{ce060035-43e5-11e4-916c-0800200c9a66}", "C2 rowing stroke data");
        names.Add("{ce060036-43e5-11e4-916c-0800200c9a66}", "C2 rowing additional stroke data");
        names.Add("{ce060037-43e5-11e4-916c-0800200c9a66}", "C2 rowing split/interval data");
        names.Add("{ce060038-43e5-11e4-916c-0800200c9a66}", "C2 rowing additional split/interval data");
        names.Add("{ce060039-43e5-11e4-916c-0800200c9a66}", "C2 rowing end workout summary data");
        names.Add("{ce06003A-43e5-11e4-916c-0800200c9a66}", "C2 rowing end of workout additional summary data 1");
        names.Add("{ce06003B-43e5-11e4-916c-0800200c9a66}", "C2 rowing heart rate belt information");
        names.Add("{ce06003C-43e5-11e4-916c-0800200c9a66}", "C2 rowing end of workout additional summary data 2");
        names.Add("{ce06003D-43e5-11e4-916c-0800200c9a66}", "C2 force curve data");
        names.Add("{ce060080-43e5-11e4-916c-0800200c9a66}", "C2 multiplexed information");

        ids = new Dictionary<string, string>();
        foreach (var item in names)
        {
            ids.Add(item.Value, item.Key);
        }
    }
}
