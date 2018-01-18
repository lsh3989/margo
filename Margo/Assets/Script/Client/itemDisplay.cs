using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemDisplay : MonoBehaviour {
    public GameObject chatprefab;
    public GameObject friendprefab;
    public GameObject mode;
	// Use this for initialization
	void Start () {
		//DisplayFriend();
        //Displaychatroom();
        

    }
    public void DisplayFriend()
    {
        foreach (Friend item in XMLManager.ins.userDB.Frlist)
        {
            Debug.Log("@@@@");
            GameObject newfriend = Instantiate(friendprefab) as GameObject;
            newfriend.transform.SetParent(mode.transform.GetChild(0).transform.GetChild(0).transform, false);
            newfriend.transform.GetChild(0).GetComponent<Text>().text = item.name;
        }
    }
    public void Displaychatroom()
    {
        foreach (Chatroom item in XMLManager.ins.userDB.chatlist)
        {
            GameObject newchatroom = Instantiate(chatprefab) as GameObject;
            newchatroom.transform.SetParent(mode.transform.GetChild(1).transform.GetChild(0).transform, false);
            newchatroom.transform.GetChild(0).GetComponent<Text>().text = item.chatroomname;
            if (item.chatroomname.Contains("&Gps"))
                newchatroom.GetComponent<Image>().color = Color.yellow;
        }
    }
    public void makeroom(string roomname)
    {
            GameObject newchatroom = Instantiate(chatprefab) as GameObject;
            newchatroom.transform.SetParent(mode.transform.GetChild(1).transform.GetChild(0).transform, false);
            newchatroom.transform.GetChild(0).GetComponent<Text>().text = roomname;
           
        
    }
    // Update is called once per frame
    void Update () {
		
	}
}
