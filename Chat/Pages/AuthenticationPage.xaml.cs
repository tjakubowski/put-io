using Chat.ViewModels;
using System.Windows.Controls;

namespace Chat.Pages
{
    /// <summary>
    /// Interaction logic for AuthenticationPage.xaml
    /// </summary>
    public partial class AuthenticationPage : Page
    {
        public AuthenticationPage()
        {
            InitializeComponent();
        }

        private void Button_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                ((ChatViewModel)this.DataContext).SendMessageCommand.Execute(null);
            }
        }
    }
}
