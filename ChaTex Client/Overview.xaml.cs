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

namespace ChaTex_Client {
    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : Window {

        public Overview() {
            InitializeComponent();

            List<GroupDTO> groups = new List<GroupDTO>();
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
            groups = usersApi.GetGroupsForUser();
            TViewGroups.ItemsSource = groups;
        }
    }
}
