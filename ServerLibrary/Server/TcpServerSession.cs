using System;
using System.IO;
using System.Net.Sockets;
using ServerLibrary.Models;

namespace ServerLibrary.Server
{
    public class TcpServerSession
    {
        private readonly NetworkStream _stream;
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;
        public TcpClient Client { get; }
        public User User;

        public TcpServerSession(TcpClient client)
        {
            Client = client;

            _stream = client.GetStream();
            _reader = new StreamReader(_stream);
            _writer = new StreamWriter(_stream) {AutoFlush = true};
        }


        /// <summary>
        /// Reads whole line of text
        /// </summary>
        /// <returns>Returns string created from read bytes</returns>
        public String Read()
        {
            return _reader.ReadLine();
        }

        /// <summary>
        /// Reads all bytes from given NetworkStream 
        /// </summary>
        /// <returns>Returns all read bytes</returns>
        public byte[] ReadBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                _stream.CopyTo(ms);
                return ms.ToArray();
            }
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

        public void SendBytes(byte[] bytes)
        {
            _stream.Write(bytes, 0, bytes.Length);
        }
    }
}
