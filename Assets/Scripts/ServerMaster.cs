using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerMaster : MonoBehaviour
{
    public GameObject Chat;

    public string Playername;
    public string Password;
    public string Skinurl;

    void Connect(string ip)
    {
        wsClient.Connect(ip);
    }

    public void onOpen()
    {
        //Send authentication load
    }

    public void onMessage(string str)
    {
        if(wsClient.Split(str)[0] == "AUT")
        {
            if(wsClient.Split(str)[1] == "Welcome")
            {
                //join the game
            }
            else
            {
                //encrypt the hash received
            }
        }
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