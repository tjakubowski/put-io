using System;

namespace ServerLibrary.Server.Messages
{
    [Serializable]
    class ChangePasswordForm
    {
        public string Password;

        public ChangePasswordForm(string password)
        {
            Password = password;
        }
    }
}
