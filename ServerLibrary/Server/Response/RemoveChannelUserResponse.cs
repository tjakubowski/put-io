using System;

namespace ServerLibrary.Server.Response
{
    [Serializable]
    public class RemoveChannelUserResponse : BaseResponse
    {
        public RemoveChannelUserResponse(bool result, string message = "") : base(result, message)
        {
        }
    }
}
