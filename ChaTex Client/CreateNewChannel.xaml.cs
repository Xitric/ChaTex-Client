using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChaTex_Client
{
    /// <summary>
    /// Interaction logic for CreateNewChannel.xaml
    /// </summary>
    public partial class CreateNewChannel : Window
    {
        private readonly IUsersApi usersApi;
        private readonly IChannelsApi channelsApi;
        private bool groupSelected = false;
        public CreateNewChannel()
        {
            InitializeComponent();
            usersApi = new UsersApi();
            channelsApi = new ChannelsApi();
            populateUI();
        }
        private void populateUI()
        {
            try
            {
                lstBoxGroups.ItemsSource = usersApi.GetGroupsForUser();
            }
            catch (ApiException er)
            {
                MessageBox.Show("An error occured: " + er.InnerException.Message);
                throw er;
            }
        }

        private void btnCreateChannel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                channelsApi.CreateChannel(((GroupDTO)lstBoxGroups.SelectedItem).Id, new CreateChannelDTO()
                {
                    Name = txtChannelName.Text
                });
            }
            catch (ApiException er)
            {
                MessageBox.Show("An error occured: " + er.InnerException.Message);
                throw er;
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
