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

        public GroupView()
        {
            InitializeComponent();

            usersApi = new UsersApi();
            groups = new ObservableCollection<GroupDTO>(usersApi.GetGroupsForUser());

            tvGroups.ItemsSource = groups;
        }

        private void ChannelSelectionChanged(object sender, RoutedPropertyChangedEventArgs<Object> e)
        {
            if (e.NewValue is ChannelDTO channel)
            {
                ucChannelMessageView.SetChannel((int)channel.Id);
            }
        }

        private void btnEditChannel_Click(object sender, RoutedEventArgs e)
        {
            //TODO
            //var wEditChannel = new EditChannel();
            //wEditChannel.ShowDialog();
        }

        private void ucChannelMessageView_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
