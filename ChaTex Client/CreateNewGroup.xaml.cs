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
        private IUsersApi usersApi;
        private IRolesApi rolesApi = new RolesApi();

        private String employeeAcknowledgeableString = "Is acknowledgeable allowed for employees?";
        private String employeeBookmarkString = "Is bookmarks allowed for employees?";
        private String employeeStickyString = "Is stickies allowed for employees";

        public CreateNewGroup()
        {
            InitializeComponent();
            usersApi = new UsersApi();
            rolesApi = new RolesApi();
            populateUI();
           
           
        }

        private void populateUI()
        {
            lstBoxUsers.ItemsSource = usersApi.GetAllUsers();
            lstBoxRoles.ItemsSource = rolesApi.GetAllRoles();

            List<String> AllowableList = new List<string>();
            AllowableList.Add(employeeAcknowledgeableString);
            AllowableList.Add(employeeBookmarkString);
            AllowableList.Add(employeeStickyString);
            lstBoxAllowables.ItemsSource = AllowableList;
        }

        private void btnCreateGroup_Click(object sender, RoutedEventArgs e)
        {
            List<int?> rolesToAdd = new List<int?>();
            List<int?> usersToAdd = new List<int?>();
            bool AllowEA = false;
            bool AllowEB = false;
            bool AllowES = false;
            try

            {
                GroupsApi groupsApi = new GroupsApi();

                foreach (String item in lstBoxAllowables.Items)
                {
                    if ((lstBoxAllowables.SelectedItems.Contains(item) == true) & (item.Equals(employeeAcknowledgeableString)))
                    {
                        AllowEA = true;
                    }
                    if ((lstBoxAllowables.SelectedItems.Contains(item) == true) & (item.Equals(employeeBookmarkString)))
                    {
                        AllowEB = true;
                    }
                    if ((lstBoxAllowables.SelectedItems.Contains(item) == true) & (item.Equals(employeeStickyString)))
                    {
                        AllowES = true;
                    }
                }

                GroupDTO group = groupsApi.CreateGroup(new CreateGroupDTO()
                {
                    AllowEmployeeAcknowledgeable = AllowEA,
                    AllowEmployeeBookmark = AllowEB,
                    AllowEmployeeSticky = AllowES,
                    GroupName = txtGroupName.Text
                });

                foreach (RoleDTO item in lstBoxRoles.Items)
                {
                    if (lstBoxRoles.SelectedItems.Contains(item) == true)
                    {
                        rolesToAdd.Add(item.Id);
                    }
                }

                foreach (UserDTO item in lstBoxUsers.Items)
                {
                    if (lstBoxUsers.SelectedItems.Contains(item) == true)
                    {
                        usersToAdd.Add(item.Id);
                    }
                }


                groupsApi.AddRolesToGroup(new AddRolesToGroupDTO()
                {
                    GroupId = group.Id,
                    RoleIds = rolesToAdd
                });

                groupsApi.AddUsersToGroup(new AddUsersToGroupDTO()
                {
                    GroupId = group.Id,
                    UserIds = usersToAdd
                });

                MessageBox.Show("The group has now been created!");


            }
            catch (ApiException er)
            {
                MessageBox.Show("An error occured: " + er.InnerException.Message);
                throw er;
            }

            finally
            {
                Close();
            }
        }

        private void txtGroupName_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnCreateGroup.IsEnabled = txtGroupName.Text.Length > 0;
        }
    }
}
