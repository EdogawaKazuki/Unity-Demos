using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System;
using System.Text;

public class IPCamStream : MonoBehaviour
{
    Socket clientSocket;
    public string serverIp = "localhost";
    public int serverPort = 9999;
    IPEndPoint ipe;
    EndPoint serverEnd;
    bool connectToRobot = true;
    void connect()
    {
        ipe = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        string sendStr = "hello";
        byte[] sendData = Encoding.ASCII.GetBytes(sendStr);
        serverEnd = (EndPoint)ipe;
        clientSocket.SendTimeout = 500;
        clientSocket.ReceiveTimeout = 500;
        try
        {
            clientSocket.SendTo(sendData, sendData.Length, SocketFlags.None, serverEnd);
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }

    }
    private void Start()
    {
        if (connectToRobot)
            connect();
    }
    private void Update()
    {
        if (connectToRobot)
        {
            byte[] recvData = new byte[655350];
            int recvLen = clientSocket.ReceiveFrom(recvData, ref serverEnd);
            // GetImageType(recvData);
            Texture2D tex = new Texture2D(300, 400);
            tex.LoadImage(recvData);
            tex.Apply();
            // Assign texture to renderer's material.
            GetComponent<Renderer>().material.mainTexture = tex;
        }
    }
    private void OnApplicationQuit()
    {
        string sendStr = "bye";
        byte[] sendData = Encoding.ASCII.GetBytes(sendStr);
        serverEnd = (EndPoint)ipe;
        clientSocket.SendTimeout = 500;
        clientSocket.ReceiveTimeout = 500;
        try
        {
            clientSocket.SendTo(sendData, sendData.Length, SocketFlags.None, serverEnd);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}