using ChaTex_Client.UserDialogs;
using IO.ChaTex;
using IO.ChaTex.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        private async Task populateUsersListBox()
        {
            try
            {
                IList<UserDTO> allUsers = await usersApi.GetAllUsersAsync();

                if (allUsers == null)
                {
                    new ErrorDialog("Network error", "Unable to contact server.\nPlease inform your administrator.").ShowDialog();
                }
                else
                {
                    lstBoxUsers.ItemsSource = allUsers;
                }
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
            }
        }

        private async Task populateRolesListBox()
        {
            try
            {
                IList<RoleDTO> allRoles = await rolesApi.GetAllRolesAsync();

                if (allRoles == null)
                {
                    new ErrorDialog("Network error", "Unable to contact server.\nPlease inform your administrator.").ShowDialog();
                }
                else
                {
                    lstBoxUsers.ItemsSource = allRoles;
                }
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
            }
        }

        private async void populateUI()
        {
            await populateUsersListBox();
            await populateRolesListBox();

            List<String> allowableList = new List<string>
            {
                employeeAcknowledgeableString,
                employeeBookmarkString,
                employeeStickyString
            };
            lstBoxAllowables.ItemsSource = allowableList;
        }

        private async void btnCreateGroup_Click(object sender, RoutedEventArgs e)
        {
            List<int?> usersToAdd = new List<int?>();
            bool allowEmployeeAcknowledgeable = isEmployeeAcknowledgeableAllowed();
            bool allowEmployeeBookmark = isEmployeeBookmarkAllowed();
            bool allowEmployeeSticky = isEmployeeStickyAllowed();

            var group = await createGroup(new CreateGroupDTO()
            {
                AllowEmployeeAcknowledgeable = allowEmployeeAcknowledgeable,
                AllowEmployeeBookmark = allowEmployeeBookmark,
                AllowEmployeeSticky = allowEmployeeSticky,
                GroupName = txtGroupName.Text
            });

            if (group == null) return;

            bool rolesAdded = await addSelectedRolesToGroup(group);
            bool usersAdded = await addSelectedUsersToGroup(group);

            if (!rolesAdded || !usersAdded) return;

            MessageBox.Show("The group has now been created!");
            Close();
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

        private async Task<GroupDTO> createGroup(CreateGroupDTO group)
        {
            GroupDTO createdGroup = null;

            try
            {
                createdGroup = await groupsApi.CreateGroupAsync();

                if (createdGroup == null)
                {
                    new ErrorDialog("Network error", "Unable to contact server.\nPlease inform your administrator.").ShowDialog();
                }
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
            }

            return createdGroup;
        }

        private async Task<bool> addSelectedRolesToGroup(GroupDTO group)
        {
            List<int?> selectedRoles = new List<int?>();
            foreach (RoleDTO roleDto in lstBoxRoles.Items)
            {
                if (lstBoxRoles.SelectedItems.Contains(roleDto) == true)
                {
                    selectedRoles.Add(roleDto.Id);
                }
            }

            try
            {
                HttpOperationResponse response = await groupsApi.AddRolesToGroupWithHttpMessagesAsync(new AddRolesToGroupDTO()
                {
                    GroupId = group.Id,
                    RoleIds = selectedRoles
                });

                //Check for network error, as this does not generate an exception
                if (response.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Unable to connect to server. The channel name could not be changed.", "Update channel");
                    return false;
                }
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                return false;
            }

            return true;
        }

        private async Task<bool> addSelectedUsersToGroup(GroupDTO group)
        {
            List<int?> selectedUsers = new List<int?>();
            foreach (UserDTO userDto in lstBoxUsers.Items)
            {
                if (lstBoxUsers.SelectedItems.Contains(userDto) == true)
                {
                    selectedUsers.Add(userDto.Id);
                }
            }

            try
            {
                HttpOperationResponse response = await groupsApi.AddUsersToGroupWithHttpMessagesAsync(new AddUsersToGroupDTO()
                {
                    GroupId = group.Id,
                    UserIds = selectedUsers
                });

                //Check for network error, as this does not generate an exception
                if (response.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Unable to connect to server. The channel name could not be changed.", "Update channel");
                    return false;
                }
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                return false;
            }

            return true;
        }

        private void txtGroupName_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnCreateGroup.IsEnabled = txtGroupName.Text.Length > 0;
        }
    }
}
