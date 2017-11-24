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

        private readonly GroupView groupView;
        private readonly ChatView chatView;
        private UserControl currentView;

        public Overview(GroupView groupView, ChatView chatView, IUsers usersApi, IRoles rolesApi)
        {
            this.usersApi = usersApi;
            this.rolesApi = rolesApi;

            this.groupView = groupView;
            this.chatView = chatView;

            InitializeComponent();
            setCurrentView(groupView);
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
        
        private void btnAddGroup_Click(object sender, RoutedEventArgs e)
        {
            CreateNewGroup createNewGroup = new CreateNewGroup(usersApi, rolesApi);
            createNewGroup.ShowDialog();

            setCurrentView(groupView);
        }


        private void btnChat_Click(object sender, RoutedEventArgs e)
        {
            setCurrentView(chatView);
        }

        private void btnGroups_Click(object sender, RoutedEventArgs e)
        {
            setCurrentView(groupView);
        }

        private void btnAddChannel_Click(object sender, RoutedEventArgs e)
        {
            CreateNewChannel createNewChannel = new CreateNewChannel();
            createNewChannel.ShowDialog();
            dpnlMainUI.Children.Clear();
            dpnlMainUI.Children.Add(GroupView.GetInstance());
        }
    }
}
