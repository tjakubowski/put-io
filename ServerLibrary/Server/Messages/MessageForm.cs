using System;

namespace ServerLibrary.Server.Messages
{
    [Serializable]
    public class MessageForm
    {
        public int ChannelId;
        public string Text;

        public MessageForm(int channelId, string text)
        {
            ChannelId = channelId;
            Text = text;
        }
    }
}
