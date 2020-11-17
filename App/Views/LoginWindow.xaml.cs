using ServerLibrary.Client;
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
using System.Windows.Shapes;

namespace App.Views
{
    /// <summary>
    /// Logika interakcji dla klasy LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //need global client class instance to send msg
            client.SendLoginData(Username.Text, Password.Text);
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            //need global client class instance to send msg
            client.SendRegisterData(Username.Text, Password.Text);
        }
    }
}
