using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary
{
    public class TcpServerAsync : TcpServer
    {
        private delegate void HandleDataTransmissionDelegate(NetworkStream stream);

        public TcpServerAsync(IPAddress ipAddress, int port) : base(ipAddress, port)
        {
        }

        public override void Start()
        {
            StartListening();
            AcceptClient();
        }

        protected override void AcceptClient()
        {
            while (true)
            {
                TcpClient tcpClient = _listener.AcceptTcpClient();
                NetworkStream stream = tcpClient.GetStream();

                Console.WriteLine($"Client connected");

                HandleDataTransmissionDelegate handleDataTransmissionDelegate = HandleDataTransmission;

                handleDataTransmissionDelegate.BeginInvoke(stream, CloseClientConnection, tcpClient);
            }

        }

        protected override void HandleDataTransmission(NetworkStream stream)
        {
            var buffer = new byte[BufferSize];

            while(true)
            {
                try
                {
                    var message = Read(stream, buffer);
                    Send(stream, message);
                }
                catch
                {
                    break;
                }
            }
        }

        void CloseClientConnection(IAsyncResult result)
        {
            TcpClient client = result as TcpClient;
            client.Close();

            Console.WriteLine($"Client disconnected");
        }


    }
}
