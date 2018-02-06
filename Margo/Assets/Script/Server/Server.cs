using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server : MonoBehaviour {

    public List<ServerClient> clients;
    public List<ServerClient> disconnectList;
    public List<Chatingroom> chatingroom;
    public int port = 6321;
    public int roomcnt;
    private TcpListener server;
    private bool serverStarted;
    private void Start()
    {
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            StartListening();
            serverStarted = true;
            Debug.Log("Server has been started on port" + port.ToString());
        }
        catch(Exception e)
        {
            Debug.Log("Socket error: " + e.Message);
        }


        chatingroom = new List<Chatingroom>();
        chatingroom.Add(new Chatingroom());
        chatingroom[0].roomid = roomcnt++;
        chatingroom[0].ServerroomName = "제1공학관";
        chatingroom[0].ClientroomName = "제1공학관";
        chatingroom[0].latitude = (float)37.561845;
        chatingroom[0].longitude = (float)126.936167;
        chatingroom[0].clients = new List<ServerClient>();
        chatingroom.Add(new Chatingroom());
        chatingroom[1].roomid = roomcnt++;
        chatingroom[1].ServerroomName = "중앙도서관";
        chatingroom[1].ClientroomName = "중앙도서관";
        chatingroom[1].latitude = (float)37.563703;
        chatingroom[1].longitude = (float)126.936911;

        chatingroom[1].clients = new List<ServerClient>();
        chatingroom.Add(new Chatingroom());
        chatingroom[2].roomid = roomcnt++;
        chatingroom[2].ServerroomName = "공학원";
        chatingroom[2].ClientroomName = "공학원";
        chatingroom[2].latitude = (float)37.560837;
        chatingroom[2].longitude = (float)126.9354687;

        chatingroom[2].clients = new List<ServerClient>();

        chatingroom.Add(new Chatingroom());
        chatingroom[3].roomid = roomcnt++;
        chatingroom[3].ServerroomName = "전체방";
        chatingroom[3].clients = new List<ServerClient>();
        // chatingroom[3].latitude = (float)37.560837;
        //  chatingroom[3].longitude = (float)126.9354687;

    }

    private void Update()
    {
        if (!serverStarted)
            return;

        foreach(ServerClient c in clients)
        {
            // Is the client still connected?

            if(!IsConnected(c.tcp))
            {
                c.tcp.Close();
                disconnectList.Add(c);
                continue;
            }
            // Check for message from the client
            else
            {
                NetworkStream s = c.tcp.GetStream();
                if(s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();

                    if (data != null)
                        OnIncomingData(c, data);
                }
            }
        }
        for(int i=0;i < disconnectList.Count-1;i++)
        {
       //     Broadcast(disconnectList[i].clientName + " has disconnected", clients);

            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
        }
    }

    private void StartListening()
    {
        server.BeginAcceptTcpClient(AccepTcpClient, server);
    }

    private bool IsConnected(TcpClient c)
    {
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                {
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

                }
                return true;
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }
    private void AccepTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;

        clients.Add(new ServerClient(listener.EndAcceptTcpClient(ar)));
        StartListening();

        // Send a message to everyone, say someone has connected

        //Broadcast("%NAME", new List<ServerClient>() { clients[clients.Count-1]});
    }

    private void OnIncomingData (ServerClient c, string data)
    {
        Debug.Log(data + "in onincoingdata in server");
        int serveridx;
        serveridx = int.Parse(data.Split('|')[data.Split('|').Length-1]);
        
        if(data.Contains("&MakeRoom"))
            {
            string roomname;
            string ssid="", bssid="";

            string broadmessage;
            roomname = data.Split('|')[1];
            if(roomname.Contains("&wifi"))
            {
                ssid = roomname.Split('<')[1];
                bssid = roomname.Split('<')[2];
                roomname = bssid; //BSSID;
            }
            for(int i = 0;i<roomcnt;i++)
            {
                if(chatingroom[i].ServerroomName == roomname)
                {
                    if (chatingroom[i].clients.Contains(c) == false)
                        chatingroom[i].clients.Add(c);
                    broadmessage = "Enter|" + chatingroom[serveridx].ClientroomName + '|' + chatingroom[serveridx].roomid;
                    return;
                }
            }
            Debug.Log(roomcnt + "roomcnt in server");
       //     chatingroom = new List<Chatingroom>();
            chatingroom.Add(new Chatingroom());
            chatingroom[roomcnt].roomid = roomcnt;
            if (bssid == "")
            {
                chatingroom[roomcnt].ServerroomName = data.Split('|')[1];
                chatingroom[roomcnt].ClientroomName = data.Split('|')[1];
            }
            else
            {
                chatingroom[roomcnt].ServerroomName = bssid;
                chatingroom[roomcnt].ClientroomName = ssid;

            }
            chatingroom[roomcnt].latitude = float.Parse(data.Split('|')[2]);
            chatingroom[roomcnt].longitude = float.Parse(data.Split('|')[3]);
            chatingroom[roomcnt].clients = new List<ServerClient>();
            broadmessage = "&MakeRoom|" + chatingroom[roomcnt].ClientroomName+"|"+ roomcnt.ToString();
            Makeroom(broadmessage, c);

            roomcnt++;
            return;
        }
        if (data.Contains("&Payment"))
        {
            string broadmessage;
            broadmessage = "&MASTER|";
            if(chatingroom[serveridx].totalprice == 0)
            {
                broadmessage += "결제하실 내역이 없습니다.";
                Broadcast(broadmessage, chatingroom[serveridx].clients, serveridx);
                return;
            }
            if (data.Split('|')[1] == "1")
            {
               broadmessage += "각자 " + (chatingroom[serveridx].totalprice / chatingroom[serveridx].clients.Count).ToString() +"원 입니다.";
               Broadcast(broadmessage, chatingroom[serveridx].clients, serveridx);
            }
            else if(data.Split('|')[1] == "2")
            {
               for(int i=0;i< chatingroom[serveridx].clients.Count; i++)
                {
                    if(chatingroom[serveridx].clients[i].orderprice != 0)
                    broadmessage += chatingroom[serveridx].clients[i].clientName + "님 " + chatingroom[serveridx].clients[i].orderprice + "원 ";
                }
                broadmessage += "입니다.";
                Broadcast(broadmessage, chatingroom[serveridx].clients, serveridx);
            }
            else
            {
                broadmessage += c.clientName.ToString() + "님" + chatingroom[serveridx].totalprice.ToString() + "원 입니다.";
                Broadcast(broadmessage, chatingroom[serveridx].clients, serveridx);
            }
            for (int i = 0; i < chatingroom[serveridx].clients.Count; i++)//계산후 초기화
                chatingroom[serveridx].clients[i].orderprice = 0;
           
                chatingroom[serveridx].totalprice = 0; //계산후 초기화
            return;
        }
        if (data.Contains("&Pricerequest"))
        {
           
            Broadcast("&Pricerequest|" + chatingroom[serveridx].totalprice.ToString() + "|" + chatingroom[serveridx].clients.Count, chatingroom[serveridx].clients, serveridx);
            return;
        }
        if (data.Contains("&Enter"))
        {
            c.clientName = data.Split('|')[1];
            if (chatingroom[serveridx].clients.Contains(c) == false)
                chatingroom[serveridx].clients.Add(c);
           // string broadmessage;
           // broadmessage = "Enter|" + chatingroom[serveridx].ClientroomName + '|' + chatingroom[serveridx].roomid;
           // Broadcast(broadmessage, chatingroom[serveridx].clients, serveridx);
            return;
        }
        if (data.Contains("&Gps"))
        {
            Debug.Log("Gps in server");
            float latitude, longitude;
            latitude = float.Parse(data.Split('|')[1]);
            longitude = float.Parse(data.Split('|')[2]);
            LocationRoom(latitude, longitude, c);
            return;
        }
        if (data.Contains("&NAME"))
        {
            c.clientName = data.Split('|')[1];
            Broadcast("&MASTER|"+c.clientName + " has connected!", chatingroom[serveridx].clients, serveridx);
            return;
        }
        if(data.Contains("&Order"))
        {
            int orderprice;
            orderprice = int.Parse(data.Split('|')[2]);
            data = data.Split('|')[1];
            c.orderprice += orderprice;
            chatingroom[serveridx].totalprice += orderprice;
            Broadcast("&MASTER|" + c.clientName+"님이 "+data + "주문하셨습니다.", chatingroom[serveridx].clients, serveridx);
            return;
        }
        if (data.Contains("&SERVICE"))
        {
            Broadcast( data.Split('|')[1], chatingroom[serveridx].clients, serveridx);
        }
        Debug.Log(data + "Broadcast in oncomingdata in server");
        foreach (ServerClient cc in chatingroom[serveridx].clients)
        {
            try
            {
                Debug.Log(cc.clientName + "Clientname Broadcast in server");
                
            }
            catch (Exception e)
            {
                
            }
        }
        Broadcast(c.clientName + "|&CHAT|" + data, chatingroom[serveridx].clients, serveridx);
    }
    private void Broadcast(string data, List<ServerClient> cl, int serveridx)
    {
        data += "|" + serveridx.ToString();
        
        foreach (ServerClient c in cl)
        {
            try
            {
                Debug.Log(data + "Broadcast in server");
                StreamWriter writer = new StreamWriter(c.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch (Exception e)
            {
                Debug.Log("Write error : " + e.Message + " to client " + c.clientName);
            }
        }
    }
    public struct Mindistance
    {
        public float distance;
        public int idx;
    }
    public void LocationRoom(float latitude, float longitude, ServerClient c)
    {
        Debug.Log("LocationRoom in server");
        string data;
        Mindistance[] min = new Mindistance[3];
        
        double distance;
        min[0].distance = min[1].distance = min[2].distance = 9999999999;
        data = "&Gps";
        Debug.Log("chatingroom.Count" + chatingroom.Count);
        for (int i=0;i<chatingroom.Count;i++)
        {
            if (chatingroom[i].latitude == 0 && chatingroom[i].longitude == 0) continue;
            distance = DistanceTo((double)latitude, (double)longitude, (double)chatingroom[i].latitude, (double)chatingroom[i].longitude);
            
            for (int j=0;j<3;j++)
            {
                if(distance < min[j].distance)
                {
                    for (int k = 2; k >= j+1; k--)
                        min[k] = min[k - 1];
                    min[j].distance = (float)distance;
                    min[j].idx = i;
                    break;
                }
            }
        }
        //Array.Sort(min);
        data += "|";
        data += (chatingroom[min[0].idx].ClientroomName + "|" + min[0].distance.ToString() + "|" + min[0].idx.ToString() + "|");
        data += (chatingroom[min[1].idx].ClientroomName + "|" + min[1].distance.ToString() + "|" + min[1].idx.ToString() + "|");
        data += (chatingroom[min[2].idx].ClientroomName + "|" + min[2].distance.ToString() + "|" + min[2].idx.ToString());
        try
        {
            StreamWriter writer = new StreamWriter(c.tcp.GetStream());
            writer.WriteLine(data);
            writer.Flush();
        }
        catch (Exception e)
        {
            Debug.Log("Write error : " + e.Message + " to client " + c.clientName);
        }
    }
    public void Makeroom(string data, ServerClient c)
    {
        try
        {
            StreamWriter writer = new StreamWriter(c.tcp.GetStream());
            writer.WriteLine(data);
            writer.Flush();
        }
        catch (Exception e)
        {
            Debug.Log("Write error : " + e.Message + " to client " + c.clientName);
        }
    }
    public static double DistanceTo(double lat1, double lon1, double lat2, double lon2)
    {
        double rlat1 = Math.PI * lat1 / 180;
        double rlat2 = Math.PI * lat2 / 180;
        double theta = lon1 - lon2;
        double rtheta = Math.PI * theta / 180;
        double dist =
            Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
            Math.Cos(rlat2) * Math.Cos(rtheta);
        dist = Math.Acos(dist);
        dist = dist * 180 / Math.PI;
        dist = dist * 60 * 1.1515;

        

        return dist*1000;
    }
}

public class ServerClient
{
    public TcpClient tcp;
    public string clientName;
    public int orderprice;
    public ServerClient(TcpClient clientSocket)
    {
        clientName = "Guest";
        tcp = clientSocket;
    }
    
}
public class Chatingroom
{
    public int roomid;
    public int totalprice;
    public string ServerroomName;
    public string ClientroomName;
    public int peapleNum;
    public float latitude;
    public float longitude;
    public List<ServerClient> clients;
}

