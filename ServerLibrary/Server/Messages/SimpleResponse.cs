using System;

namespace ServerLibrary.Server.Messages
{
    [Serializable]
    class SimpleResponse
    {
        public bool Result;
        public string Message;

        public SimpleResponse(bool result, string message = "")
        {
            this.Result = result;
            this.Message = message;
        }
    }
}
