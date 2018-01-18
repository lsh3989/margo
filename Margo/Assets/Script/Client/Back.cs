using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Back : MonoBehaviour {

    // Use this for initialization
    public GameObject Main;
    public GameObject Chat;
    public int Chatroomorder;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Backbtn()
    {
        
        Main = GameObject.Find("Total").transform.GetChild(0).gameObject;
        Chat = GameObject.Find("Total").transform.GetChild(1).gameObject;


        gameObject.SetActive(false);
        Main.SetActive(true);
        Chat.SetActive(false);

    }
}
