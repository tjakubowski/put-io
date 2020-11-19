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
        public string Username;
        public int ChannelId;

        protected BaseChannelUserRequest(string username, int channelId, ChannelUserActionType channelUserActionType)
        {
            Username = username;
            ChannelId = channelId;
            ChannelUserActionType = channelUserActionType;
        }
    }
}
