using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using DysnomiaWebSocketClient;

public class wsClient : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void wsConnect(string ip);

    [DllImport("__Internal")]
    private static extern void wsSend(string str);

    [DllImport("__Internal")]
    private static extern void wsClose();

    public enum platform {WebGL, Standalone}

    public platform Platform = platform.WebGL;

    public string ServerAddress;
    ClientWS client;

    void Start()
    {
        if(Platform == platform.Standalone)
        {
            client = new ClientWS();

            client.ChooseOpenMethod(onOpen);
            client.ChooseCloseMethod(onClose);
            client.ChooseMessageMethod(onMessage);
            client.ChooseErrorMethod(onError);
        }

        Connect(ServerAddress);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Send("shit");
        }
    }

    public void Connect(string ip)
    {
        if(Platform == platform.WebGL)
        {
            wsConnect(ServerAddress);
        }
        else if(Platform == platform.Standalone)
        {
            client.Connect(ip);
        }
    }

    public void Send(string str)
    {
        if (Platform == platform.WebGL)
        {
            wsSend(str);
        }
        else if (Platform == platform.Standalone)
        {
            client.Send(str);
        }
    }

    public void Close()
    {
        if (Platform == platform.WebGL)
        {
            wsClose();
        }
        else if (Platform == platform.Standalone)
        {
            client.Close();
        }
    }

    public void onOpen()
    {
        Debug.Log("Connected");
        Send("ping");
    }

    public void onClose()
    {
        Debug.Log("Disconnected");
    }

    public void onMessage(string str)
    {
        Debug.Log(str);
    }

    public void onError()
    {

    }
}