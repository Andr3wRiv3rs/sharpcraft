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

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().useGravity = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().LockPlayer();
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

        if(wsClient.Split(str)[0] == "PNG")
        {
            PingMessage = wsClient.Split(str)[1];
        }
    }

    public void DisconnectPage()
    {
        ServerMaster.inGame = false;
        GameObject.FindGameObjectWithTag("HUD").SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().useGravity = false;
        Disconnect();
        if(KickMessage != "")
        {
            //set disconnect message
            KickMessage = "";
        }
    }

    public void Disconnect()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().useGravity = false;
        ShowChat(false);
        //tell MobLoader to remove mobs and clear
        //tell ChunkLoader to remove chunks and clear
    }

    public void LeaveGame()
    {
        GameObject.FindGameObjectWithTag("HUD").SetActive(true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().useGravity = false;
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
        GameObject.FindGameObjectWithTag("HUD").SetActive(true);
    }

    public float SpawnWait = 2;
    public IEnumerator StartRoutine()
    {
        yield return new WaitForSeconds(SpawnWait);
        //waiting
        ServerMaster.inGame = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().useGravity = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UnlockPlayer();
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
    
    public static bool inGame = false;
    public static bool Paused = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //only if the chat isn't open
            if (Chat.FieldSwitch == false)
            {
                if (ServerMaster.Paused)
                {
                    //Unpause
                    ServerMaster.Paused = false;
                    ServerMaster.inGame = true;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UnlockPlayer();
                }
                else
                {
                    //Pause
                    ServerMaster.Paused = true;
                    ServerMaster.inGame = false;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().LockPlayer();
                }
            }
        }
    }

    public class ServerInfo
    {
        public string Name;
        public string Description;
        public string ImageURL;
        public int ping;
    }
    
    private string PingMessage = "";
    public ServerInfo SendPing()
    {
        int gotPing = 0;
        StartCoroutine(Pinging((ping) => {
            gotPing = ping;
        }));

        ServerInfo serverInfo = new ServerInfo();
        serverInfo.Name = wsClient.Split(PingMessage)[0];
        serverInfo.Description = wsClient.Split(PingMessage)[1];
        serverInfo.ImageURL = wsClient.Split(PingMessage)[2];
        serverInfo.ping = gotPing;
        PingMessage = "";
        return serverInfo;
    }

    public int Timeout = 1000;
    public IEnumerator Pinging(System.Action<int> callback)
    {
        wsClient.Send(wsClient.AddPrefix("PNG", "GET"));
        int Milliseconds = 0;
        while (true)
        {
            if(PingMessage != "")
            {
                callback(Milliseconds);
                break;
            }
            else
            {
                if(Milliseconds >= Timeout)
                {
                    callback(Milliseconds);
                    break;
                }
            }
            yield return new WaitForSeconds(0.001f);
            Milliseconds++;
        }
    }

    //Server List
    public GameObject ServerBox;
    List<string> ServerList;

    public void AddServer(string ip)
    {
        ServerList.Add(ip);
        int Number = PlayerPrefs.GetInt("ServerCount", 0);

        PlayerPrefs.SetString("SavedServer" + Number.ToString(), ip);
        PlayerPrefs.SetInt("ServerCount", Number + 1);

        PlayerPrefs.Save();
    }

    public void LoadServers()
    {
        ServerList.Clear();
        int Number = PlayerPrefs.GetInt("ServerCount", 0);

        for(int i = 0; i < Number; i++)
        {
            ServerList.Add(PlayerPrefs.GetString("SavedServer" + i.ToString()));
        }
    }

    public void CleanServers()
    {

    }

    public void DisplayServers()
    {
        for(int i = 0; i < ServerList.Count; i++)
        {

        }
    }
}