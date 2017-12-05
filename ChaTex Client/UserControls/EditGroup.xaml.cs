using ChaTex_Client.UserDialogs;
using IO.ChaTex;
using IO.ChaTex.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChaTex_Client.UserControls
{
    /// <summary>
    /// Interaction logic for EditGroup.xaml
    /// </summary>
    public partial class EditGroup : UserControl
    {
        private readonly IGroups groupsApi;
        private readonly IChannels channelsApi;
        private readonly GroupSettings ucGroupSettings;

        public event Action<GroupDTO> GroupUpdated;

        private GroupDTO group;
        private IList<int?> originalUsers;
        private IList<int?> originalRoles;
        private IList<ChannelDTO> originalChannels;

        public EditGroup(IGroups groupsApi, IChannels channelsApi, GroupSettings ucGroupSettings)
        {
            this.groupsApi = groupsApi;
            this.channelsApi = channelsApi;

            InitializeComponent();

            this.ucGroupSettings = ucGroupSettings;
            ucGroupSettings.GroupValidStateChanged += UcGroupSettings_GroupValidStateChanged;
            DockPanel.SetDock(ucGroupSettings, Dock.Top);
            dpnlSettings.Children.Add(ucGroupSettings);
        }

        public async Task SetGroup(GroupDTO group)
        {
            svMasterScroll.ScrollToTop();
            ucGroupSettings.Reset();

            this.group = group;

            try
            {
                originalUsers = (await groupsApi.GetAllDirectGroupUsersAsync(group.Id)).Select(u => (int?)u.Id).ToList();
                originalRoles = (await groupsApi.GetAllGroupRolesAsync(group.Id)).Select(r => (int?)r.Id).ToList();
                originalChannels = group.Channels;
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                return;
            }

            await ucGroupSettings.LoadFromGroup(group);
        }

        private List<int?> getAddedUsers()
        {
            List<int?> selectedUsers = ucGroupSettings.GetSelectedUsers();
            return selectedUsers.Where(u => !originalUsers.Contains(u)).ToList();
        }

        private List<int?> getRemovedUsers()
        {
            List<int?> selectedUsers = ucGroupSettings.GetSelectedUsers();
            return originalUsers.Where(u => !selectedUsers.Contains(u)).ToList();
        }

        private List<int?> getAddedRoles()
        {
            List<int?> selectedRoles = ucGroupSettings.GetSelectedRoles();
            return selectedRoles.Where(u => !originalRoles.Contains(u)).ToList();
        }

        private List<int?> getRemovedRoles()
        {
            List<int?> selectedRoles = ucGroupSettings.GetSelectedRoles();
            return originalRoles.Where(u => !selectedRoles.Contains(u)).ToList();
        }

        private List<string> getAddedChannels()
        {
            List<ChannelDTO> currentChannels = ucGroupSettings.GetChannels();
            return currentChannels.Where(c => c.Id == -1).Select(c => c.Name).ToList();
        }

        private List<int?> getRemovedChannels()
        {
            List<ChannelDTO> currentChannels = ucGroupSettings.GetChannels();
            return originalChannels.Where(original =>
            {
                foreach (ChannelDTO current in currentChannels)
                {
                    if (current.Id == original.Id) return false;
                }

                return true;
            }).Select(c => (int?)c.Id).ToList();
        }

        private async void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            List<int?> addedUsers = getAddedUsers();
            List<int?> removedUsers = getRemovedUsers();
            List<int?> addedRoles = getAddedRoles();
            List<int?> removedRoles = getRemovedRoles();
            List<string> addedChannels = getAddedChannels();
            List<int?> removedChannels = getRemovedChannels();

            try
            {
                if (addedUsers.Count != 0)
                {
                    await groupsApi.AddUsersToGroupAsync(group.Id, addedUsers);
                }

                if (removedUsers.Count != 0)
                {
                    //await groupsApi.RemoveUsersFromGroupAsync(groupId, removedUsers);
                }

                if (addedRoles.Count != 0)
                {
                    await groupsApi.AddRolesToGroupAsync(group.Id, addedRoles);
                }

                if (removedRoles.Count != 0)
                {
                    //await groupsApi.RemoveRolesFromGroupAsync(groupId, removedRoles);
                }

                foreach (string channelName in addedChannels)
                {
                    await channelsApi.CreateChannelAsync(group.Id, channelName);
                }

                foreach (int channelId in removedChannels)
                {
                    await channelsApi.DeleteChannelAsync(channelId);
                }
            }
            catch(HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                return;
            }

            GroupUpdated?.Invoke(group);
        }

        private void UcGroupSettings_GroupValidStateChanged(bool valid)
        {
            btnSaveChanges.IsEnabled = valid;
        }
    }
}
