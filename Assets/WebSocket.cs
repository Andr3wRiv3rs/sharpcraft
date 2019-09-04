using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.WebSockets;
using System;
using System.Threading;
using System.Text;

public class WebSocket : MonoBehaviour
{
    ArraySegment<byte> buf = new ArraySegment<byte>();

    private ClientWebSocket client;
    public void wsConnect(string ip)
    {
        client = new ClientWebSocket();
        client.ConnectAsync(new Uri(ip), CancellationToken.None);
    }

    public void wsClose()
    {
        client.CloseAsync(new WebSocketCloseStatus(), "", CancellationToken.None);
    }

    public void wsSend(string str)
    {
        byte[] strB = Encoding.UTF8.GetBytes(str);
        ArraySegment<Byte> AS = new ArraySegment<Byte>(strB);

        client.SendAsync(AS, new WebSocketMessageType(), false, CancellationToken.None);
    }

    wsClient clientscript;
    void Start()
    {
        clientscript = gameObject.GetComponent<wsClient>();
        Reading(client);
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

    async void Reading(ClientWebSocket client)
    {
        WebSocketReceiveResult result = await client.ReceiveAsync(new ArraySegment<byte>(), CancellationToken.None);
        Debug.Log(Encoding.UTF8.GetString(buf.Array, 0, buf.Count));
    }
}