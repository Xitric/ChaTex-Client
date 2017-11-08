using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IO.Swagger.Api;
using IO.Swagger.Model;



namespace ChaTex_Client
{
    /// <summary>
    /// Interaction logic for EditChannel.xaml
    /// </summary>
    public partial class EditChannel : Window
    {
        ChannelsApi _channelApi = new ChannelsApi();
        ChannelDTO _selectedChannel;

        public EditChannel(ChannelDTO channel)
        {
            InitializeComponent();

            this._selectedChannel = channel;
            txtChannelName.Text = channel.Name;
        }


        //MessageBox for deleting a channel
        private void btnDeleteChannel_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Are you sure, you want to delete: " + _selectedChannel.Name + "?", "Delete channel", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                _channelApi.DeleteChannel(_selectedChannel.Id);
                MessageBox.Show("The channel was succesfully deleted!", "Delete channel");

                Close();  //the edit window will be closed upon sucseful deleting channel
            }
        }

    private void btnSaveChannel_Click(object sender, RoutedEventArgs e)
    {
        _channelApi.UpdateChannel(_selectedChannel.Id, txtChannelName.Text);
        Close();
    }
}
}
