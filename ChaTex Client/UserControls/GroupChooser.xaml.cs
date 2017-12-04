using ChaTex_Client.UserDialogs;
using IO.ChaTex;
using IO.ChaTex.Models;
using Microsoft.Rest;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChaTex_Client.UserControls
{
    /// <summary>
    /// Interaction logic for GroupView.xaml
    /// </summary>
    public partial class GroupChooser : UserControl
    {
        private readonly IUsers usersApi;
        private readonly IChannels channelsApi;

        private readonly ChannelView ucChannelView;
        private readonly GroupView ucGroupView;
        private readonly CreateGroup createGroupView;
        private EditableElementView currentView;
        private ObservableCollection<GroupDTO> groups;

        public GroupChooser(IUsers usersApi, IChannels channelsApi, ChannelView ucChannelView, GroupView ucGroupView, CreateGroup createGroupView)
        {
            this.usersApi = usersApi;
            this.channelsApi = channelsApi;

            InitializeComponent();

            this.ucChannelView = ucChannelView;
            this.ucGroupView = ucGroupView;
            this.createGroupView = createGroupView;
        }

        private async Task update()
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

        private void enterChannel(ChannelDTO channel)
        {
            exitCurrentView();

            ucChannelView.ChannelDeleted += ucChannelView_ChannelDeleted;
            ucChannelView.ChannelRenamed += ucChannelView_ChannelRenamed;

            txtViewName.Text = channel.Name;
            ucChannelView.SetChannel(channel.Id);

            currentView = ucChannelView;
            bViewArea.Child = ucChannelView;

            btnExit.Visibility = Visibility.Visible;
            btnEdit.Visibility = Visibility.Visible;
        }

        private void enterGroup(GroupDTO group)
        {
            exitCurrentView();

            txtViewName.Text = group.Name;
            ucGroupView.SetGroup(group);

            currentView = ucGroupView;
            bViewArea.Child = ucGroupView;

            btnExit.Visibility = Visibility.Visible;
            btnEdit.Visibility = Visibility.Visible;
        }

        private void enterCreateGroup()
        {
            exitCurrentView();

            txtViewName.Text = "Create your group";
            createGroupView.Reset();


            bViewArea.Child = createGroupView;

            btnExit.Visibility = Visibility.Visible;
        }

        private void exitCurrentView()
        {
            if (currentView == ucChannelView)
            {
                ucChannelView.ChannelDeleted += ucChannelView_ChannelDeleted;
                ucChannelView.ChannelRenamed += ucChannelView_ChannelRenamed;
                ucChannelView.SetChannel(null);
            }

            txtViewName.Text = "";
            bViewArea.Child = null;
            btnExit.Visibility = Visibility.Hidden;
            btnEdit.Visibility = Visibility.Hidden;
        }

        private void channelSelectionChanged(object sender, RoutedPropertyChangedEventArgs<Object> e)
        {
            if (e.NewValue is ChannelDTO channel)
            {
                enterChannel(channel);
            }
            else if (e.NewValue is GroupDTO group)
            {
                enterGroup(group);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            exitCurrentView();
        }

        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (currentView.Edit())
            {
                //Something changed
                await update();
            }

            //EditChannel dlgEditChannel = new EditChannel(selectedChannel, channelsApi);
            //bool? result = dlgEditChannel.ShowDialog();

            //if (result == true) await update();
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
                await update();
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
            }
        }

        private async void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible) await update();
        }

        private async void ucChannelView_ChannelDeleted(ChannelDTO channel)
        {
            exitCurrentView();
            await update();
        }

        private async void ucChannelView_ChannelRenamed(ChannelDTO channel)
        {
            txtViewName.Text = channel.Name;
            await update();
        }

        private void btnCreateGroup_Click(object sender, RoutedEventArgs e)
        {
            enterCreateGroup();
        }
    }
}
