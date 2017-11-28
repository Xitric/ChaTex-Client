using ChaTex_Client.UserDialogs;
using IO.ChaTex;
using IO.ChaTex.Models;
using Microsoft.Rest;
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
        private readonly IUsers usersApi;
        private readonly IRoles rolesApi;
        private readonly IGroups groupsApi;

        private String employeeAcknowledgeableString = "Is acknowledgeable allowed for employees?";
        private String employeeBookmarkString = "Is bookmarks allowed for employees?";
        private String employeeStickyString = "Is stickies allowed for employees";

        public CreateNewGroup(IUsers usersApi, IRoles rolesApi, IGroups groupsApi)
        {
            this.usersApi = usersApi;
            this.rolesApi = rolesApi;
            this.groupsApi = groupsApi;

            InitializeComponent();
            populateUI();
        }

        private async void populateUI()
        {
            try
            {
                lstBoxUsers.ItemsSource = await usersApi.GetAllUsersAsync();
            }
            catch (HttpOperationException er)
            {
                //TODO: Exception handling
                throw er;
            }

            try
            {
                lstBoxRoles.ItemsSource = rolesApi.GetAllRoles();
            }
            catch (HttpOperationException er)
            {
                //TODO: Exception handling
                throw er;
            }

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
            catch (HttpOperationException er)
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
