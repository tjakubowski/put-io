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

        public User User;
        public ObservableCollection<Channel> Channels;
        public Channel Channel;
        public ObservableCollection<User> ChannelUsers;

        public async void HandleResponses()
        {
            while (tcpClient.Connected)
            {
                try
                {
                    var readBytes = await ReadBytes();
                    var response = MessageSerializer.Deserialize(new TcpMessage(readBytes));

                    if (response is ChannelsResponse channelsResponse && channelsResponse.Result)
                    {
                        Channels = new ObservableCollection<Channel>(channelsResponse.Channels);
                        Channel = Channels.FirstOrDefault(ch => ch.Id == Channel.Id) ?? Channels[0];
                    }

                    else if (response is ChannelResponse channelResponse && channelResponse.Result)
                        Channel = channelResponse.Channel;

                    ChannelUsers = new ObservableCollection<User>(Channel.Users);

                    ReceivedDataAction();
                }
                catch (IOException)
                {
                    return;
                }
            }
        }
        public void SendMessage(string text)
        {
            var request = new AddMessageRequest(Channel.Id, text);
            var serializedRequest = MessageSerializer.Serialize(request);
            stream.Write(serializedRequest.Data, 0, serializedRequest.Data.Length);
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
        
        public void DeleteMessage(Message message)
        {
            var request = new RemoveMessageRequest(message.Id);
            var serializedRequest = MessageSerializer.Serialize(request);
            stream.Write(serializedRequest.Data, 0, serializedRequest.Data.Length);
        }

        public void AddUser(string username)
        {
            var request = new AddChannelUserRequest(username, Channel.Id);
            var serializedRequest = MessageSerializer.Serialize(request);
            stream.Write(serializedRequest.Data, 0, serializedRequest.Data.Length);
        }

        public void SendChangePasswordRequest(string password)
        {
            var request = new ChangePasswordRequest(password);
            var serializedRequest = MessageSerializer.Serialize(request);
            stream.Write(serializedRequest.Data, 0, serializedRequest.Data.Length);
        }

        public void SendDeleteChannelRequest(Channel channel)
        {
            var request = new RemoveChannelRequest(channel.Id);
            var serializedRequest = MessageSerializer.Serialize(request);
            stream.Write(serializedRequest.Data, 0, serializedRequest.Data.Length);
        }

        public Client(string hostname, int port)
        {
            if(!IsPortValid(port))
            {
                throw new FormatException("Wrong port value");
            }

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
            { 
                Channels = new ObservableCollection<Channel>(response.Channels);
                User = response.User;
            }
                

            return response;
        }

        public async Task<RegisterResponse> SendRegisterRequest(string username, string password)
        {
            var request = new RegisterRequest(username, password);
            var serializedRequest = MessageSerializer.Serialize(request);
            stream.Write(serializedRequest.Data, 0, serializedRequest.Data.Length);

            var responseData = await ReadBytes();
            var response = (RegisterResponse)MessageSerializer.Deserialize(new TcpMessage(responseData));

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
        private bool IsPortValid(int port)
        {
            return port >= 1024 && port <= 49151;
        }

        private void HandleClient()
        {
        }
    }
}