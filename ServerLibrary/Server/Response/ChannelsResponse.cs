using System;
using System.Collections.Generic;
using ServerLibrary.Models;

namespace ServerLibrary.Server.Response
{
    [Serializable]
    public class ChannelsResponse : BaseResponse
    {
        public List<Channel> Channels;

        public ChannelsResponse(string message = "") : base(false, message)
        {
        }

        public ChannelsResponse(List<Channel> channels) : base(true)
        {
            Channels = channels;
        }
    }
}
