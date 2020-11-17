using System;

namespace ServerLibrary.Server.Response
{
    [Serializable]
    public class LoginResponse : BaseResponse
    {
        public LoginResponse(bool result, string message = "") : base(result, message)
        {
        }
    }
}
