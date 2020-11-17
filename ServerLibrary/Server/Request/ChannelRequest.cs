using System;

namespace ServerLibrary.Server.Request
{
    [Serializable]
    public class ChannelRequest
    {
        public int ChannelId;

        public ChannelRequest(int channelId)
        {
            ChannelId = channelId;
        }
    }
}
