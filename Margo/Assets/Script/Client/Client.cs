using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour {

    public static GameObject chatContainer;
    public GameObject messagePrefab;
    public GameObject friendprefab;
    public GameObject chatprefab;
    public GameObject mode;
    public string clientName;
    public List<Chatingroom> clientChatingroom;
    public int clientid;
    public int serverid;
    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    private int[] messangeCnt= { 1,1,1,1,1,1,1,1,1,1,1,1};
    public GameObject scenemanager;
    public XMLManager XMLManager;
    public GPS GPS;
    public itemDisplay itemDisplay;

    private void Start()
    {
        itemDisplay = scenemanager.GetComponent<itemDisplay>();
        serverid = 0;
    }
    public void ConnectToServer()
    {
        XMLManager = scenemanager.GetComponent<XMLManager>();
        GPS = scenemanager.GetComponent<GPS>();
        XMLManager.userDB.username = clientName;
        //If alreay connected, ignore this function
        if (socketReady)
            return;
        // Default host / port values
        string host = "165.132.58.240";         //연구실
      //string host = "192.168.0.2";              //집

        int port = 6321;
        
        
        //overwrite default host / port values, if there is something in those boxes
        /* default로 처리   
          string h;
               int p;

               h = GameObject.Find("HostInput").GetComponent<InputField>().text;
               if (h != "")
                   host = h;
               int.TryParse(GameObject.Find("PortInput").GetComponent<InputField>().text, out p);
               if (p != 0)
                   port = p;
                   */

        // Create the socket
        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;
        }
        catch(Exception e)
        {
            Debug.Log("Socket error :" + e.Message);
        }
        Destroy(GameObject.Find("Login"));
        //  GameObject.Find("ChatWindow").GetComponent<Image>().enabled = true;

        GameObject.Find("Canvas").transform.GetChild(1).gameObject.SetActive(true);
  //      XMLManager.Save();
    }
    public Chatroom item;
    private void Update()
    {
    
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                string data = reader.ReadLine();
                if (data != null)
                    OnIncomingData(data);
            }
        }
        else
        {
            if (GameObject.Find("ClientName"))
            {
                clientName = GameObject.Find("ClientName").GetComponent<InputField>().text;
                if (clientName != "")
                    GameObject.Find("ConnectBtn").GetComponent<Button>().interactable = true;
            }
        }
    }

    private void OnIncomingData(string data)
    {

        
        Debug.Log(data + "Onincomingdata in client");
        int serveridx;
        int index = new int();
        serveridx = int.Parse(data.Split('|')[data.Split('|').Length - 1]);
        if (data.Contains("&Enter"))
        {
            int roomid = int.Parse(data.Split('|')[2]);
            for (int i = 0; i < XMLManager.clienttotalroomcnt; i++)
                if (XMLManager.totalroom[i].serveridx == roomid)
                    return;
            itemDisplay.makeroom(data.Split('|')[1]); ;
            XMLManager.makeroom(data.Split('|')[1], roomid);
            return;
        }
            if (data.Contains("&MakeRoom"))
        {
            itemDisplay.makeroom(data.Split('|')[1]);
            XMLManager.makeroom(data.Split('|')[1], int.Parse(data.Split('|')[2]));
            return;
        }
        if(data.Contains("&Pricerequest"))
        {
            int totalprice, peapleNum;
            totalprice = int.Parse( data.Split('|')[1]);
            peapleNum = int.Parse(data.Split('|')[2]);
            int childcount;
            childcount = GameObject.Find("ChatCanvas(Clone)").transform.childCount;
            GameObject.Find("ChatCanvas(Clone)").transform.GetChild(childcount-1). GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "총금액 : "+totalprice.ToString() + "원";
            GameObject.Find("ChatCanvas(Clone)").transform.GetChild(childcount - 1).GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = "총인원 : "+peapleNum.ToString()+"명";
            return;
        }
        if(data.Contains( "&Gps"))
        {
            Debug.Log("gps from server in client");

            XMLManager = scenemanager.GetComponent<XMLManager>();
                       

            for(int i = 0; i < 3; i++)
            {
                XMLManager.userDB.chatlist[i].chatroomname = data.Split('|')[i*3+1] + "    거리 : " + data.Split('|')[i * 3 + 2] + "미터";
                XMLManager.userDB.chatlist[i].serveridx = int.Parse(data.Split('|')[i * 3 + 3]);
            }
            
            Debug.Log("@@@@@@@@@@@@@5" + XMLManager.userDB.chatlist[0].chatroomname+ XMLManager.userDB.chatlist[1].chatroomname + XMLManager.userDB.chatlist[2].chatroomname);
            Debug.Log("gps from server in client check1");
            mode.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = XMLManager.userDB.chatlist[0].chatroomname;
            mode.transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = XMLManager.userDB.chatlist[1].chatroomname;
            mode.transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = XMLManager.userDB.chatlist[2].chatroomname;

            return;
           
        }
        GameObject tmpchatcontainer = chatContainer;
        for (int i = 0; i < XMLManager.userDB.chatlist.Count; i++) {
            if (XMLManager.userDB.chatlist[i].serveridx == serveridx)
            {
                index = i;
            }
                }
             chatContainer = GameObject.Find("Total").transform.GetChild(1).GetChild(index).GetChild(1).GetChild(0).gameObject;
        if (data == "%NAME")
        {
            Send("&NAME|" + clientName);

            return;
        }
        
        messangeCnt[clientid]++;
        chatContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(chatContainer.GetComponent<RectTransform>().sizeDelta.x, messangeCnt[clientid] * 100+200);

        GameObject go = Instantiate(messagePrefab, chatContainer.transform) as GameObject;
        go.GetComponent<RectTransform>().localPosition = new Vector3(0, -(messangeCnt[clientid] - 1) * 100, 0);
        if (data.Contains("&CHAT"))
        {
            if (data.Split('|')[0] == clientName)
            {
                go.transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
                go.GetComponent<RectTransform>().localPosition = new Vector3(120, -(messangeCnt[clientid] - 1) * 100, 0);
                go.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(-600, -(messangeCnt[clientid] - 1) * 100, 0);
                go.transform.GetChild(2).GetComponent<RectTransform>().localPosition = new Vector3(-600, -(messangeCnt[clientid] - 1) * 100, 0);

            }
            go.transform.GetChild(1).GetComponent<Text>().text = data.Split('|')[0];
            data = data.Split('|')[2];
        }
        else if(data.Contains("&MASTER"))
        {
            go.transform.GetChild(0).GetComponent<Image>().color = Color.green;
            go.transform.GetChild(1).GetComponent<Text>().text = "Master";
            data = data.Split('|')[1];
        }
        go.transform.GetChild(0).GetComponentInChildren<Text>().text = data;


        chatContainer = tmpchatcontainer;
    }
    private void Send(string data)
    {
       data += ("|"+serverid.ToString());
        if (data == "")
            return;
        if (!socketReady)
            return;
        Debug.Log(data + "in send3");
        writer.WriteLine(data);
        writer.Flush();
    }
    public void OnSendButton(string message)
    {
        
        Send(message);
        GameObject.Find("ChatScrollbar").GetComponent<Scrollbar>().value = 0;
        GameObject.Find("SendInput").GetComponent<InputField>().text = "";
    }
    private void CloseSocket()
    {
        if (!socketReady)
            return;
        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }
    public void order(string data)
    {
        Debug.Log(data + "order in client");
        Send(data);
    }
    public void MakeRoom(string data)
    {
        serverid = 0;
        Debug.Log(data + "MakeRoom in client");
        data += "|" + GPS.latitude.ToString() + "|" + GPS.longitude.ToString();
        Send(data);
    }
    public void pricerequest()
    {
        Debug.Log("&Pricerequest!!");
        Send("&Pricerequest");
    }
    public void payment(string data)
    {
        Debug.Log(data + "payment in client");
        Send(data);
    }
    public void location(string data)
    {
        Debug.Log(data + "location in client");
        Send(data);
    }
    public void Enterroom(int idx)
    {
        clientid = idx;
        serverid = XMLManager.userDB.chatlist[idx].serveridx;
        Debug.Log(serverid + "serverid in enterroom in client");
        Send("&Enter|"+clientName+"|");
    }
    private void OnApplicationQuit()
    {
        CloseSocket();
    }
    private void OnDisable()
    {
        CloseSocket();
    }
}
