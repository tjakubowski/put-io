using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using ServerLibrary.Server;
using ServerLibrary.Server.Messages;

namespace ServerLibrary.Client
{
    class Client
    {

        protected Client()
        {
            TcpClient tcpclient = new TcpClient();
        }

        public void StartClient()
        {
            try
            {
                TcpClient tcpclient = new TcpClient();
                tcpclient.Connect("127.0.0.1", 5000);


                tcpclient.Close();
            }

            catch (Exception e)
            {
               //
            }
        }

        public void SendLoginData(string log, string pass)
        {
            try {
                AuthenticationForm form = new AuthenticationForm(log, pass);
                TcpMessage login_msg = MessageSerializer.Serialize(form);
                Stream.Write(login_msg);
            }
            catch (Exception e) {
                Console.WriteLine(String.Format("Error: {0}", e.StackTrace)); 
            }

        }

        public static void SendMessage(int ch_id, string msg)
        {
            MessageForm form = new MessageForm(ch_id, msg);
            TcpMessage message = MessageSerializer.Serialize(form);
            Stream.Write(message);
        }
    }
}