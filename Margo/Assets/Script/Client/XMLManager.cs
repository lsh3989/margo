using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Net.NetworkInformation;
using System.Net.Sockets;


public class XMLManager : MonoBehaviour {
    
   
    public static XMLManager ins;
    public GameObject scenemanager;
    public GameObject ChatCanvas;
    public GameObject Chat;

    public itemDisplay itemDisplay;
    public Chatroom item1, item2, item3;
    public Chatroom[] totalroom = new Chatroom[10];
    public int clienttotalroomcnt; //전체방개수
    void Awake()
    {
        ins = this;
    }

    public Userdatabase userDB = new Userdatabase();

    //save
    public void Save()
    {
        
        Debug.Log("SAVE");
        //open a new xmL file
        XmlSerializer serializer = new XmlSerializer(typeof(Userdatabase));

        FileStream stream = new FileStream(Application.dataPath + "/item_data.xml", FileMode.Create);
        ///수정
        StreamWriter sw = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(sw, userDB);
   //     serializer.Serialize(stream, userDB);
        stream.Close();
    }
    private void Start()
    {
    itemDisplay = scenemanager.GetComponent<itemDisplay>();
        //Save();
        Load();
        itemDisplay.DisplayFriend();
        itemDisplay.Displaychatroom();
      //  clienttotalroomcnt = 0;
    }


    //Load
    public void Load()
    {
 //       Debug.Log("LOAD");
 //       XmlSerializer serializer = new XmlSerializer(typeof(Userdatabase));
        //   FileStream stream = new FileStream(Application.dataPath + "/item_data.xml", FileMode.OpenOrCreate);
        //임시로 항상 생성
 //       FileStream stream = new FileStream(Application.dataPath + "/item_data.xml", FileMode.Create);


  //      Debug.Log("length"+stream.Length);

 //       if (stream.Length > 0)
  //      {
 //           Debug.Log("stream.Length = " + stream.Length);
 //           userDB = serializer.Deserialize(stream) as Userdatabase;
 //           GameObject.Find("Login").SetActive(false);
 //       }
 //       else
 //       {
            userDB = new Userdatabase();


            //기본으로 3개세팅
            item1.chatroomname = "&Gps";
            item2.chatroomname = "&Gps";
            item3.chatroomname = "&Gps";

            totalroom[clienttotalroomcnt].chatroomname = "전체방";
            totalroom[clienttotalroomcnt++].serveridx = 3;

        Debug.Log(clienttotalroomcnt + "clienttotalroomcnt in makeroom2");

        userDB.chatlist.Add(item1);
            userDB.chatlist.Add(item2);
            userDB.chatlist.Add(item3);
            userDB.chatlist.Add(totalroom[0]);
            GameObject newchatroom = Instantiate(ChatCanvas) as GameObject;
            newchatroom.transform.SetParent(Chat.transform, false);
            newchatroom.SetActive(false);
            newchatroom = Instantiate(ChatCanvas) as GameObject;
            newchatroom.transform.SetParent(Chat.transform, false);
            newchatroom.SetActive(false);
            newchatroom = Instantiate(ChatCanvas) as GameObject;
            newchatroom.transform.SetParent(Chat.transform, false);
            newchatroom.SetActive(false);
            newchatroom = Instantiate(ChatCanvas) as GameObject;
            newchatroom.transform.SetParent(Chat.transform, false);
            newchatroom.SetActive(false);

        
            
            GameObject.Find("ScroolRects").SetActive(false);
    //    }
   //     stream.Close();
    }
    public void makeroom(string roomname, int roomserveridx)
    {
        Debug.Log(clienttotalroomcnt + "clienttotalroomcnt in makeroom");
        Debug.Log(roomserveridx + "roomserveridx in makeroom");
        
        userDB.chatlist.Add(totalroom[clienttotalroomcnt]);
        totalroom[clienttotalroomcnt++].serveridx = roomserveridx;
        GameObject newchatroom = Instantiate(ChatCanvas) as GameObject;
        //newchatroom = Instantiate(ChatCanvas) as GameObject;
        newchatroom.transform.SetParent(Chat.transform, false);
        newchatroom.SetActive(false);
        
    }



}
[System.Serializable]
public class Friend
{
    public string name;
}

[System.Serializable]
public class Chatroom
{
    public string chatroomname;
    public int talknum;
    public string[] talks;
    public int serveridx;
}

[System.Serializable]
public class Userdatabase
{
    
    public string username;
    public int FriendsNum;
    public int ChatNum;
   // [XmlArray("CombatEqupment")]
    public List<Friend> Frlist = new List<Friend>();
    public List<Chatroom> chatlist = new List<Chatroom>();
}

