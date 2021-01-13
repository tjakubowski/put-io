using System;

namespace ServerLibrary.Server.Request
{
    [Serializable]
    public class RemoveMessageRequest
    {
        public int MessageId;

        public RemoveMessageRequest(int messageId)
        {
            MessageId = messageId;
        }
    }
}
