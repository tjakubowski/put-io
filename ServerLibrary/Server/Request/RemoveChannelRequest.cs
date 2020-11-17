using System;

namespace ServerLibrary.Server.Request
{
    [Serializable]
    public class RemoveChannelRequest
    {
        public int ChannelId;

        public RemoveChannelRequest(int channelId)
        {
            ChannelId = channelId;
        }
    }
}
