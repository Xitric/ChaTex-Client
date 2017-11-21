using System.Windows;
using IO.Swagger.Api;
using IO.Swagger.Model;



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
            channelApi.DeleteChannel(selectedChannel.Id);
            MessageBox.Show("The channel was succesfully deleted!", "Delete channel");
            Close();  //the edit window will be closed upon sucseful deleting channel
        }

    private void btnSaveChannel_Click(object sender, RoutedEventArgs e)
    {
        channelApi.UpdateChannel(selectedChannel.Id, txtChannelName.Text);
        Close();
    }
}
}
