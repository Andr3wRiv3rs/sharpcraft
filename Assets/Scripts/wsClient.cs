using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using DysnomiaWebSocketClient;

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

    public bool AutoDetectPlatform = true;

    public string ServerAddress;

    [System.Serializable]
    public class UseCase
    {
        public Component component;
        public bool HASonOpen;
        public bool HASonClose;
        public bool HASonMessage;
        public bool HASonError;
    }

    public UseCase[] UseCases;

    ClientWS client;

    void Start()
    {
        if (AutoDetectPlatform)
        {
            if(Application.platform == RuntimePlatform.WebGLPlayer)
            {
                Platform = platform.WebGL;
            }
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                Platform = platform.Standalone;
            }
        }

        if(Platform == platform.Standalone)
        {
            client = new ClientWS();

            client.ChooseOpenMethod(onOpen);
            client.ChooseCloseMethod(onClose);
            client.ChooseMessageMethod(onMessage);
            client.ChooseErrorMethod(onError);
        }
    }

    public void ConnectWS(string ip)
    {
        if(Platform == platform.WebGL)
        {
            wsConnect(ServerAddress);
        }
        else if(Platform == platform.Standalone)
        {
            client.Connect(ip);
        }
    }

    public void SendWS(string str)
    {
        if (Platform == platform.WebGL)
        {
            wsSend(str);
        }
        else if (Platform == platform.Standalone)
        {
            client.Send(str);
        }
    }

    public void CloseWS()
    {
        if (Platform == platform.WebGL)
        {
            wsClose();
        }
        else if (Platform == platform.Standalone)
        {
            client.Close();
        }
    }

    public void onOpen()
    {
        MakeCalls("onOpen", "");
    }

    public void onClose()
    {
        MakeCalls("onClose", "");
    }

    public void onMessage(string str)
    {
        MakeCalls("onMessage", str);
    }

    public void onError()
    {
        MakeCalls("onError", "");
    }

    public void MakeCalls(string Method, string str)
    {
        for(int i = 0; i < UseCases.Length; i++)
        {
            if(Method == "onOpen" && UseCases[i].HASonOpen)
            {
                UseCases[i].component.SendMessage(Method);
            }
            if (Method == "onClose" && UseCases[i].HASonClose)
            {
                UseCases[i].component.SendMessage(Method);
            }
            if (Method == "onMessage" && UseCases[i].HASonMessage)
            {
                UseCases[i].component.SendMessage(Method, str);
            }
            if (Method == "onError" && UseCases[i].HASonError)
            {
                UseCases[i].component.SendMessage(Method);
            }
        }
    }

    public static void Send(string str)
    {
        GameObject.FindGameObjectWithTag("Setup").GetComponent<wsClient>().SendWS(str);
    }
    public static void Close()
    {
        GameObject.FindGameObjectWithTag("Setup").GetComponent<wsClient>().CloseWS();
    }
    public static void Connect(string ip)
    {
        GameObject.FindGameObjectWithTag("Setup").GetComponent<wsClient>().ConnectWS(ip);
    }

    public static string[] Split(string str)
    {
        return str.Split(new char[] { 'γ' });
    }

    public static string AddPrefix(string Prefix, string Body)
    {
        return Prefix + "γ" + Body;
    }
}