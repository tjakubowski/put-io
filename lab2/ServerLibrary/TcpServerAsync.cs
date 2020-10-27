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
        private delegate void HandleDataTransmissionDelegate(TcpServerConnection connection);

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
                TcpServerConnection connection = new TcpServerConnection(tcpClient);

                Console.WriteLine($"Client connected");

                HandleDataTransmissionDelegate handleDataTransmissionDelegate = HandleDataTransmission;

                handleDataTransmissionDelegate.BeginInvoke(connection, CloseClientConnection, tcpClient);
            }

        }



        protected override void HandleDataTransmission(TcpServerConnection connection)
        {
            try
            {
                AuthenticateUser(connection);

                while (true)
                {
                    var message = connection.Read();
                    Console.WriteLine(message);
                    connection.Send(message);
                }
            }
            catch(IOException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void AuthenticateUser(TcpServerConnection connection)
        {
            connection.Send("User authentication");

            connection.Send("Username: ");
            var username = connection.Read();

            connection.Send("Password: ");
            var password = connection.Read();

            if (username == "admin" && password == "test")
                connection.Send("Success");
            else
                connection.Send("Error");
        }

        void CloseClientConnection(IAsyncResult result)
        {
            TcpClient client = (TcpClient) result.AsyncState;
            client.Close();

            Console.WriteLine($"Client disconnected");
        }

    }
}
