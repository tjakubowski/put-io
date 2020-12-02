using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using ServerLibrary.Server;


namespace ProjectUnitTests
{
    [TestClass]
    public class LoggerTests
    {
        [TestMethod]
        public void Logger_FilePath_Absolute_Test()
        {
            var logger = new Logger("./logs.txt");
            Assert.AreNotEqual(logger.LogFilePath, "./logs.txt");
        }

        [TestMethod]
        public void Logger_Wrong_FilePath_Test()
        {
            var logger = new Logger("./logs.txt");
        }
    }
}
