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
using IO.Swagger.Api;
using IO.Swagger.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ChaTex_Client.UserControls;

namespace ChaTex_Client {
    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : Window {

        private ObservableCollection<GroupDTO> groups;

        public Overview() {

            InitializeComponent();

            UsersApi usersApi = new UsersApi();   //new instans of UserApi    
            groups = new ObservableCollection<GroupDTO>(usersApi.GetGroupsForUser());

            //tvGroups.ItemsSource = groups;
        }

        private void ChannelSelectionChanged(object sender, RoutedPropertyChangedEventArgs<Object> e) {
            Console.WriteLine("Selection change!");
            
            if (e.NewValue is ChannelDTO channel)
            {
                ucChannelMessageView.SetChannel((int) channel.Id);
            }
        }

        private void btnEditChannel_Click(object sender, RoutedEventArgs e)
        {
            //TODO
            //var wEditChannel = new EditChannel();
            //wEditChannel.ShowDialog();
        }
        
        private void btnNewGroup_Click(object sender, RoutedEventArgs e)
        {
            CreateNewGroup createNewGroup = new CreateNewGroup();
            createNewGroup.ShowDialog();
        }

        private void ChannelMessageView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnChat_Click(object sender, RoutedEventArgs e)
        {
            spnlMainUI.Children.Clear();
            spnlMainUI.Children.Add(new ChatView());
        }

        private void btnGroups_Click(object sender, RoutedEventArgs e)
        {
            spnlMainUI.Children.Clear();
            spnlMainUI.Children.Add(new ChannelMessageView());
        }
    }
}
