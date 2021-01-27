using System;
using System.Collections.ObjectModel;
using System.Windows;
using Chat.ViewModels.Base;
using Prism.Commands;
using ServerLibrary.Models;

namespace Chat.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        public ObservableCollection<Channel> Channels => App.Client.Channels;
        public ObservableCollection<User> ChannelUsers => App.Client.ChannelUsers;
        public User User => App.Client.User;

        public bool IsAdmin => User.Admin;

        public bool IsChannelDeletable => Channel?.Id != 1;

        public bool AutoScrollEvent
        {
            get => Get<bool>();
            set => Set(value);
        }

        public Channel Channel
        {
            get
            {
                AutoScrollEvent = true;
                return App.Client.Channel;
            }
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

        public RelayCommand DeleteChannelCommand => new RelayCommand(o =>
        {
            App.Client.SendDeleteChannelRequest(Channel);
        });

        public RelayCommand RemoveChannelUserCommand => new RelayCommand(o =>
        {
            var username = (string)o;
            App.Client.SendRemoveChannelUserRequest(username, Channel);
        });

        public RelayCommand DeleteMessageCommand => new RelayCommand(o =>
        {
            var message = (Message)o;
            App.Client.DeleteMessage(message);
        });

        public RelayCommand AddChannelCommand => new RelayCommand(o =>
        {
            App.Client.AddChannel(NewChannelName);
            NewChannelName = string.Empty;
        });

        public RelayCommand SendMessageCommand => new RelayCommand(o =>
        {
            App.Client.SendMessage(Message);
            Message = string.Empty;
        });

        public RelayCommand AddChannelUserNameCommand => new RelayCommand(o =>
        {
            App.Client.AddUser(NewChannelUserName);
            NewChannelUserName = string.Empty;
        });

        public RelayCommand ChangeUserPasswordCommand => new RelayCommand(o =>
        {
            App.Client.SendChangePasswordRequest(NewUserPassword);
            NewUserPassword = string.Empty;
        });

        public ChatViewModel()
        {
            var client = App.Client;
            client.ReceivedDataAction = () => { OnPropertyChanged("Channel"); OnPropertyChanged("Channels"); OnPropertyChanged("ChannelUsers"); OnPropertyChanged("IsChannelDeletable"); };

            client.SendChannelRequest();
            client.HandleResponses();
        }
    }
}
