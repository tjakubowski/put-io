namespace ServerLibrary.Server
{
    public class TcpMessage
    {
        public byte[] Data { get; set; }

        public TcpMessage(byte[] data)
        {
            Data = data;
        }
    }
}
