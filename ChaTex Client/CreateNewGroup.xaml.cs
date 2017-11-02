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
        private String EA = "EmployeeAcknowledgeable";
        private String EB = "EmployeeBookmark";
        private String ES = "EmployeeSticky";
        public CreateNewGroup()
        {
            InitializeComponent();

            IUsersApi usersApi = new UsersApi();
            IRolesApi rolesApi = new RolesApi();
            LBusers.ItemsSource = usersApi.GetAllUsers();
            LBroles.ItemsSource = rolesApi.GetAllRoles();

            List<String> AllowableList = new List<string>();
            AllowableList.Add(EA);
            AllowableList.Add(EB);
            AllowableList.Add(ES);
            LBallowable.ItemsSource = AllowableList;
        }
       
            private void Button_Click(object sender, RoutedEventArgs e)
              {
                List<int?> rolesToAdd = new List<int?>();
                List<int?> usersToAdd = new List<int?>();
                bool AllowEA = false;
                bool AllowEB = false;
                bool AllowES = false;
            try

            {
                GroupsApi groupsApi = new GroupsApi();

                foreach (String item in LBallowable.Items)
                {
                    if ((LBallowable.SelectedItems.Contains(item) == true) & (item.Equals(EA)))
                    {
                        AllowEA = true;
                    }
                    if ((LBallowable.SelectedItems.Contains(item) == true) & (item.Equals(EB)))
                    {
                        AllowEB = true;
                    }
                    if ((LBallowable.SelectedItems.Contains(item) == true) & (item.Equals(ES)))
                    {
                        AllowES = true;
                    }
                }
                
                GroupDTO group = groupsApi.CreateGroup(new CreateGroupDTO()
                {
                    AllowEmployeeAcknowledgeable = AllowEA,
                    AllowEmployeeBookmark = AllowEB,
                    AllowEmployeeSticky = AllowES,
                    GroupName = TBgroupName.Text
                });
                
                foreach (RoleDTO item in LBroles.Items)
                {
                    if (LBroles.SelectedItems.Contains(item) == true)
                    {
                        rolesToAdd.Add(item.Id);
                    }
                }
            
                foreach (UserDTO item in LBusers.Items)
                {
                    if (LBusers.SelectedItems.Contains(item) == true)
                    {
                        usersToAdd.Add(item.Id);
                    }
                }


                groupsApi.AddRolesToGroup(new AddRolesToGroupDTO()
                {
                     GroupId = group.Id,
                     RoleIds = rolesToAdd
                   } );

                groupsApi.AddUsersToGroup(new AddUsersToGroupDTO()
                {
                   GroupId = group.Id,
                   UserIds = usersToAdd
                });



                /*
                 groupsApi.AddUsersToGroup(new AddUsersToGroupDTO()
                 {
                     UserIds = usersToAdd
                 } );
                 */

            }
            catch (ApiException er)
            {
                //lav message boks med fejlen
                    throw er;
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
