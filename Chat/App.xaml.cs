using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ServerLibrary.Client;

namespace Chat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Client Client;
        public App()
        {
            Client = new Client("127.0.0.1", 2048);
        }
    }
}
