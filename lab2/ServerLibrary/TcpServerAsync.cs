using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary
{
    public class TcpServerAsync : TcpServer
    {
        public TcpServerAsync(IPAddress ipAddress, int port) : base(ipAddress, port)
        {
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }
    }
}
