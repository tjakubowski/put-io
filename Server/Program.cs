using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ServerLibrary;
using ServerLibrary.Server;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpServerAsync server = new TcpServerAsync(IPAddress.Parse("127.0.0.1"), 2048);
            server.Start();
        }
    }
}
