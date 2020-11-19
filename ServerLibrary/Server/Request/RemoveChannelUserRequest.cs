using System;

namespace ServerLibrary.Server.Request
{
    [Serializable]
    public class RemoveChannelUserRequest : BaseChannelUserRequest
    {
        public RemoveChannelUserRequest(string username, int channelId) : base(username, channelId, ChannelUserActionType.Remove)
        {
        }
    }
}
