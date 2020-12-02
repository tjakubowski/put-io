using System;
using System.Net;
using System.Net.Sockets;

namespace ServerLibrary.Server
{
    public abstract class TcpServer
    {
        protected Logger Logger;
        protected TcpListener _listener;
        protected IPAddress _ipAddress;
        private int _port;
        private bool _isListening;
        private int _buffer_size = 2048;
        
        /// <summary>
        /// Returns the server IP address
        /// </summary>
        public IPAddress IP
        {
            get => _ipAddress;
            set
            {
                if (_isListening)
                    throw new Exception("IP address cannot be changed while the server is running");

                _ipAddress = value;
            }
        }

        /// <summary>
        /// Returns the server port
        /// </summary>
        public int Port
        {
            get => _port;
            set
            {
                if (_isListening)
                    throw new Exception("Server port cannot be changed while the server is running");

                if (!IsPortValid(value))
                    throw new Exception("Server port is not valid");
                
                _port = value;
            }
        }

        /// <summary>
        /// Returns the buffer size
        /// </summary>
        public int BufferSize
        {
            get => _buffer_size;
            set
            {
                if (_isListening)
                    throw new Exception("Buffer size cannot be changed while the server is running");

                _buffer_size = value;
            }
        }

        /// <summary>
        /// Creates a new TCP server instance with given IP address and port
        /// </summary>
        /// <param name="ipAddress">Server IP address</param>
        /// <param name="port">Server port number</param>
        /// <param name="loggerPath"></param>
        protected TcpServer(IPAddress ipAddress, int port, string loggerPath = "./logs.txt")
        {
            Logger = new Logger(loggerPath);
            IP = ipAddress;
            Port = port;
        }

        /// <summary>
        /// Starts listening on given IP address and port
        /// </summary>
        protected void StartListening()
        {
            _listener = new TcpListener(IP, Port);
            _listener.Start();
            _isListening = true;

            Console.WriteLine($"Server is listening at {IP}:{Port}");
        }

        private bool IsPortValid(int port)
        {
            return port >= 1024 && port <= 49151;
        }

        /// <summary>
        /// Starts TCP server
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Accepts one or more clients
        /// </summary>
        protected abstract void AcceptClient();

        /// <summary>
        /// Handle data transmission between the user and server
        /// </summary>
        /// <param name="session"></param>
        protected abstract void HandleClientSession(TcpServerSession session);
    }
}


