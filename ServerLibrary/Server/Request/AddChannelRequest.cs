using System;

namespace ServerLibrary.Server.Request
{
    [Serializable]
    public class AddChannelRequest
    {
        public string Name;

        public AddChannelRequest(string name)
        {
            Name = name;
        }
    }
}
