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
                    Call_onOpen(EventArgs.Empty);
                    Read();
                    t = new Thread(new ThreadStart(ClosingCheck));
                    t.Start();
                }
            }
            catch (Exception e) { Call_onError(EventArgs.Empty); }
        }

        void ClosingCheck()
        {
            while (true)
            {
                if (client.State == WebSocketState.Closed)
                {
                    t.Abort();
                    break;
                }

                if (client.State == WebSocketState.Aborted)
                {
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
            Call_onMessage(new wsEventArgs(Encoding.UTF8.GetString(buf.Array, 0, r.Count)));
            Console.WriteLine(Encoding.UTF8.GetString(buf.Array, 0, r.Count));
            Read();
        }

        public void Close()
        {
            client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            Console.WriteLine("Connection Closed");
        }

        protected virtual void Call_onOpen(EventArgs e)
        {
            EventHandler handler = onOpen;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void Call_onClose(EventArgs e)
        {
            EventHandler handler = onOpen;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void Call_onError(EventArgs e)
        {
            EventHandler handler = onOpen;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void Call_onMessage(EventArgs e)
        {
            EventHandler handler = onOpen;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler onOpen;
        public event EventHandler onClose;
        public event EventHandler onError;
        public event EventHandler onMessage;
    }

    public class wsEventArgs : EventArgs
    {
        public string mystring { get; set; }

        public wsEventArgs(string myString)
        {
            this.mystring = myString;
        }
    }
}