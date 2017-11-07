using IO.Swagger.Api;
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
        private ObservableCollection<GroupDTO> groups;
        private readonly UsersApi usersApi;
        ChannelDTO selectedChannel;
        private readonly ChannelsApi channelsApi;

        public static GroupView m_Instance;

        private GroupView()
        {
            InitializeComponent();
            usersApi = new UsersApi();
            channelsApi = new ChannelsApi();
        }

        private void populateUI()
        {
            groups = new ObservableCollection<GroupDTO>(usersApi.GetGroupsForUser());
            tvGroups.ItemsSource = groups;
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

        private void ChannelSelectionChanged(object sender, RoutedPropertyChangedEventArgs<Object> e)
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
        }

        private void ucChannelMessageView_Loaded(object sender, RoutedEventArgs e)
        {
            
          

        }

        private void miDeleteChannel_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            ChannelDTO channel = (ChannelDTO)menuItem.DataContext;
            MessageBoxResult result = MessageBox.Show("Are you sure, you want to delete: " + channel.Name + "?","Delete channel", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    channelsApi.DeleteChannel(channel.Id);
                    MessageBox.Show("The channel was succesfully deleted!", "Delete channel");
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}
