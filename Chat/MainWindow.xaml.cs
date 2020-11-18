using System.Windows;
using Chat.Pages;

namespace Chat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            WindowManager.MainWindow = this;
            WindowManager.OpenPage(new AuthenticationPage());
        }
    }
}
