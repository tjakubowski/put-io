using System;

namespace ServerLibrary.Server.Response
{
    [Serializable]
    public class AddMessageResponse : BaseResponse
    {
        public AddMessageResponse(bool result, string message = "") : base(result, message)
        {
        }
    }
}
