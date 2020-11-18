using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using ServerLibrary.Models;

namespace ServerLibrary.Server
{
    public class TcpServerSession
    {
        private readonly NetworkStream stream;
        private readonly StreamReader reader;
        private readonly StreamWriter writer;

        public TcpClient Client { get; }
        public User User;
        public int ChannelId;

        public TcpServerSession(TcpClient client)
        {
            Client = client;

            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) {AutoFlush = true};
        }


        /// <summary>
        /// Reads whole line of text
        /// </summary>
        /// <returns>Returns string created from read bytes</returns>
        public String Read()
        {
            return reader.ReadLine();
        }

        /// <summary>
        /// Reads all bytes from given NetworkStream 
        /// </summary>
        /// <returns>Returns all read bytes</returns>
        public byte[] ReadBytes()
        {
            MemoryStream messageStream = new MemoryStream();
            var buffer = new byte[2048];
            do
            {
                var bytesRead = stream.Read(buffer, 0, buffer.Length);
                messageStream.Write(buffer, 0, bytesRead);
            } while (stream.DataAvailable);

            return messageStream.ToArray();
        }

        /// <summary>
        /// Sends a string to the client
        /// </summary>
        /// <param name="message"></param>
        public void Send(String message)
        {
            writer.Write(message);
        }

        /// <summary>
        /// Sends a string with new line character to the client
        /// </summary>
        /// <param name="message"></param>
        public void SendLine(String message)
        {
            writer.WriteLine(message);
        }

        public void SendBytes(byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}
