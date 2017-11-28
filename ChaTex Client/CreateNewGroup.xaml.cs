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
            List<int?> rolesToAdd = new List<int?>();
            List<int?> usersToAdd = new List<int?>();
            bool allowEmployeeAcknowledgeable = false;
            bool allowEmployeeBookmark = false;
            bool allowEmployeeSticky = false;

            try
            {
                foreach (String item in lstBoxAllowables.Items)
                {
                    if ((lstBoxAllowables.SelectedItems.Contains(item) == true) & (item.Equals(employeeAcknowledgeableString)))
                    {
                        allowEmployeeAcknowledgeable = true;
                    }
                    if ((lstBoxAllowables.SelectedItems.Contains(item) == true) & (item.Equals(employeeBookmarkString)))
                    {
                        allowEmployeeBookmark = true;
                    }
                    if ((lstBoxAllowables.SelectedItems.Contains(item) == true) & (item.Equals(employeeStickyString)))
                    {
                        allowEmployeeSticky = true;
                    }
                }

                GroupDTO group = groupsApi.CreateGroup(new CreateGroupDTO()
                {
                    AllowEmployeeAcknowledgeable = allowEmployeeAcknowledgeable,
                    AllowEmployeeBookmark = allowEmployeeBookmark,
                    AllowEmployeeSticky = allowEmployeeSticky,
                    GroupName = txtGroupName.Text
                });

                foreach (RoleDTO roleDto in lstBoxRoles.Items)
                {
                    if (lstBoxRoles.SelectedItems.Contains(roleDto) == true)
                    {
                        rolesToAdd.Add(roleDto.Id);
                    }
                }

                foreach (UserDTO userDto in lstBoxUsers.Items)
                {
                    if (lstBoxUsers.SelectedItems.Contains(userDto) == true)
                    {
                        usersToAdd.Add(userDto.Id);
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

        private void txtGroupName_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnCreateGroup.IsEnabled = txtGroupName.Text.Length > 0;
        }
    }
}
