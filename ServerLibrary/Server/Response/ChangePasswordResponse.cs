using System;

namespace ServerLibrary.Server.Response
{
    [Serializable]
    public class ChangePasswordResponse : BaseResponse
    {
        public ChangePasswordResponse(bool result, string message = "") : base(result, message)
        {
        }
    }
}
