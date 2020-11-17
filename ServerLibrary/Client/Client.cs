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
    public class Client
    {
        protected TcpClient tcpC;
        protected Stream stream;
        protected bool loggedIn;
        protected bool conn;
        private byte[] readBytes;

        public Client()
        {
            tcpC = new TcpClient("127.0.0.1", 2048);
            stream = tcpC.GetStream();
            loggedIn = false;
            conn = false;
        }

        public void StartClient()
        {
            try
            {
                RunSession();
                tcpC.Close();
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public void RunSession()
        {
            Task.Run(() =>
            {
                HandleClient();
                //TODO close.client()
            });
        }

        protected void HandleClient()
        {
            while (!loggedIn)
            {
                try
                {
                    //#TO DO stream.Read();
                    var request = MessageSerializer.Deserialize(new TcpMessage(readBytes));
                    //if ('OK')     #TO DO
                    //{
                    //    loggedIn = true;
                    //}
                }catch(ArgumentNullException)
                {
                    continue;
                }
            }


            if (loggedIn)
            {
                while (this.conn == true)
                {
                    try
                    {
                        
                        //TcpMessage msg = stream.Read();
                        //var request = MessageSerializer.Deserialize(new TcpMessage(readBytes));

                        //if (request is AuthenticationForm authenticationForm)
                        //{
                        //    switch (authenticationForm.AuthenticationType)
                        //    {
                        //        case AuthenticationType.Login:
                        //            Login(session, authenticationForm);
                        //            break;
                        //        case AuthenticationType.Register:
                        //            Register(session, authenticationForm);
                        //            break;
                        //    }
                        //}

                        //if (session.User == null)
                        //    continue;

                        //if (request is ChangePasswordForm changePasswordForm)
                        //{
                        //    ChangePassword(session, changePasswordForm);
                        //}
                    }
                    catch (ArgumentNullException)
                    {
                        return;
                    }
                }
            }

        }

        public void SendLoginData(string log, string pass)
        {
            try
            {
                AuthenticationForm form = new AuthenticationForm(log, pass, AuthenticationType.Login);
                TcpMessage login_msg = MessageSerializer.Serialize(form);
                stream.Write(login_msg.Data, 0, login_msg.Data.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(String.Format("Error: {0}", e.StackTrace));
            }

        }

        public void SendRegisterData(string log, string pass)
        {
            try
            {
                AuthenticationForm form = new AuthenticationForm(log, pass, AuthenticationType.Register);
                TcpMessage reg_msg = MessageSerializer.Serialize(form);
                stream.Write(reg_msg.Data, 0, reg_msg.Data.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(String.Format("Error: {0}", e.StackTrace));
            }

        }

        public void SendMessage(string msg)
        {
            MessageForm form = new MessageForm(msg);
            TcpMessage message = MessageSerializer.Serialize(form);
            stream.Write(message.Data, 0, message.Data.Length);
        }
    }
}