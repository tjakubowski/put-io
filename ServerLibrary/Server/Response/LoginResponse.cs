using System;
using System.Collections.Generic;
using ServerLibrary.Models;

namespace ServerLibrary.Server.Response
{
    [Serializable]
    public class LoginResponse : BaseResponse
    {
        public List<Channel> Channels;

        public LoginResponse(bool result, string message = "") : base(result, message)
        {
        }
    }
}
