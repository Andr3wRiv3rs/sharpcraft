using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crypto;

public class ServerMaster : MonoBehaviour
{
    public Chat Chat;

    public string Playername;
    public string Email;
    public string Password;
    public string Skinurl;

    [System.Serializable]
    private class Login
    {
        public string Playername;
        public string publicKey;
        public string Skinurl;
    }

    bool inGame = false;
    bool Connecting = false;
    void Connect(string ip)
    {
        if (wsClient.isConnected())
        {
            wsClient.Close();
        }
        wsClient.Connect(ip);
        Connecting = true;
    }

    public void onOpen()
    {
        if (Connecting)
        {
            Connecting = false;
            AttemptLogin();
        }
    }

    public void AttemptLogin()
    {
        Login login = new Login();
        login.Playername = Playername;
        PrepareSigning();
        login.publicKey = Signature.publicKey;
        login.Skinurl = Skinurl;

        string json = JsonUtility.ToJson(login);
        wsClient.Send(wsClient.AddPrefix("LGN", json));
    }

    private string KickMessage = "";
    public void onMessage(string str)
    {
        if(wsClient.Split(str)[0] == "AUT")
        {
            if(wsClient.Split(str)[1] == "Accepted")
            {
                StartAllSystems();
            }
            else if (wsClient.Split(str)[1] == "Denied")
            {
                DisconnectPage();
            }
            else
            {
                wsClient.Send(wsClient.AddPrefix("SGN", GetSignature(wsClient.Split(str)[1])));
            }
        }

        if(wsClient.Split(str)[0] == "KICK")
        {
            KickMessage = wsClient.Split(str)[1];
        }
    }

    public void DisconnectPage()
    {
        Disconnect();
        if(KickMessage != "")
        {
            //set disconnect message
            KickMessage = "";
        }
    }

    public void Disconnect()
    {
        ShowChat(false);
        //tell MobLoader to remove mobs and clear
        //tell ChunkLoader to remove chunks and clear
    }

    public void RemoveObjectsWithTag(string str)
    {
        for(int i = 0; i < GameObject.FindGameObjectsWithTag(str).Length; i++)
        {
            Destroy(GameObject.FindGameObjectsWithTag(str)[i]);
        }
    }

    public void StartAllSystems()
    {
        //Start Player script's position updates
    }

    public string GetSignature(string str)
    {
        PrepareSigning();
        return Signature.Sign(str);
    }

    public void PrepareSigning()
    {
        if (PlayerPrefs.GetString("dhs8n2", "null") != "null")
        {
            Signature.Generate(Password, Email);
        }
        else
        {
            Signature.Generate(Playername, Email);
            PlayerPrefs.SetString("dhs8n2", Signature.signingKey);
            PlayerPrefs.Save();
        }
    }

    public void onClose()
    {
        DisconnectPage();
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