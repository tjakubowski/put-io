namespace ServerLibrary.Server.Response
{
    public class RemoveChannelResponse : BaseResponse
    {
        public RemoveChannelResponse(bool result, string message = "") : base(result, message)
        {
        }
    }
}
