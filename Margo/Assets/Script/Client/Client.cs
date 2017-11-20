using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour {

    public GameObject chatContainer;
    public GameObject messagePrefab;

    public string clientName;

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    private int messangeCnt=1;
    public void ConnectToServer()
    {
        //If alreay connected, ignore this function
        if (socketReady)
            return;
        // Default host / port values
        string host = "165.132.58.240";
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
        GameObject.Find("ChatWindow").GetComponent<Image>().enabled = true;
    }

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

            clientName = GameObject.Find("ClientName").GetComponent<InputField>().text;
            if (clientName != "")
                GameObject.Find("ConnectBtn").GetComponent<Button>().interactable = true;
        }
    }

    private void OnIncomingData(string data)
    {
        if(data == "%NAME")
        {
            Send("&NAME|" + clientName);

            return;
        }
        
        messangeCnt++;
        Debug.Log(messangeCnt);
        chatContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(chatContainer.GetComponent<RectTransform>().sizeDelta.x, messangeCnt*100+200);

        GameObject go = Instantiate(messagePrefab, chatContainer.transform) as GameObject;
        go.GetComponent<RectTransform>().localPosition = new Vector3(0, -(messangeCnt - 1) * 100, 0);
        if (data.Contains("&CHAT"))
        {
            if (data.Split('|')[0] == clientName)
            {
                go.transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
                go.GetComponent<RectTransform>().localPosition = new Vector3(120, -(messangeCnt - 1) * 100, 0);
                go.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(600, -(messangeCnt - 1) * 100, 0);
                go.transform.GetChild(2).GetComponent<RectTransform>().localPosition = new Vector3(600, -(messangeCnt - 1) * 100, 0);

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
    }
    private void Send(string data)
    {
        if (data == "")
            return;
        if (!socketReady)
            return;
        writer.WriteLine(data);
        writer.Flush();
    }

    public void OnSendButton()
    {
        string message = GameObject.Find("SendInput").GetComponent<InputField>().text;
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

    private void OnApplicationQuit()
    {
        CloseSocket();
    }
    private void OnDisable()
    {
        CloseSocket();
    }
}
