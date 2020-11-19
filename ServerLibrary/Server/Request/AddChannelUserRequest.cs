using System;

namespace ServerLibrary.Server.Request
{
    [Serializable]
    public class AddChannelUserRequest : BaseChannelUserRequest
    {
        public AddChannelUserRequest(string username, int channelId) : base(username, channelId, ChannelUserActionType.Add)
        {
        }
    }
}
