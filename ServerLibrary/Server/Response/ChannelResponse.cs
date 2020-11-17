using System;
using System.Collections.Generic;
using ServerLibrary.Models;

namespace ServerLibrary.Server.Response
{
    [Serializable]
    public class ChannelResponse : BaseResponse
    {
        public Channel Channel;

        public ChannelResponse(bool result = true, string message = "") : base(result, message)
        {
        }


        public ChannelResponse(Channel channel) : base(true)
        {
            Channel = channel;
        }
    }
}
