using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DysnomiaWebSocketClient;

public class WebSocket : MonoBehaviour
{
    DysnomiaWebSocketClient.ClientWS client;
    private void Start()
    {
        client = new ClientWS();
        client.Connect("ws://shielded-plateau-64817.herokuapp.com/");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            client.Send("hey meheeeecanooooo");
            Debug.Log("ok");
        }
    }

    public void onOpen(object sender, wsEventArgs eventargs)
    {

    }

}