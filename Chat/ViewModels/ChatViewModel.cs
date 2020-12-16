﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using Chat.ViewModels.Base;
using ServerLibrary.Models;

namespace Chat.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        public ObservableCollection<Channel> Channels => App.Client.Channels;
        public ObservableCollection<User> ChannelUsers => App.Client.ChannelUsers;
        public Channel Channel => App.Client.Channel;
        public User User => App.Client.User;

        public bool IsAdmin
        {
            get => User.Admin;
        }

        public string NewChannelUserName
        {
            get => Get<string>();
            set => Set(value);
        }

        public string NewUserPassword
        {
            get => Get<string>();
            set => Set(value);
        }

        public string NewChannelName
        {
            get => Get<string>();
            set => Set(value);
        }

        public string Message
        {
            get => Get<string>();
            set => Set(value);
        }

        public Channel SelectedChannel
        {
            get => Get<Channel>();
            set
            {
                if (value == null)
                    value = Channels[0];
                Set(value);
                App.Client.ChangeChannel(SelectedChannel.Id);
            }
        }

        public RelayCommand AddChannelCommand => new RelayCommand(o =>
        {
            App.Client.AddChannel(NewChannelName);
        });

        public RelayCommand SendMessageCommand => new RelayCommand(o =>
        {
            App.Client.SendMessage(Message);
        });
        
        public RelayCommand AddChannelUserNameCommand => new RelayCommand(o =>
        {
            App.Client.AddUser(NewChannelUserName);
        });

        public RelayCommand ChangeUserPasswordCommand => new RelayCommand(o =>
        {
            App.Client.SendChangePasswordRequest(NewUserPassword);
        });

        public ChatViewModel()
        {
            var client = App.Client;
            client.ReceivedDataAction = () => { OnPropertyChanged("Channel"); OnPropertyChanged("Channels"); OnPropertyChanged("ChannelUsers"); };

            client.SendChannelRequest();
            client.HandleResponses();
        }
    }
}
