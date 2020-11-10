using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ServerLibrary.Server
{
    public static class TcpMessageSerializer
    {
        public static TcpMessage Serialize(object anySerializableObject)
        {
            using (var memoryStream = new MemoryStream())
            {
                (new BinaryFormatter()).Serialize(memoryStream, anySerializableObject);
                return new TcpMessage { Data = memoryStream.ToArray() };
            }
        }

        public static object Deserialize(TcpMessage tcpMessage)
        {
            using (var memoryStream = new MemoryStream(tcpMessage.Data))
                return (new BinaryFormatter()).Deserialize(memoryStream);
        }
    }
}
