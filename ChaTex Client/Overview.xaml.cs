using System.Windows;
using ChaTex_Client.UserControls;

namespace ChaTex_Client
{
    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : Window {

        public Overview() {
            InitializeComponent();
            dpnlMainUI.Children.Add(GroupView.GetInstance());
        }
        
        private void btnAddGroup_Click(object sender, RoutedEventArgs e)
        {
            CreateNewGroup createNewGroup = new CreateNewGroup();
            createNewGroup.ShowDialog();
            dpnlMainUI.Children.Clear();
            dpnlMainUI.Children.Add(GroupView.GetInstance());
        }


        private void btnChat_Click(object sender, RoutedEventArgs e)
        {
            dpnlMainUI.Children.Clear();
            dpnlMainUI.Children.Add(ChatView.GetInstance());
        }

        private void btnGroups_Click(object sender, RoutedEventArgs e)
        {
            dpnlMainUI.Children.Clear();
            dpnlMainUI.Children.Add(GroupView.GetInstance());
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
