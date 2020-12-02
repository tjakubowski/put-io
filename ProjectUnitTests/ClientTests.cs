using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServerLibrary.Client;
using ServerLibrary.Server;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ProjectUnitTests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public void ClientWrongIPAdressTest()
        {
            Task.Run(() =>
            {
                TcpServerAsync server = new TcpServerAsync(IPAddress.Parse("127.0.0.1"), 2048);
                server.Start();
            });
            Assert.ThrowsException<SocketException>(() => new Client("127.AB.0.1", 2048));
        }


        [TestMethod]
        public void ClientWrongPort_Lesser()
        {
            Assert.ThrowsException<FormatException>(() => new Client("127.0.0.1", 1000));
        }

        [TestMethod]
        public void ClientWrongPort_Higher()
        {
            Assert.ThrowsException<FormatException>(() => new Client("127.0.0.1", 50000));
        }
    }
}
