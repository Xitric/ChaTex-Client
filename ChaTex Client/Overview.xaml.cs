using System.Windows;
using ChaTex_Client.UserControls;
using IO.ChaTex;
using System;
using System.Windows.Controls;

namespace ChaTex_Client
{
    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : Window {

        private readonly IUsers usersApi;
        private readonly IRoles rolesApi;
        private readonly IChannels channelsApi;
        private readonly IGroups groupsApi;

        private readonly GroupChooser groupChooser;
        private readonly ChatView chatView;
        private UserControl currentView;

        public Overview(GroupChooser groupChooser, ChatView chatView, IUsers usersApi, IRoles rolesApi, IChannels channelsApi, IGroups groupsApi)
        {
            this.usersApi = usersApi;
            this.rolesApi = rolesApi;
            this.channelsApi = channelsApi;
            this.groupsApi = groupsApi;

            this.groupChooser = groupChooser;
            this.chatView = chatView;

            InitializeComponent();
            setCurrentView(groupChooser);
        }

        private void setCurrentView(UserControl view)
        {
            if (currentView == view)
            {
                return;
            }

            if (currentView != null) {
                currentView.Visibility = Visibility.Hidden;
            }

            currentView = view;
            currentView.Visibility = Visibility.Visible;
            dpnlMainUI.Children.Clear();
            dpnlMainUI.Children.Add(view);
        }

        private void btnChat_Click(object sender, RoutedEventArgs e)
        {
            setCurrentView(chatView);
        }

        private void btnGroups_Click(object sender, RoutedEventArgs e)
        {
            setCurrentView(groupChooser);
        }
    }
}
