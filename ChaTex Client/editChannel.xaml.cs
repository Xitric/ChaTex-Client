using System.Windows;
using ChaTex_Client.UserDialogs;
using IO.ChaTex.Models;
using IO.ChaTex;
using Microsoft.Rest;

namespace ChaTex_Client
{
    /// <summary>
    /// Interaction logic for EditChannel.xaml
    /// </summary>
    public partial class EditChannel : Window
    {
        private readonly IChannels channelsApi;
        private ChannelDTO channel;

        public EditChannel(ChannelDTO channel, IChannels channelsApi)
        {
            this.channelsApi = channelsApi;
            this.channel = channel;

            InitializeComponent();
            txtChannelName.Text = channel.Name;
        }

        private async void btnDeleteChannel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure, you want to delete: " + channel.Name + "?", "Delete channel", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                await channelsApi.DeleteChannelAsync(channel.Id);
            }
            catch (HttpOperationException er)
            {
                //TODO: Handle exception
                throw er;
            }

            MessageBox.Show("The channel was succesfully deleted!", "Delete channel");
            DialogResult = true;
            Close();
        }

        private async void btnSaveChannel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await channelsApi.UpdateChannelAsync(channel.Id, txtChannelName.Text);
            }
            catch (HttpOperationException er)
            {
                //TODO: Handle exception
                throw er;
            }

            DialogResult = true;
            Close();
        }
    }
}
