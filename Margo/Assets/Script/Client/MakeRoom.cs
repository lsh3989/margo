using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeRoom : MonoBehaviour {
    public string RoomName;
    public GameObject Room;
    public string ordermessage;
    // Use this for initialization
    void Start()
    {
       //Room.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = MenuName[i];
        
    }
    public void MakeRoombtn()
    {
        ordermessage += "&MakeRoom|";
        RoomName = Room.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<Text>().text;
        ordermessage += RoomName;

        Debug.Log(ordermessage);
        GameObject.Find("Server").GetComponent<Client>().MakeRoom(ordermessage);
        //   Menu.transform.parent.parent.GetComponent<PanelDestroy>().destroy();
        //   GameObject.Destroy(Menu.transform.parent);
        Room.transform.parent.GetComponent<RectTransform>().position = new Vector3(7000, 0, 0);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
