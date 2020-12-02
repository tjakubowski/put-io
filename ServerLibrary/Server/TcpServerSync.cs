using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace ServerLibrary.Server
{
    public class TcpServerSync : TcpServer
    {
        public TcpServerSync(IPAddress ipAddress, int port, string loggerPath) : base(ipAddress, port, loggerPath)
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
            TcpServerSession session = new TcpServerSession(tcpClient);
            Console.WriteLine($"Client connected");

            var startText = @"Zamiana malych pierwszych liter na wielkie
lorem Ipsum dolor Sit amet => Lorem Ipsum Dolor Sit Amet
";
            session.Send(startText);

            HandleClientSession(session);

            tcpClient.Close();
            _listener.Stop();
        }

        protected override void HandleClientSession(TcpServerSession session)
        {
            while (true)
            {
                try
                {
                    var clientText = session.Read();

                    TextInfo cultureInfo = new CultureInfo("en-US", false).TextInfo;
                    var capitalizedText = cultureInfo.ToTitleCase(clientText);

                    session.Send(capitalizedText);
                }
                catch
                {
                    break;
                }
            }
        }
    }
}
