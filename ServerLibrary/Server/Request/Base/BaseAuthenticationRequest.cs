using System;

namespace ServerLibrary.Server.Request
{
    [Serializable]
    public enum AuthenticationType
    {
        Login,
        Register
    }

    [Serializable]
    public abstract class AuthenticationRequest
    {
        public AuthenticationType AuthenticationType;
        public string Username;
        public string Password;

        protected AuthenticationRequest(string username, string password, AuthenticationType authenticationType)
        {
            Username = username;
            Password = password;
            AuthenticationType = authenticationType;
        }
    }
}
