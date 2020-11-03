using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary
{
    public class TcpServerConnection
    {
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;
        public TcpClient Client { get; }
        public UserModel User;

        public TcpServerConnection(TcpClient client)
        {
            var stream = client.GetStream();

            Client = client;
            _reader = new StreamReader(stream);
            _writer = new StreamWriter(stream) {AutoFlush = true};
        }


        /// <summary>
        /// Reads bytes from given NetworkStream
        /// </summary>
        /// <param name="connection"></param>
        /// <returns>Returns string created from read bytes</returns>
        public String Read()
        {
            return _reader.ReadLine();
        }

        /// <summary>
        /// Sends a string to the client
        /// </summary>
        /// <param name="message"></param>
        public void Send(String message)
        {
            _writer.Write(message);
        }

        /// <summary>
        /// Sends a string with new line character to the client
        /// </summary>
        /// <param name="message"></param>
        public void SendLine(String message)
        {
            _writer.WriteLine(message);
        }
    }
}
