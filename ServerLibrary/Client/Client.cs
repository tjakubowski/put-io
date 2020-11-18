using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ServerLibrary.Models;
using ServerLibrary.Server;
using ServerLibrary.Server.Response;
using ServerLibrary.Server.Request;

namespace ServerLibrary.Client
{
    public class Client
    {
        protected TcpClient tcpClient;
        protected NetworkStream stream;

        public Action ReceivedDataAction;

        public ObservableCollection<Channel> Channels;
        public Channel Channel;

        public async void HandleResponses()
        {
            while (tcpClient.Connected)
            {
                try
                {
                    var readBytes = await ReadBytes();
                    var response = MessageSerializer.Deserialize(new TcpMessage(readBytes));

                    if (response is ChannelsResponse channelsResponse && channelsResponse.Result)
                        Channels = new ObservableCollection<Channel>(channelsResponse.Channels);

                    else if (response is ChannelResponse channelResponse && channelResponse.Result)
                        Channel = channelResponse.Channel;

                    ReceivedDataAction();
                }
                catch (IOException)
                {
                    return;
                }
            }
        }

        public void AddChannel(string name)
        {
            var request = new AddChannelRequest(name);
            var serializedRequest = MessageSerializer.Serialize(request);
            stream.Write(serializedRequest.Data, 0, serializedRequest.Data.Length);
        }

        public void ChangeChannel(int id)
        {
            var request = new ChannelRequest(id);
            var serializedRequest = MessageSerializer.Serialize(request);
            stream.Write(serializedRequest.Data, 0, serializedRequest.Data.Length);
        }

        public Client(string hostname, int port)
        {
            tcpClient = new TcpClient(hostname, port);
            stream = tcpClient.GetStream();
        }

        public async Task<LoginResponse> SendLoginRequest(string username, string password)
        {
            var request = new LoginRequest(username, password);
            var serializedRequest = MessageSerializer.Serialize(request);
            stream.Write(serializedRequest.Data, 0, serializedRequest.Data.Length);

            var responseData = await ReadBytes();
            var response = (LoginResponse)MessageSerializer.Deserialize(new TcpMessage(responseData));

            if (response.Result)
                Channels = new ObservableCollection<Channel>(response.Channels);

            return response;
        }

        public async Task<RegisterResponse> SendRegisterRequest(string username, string password)
        {
            var request = new RegisterRequest(username, password);
            var serializedRequest = MessageSerializer.Serialize(request);
            stream.Write(serializedRequest.Data, 0, serializedRequest.Data.Length);

            var responseData = await ReadBytes();
            var response = (RegisterResponse)MessageSerializer.Deserialize(new TcpMessage(responseData));

            if (response.Result)
                Channels = new ObservableCollection<Channel>(response.Channels);

            return response;
        }

        public void SendChannelRequest()
        {
            ChangeChannel(Channels[0].Id);
        }

        public async Task<byte[]> ReadBytes()
        {
            MemoryStream messageStream = new MemoryStream();
            var buffer = new byte[2048];
            do
            {
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                await messageStream.WriteAsync(buffer, 0, bytesRead);
            } while (stream.DataAvailable);

            return messageStream.ToArray();
        }

        /*public void StartClient()
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
        }*/

        private void HandleClient()
        {
        }

        /*protected void HandleClient()
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

        }*/

        /*public void SendLoginData(string log, string pass)
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

        }*/

        /*public void SendRegisterData(string log, string pass)
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

        public void SendMessage(string msg)
        {
            MessageForm form = new MessageForm(msg);
            TcpMessage message = MessageSerializer.Serialize(form);
            stream.Write(message.Data, 0, message.Data.Length);
        }*/
    }
}