
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System;
using System.Linq;
using UnityEngine.UI;

public class Wifi : MonoBehaviour {

    public float LastWifiChecktime;
    public GameObject wifiname;
    // Use this for initialization
    public String wifi;
    void Start () {

         wifi= "<unknown ssid>";
    }
    /*string GetSSID()
    {

        string ssid = "N/A";

        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        wifiname.GetComponent<Text>().text = "1st";
        var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi");
        wifiname.GetComponent<Text>().text = wifiManager.ToString();
        ssid = wifiManager.Call<AndroidJavaObject>("getConnectionInfo").Call<string>("getBSSID");
        wifiname.GetComponent<Text>().text = "3rd";
       //  using (var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
       //  {
       //      var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi");
       //      ssid = wifiManager.Call<AndroidJavaObject>("getConnectionInfo").Call<string>("getSSID");
       //  }
        return ssid;
    }*/
    public  string getSSID()
{
    string tempSSID = "";

    try
    {
          //  wifiname.GetComponent<Text>().text = "1";
        using (var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
              //  wifiname.GetComponent<Text>().text = "2";
            using (var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi"))
            {
                 //   wifiname.GetComponent<Text>().text = "3";
                    //wifiname.GetComponent<Text>().text = wifiManager.Call<AndroidJavaObject>("getConnectionInfo").ToString();
                    tempSSID = wifiManager.Call<AndroidJavaObject>("getConnectionInfo").Call<string>("getSSID");
                    tempSSID += "<";
                    tempSSID += wifiManager.Call<AndroidJavaObject>("getConnectionInfo").Call<string>("getBSSID");
                }
        }
    }
    catch (Exception e)
    {

    }
    return tempSSID;
}
// Update is called once per frame
void Update () {

        wifiname.GetComponent<Text>().text = wifi;
        
        if ((Time.fixedTime) - LastWifiChecktime >= 5)
        {
            
            LastWifiChecktime = Time.fixedTime;
           // if (wifi != getSSID())
           // {
                wifi = getSSID();
                Debug.Log(wifi);
                string ordermessage = "&MakeRoom|&wifi<";
                ordermessage += wifi;

                GameObject.Find("Server").GetComponent<Client>().MakeRoom(ordermessage);
           // }
            
            
         
        }
        

        }
    public void MakeRoomWifi()
    {
        
    }

}
/*
using System;
using UnityEngine;

using UnityEngine.UI;

// Assets/Plugins/Android에 AndroidManifest.xml을 별도로 생성하고 와이파이 상태 권한을 명시 해야함.
// <uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />

public class AndroidWifiInfo : MonoBehaviour
{
    public GameObject wifiname;
    // Use this for initialization
    public String wifi;

    [SerializeField] private float _frequencyTime = 1f;
    [SerializeField] private int _numberOfLevels = 4;

    private float _elapsedTime;

    public string NetworkName { private set; get; }
    public int SignalLevel { private set; get; }

#if UNITY_ANDROID 
    private void Start()
    {
        _elapsedTime = _frequencyTime;
    }

    private void Update()
    {
        if (_elapsedTime >= _frequencyTime)
        {
            _elapsedTime = 0f;

            NetworkName = GetNetworkName().Trim('\"');
            SignalLevel = GetSignalLevel(_numberOfLevels);
            wifiname.GetComponent<Text>().text = NetworkName;
        }

        _elapsedTime += Time.deltaTime;
    }
#endif


    private static string GetNetworkName()
    {
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            return "Cellular";
        }

        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {

            using (var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
            {
                var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi");
                return wifiManager.Call<AndroidJavaObject>("getConnectionInfo").Call<string>("getSSID");
            }
        }

        return "N/A";
    }

    private static int GetSignalLevel(int numberOfLevels)
    {
        using (var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
            var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi");
            var wifiInfo = wifiManager.Call<AndroidJavaObject>("getConnectionInfo");
            int rssi = wifiInfo.Call<int>("getRssi");
            return wifiManager.CallStatic<int>("calculateSignalLevel", rssi, numberOfLevels);
        }
    }
}*/