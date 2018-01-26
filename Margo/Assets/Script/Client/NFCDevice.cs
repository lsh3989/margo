using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class NFCDevice : MonoBehaviour
{

    public string tagID;
    public Text tag_output_text;
    public bool tagFound = false;

    private AndroidJavaObject mActivity;
    private AndroidJavaObject mIntent;
    private string sAction;

    public static string ByteArrayToString(byte[] ba)
    {
        StringBuilder hex = new StringBuilder(ba.Length * 2);
        foreach (byte b in ba)
            hex.AppendFormat("{0:x2}", b);
        return hex.ToString();
    }


    void Start()
    {
        tag_output_text.text = "No tag...";
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!tagFound)
            {
                try
                {
                    // Create new NFC Android object
                    mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");//UnityPlayer
                    mIntent = mActivity.Call<AndroidJavaObject>("getIntent");
                    sAction = mIntent.Call<String>("getAction");
                    if (sAction == "android.nfc.action.NDEF_DISCOVERED")
                    {
                        Debug.Log("Tag of type NDEF");
                    }
                    else if (sAction == "android.nfc.action.TECH_DISCOVERED")
                    {
                        Debug.Log("TAG DISCOVERED");
                        // Get ID of tag
                        AndroidJavaObject mNdefMessage = mIntent.Call<AndroidJavaObject>("getParcelableExtra", "android.nfc.extra.TAG");
                        if (mNdefMessage != null)
                        {
                            byte[] payLoad = mNdefMessage.Call<byte[]>("getId");
                            Array.Reverse(payLoad);
                            string text = ByteArrayToString(payLoad);
                            //System.Convert.ToBase64String(payLoad);
                            tag_output_text.text = text;
                            //make room
                            string ordermessage = "&MakeRoom|";
                            ordermessage += tag_output_text.text;                            

                            GameObject.Find("Server").GetComponent<Client>().MakeRoom(ordermessage);


                            tagID = text;
                        }
                        else
                        {
                            tag_output_text.text = "No ID found !";
                        }
                        tagFound = true;
                        return;
                    }
                    else if (sAction == "android.nfc.action.TAG_DISCOVERED")
                    {
                        Debug.Log("This type of tag is not supported !");
                    }
                    else
                    {
                        tag_output_text.text = "No NFC ID...";
                        return;
                    }
                }
                catch (Exception ex)
                {
                    string text = ex.Message;
                    tag_output_text.text = text;
                }
            }
        }
    }
}
