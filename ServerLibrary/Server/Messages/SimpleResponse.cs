using System;

namespace ServerLibrary.Server.Messages
{
    [Serializable]
    public class SimpleResponse
    {
        public bool Result;
        public string Message;

        public SimpleResponse(bool result, string message = "")
        {
            Result = result;
            Message = message;
        }
    }
}
