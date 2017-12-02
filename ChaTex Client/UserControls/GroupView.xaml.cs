using ChaTex_Client.UserDialogs;
using IO.ChaTex;
using IO.ChaTex.Models;
using Microsoft.Rest;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChaTex_Client.UserControls
{
    /// <summary>
    /// Interaction logic for GroupView.xaml
    /// </summary>
    public partial class GroupView : UserControl
    {
        private readonly IUsers usersApi;
        private readonly IChannels channelsApi;

        private readonly ChannelMessageView ucChannelMessageView;
        private ChannelDTO selectedChannel;
        private ObservableCollection<GroupDTO> groups;

        public GroupView(IUsers usersApi, IChannels channelsApi, ChannelMessageView ucChannelMessageView)
        {
            this.usersApi = usersApi;
            this.channelsApi = channelsApi;

            InitializeComponent();
            this.ucChannelMessageView = ucChannelMessageView;
            this.ucChannelMessageView.ChannelDeleted += ucChannelMessageView_ChannelDeleted;
            this.ucChannelMessageView.ChannelRenamed += ucChannelMessageView_ChannelRenamed;
            bChannelMessageArea.Child = ucChannelMessageView;
            ucChannelMessageView.Visibility = Visibility.Hidden;
        }

        public async Task Update()
        {
            try
            {
                groups = new ObservableCollection<GroupDTO>(await usersApi.GetGroupsForUserAsync());
                tvGroups.ItemsSource = groups;
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
            }
        }

        private void channelSelectionChanged(object sender, RoutedPropertyChangedEventArgs<Object> e)
        {
            if (e.NewValue is ChannelDTO channel)
            {
                selectedChannel = channel;
                txtChannelName.Text = channel.Name;
                ucChannelMessageView.SetChannel(channel.Id);
                ucChannelMessageView.Visibility = Visibility.Visible;
            }
        }

        private async void btnEditChannel_Click(object sender, RoutedEventArgs e)
        {
            if (selectedChannel == null) return;

            EditChannel dlgEditChannel = new EditChannel(selectedChannel, channelsApi);
            bool? result = dlgEditChannel.ShowDialog();
            
            if (result == true) await Update();
        }

        private async void miDeleteChannel_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            ChannelDTO channel = (ChannelDTO)menuItem.DataContext;

            MessageBoxResult result = MessageBox.Show("Are you sure, you want to delete: " + channel.Name + "?", "Delete channel", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                await channelsApi.DeleteChannelAsync(channel.Id);
                MessageBox.Show("The channel was succesfully deleted!", "Delete channel");
                await Update();
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
            }
        }

        private async void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible) await Update();
        }

        private async void ucChannelMessageView_ChannelDeleted(ChannelDTO channel)
        {
            ucChannelMessageView.SetChannel(null);
            txtChannelName.Text = "";
            ucChannelMessageView.Visibility = Visibility.Hidden;
            await Update();
        }

        private async void ucChannelMessageView_ChannelRenamed(ChannelDTO channel)
        {
            txtChannelName.Text = channel.Name;
            await Update();
        }
    }
}
