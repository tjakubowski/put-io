﻿using ServerLibrary.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Client client;
        public MainWindow()
        {
            client = new Client();
            try {
                client.StartClient();
            }catch(Exception e)
            {
                Console.WriteLine(String.Format("Error: {0}", e.StackTrace));
            }
            InitializeComponent();
        }

        private void SendChannelMsgButton_Click(object sender, RoutedEventArgs e)
        {
            client.SendMessage(ChannelMessage.Text);
        }
    }
}
