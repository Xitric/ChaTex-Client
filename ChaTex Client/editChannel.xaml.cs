using System.Windows;
using IO.Swagger.Api;
using IO.Swagger.Model;
using IO.Swagger.Client;
using ChaTex_Client.UserDialogs;

namespace ChaTex_Client
{
    /// <summary>
    /// Interaction logic for EditChannel.xaml
    /// </summary>
    public partial class EditChannel : Window
    {
        private readonly ChannelsApi channelApi;
        private readonly ChannelDTO selectedChannel;

        public EditChannel(ChannelDTO channel)
        {
            InitializeComponent();
            channelApi = new ChannelsApi();
            this.selectedChannel = channel;
            txtChannelName.Text = channel.Name;
        }


        //MessageBox for deleting a channel
        private void btnDeleteChannel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure, you want to delete: " + selectedChannel.Name + "?", "Delete channel", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (result != MessageBoxResult.Yes) return;
            try
            {
                channelApi.DeleteChannel(selectedChannel.Id);
            }
            catch (ApiException er)
            {
                new ExceptionDialog(er).ShowDialog();
            }
            MessageBox.Show("The channel was succesfully deleted!", "Delete channel");
            Close();  //the edit window will be closed upon sucseful deleting channel
        }

        private void btnSaveChannel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                channelApi.UpdateChannel(selectedChannel.Id, txtChannelName.Text);
            }
            catch (ApiException er)
            {
                new ExceptionDialog(er).ShowDialog();
            }
            Close();
        }
    }
}
