using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServerLibrary.Client;
using System;
using System.Net;
using ServerLibrary.Server;


namespace ProjectUnitTests
{
    [TestClass]
    public class ServerTests
    {
        [TestMethod]
        public void Server_WrongPort_Lesser()
        {
            Assert.ThrowsException<Exception>(() => new TcpServerAsync(IPAddress.Parse("127.0.0.1"), 80));
        }

        [TestMethod]
        public void Server_WrongPort_LesserEqual()
        {
            TcpServerAsync server = new TcpServerAsync(IPAddress.Parse("127.0.0.1"), 1024);
            Assert.AreEqual(server.Port, 1024);
        }

        [TestMethod]
        public void Server_WrongPort_Greater()
        {
            Assert.ThrowsException<Exception>(() => new TcpServerAsync(IPAddress.Parse("127.0.0.1"), 80));
        }

        [TestMethod]
        public void Server_WrongPort_GreaterEqual()
        {
            TcpServerAsync server = new TcpServerAsync(IPAddress.Parse("127.0.0.1"), 49151);
            Assert.AreEqual(server.Port, 49151);
        }

        [TestMethod]
        public void Server_WrongAddress_Localhost_Text()
        {
            Assert.ThrowsException<FormatException>(() => new TcpServerAsync(IPAddress.Parse("localhost"), 2048));
        }
    }
}
