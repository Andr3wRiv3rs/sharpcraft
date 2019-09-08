using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.WebSockets;

namespace DysnomiaWebSocketClient
{
    class ClientWS
    {
        ClientWebSocket client;
        ArraySegment<byte> buf = new ArraySegment<byte>(new byte[1024]);




        public void Connect(string ip)
        {
            Start(ip);
        }

        Thread t;
        async void Start(string ip)
        {
            client = new ClientWebSocket();
            try
            {
                await client.ConnectAsync(new Uri(ip), CancellationToken.None);
                if (client.State == WebSocketState.Open)
                {
                    Read();
                    if (onOpen != null)
                    {
                        onOpen();
                    }
                    t = new Thread(new ThreadStart(ClosingCheck));
                    t.Start();
                }
            }
            catch (Exception e) { if (onError != null) onError(); }
        }

        void ClosingCheck()
        {
            while (true)
            {
                if (client.State == WebSocketState.Closed)
                {
                    if (onClose != null)
                    {
                        onClose();
                    }
                    t.Abort();
                    break;
                }

                if (client.State == WebSocketState.Aborted)
                {
                    if (onClose != null)
                    {
                        onClose();
                    }
                    t.Abort();
                    break;
                }
                Thread.Sleep(1000);
            }
        }

        public void Send(string str)
        {
            wsSend(str);
        }

        async void wsSend(string str)
        {
            ArraySegment<byte> b = new ArraySegment<byte>(Encoding.UTF8.GetBytes(str));
            await client.SendAsync(b, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        async void Read()
        {
            WebSocketReceiveResult r = await client.ReceiveAsync(buf, CancellationToken.None);
            if (onMessage != null)
            {
                onMessage(Encoding.UTF8.GetString(buf.Array, 0, r.Count));
            }
            Read();
        }

        public void Close()
        {
            client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
        }

        private Action onOpen;
        private Action onClose;
        private Action<string> onMessage;
        private Action onError;

        public void ChooseOpenMethod(Action T)
        {
            onOpen = T;
        }
        public void ChooseCloseMethod(Action T)
        {
            onClose = T;
        }
        public void ChooseMessageMethod(Action<string> T)
        {
            onMessage = T;
        }
        public void ChooseErrorMethod(Action T)
        {
            onError = T;
        }
    }
}