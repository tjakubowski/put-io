using System;

namespace ServerLibrary.Server.Request
{
    [Serializable]
    public enum ChannelUserActionType
    {
        Add,
        Remove
    }

    [Serializable]
    public abstract class BaseChannelUserRequest
    {
        public ChannelUserActionType ChannelUserActionType;
        public int UserId;
        public int ChannelId;

        protected BaseChannelUserRequest(int userId, int channelId, ChannelUserActionType channelUserActionType)
        {
            UserId = userId;
            ChannelId = channelId;
            ChannelUserActionType = channelUserActionType;
        }
    }
}
