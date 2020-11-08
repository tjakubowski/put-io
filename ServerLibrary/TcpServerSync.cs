using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary
{
    public class TcpServerSync : TcpServer
    {
        public TcpServerSync(IPAddress ipAddress, int port) : base(ipAddress, port)
        {
        }

        public override void Start()
        {
            StartListening();
            AcceptClient();
        }

        protected override void AcceptClient()
        {
            TcpClient tcpClient = _listener.AcceptTcpClient();
            TcpServerConnection connection = new TcpServerConnection(tcpClient);
            Console.WriteLine($"Client connected");

            var startText = @"Zamiana malych pierwszych liter na wielkie
lorem Ipsum dolor Sit amet => Lorem Ipsum Dolor Sit Amet
";
            connection.Send(startText);

            HandleDataTransmission(connection);

            tcpClient.Close();
            _listener.Stop();
        }

        protected override void HandleDataTransmission(TcpServerConnection connection)
        {
            while (true)
            {
                try
                {
                    var clientText = connection.Read();

                    TextInfo cultureInfo = new CultureInfo("en-US", false).TextInfo;
                    var capitalizedText = cultureInfo.ToTitleCase(clientText);

                    connection.Send(capitalizedText);
                }
                catch
                {
                    break;
                }
            }
        }
    }
}
