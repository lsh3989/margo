using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendMessage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void sendbuttonon()
    {
        string message = gameObject.GetComponent<InputField>().text;
        GameObject.Find("Server").GetComponent<Client>().OnSendButton(message);
    }
}
