using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CSharpSDK.Models;

namespace CSharpSDK
{
    public class BsgCSharpSdk
    {
        protected Queue<Redemption> Redemptions = new Queue<Redemption>();
        private const int PORT = 29175;

        public void Start()
        {
            Listen();
        }
        
        private void Listen()
        {
            var ipAddress = IPAddress.Any;

            while (true)
            {
                var listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    var localEndPoint = new IPEndPoint(ipAddress, PORT);

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
                        
                        Console.WriteLine("Text received : {0}", data);
                        
                        handler.Send(new byte[] {0xD});
                        data = null;
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
                finally
                {
                    Console.WriteLine("Lost connection to client");
                    listener.Close();
                }
            }
        }
    }
}