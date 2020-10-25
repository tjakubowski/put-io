using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace ServerLibrary
{
    public abstract class TcpServer
    {
        protected TcpListener _listener;
        protected IPAddress _ipAddress;
        protected int _port;
        protected bool _isListening;

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
                    throw new Exception("IP address cannot be changed while the server is running");
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
                    _port = value;
                else
                    throw new Exception("Server port cannot be changed while the server is running");
            }
        }

        /// <summary>
        /// Creates a new TCP server instance with given IP address and port
        /// </summary>
        /// <param name="ipAddress">Server IP address</param>
        /// <param name="port">Server port number</param>
        protected TcpServer(IPAddress ipAddress, int port)
        {
            _ipAddress = ipAddress;
            _port = port;
        }


        /// <summary>
        /// Starts TCP server
        /// </summary>
        public abstract void Start();
        public abstract void AcceptClient();
        public abstract void HandleDataTransmission();
    }
}


