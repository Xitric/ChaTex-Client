using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ChaTex_Client
{
    /// <summary>
    /// Interaction logic for CreateNewGroup.xaml
    /// </summary>
    public partial class CreateNewGroup : Window
    {
        private readonly IUsersApi usersApi;
        private readonly IRolesApi rolesApi;
        private readonly IGroupsApi groupsApi;

        private String employeeAcknowledgeableString = "Is acknowledgeable allowed for employees?";
        private String employeeBookmarkString = "Is bookmarks allowed for employees?";
        private String employeeStickyString = "Is stickies allowed for employees";

        public CreateNewGroup()
        {
            InitializeComponent();
            usersApi = new UsersApi();
            rolesApi = new RolesApi();
            groupsApi = new GroupsApi();
            populateUI();
        }

        private void populateUI()
        {
            lstBoxUsers.ItemsSource = usersApi.GetAllUsers();
            lstBoxRoles.ItemsSource = rolesApi.GetAllRoles();

            List<String> allowableList = new List<string>
            {
                employeeAcknowledgeableString,
                employeeBookmarkString,
                employeeStickyString
            };
            lstBoxAllowables.ItemsSource = allowableList;
        }

        private void btnCreateGroup_Click(object sender, RoutedEventArgs e)
        {
            List<int?> usersToAdd = new List<int?>();
            bool allowEmployeeAcknowledgeable = isEmployeeAcknowledgeableAllowed();
            bool allowEmployeeBookmark = isEmployeeBookmarkAllowed();
            bool allowEmployeeSticky = isEmployeeStickyAllowed();
            try
            {
                GroupDTO group = groupsApi.CreateGroup(new CreateGroupDTO()
                {
                    AllowEmployeeAcknowledgeable = allowEmployeeAcknowledgeable,
                    AllowEmployeeBookmark = allowEmployeeBookmark,
                    AllowEmployeeSticky = allowEmployeeSticky,
                    GroupName = txtGroupName.Text
                });

                addSelectedRolesToGroup(group);
                addSelectedUsersToGroup(group);

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

        private bool isEmployeeAcknowledgeableAllowed()
        {
            return lstBoxAllowables.SelectedItems.Contains(employeeAcknowledgeableString);
        }

        private bool isEmployeeBookmarkAllowed()
        {
            return lstBoxAllowables.SelectedItems.Contains(employeeBookmarkString);
        }

        private bool isEmployeeStickyAllowed()
        {
            return lstBoxAllowables.SelectedItems.Contains(employeeStickyString);
        }

        private void addSelectedRolesToGroup(GroupDTO group)
        {
            List<int?> selectedRoles = new List<int?>();
            foreach (RoleDTO roleDto in lstBoxRoles.Items)
            {
                if (lstBoxRoles.SelectedItems.Contains(roleDto) == true)
                {
                    selectedRoles.Add(roleDto.Id);
                }
            }

            groupsApi.AddRolesToGroup(new AddRolesToGroupDTO()
            {
                GroupId = group.Id,
                RoleIds = selectedRoles
            });
            
        }

        private void addSelectedUsersToGroup(GroupDTO group)
        {
            List<int?> selectedUsers = new List<int?>();
            foreach (UserDTO userDto in lstBoxUsers.Items)
            {
                if (lstBoxUsers.SelectedItems.Contains(userDto) == true)
                {
                    selectedUsers.Add(userDto.Id);
                }
            }

            groupsApi.AddUsersToGroup(new AddUsersToGroupDTO()
            {
                GroupId = group.Id,
                UserIds = selectedUsers
            });
        }

        private void txtGroupName_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnCreateGroup.IsEnabled = txtGroupName.Text.Length > 0;
        }
    }
}
