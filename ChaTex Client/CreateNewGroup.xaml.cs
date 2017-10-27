using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;
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

namespace ChaTex_Client
{
    /// <summary>
    /// Interaction logic for CreateNewGroup.xaml
    /// </summary>
    public partial class CreateNewGroup : Window
    {
       
        public CreateNewGroup()
        {
            InitializeComponent();
            List<GroupDTO> Groups = new List<GroupDTO>();
            UsersApi usersApi = new UsersApi();
            Groups = usersApi.GetGroupsForUser();
            LBroles.ItemsSource = Groups;
            LBmembers.ItemsSource = Groups;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IGroupsApi groupsApi = new GroupsApi();
            IChannelsApi channelApi = new ChannelsApi();
            try
            {
                //groupsApi.CreateGroup(TBgroupName.Text, false, false, false);
                //groupsApi.DeleteGroup(8);
                channelApi.CreateChannel(6, "EtNavn");


                //if some boxes are checked for roles
                //make a new list roles for roleIDs
                //groupApi.AddRolesToGroup(thenewgroup.Id, newListRoles)

                //checkboxes should be members not in checked roles
                //if some boxes are checked for members
                //make a new list users for userIDs
                //groupApi.AddUsersToGroup (thenewgroup.ID, newListUsers)

            }
            catch (ApiException er)
            {
                    throw er;
            }
        }
    }
}
