using System;

namespace ServerLibrary.Server.Response
{
    [Serializable]
    public class AddChannelUserResponse : BaseResponse
    {
        public AddChannelUserResponse(bool result, string message = "") : base(result, message)
        {
        }
    }
}
