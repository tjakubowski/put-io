using System;

namespace ServerLibrary.Server.Request
{
    [Serializable]
    public class LoginRequest : AuthenticationRequest
    {
        public LoginRequest(string username, string password) : base(username, password, AuthenticationType.Login)
        {
        }
    }
}
