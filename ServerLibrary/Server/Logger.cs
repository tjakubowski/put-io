using System;
using System.IO;

namespace ServerLibrary.Server
{
    public class Logger
    {
        public string LogFilePath { get; set; }

        public Logger(string logFilePath)
        {
            LogFilePath = logFilePath;
        }

        public void Log(string message, bool logToConsole = true)
        {
            message = $"{GetTimestamp()} - {message}";

            LogToFile(message);

            if (logToConsole)
            {
                LogToConsole(message);
            }
        }

        private void LogToFile(string message)
        {
            using (StreamWriter file = new StreamWriter(LogFilePath, true))
            {
                file.WriteLine(message);
            }
        }

        private void LogToConsole(string message)
        {
            Console.WriteLine(message);
        }

        private string GetTimestamp()
        {
            var today = DateTime.Now;
            return today.ToString("u");
        }
    }
}
