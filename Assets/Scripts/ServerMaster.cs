using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerMaster : MonoBehaviour
{
    public Chat Chat;

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
            if(wsClient.Split(str)[1] == "AUT")
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
        Chat.gameObject.GetComponent<Chat>().HideInput();
        Chat.gameObject.SetActive(Fact);
    }

    public bool Paused = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
            {
                Paused = true;
                if (GameObject.FindGameObjectWithTag("InputField").activeInHierarchy)
                {
                    Chat.HideInput();
                }

                //show pause menu
            }
            else
            {
                Paused = false;
            }
        }
    }
}