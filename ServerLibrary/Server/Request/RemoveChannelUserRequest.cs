using System;

namespace ServerLibrary.Server.Request
{
    [Serializable]
    public class RemoveChannelUserRequest : BaseChannelUserRequest
    {
        public RemoveChannelUserRequest(int userId, int channelId) : base(userId, channelId, ChannelUserActionType.Remove)
        {
        }
    }
}
