using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Server.Messages
{
    [Serializable]
    public enum ChannelUserActionType
    {
        Add,
        Remove
    }

    [Serializable]
    public class ChannelUserForm
    {
        public ChannelUserActionType ChannelUserActionType;
        public int UserId;
        public int ChannelId;

        public ChannelUserForm(int userId, int channelId, ChannelUserActionType channelUserActionType = ChannelUserActionType.Add)
        {
            UserId = userId;
            ChannelId = channelId;
            ChannelUserActionType = channelUserActionType;
        }
    }
}
