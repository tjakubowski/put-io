using System;

namespace ServerLibrary.Server.Response
{
    [Serializable]
    public class AddChannelResponse : BaseResponse
    {
        public AddChannelResponse(bool result, string message = "") : base(result, message)
        {
        }
    }
}
