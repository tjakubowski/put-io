using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
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

            try
            {
                AuthenticateUser(stream, buffer);

                while (true)
                {
                    var message = Read(stream, buffer);
                    Console.WriteLine(message);
                    Send(stream, message);
                }
            }
            catch(IOException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void AuthenticateUser(NetworkStream stream, byte[] buffer)
        {
            Send(stream, "User authentication");

            Send(stream, "Username: ");
            var username = Read(stream, buffer);

            Send(stream, "Password: ");
            var password = Read(stream, buffer);

            if(password != "asdf" && username != "admin")
                Send(stream, "Blad");
            else
                Send(stream, "Dobrze");
        }

        void CloseClientConnection(IAsyncResult result)
        {
            TcpClient client = (TcpClient) result.AsyncState;
            client.Close();

            Console.WriteLine($"Client disconnected");
        }


    }
}
