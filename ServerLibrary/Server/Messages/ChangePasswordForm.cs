using System;

namespace ServerLibrary.Server.Messages
{
    [Serializable]
    public class ChangePasswordForm
    {
        public string Password;

        public ChangePasswordForm(string password)
        {
            Password = password;
        }
    }
}
