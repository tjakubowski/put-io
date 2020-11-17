﻿using System;

namespace ServerLibrary.Server.Response
{
    [Serializable]
    public class RegisterResponse : BaseResponse
    {
        public RegisterResponse(bool result, string message = "") : base(result, message)
        {
        }
    }
}
