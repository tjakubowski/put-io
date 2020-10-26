﻿using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace ServerLibrary
{
    public abstract class TcpServer
    {
        protected TcpListener _listener;
        protected IPAddress _ipAddress;
        private int _port;
        private bool _isListening;
        private int _buffer_size;

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
        protected TcpServer(IPAddress ipAddress, int port)
        {
            _ipAddress = ipAddress;
            _port = port;
        }

        protected void StartListening()
        {
            _listener = new TcpListener(IP, Port);
            _listener.Start();
            _isListening = true;

            Console.WriteLine($"Server is listening at {IP}:{Port}");
        }


        /// <summary>
        /// Starts TCP server
        /// </summary>
        public abstract void Start();
        public abstract void AcceptClient();
        public abstract void HandleDataTransmission(NetworkStream stream);
    }
}


