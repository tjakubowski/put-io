using System;

namespace ServerLibrary.Server.Request
{
    [Serializable]
    public class AddChannelUserRequest : BaseChannelUserRequest
    {
        public AddChannelUserRequest(int userId, int channelId) : base(userId, channelId, ChannelUserActionType.Add)
        {
        }
    }
}
