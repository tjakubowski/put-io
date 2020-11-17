using System;

namespace ServerLibrary.Server.Response
{
    [Serializable]
    public abstract class BaseResponse
    {
        public bool Result;
        public string Message;

        protected BaseResponse(bool result, string message = "")
        {
            Result = result;
            Message = message;
        }
    }
}
