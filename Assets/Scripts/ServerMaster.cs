using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerMaster : MonoBehaviour
{
    public wsClient WSCLIENT;
    public GameObject Chat;

    void Connect(string ip)
    {
        WSCLIENT.ConnectWS(ip);
    }

    public void onOpen()
    {

    }

    public void onClose()
    {
        //show disconnected page
        ShowChat(false);
    }

    void ClearChat()
    {
        for(int i = 0; i < GameObject.FindGameObjectsWithTag("Bubble").Length; i++)
        {
            Destroy(GameObject.FindGameObjectsWithTag("Bubble")[i]);
        }
        GameObject.FindGameObjectWithTag("InputField").GetComponent<InputField>().text = "";
    }

    void ShowChat(bool Fact)
    {
        Chat.GetComponent<Chat>().HideInput();
        Chat.SetActive(Fact);
    }
}