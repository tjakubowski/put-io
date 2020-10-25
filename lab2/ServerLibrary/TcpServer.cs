using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace ServerLibrary
{
    public class TcpServer
    {
        private TcpListener _listener;
        private IPAddress _ipAddress;
        private int _port;
        private bool _isListening;

        /// <summary>
        /// Returns the server IP address
        /// </summary>
        public IPAddress IP
        {
            get => _ipAddress;
            set
            {
                if (_isListening)
                    _ipAddress = value;
                else
                    throw new Exception();
            }
        }

        /// <summary>
        /// Returns the server address
        /// </summary>
        public int Port
        {
            get => _port;
            set
            {
                if (_isListening)
                    _port = value;
                else
                    throw new Exception();
            }
        }

        /// <summary>
        /// Creates a new TCP server instance with given IP address and port
        /// </summary>
        /// <param name="ipAddress">Server IP address</param>
        /// <param name="port">Server port number</param>
        public MyTcpServer(IPAddress ipAddress, int port)
        {
            _ipAddress = ipAddress;
            _port = port;
        }


        /// <summary>
        /// Starts TCP server
        /// </summary>
        public void Start()
        {
            _listener = new TcpListener(IP, Port);
            _listener.Start();
            _isListening = true;

            Console.WriteLine($"Server is listening at {IP}:{Port}");

            TcpClient client = _listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            Console.WriteLine($"Client connected");

            var startText = System.Text.Encoding.ASCII.GetBytes(
@"Zamiana malych pierwszych liter na wielkie
lorem Ipsum dolor Sit amet => Lorem Ipsum Dolor Sit Amet
");
            stream.Write(startText, 0, startText.Length);

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

            client.Close();
            _listener.Stop();
        }
    }
}


