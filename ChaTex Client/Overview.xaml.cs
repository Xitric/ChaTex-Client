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

namespace ChaTex_Client {
    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : Window {

        private Window CreateNewGroup;
        private ObservableCollection<GroupDTO> groups;

        public Overview() {
            InitializeComponent();

          
            
            // Dummy data
            //Group g = new Group();
            //g.Name = "ABC";
            //g.Channels = new List<Channel>();
            //Channel c1 = new Channel();
            //c1.Name = "TC1";

            //g.Channels.Add(c1);
            //groups.Add(g);
            // End of dummy data
            UsersApi usersApi = new UsersApi();       
            groups = new ObservableCollection<GroupDTO>(usersApi.GetGroupsForUser());
            TViewGroups.ItemsSource = groups;
        }

        private void NewGroupBtn_Click(object sender, RoutedEventArgs e)
        {

            //GroupDTO newGroup = new GroupDTO();
            //newGroup.Id = 112312321;
            //newGroup.Name = "TestGroup123";
            //newGroup.Channels = new List<ChannelDTO>();
            //ChannelDTO c1 = new ChannelDTO();
            //c1.Name = "Channel 123";
            //newGroup.Channels.Add(c1);
            //groups.Add(newGroup);

              CreateNewGroup = new CreateNewGroup();
             CreateNewGroup.Show();
        }

    }
}
