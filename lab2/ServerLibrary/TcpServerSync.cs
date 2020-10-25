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

        public override void AcceptClient()
        {
            TcpClient client = _listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            Console.WriteLine($"Client connected");

            var startText = System.Text.Encoding.ASCII.GetBytes(
                @"Zamiana malych pierwszych liter na wielkie
lorem Ipsum dolor Sit amet => Lorem Ipsum Dolor Sit Amet
");
            stream.Write(startText, 0, startText.Length);

            HandleDataTransmission(stream);

            client.Close();
            _listener.Stop();
        }

        public override void HandleDataTransmission(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            int readSize;

            while ((readSize = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                var clientText = System.Text.Encoding.ASCII.GetString(buffer, 0, readSize);

                TextInfo cultureInfo = new CultureInfo("en-US", false).TextInfo;
                var capitalizedText = cultureInfo.ToTitleCase(clientText);

                var capitalizedTextBytes = System.Text.Encoding.ASCII.GetBytes(capitalizedText);
                stream.Write(capitalizedTextBytes, 0, capitalizedTextBytes.Length);
            }
        }
    }
}
