using System;

namespace ServerLibrary.Server.Request
{
    [Serializable]
    public class ChangePasswordRequest
    {
        public string Password;

        public ChangePasswordRequest(string password)
        {
            Password = password;
        }
    }
}
