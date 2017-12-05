using ChaTex_Client.UserDialogs;
using IO.ChaTex;
using IO.ChaTex.Models;
using Microsoft.Rest;
using System.Windows;
using System.Windows.Controls;

namespace ChaTex_Client
{
    /// <summary>
    /// Interaction logic for CreateNewChannel.xaml
    /// </summary>
    public partial class CreateNewChannel : Window
    {
        private readonly IUsers usersApi;
        private readonly IChannels channelsApi;
        private bool groupSelected = false;

        public CreateNewChannel(IUsers usersApi, IChannels channelsApi)
        {
            InitializeComponent();
            this.usersApi = usersApi;
            this.channelsApi = channelsApi;
            populateUI();
        }
        private async void populateUI()
        {
            try
            {
                lstBoxGroups.ItemsSource = await usersApi.GetGroupsForUserAsync();
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
            }
        }

        private async void btnCreateChannel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await channelsApi.CreateChannelAsync(((GroupDTO)lstBoxGroups.SelectedItem).Id, txtChannelName.Text);
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
            }
            finally
            {
                Close();
            }
        }

        private void txtChannelName_TextChanged(object sender, TextChangedEventArgs e)
        {
            enableButton();
        }

        private void lstBoxGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!groupSelected)
            {
                groupSelected = true;
            }
            enableButton();
        }

        private void enableButton()
        {
            btnCreateChannel.IsEnabled = (txtChannelName.Text.Length > 0 && groupSelected);
        }
    }
}
