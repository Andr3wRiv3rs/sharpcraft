using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.WebSockets;
using System;
using System.Threading;
using System.Text;

public class WebSocket : MonoBehaviour
{
    private ClientWebSocket client;
    public void wsConnect(string ip)
    {
        client = new ClientWebSocket();
        client.ConnectAsync(new Uri(ip), new CancellationToken());
    }

    public void wsClose()
    {
        client.CloseAsync(new WebSocketCloseStatus(), "", new CancellationToken());
    }

    public void wsSend(string str)
    {
        byte[] strB = Encoding.UTF8.GetBytes(str);
        ArraySegment<Byte> AS = new ArraySegment<Byte>(strB);

        client.SendAsync(AS, new WebSocketMessageType(), false, new CancellationToken());
    }

    wsClient clientscript;
    void Start()
    {
        clientscript = gameObject.GetComponent<wsClient>();
        Reading();
    }

    string Data;
    public IEnumerator Listen()
    {
        while (true)
        {
            if(client.State == WebSocketState.Open)
            {
                clientscript.onOpen();
            }
            if (client.State == WebSocketState.Closed)
            {
                clientscript.onClose();
            }
            if (client.State == WebSocketState.Aborted)
            {
                clientscript.onError();
            }
        }
    }

    async void Reading()
    {

    }
}