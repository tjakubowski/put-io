using System;

namespace ServerLibrary.Server.Request
{
    [Serializable]
    public class AddMessageRequest
    {
        public int ChannelId;
        public string Text;

        public AddMessageRequest(int channelId, string text)
        {
            ChannelId = channelId;
            Text = text;
        }
    }
}
