using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CSharpSDK.Events;
using CSharpSDK.Models;
using Newtonsoft.Json;

namespace CSharpSDK
{
    public abstract class BsgCSharpSdk
    {
        protected readonly Queue<Redemption> EventQueue = new Queue<Redemption>();
        private const int Port = 29175;
        private Thread _listenThread;

        public void Start()
        {
            _listenThread = new Thread(Listen)
            {
                Name = "BSG TCP/IP Server",
                IsBackground = true
                
            };
            _listenThread.Start();
        }
        
        private void Listen()
        {
            var ipAddress = IPAddress.Any;

            while (true)
            {
                var listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    var localEndPoint = new IPEndPoint(ipAddress, Port);

                    listener.Bind(localEndPoint);
                    listener.Listen(10);

                    Console.WriteLine("Waiting for a connection...");
                    var handler = listener.Accept();

                    Console.WriteLine("Established Connection");

                    string data = null;

                    while (true)
                    {
                        var bytes = new byte[1024];
                        var bytesRec = handler.Receive(bytes);

                        if (bytesRec == 0)
                            break;

                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("\n", StringComparison.Ordinal) <= -1) continue;

                        var redemption = JsonConvert.DeserializeObject<Redemption>(data);
                        if (redemption != null)
                        {
                            EventQueue.Enqueue(redemption);
                        }

                        handler.Send(new byte[] {0xD});
                        data = null;
                    }
                }
                catch (SocketException)
                {
                    // ignored
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    Console.WriteLine("Lost connection to client");
                    listener.Close();
                }
            }
        }
        
        public void Poll(params object[] args)
        {
            if (EventQueue.Count == 0) return;
            
            var redemption = EventQueue.Dequeue();
            OnRedemptionReceived(redemption, args);
            GetEvent(redemption).Execute(args);
        }
        
        protected abstract void OnRedemptionReceived(Redemption redemption, params object[] args);

        protected abstract BaseEvent GetEvent(Redemption redemption);
    }
}