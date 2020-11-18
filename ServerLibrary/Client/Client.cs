using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using ServerLibrary.Server;
using ServerLibrary.Server.Response;
using ServerLibrary.Server.Request;

namespace ServerLibrary.Client
{
    public class Client
    {
        protected TcpClient tcpC;
        protected Stream stream;
        protected bool loggedIn;
        protected bool conn;

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
                    var readBytes = ReadBytes();
                    var response = MessageSerializer.Deserialize(new TcpMessage(readBytes));
                    if (response is LoginResponse loginResponse)
                    {
                        if (loginResponse.Result)
                        {
                            loggedIn = true;
                        }
                    }else if(response is RegisterResponse registerResponse)
                    {
                        if (registerResponse.Result)
                        {
                            
                        }
                    }
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
                        
                        var msg = ReadBytes();
                        var response = MessageSerializer.Deserialize(new TcpMessage(readBytes));

                        if (response is AuthenticationForm authenticationForm)
                        {
                            switch (authenticationForm.AuthenticationType)
                            {
                                case AuthenticationType.Login:
                                    Login(session, authenticationForm);
                                    break;
                                case AuthenticationType.Register:
                                    Register(session, authenticationForm);
                                    break;
                            }
                        }

                        if (request is ChangePasswordForm changePasswordForm)
                        {
                            ChangePassword(session, changePasswordForm);
                        }
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
                LoginRequest form = new LoginRequest(log, pass);
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
                RegisterRequest form = new RegisterRequest(log, pass);
                TcpMessage reg_msg = MessageSerializer.Serialize(form);
                stream.Write(reg_msg.Data, 0, reg_msg.Data.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(String.Format("Error: {0}", e.StackTrace));
            }
        }
        public byte[] ReadBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
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