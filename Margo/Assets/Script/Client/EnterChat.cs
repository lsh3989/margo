using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterChat : MonoBehaviour {

    // Use this for initialization
    public GameObject Main;
    public GameObject Chat;
    //public Client client;
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void Enterchatbtn()
    {
        
        int index = transform.GetSiblingIndex();
        Debug.Log(index + "in enterchatbtn");
        Main = GameObject.Find("Total").transform.GetChild(0).gameObject;
        Chat = GameObject.Find("Total").transform.GetChild(1).gameObject;

        Main.SetActive(false);
        Chat.SetActive(true);
        Chat.transform.GetChild(index).gameObject.SetActive(true);

        
        Client.chatContainer = GameObject.Find("Total").transform.GetChild(1).GetChild(index).GetChild(1).GetChild(0).gameObject;

        GameObject.Find("Server").GetComponent<Client>().Enterroom(index);
    }
}
