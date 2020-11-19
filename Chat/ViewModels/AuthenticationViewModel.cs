using System.Windows;
using System.Windows.Input;
using Chat.Pages;
using Chat.ViewModels.Base;
using ServerLibrary.Client;

namespace Chat.ViewModels
{
    public class AuthenticationViewModel : BaseViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public RelayCommand LoginCommand => new RelayCommand(async o =>
        {
            var loginResponse = await App.Client.SendLoginRequest(Username, Password);

            if (loginResponse.Result)
                WindowManager.OpenPage(new ChatPage());
            else
                MessageBox.Show(loginResponse.Message);
        });

        public RelayCommand RegisterCommand => new RelayCommand(async o =>
        {
            var registerResponse = await App.Client.SendRegisterRequest(Username, Password);

            if (registerResponse.Result)
                //WindowManager.OpenPage(new ChatPage());
                MessageBox.Show("User registered");
            else
                MessageBox.Show(registerResponse.Message);
        });
    }
}
