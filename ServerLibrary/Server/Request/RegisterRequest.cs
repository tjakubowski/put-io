using System;

namespace ServerLibrary.Server.Request
{
    [Serializable]
    public class RegisterRequest : AuthenticationRequest
    {
        public RegisterRequest(string username, string password) : base(username, password, AuthenticationType.Register)
        {
        }
    }
}
