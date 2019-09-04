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

    void Start()
    {
        wsConnect("ws://189.223.237.64:8080");
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