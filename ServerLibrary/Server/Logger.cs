using System;
using System.IO;

namespace ServerLibrary.Server
{
    public class Logger
    {
        public bool ConsoleLogEnabled { get; set; }
        private string _logFilePath;

        public string LogFilePath
        {
            get => _logFilePath;
            set => _logFilePath = Path.GetFullPath(value);
        }

        public Logger(string logFilePath, bool consoleLogEnabled = true)
        {
            LogFilePath = logFilePath;
            ConsoleLogEnabled = consoleLogEnabled;
        }

        public void Log(string message)
        {
            message = $"{GetTimestamp()} - {message}";

            LogToFile(message);

            if (ConsoleLogEnabled)
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
