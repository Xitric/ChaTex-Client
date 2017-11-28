using System.Windows;
using ChaTex_Client.UserDialogs;
using IO.ChaTex.Models;
using IO.ChaTex;
using Microsoft.Rest;
using System.Threading.Tasks;

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
        
        private async Task<bool> deleteChannel(int channelId)
        {
            try
            {
                HttpOperationResponse response = await channelsApi.DeleteChannelWithHttpMessagesAsync(channelId);

                //Check for network error, as this does not generate an exception
                if (response.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Unable to connect to server. The channel could not be deleted.", "Delete channel");
                    return false;
                }
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                return false;
            }

            return true;
        }

        private async Task<bool> updateChannelName(int channelId, string channelName)
        {
            try
            {
                HttpOperationResponse response = await channelsApi.UpdateChannelWithHttpMessagesAsync(channelId, channelName);

                //Check for network error, as this does not generate an exception
                if (response.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Unable to connect to server. The channel name could not be changed.", "Update channel");
                    return false;
                }
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                return false;
            }

            return true;
        }

        private async void btnDeleteChannel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure, you want to delete: {channel.Name}?", "Delete channel", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (result != MessageBoxResult.Yes) return;

            bool success = await deleteChannel(channel.Id);

            if (success)
            {
                MessageBox.Show("The channel was succesfully deleted!", "Delete channel");
                DialogResult = true;
                Close();
            }
        }

        private async void btnSaveChannel_Click(object sender, RoutedEventArgs e)
        {
            bool success = await updateChannelName(channel.Id, txtChannelName.Text);

            if (success)
            {
                DialogResult = true;
                Close();
            }
        }
    }
}
