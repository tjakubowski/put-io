﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ServerLibrary.Models;
using ServerLibrary.Server.Request;
using ServerLibrary.Server.Response;

namespace ServerLibrary.Server
{
    public class TcpServerAsync : TcpServer
    {
        private List<TcpServerSession> sessions;

        public TcpServerAsync(IPAddress ipAddress, int port, string loggerPath = "./logs.txt") : base(ipAddress, port, loggerPath)
        {
            sessions = new List<TcpServerSession>();
        }

        public override void Start()
        {
            StartListening();

            while (true)
                AcceptClient();
        }

        protected override void AcceptClient()
        {
            TcpClient tcpClient = _listener.AcceptTcpClient();
            TcpServerSession session = new TcpServerSession(tcpClient);
            sessions.Add(session);

            Logger.Log($"Client connected");

            Task.Run(() =>
            {
                HandleClientSession(session);
                CloseClientSession(session);
            });
        }

        protected override void HandleClientSession(TcpServerSession session)
        {
            while (session.Client.Connected)
            {
                try
                {
                    var readBytes = session.ReadBytes();
                    var request = MessageSerializer.Deserialize(new TcpMessage(readBytes));

                    if (session.User == null)
                    {
                        if (request is LoginRequest loginRequest)
                            Login(session, loginRequest);

                        else if (request is RegisterRequest registerRequest)
                            Register(session, registerRequest);

                        continue;
                    }

                    if (request is ChangePasswordRequest changePasswordRequest)
                        ChangePassword(session, changePasswordRequest);

                    else if (request is ChannelRequest channelRequest)
                        MoveToChannel(session, channelRequest);

                    else if (request is AddChannelRequest addChannelRequest)
                        AddChannel(session, addChannelRequest);

                    else if (request is RemoveChannelRequest removeChannelRequest)
                        RemoveChannel(session, removeChannelRequest);

                    else if (request is AddMessageRequest addMessageRequest)
                        AddNewMessage(session, addMessageRequest);

                    else if (request is AddChannelUserRequest addChannelUserRequest)
                        AddChannelUser(session, addChannelUserRequest);

                    else if (request is RemoveChannelUserRequest removeChannelUserRequest)
                        RemoveChannelUser(session, removeChannelUserRequest);

                    else if (request is RemoveMessageRequest removeMessageRequest)
                        RemoveMessage(session, removeMessageRequest);
                }
                catch (Exception e)
                {
                    return;
                }
            }
        }

        private void AddChannel(TcpServerSession session, AddChannelRequest addChannelRequest)
        {
            try
            {
                if (!session.User.Admin)
                    throw new Exception();

                using (var context = new DatabaseContext())
                {
                    var user = context.Users.Single(u => u.Id == session.User.Id);
                    var channel = new Channel
                    {
                        Name = addChannelRequest.Name,
                        Users = new List<User> { user }
                    };

                    context.Channels.Add(channel);
                    context.SaveChanges();

                    Logger.Log(
                        $"[New channel] Admin {session.User.Username} created a new channel {channel.Name}");

                    UpdateChannels();
                }
            }
            catch(Exception e)
            {
                var response = new AddChannelResponse(false, "Channel not created");
                var serializedResponse = MessageSerializer.Serialize(response);
                session.SendBytes(serializedResponse.Data);
                Logger.Log(e.Message);
            }
        }

        private void RemoveChannel(TcpServerSession session, RemoveChannelRequest removeChannelRequest)
        {
            try
            {
                if (!session.User.Admin)
                    throw new Exception();

                using (var context = new DatabaseContext())
                {
                    var channel = context.Channels.Single(ch => ch.Id == removeChannelRequest.ChannelId);
                    context.Channels.Remove(channel);
                    context.SaveChanges();

                    Logger.Log(
                        $"[Remove channel] Admin {session.User.Username} deleted {channel.Name} channel");

                    var filteredSessions = sessions.Where(s => s.ChannelId == removeChannelRequest.ChannelId && s.User != null);
                    foreach (var userSession in filteredSessions)
                        MoveToChannel(userSession, new ChannelRequest(1));

                    UpdateChannels();
                }
            }
            catch
            {
                var response = new AddChannelResponse(false, "Channel not deleted");
                var serializedResponse = MessageSerializer.Serialize(response);
                session.SendBytes(serializedResponse.Data);
            }
        }

        private void RemoveMessage(TcpServerSession session, RemoveMessageRequest removeMessageRequest)
        {
            try
            {
                if (!session.User.Admin)
                    throw new Exception();

                using (var context = new DatabaseContext())
                {
                    var message = context.Messages.Single(m => m.Id == removeMessageRequest.MessageId);
                    context.Messages.Remove(message);
                    context.SaveChanges();

                    Logger.Log(
                        $"[Remove message] Admin {session.User.Username} deleted {message.Id} message");

                    UpdateChannel(message.ChannelId);
                }
            }
            catch
            {
                var response = new AddChannelResponse(false, "Message not deleted");
                var serializedResponse = MessageSerializer.Serialize(response);
                session.SendBytes(serializedResponse.Data);
            }
        }

        private void RemoveChannelUser(TcpServerSession session, RemoveChannelUserRequest removeChannelUserRequest)
        {
            try
            {
                if (removeChannelUserRequest.ChannelId == 1 || !session.User.Admin)
                    throw new Exception();

                using (var context = new DatabaseContext())
                {
                    var user = context.Users.Single(u => u.Username == removeChannelUserRequest.Username);
                    var channel = context.Channels.Include(ch => ch.Users).Single(ch => ch.Id == removeChannelUserRequest.ChannelId);


                    channel.Users.Remove(user);
                    context.SaveChanges();

                    Logger.Log(
                        $"[Remove channel user] User {user.Username} has been removed from the channel {channel.Name}");

                    var sessionToMove = sessions.SingleOrDefault(s => s.User.Id == user.Id );
                    if(sessionToMove != null)
                    {
                        MoveToChannel(sessionToMove, new ChannelRequest(1));
                    }
                    
                    UpdateChannel(channel.Id);
                }
            }
            catch
            {
                var response = new RemoveChannelUserResponse(false, "User not removed from the channel");
                var serializedResponse = MessageSerializer.Serialize(response);
                session.SendBytes(serializedResponse.Data);
            }
        }

        private void AddChannelUser(TcpServerSession session, AddChannelUserRequest addChannelUserRequest)
        {
            try
            {
                if (!session.User.Admin)
                    throw new Exception();

                using (var context = new DatabaseContext())
                {
                    var user = context.Users.Single(u => u.Username == addChannelUserRequest.Username);
                    var channel = context.Channels.Single(ch => ch.Id == addChannelUserRequest.ChannelId);

                    channel.Users.Add(user);
                    context.SaveChanges();

                    Logger.Log($"[Add channel user] User {user.Username} has been added to the channel {channel.Name}");

                    UpdateChannels(user.Id);
                    UpdateChannel(channel.Id);
                }
            }
            catch
            {
                var response = new RemoveChannelUserResponse(false, "User not added to the channel");
                var serializedResponse = MessageSerializer.Serialize(response);
                session.SendBytes(serializedResponse.Data);
            }
        }

        private void AddNewMessage(TcpServerSession session, AddMessageRequest addMessageRequest)
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    var message = new Message()
                    {
                        ChannelId = addMessageRequest.ChannelId,
                        UserId = session.User.Id,
                        Text = addMessageRequest.Text,
                    };

                    context.Messages.Add(message);
                    context.SaveChanges();

                    Logger.Log(
                        $"[New message] Client {session.User.Username} sent a new message on channel {session.ChannelId}");

                    UpdateChannel(addMessageRequest.ChannelId);
                }
            }
            catch
            {
                var response = new AddMessageResponse(false, "Message not added");
                var serializedResponse = MessageSerializer.Serialize(response);
                session.SendBytes(serializedResponse.Data);
            }
        }

        private void UpdateChannel(int channelId)
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    var filteredSessions = sessions.Where(s => s.ChannelId == channelId && s.User != null).ToList();

                    var channel = context.Channels
                        .Where(ch => ch.Id == channelId)
                        .Include(ch => ch.Users) // TODO: Exclude User.Password
                        .Include(ch => ch.Messages)
                        .Single();

                    var activeUsers = sessions.Where(s => s.User != null && s.User.Active).Select(s => s.User.Id).ToList();
                    var users = channel.Users.ToList();
                    users.ForEach(u => u.Active = activeUsers.Contains(u.Id));
                    channel.Users = users;

                    var channelResponse = new ChannelResponse(channel);
                    var serializedResponse = MessageSerializer.Serialize(channelResponse);

                    foreach (var session in filteredSessions)
                        session.SendBytes(serializedResponse.Data);

                    Logger.Log($"[Channel update] Updated channel '{channel.Name}' for {filteredSessions.Count} users");
                }
            }
            catch(Exception e)
            {
                Logger.Log($"[Channel update] Update not sent");
            }
        }

        private void UpdateChannels()
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    foreach (var session in sessions)
                    {
                        if (session.User == null) continue;

                        var channels = context.Channels
                            .Where(ch => ch.Users.Any(u => u.Id == session.User.Id)) // TODO: Exclude User.Password
                            .ToList();

                        var channelResponse = new ChannelsResponse(channels);
                        var serializedResponse = MessageSerializer.Serialize(channelResponse);
                        session.SendBytes(serializedResponse.Data);
                    }

                    Logger.Log($"[Channels update] Updated all channels for {sessions.Count} users");
                }
            }
            catch
            {
                Logger.Log($"[Channels update] Update not sent");
            }
        }

        private void UpdateChannels(int userId)
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    var session = sessions.Single(s => s.User.Id == userId);

                    var channels = context.Channels
                        .Where(ch => ch.Users.Any(u => u.Id == session.User.Id)) // TODO: Exclude User.Password
                        .ToList();

                    var channelResponse = new ChannelsResponse(channels);
                    var serializedResponse = MessageSerializer.Serialize(channelResponse);
                    session.SendBytes(serializedResponse.Data);

                    Logger.Log($"[Channels update] Updated all channels for {session.User.Username} user");
                }
            }
            catch
            {
                Logger.Log($"[Channels update] Update not sent");
            }
        }

        private void MoveToChannel(TcpServerSession session, ChannelRequest channelRequest)
        {
            var response = new ChannelResponse();

            try
            {
                using (var context = new DatabaseContext())
                {
                    var channel = context.Channels
                        .Where(ch => ch.Id == channelRequest.ChannelId)
                        .Include(ch => ch.Users)
                        .Where(ch => ch.Users.Any(u => u.Id == session.User.Id))
                        .Include(ch => ch.Messages)
                        .Single();

                    var activeUsers = sessions.Where(s=> s.User != null && s.User.Active).Select(s => s.User.Id).ToList();
                    var users = channel.Users.ToList();
                    users.ForEach(u => u.Active = activeUsers.Contains(u.Id));
                    channel.Users = users;

                    Logger.Log(
                        $"[Channel request] Client {session.User.Username} requested channel {channel.Name}");

                    response.Channel = channel;
                }
            }
            catch(Exception e)
            {
                response.Result = false;
                response.Message = "Channel access denied";
            }
            finally
            {
                var serializedResponse = MessageSerializer.Serialize(response);
                session.SendBytes(serializedResponse.Data);
                session.ChannelId = channelRequest.ChannelId;
            }
        }

        private void CloseClientSession(TcpServerSession session)
        {
            session.User.Active = false;

            using (var context = new DatabaseContext())
            {
                var channels = context.Channels
                    .Where(ch => ch.Users.Any(u => u.Id == session.User.Id))
                    .ToList();
                channels.ForEach(ch => UpdateChannel(ch.Id));
            }

            sessions.Remove(session);
            session.Client.Close();

            if(session.User != null)
                Logger.Log($"[Logout] User {session.User.Username} logged out.");

            Logger.Log($"[Close Session] Client disconnected");
        }

        private void Login(TcpServerSession session, LoginRequest request)
        {
            var response = new LoginResponse(true);

            try
            {
                if (sessions.Exists(s => s.User?.Username == request.Username))
                    throw new UnauthorizedAccessException();

                using (var context = new DatabaseContext())
                {
                    var hash = User.CreatePassword(request.Password);
                    session.User = context.Users
                        .Single(u => u.Username == request.Username && u.Password == hash);

                    var channels = context.Channels
                        .Where(ch => ch.Users.Any(u => u.Id == session.User.Id)) // TODO: Exclude User.Password
                        .ToList();
                    response.Channels = channels;
                    response.User = session.User;

                    session.User.Active = true;
                    channels.ForEach(ch => UpdateChannel(ch.Id));

                    Logger.Log($"[Login] User {session.User.Username} logged in");
                }
            }
            catch(UnauthorizedAccessException e)
            {
                response.Result = false;
                response.Message = $"User with that username is already logged in'";
            }
            catch
            {
                response.Result = false;
                response.Message = $"User with that username and password doesn't exist'";
            }
            finally
            {
                var serializedResponse = MessageSerializer.Serialize(response);
                session.SendBytes(serializedResponse.Data);
            }

        }

        private void ChangePassword(TcpServerSession session, ChangePasswordRequest request)
        {
            var response = new ChangePasswordResponse(true);

            try
            {
                using (var context = new DatabaseContext())
                {
                    session.User.Password = User.CreatePassword(request.Password);
                    context.Users.Attach(session.User);
                    context.Entry(session.User).State = EntityState.Modified;
                    context.SaveChanges();

                    Logger.Log($"[ChangePassword] User {session.User.Username} changed password");
                }
            }
            catch
            {
                response.Result = false;
                response.Message = $"Password couldn't be changed";
            }
            finally
            {
                var serializedResponse = MessageSerializer.Serialize(response);
                session.SendBytes(serializedResponse.Data);
            }
        }

        private void Register(TcpServerSession session, RegisterRequest request)
        {
            var response = new RegisterResponse(true);

            try
            {
                using (var context = new DatabaseContext())
                {
                    var generalChannel = context.Channels.Single(ch => ch.Id == 1);

                    var user = new User()
                    {
                        Username = request.Username,
                        Password = User.CreatePassword(request.Password)
                    };
                    user.Channels.Add(generalChannel);

                    context.Users.Add(user);
                    context.SaveChanges();

                    Logger.Log($"[Register] User {user.Username} registered");
                }
            }
            catch
            {
                response.Result = false;
                response.Message = $"User {request.Username} already exists";
            }
            finally
            {
                var serializedResponse = MessageSerializer.Serialize(response);
                session.SendBytes(serializedResponse.Data);
            }
        }

    }
}
