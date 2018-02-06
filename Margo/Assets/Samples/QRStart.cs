using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QRStart : MonoBehaviour {

    public Text qrdata;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        string ordermessage = "&MakeRoom|";
        ordermessage += qrdata.text;
        if(qrdata.text != "New Text")
        GameObject.Find("Server").GetComponent<Client>().MakeRoom(ordermessage);
    }
    public void QRstartbtn()
    {
        SceneManager.LoadScene("ContinuousDemo");
    }
}
