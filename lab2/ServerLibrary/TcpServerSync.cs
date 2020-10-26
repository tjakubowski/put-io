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
            TcpClient client = _listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            Console.WriteLine($"Client connected");

            var startText = @"Zamiana malych pierwszych liter na wielkie
lorem Ipsum dolor Sit amet => Lorem Ipsum Dolor Sit Amet
";
            Send(stream, startText);

            HandleDataTransmission(stream);

            client.Close();
            _listener.Stop();
        }

        protected override void HandleDataTransmission(NetworkStream stream)
        {
            byte[] buffer = new byte[BufferSize];

            while (true)
            {
                try
                {
                    var clientText = Read(stream, buffer);

                    TextInfo cultureInfo = new CultureInfo("en-US", false).TextInfo;
                    var capitalizedText = cultureInfo.ToTitleCase(clientText);

                    Send(stream, capitalizedText);
                }
                catch
                {
                    break;
                }
            }
        }
    }
}
