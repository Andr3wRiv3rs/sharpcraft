using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

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

    WebSocket csws;
    void Start()
    {
        csws = gameObject.GetComponent<WebSocket>();
    }

    void Connect(string ip)
    {
        if(Platform == platform.WebGL)
        {
            wsConnect("ws://189.223.237.64:8080");
        }
        else if(Platform == platform.Standalone)
        {
            csws.wsConnect(ip);
        }
    }

    void Send(string str)
    {
        if (Platform == platform.WebGL)
        {
            wsSend(str);
        }
        else if (Platform == platform.Standalone)
        {
            
        }
    }

    void Close()
    {
        if (Platform == platform.WebGL)
        {
            wsClose();
        }
        else if (Platform == platform.Standalone)
        {
            csws.wsClose();
        }
    }

    public void onOpen()
    {
        wsSend("Hey Meheecan");
    }

    public void onClose()
    {

    }

    public void onMessage()
    {

    }

    public void onError()
    {

    }
}