using ChaTex_Client.UserDialogs;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ChaTex_Client.UserControls
{
    /// <summary>
    /// Interaction logic for GroupView.xaml
    /// </summary>
    public partial class GroupView : UserControl
    {
        private ChannelDTO selectedChannel;
        private ObservableCollection<GroupDTO> groups;
        private readonly UsersApi usersApi;
        private readonly ChannelsApi channelsApi;
        public static GroupView m_Instance;

        private GroupView()
        {
            InitializeComponent();
            usersApi = new UsersApi();
            channelsApi = new ChannelsApi();
        }

        public static GroupView GetInstance()
        {
            if (m_Instance == null)
            {
                m_Instance = new GroupView();
            }

            m_Instance.populateUI();
            return m_Instance;
        }

        private void populateUI()
        {
            try
            {
                groups = new ObservableCollection<GroupDTO>(usersApi.GetGroupsForUser());
                tvGroups.ItemsSource = groups;
            }
            catch (ApiException er)
            {
                switch (er.ErrorCode)
                {
                    case 401:
                        new ErrorDialog("Authentication failed", "You do not have access to the groups of this user.").ShowDialog();
                        break;
                    case 404:
                        new ErrorDialog("Not found", "The specified user does not exist.").ShowDialog();
                        break;
                    default:
                        new ExceptionDialog(er).ShowDialog();
                        break;
                }
            }
        }

        private void channelSelectionChanged(object sender, RoutedPropertyChangedEventArgs<Object> e)
        {            
            if (e.NewValue is ChannelDTO channel)
            {
                selectedChannel = channel;
                ucChannelMessageView.SetChannel((int)channel.Id);
                
            }
        }

        private void btnEditChannel_Click(object sender, RoutedEventArgs e)
        {
            var wEditChannel = new EditChannel(selectedChannel);
            wEditChannel.ShowDialog();
            populateUI();
        }

        private void ucChannelMessageView_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void miDeleteChannel_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            ChannelDTO channel = (ChannelDTO)menuItem.DataContext;
            MessageBoxResult result = MessageBox.Show("Are you sure, you want to delete: " + channel.Name + "?","Delete channel", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            try
            {
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            channelsApi.DeleteChannel(channel.Id);
                            MessageBox.Show("The channel was succesfully deleted!", "Delete channel");
                            populateUI();
                        }
                        catch (ApiException er)
                        {
                            switch (er.ErrorCode)
                            {
                                case 401:
                                    new ErrorDialog("Authentication failed", "You do not have permission to delete this channel.").ShowDialog();
                                    break;
                                case 404:
                                    new ErrorDialog("Not found", "The specified channel does not exist.").ShowDialog();
                                    break;
                                default:
                                    new ExceptionDialog(er).ShowDialog();
                                    break;
                            }
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            catch (ApiException) { }
        }
    }
}
