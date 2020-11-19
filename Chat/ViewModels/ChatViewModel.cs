using System;
using System.Collections.ObjectModel;
using System.Windows;
using Chat.ViewModels.Base;
using ServerLibrary.Models;

namespace Chat.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        public ObservableCollection<Channel> Channels => App.Client.Channels;
        public Channel Channel => App.Client.Channel;

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

        public ChatViewModel()
        {
            var client = App.Client;
            client.ReceivedDataAction = () => { OnPropertyChanged("Channel"); OnPropertyChanged("Channels"); };

            client.SendChannelRequest();
            client.HandleResponses();
        }
    }
}
