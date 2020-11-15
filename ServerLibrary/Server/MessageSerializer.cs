using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ServerLibrary.Server
{
    public static class MessageSerializer
    {
        public static TcpMessage Serialize(object serializableObject)
        {
            using (var memoryStream = new MemoryStream())
            {
                (new BinaryFormatter()).Serialize(memoryStream, serializableObject);
                return new TcpMessage(memoryStream.ToArray());
            }
        }

        public static object Deserialize(TcpMessage tcpMessage)
        {
            using (var memoryStream = new MemoryStream(tcpMessage.Data))
                return (new BinaryFormatter()).Deserialize(memoryStream);
        }
    }
}
