using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLibrary.Models;

namespace ServerLibrary.Server.Messages
{
    [Serializable]
    class ChannelsResponse
    {
        public List<Channel> Channels;
    }
}
