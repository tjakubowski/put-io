using System;

namespace ServerLibrary.Server.Messages
{
    [Serializable]
    public enum AuthenticationType
    {
        Login,
        Register
    }

    [Serializable]
    class AuthenticationForm
    {
        public AuthenticationType AuthenticationType;
        public string Username;
        public string Password;

        public AuthenticationForm(string username, string password, AuthenticationType authenticationType = AuthenticationType.Login)
        {
            Username = username;
            Password = password;
            AuthenticationType = authenticationType;
        }
    }
}
