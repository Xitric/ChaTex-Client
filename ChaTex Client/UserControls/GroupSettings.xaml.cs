using ChaTex_Client.UserDialogs;
using ChaTex_Client.ViewModels;
using IO.ChaTex;
using IO.ChaTex.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChaTex_Client.UserControls
{
    /// <summary>
    /// Interaction logic for GroupSettings.xaml
    /// </summary>
    public partial class GroupSettings : UserControl
    {
        private readonly IUsers usersApi;
        private readonly IGroups groupsApi;
        private readonly IRoles rolesApi;

        private readonly ObservableCollection<ChannelDTO> channels;
        private Task fetchInformationTask;

        public event Action<bool> GroupValidStateChanged;

        public GroupSettings(IUsers usersApi, IGroups groupsApi, IRoles rolesApi)
        {
            this.usersApi = usersApi;
            this.groupsApi = groupsApi;
            this.rolesApi = rolesApi;

            InitializeComponent();

            channels = new ObservableCollection<ChannelDTO>();
            icChannels.ItemsSource = channels;
        }

        public void Reset()
        {
            fetchInformationTask = UpdateDisplay();
        }

        public async Task LoadFromGroup(GroupDTO group)
        {
            //Wait for current job to finish
            if (fetchInformationTask != null)
            {
                await fetchInformationTask;
            }

            IList<int> users;
            IList<int> roles;

            try
            {
                users = (await groupsApi.GetAllDirectGroupUsersAsync(group.Id)).Select(u => u.Id).ToList();
                roles = (await groupsApi.GetAllGroupRolesAsync(group.Id)).Select(r => r.Id).ToList();
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                return;
            }

            chklstMembers.SelectWhere(val => users.Contains(val));
            chklstRoles.SelectWhere(val => roles.Contains(val));

            foreach (ChannelDTO channel in group.Channels)
            {
                channels.Add(channel);
            }
        }

        public string GetGroupName()
        {
            return txtGroupName.Text;
        }

        public List<int?> GetSelectedUsers()
        {
            return chklstMembers.GetSelectedValues();
        }

        public List<int?> GetSelectedRoles()
        {
            return chklstRoles.GetSelectedValues();
        }

        public List<ChannelDTO> GetChannels()
        {
            return channels.ToList();
        }

        public bool IsAcknowledgeableSelected()
        {
            return chkAcknowledgeable.IsChecked == null ? false : (bool)chkAcknowledgeable.IsChecked;
        }

        public bool IsBookmarkSelected()
        {
            return chkBookmark.IsChecked == null ? false : (bool)chkBookmark.IsChecked;
        }

        public bool IsStickySelected()
        {
            return chkSticky.IsChecked == null ? false : (bool)chkSticky.IsChecked;
        }

        private async Task UpdateDisplay()
        {
            //Wait for current job to finish
            if (fetchInformationTask != null)
            {
                await fetchInformationTask;
            }

            Task userTask = updateUserDisplay();
            Task roleTask = updateRoleDisplay();

            txtGroupName.Clear();
            txtChannelName.Clear();
            channels.Clear();
            chkAcknowledgeable.IsChecked = false;
            chkBookmark.IsChecked = false;
            chkSticky.IsChecked = false;

            await userTask;
            await roleTask;
        }

        private async Task updateUserDisplay()
        {
            try
            {
                chklstMembers.Clear();
                IList<UserDTO> users = await usersApi.GetAllUsersAsync();
                chklstMembers.SetData(users.Where(u => !u.Me).Select(u => new AddMemberViewModel(u)).ToList());
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                return;
            }
        }

        private async Task updateRoleDisplay()
        {
            try
            {
                chklstRoles.Clear();
                IList<RoleDTO> roles = await rolesApi.GetAllRolesAsync();
                chklstRoles.SetData(roles.Select(r => new AddRoleViewModel(r)).ToList());
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                return;
            }
        }

        private void txtGroupName_TextChanged(object sender, TextChangedEventArgs e)
        {
            GroupValidStateChanged?.Invoke(txtGroupName.Text.Length > 0);
        }

        private void btnNewChannel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                channels.Add(new ChannelDTO()
                {
                    Name = txtChannelName.Text
                });

                txtChannelName.Text = "";
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                return;
            }
        }

        private void txtChannelName_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnNewChannel.IsEnabled = txtChannelName.Text.Length > 0;
        }

        private void btnRemoveChannel_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            ChannelDTO channel = senderButton.DataContext as ChannelDTO;
            channels.Remove(channel);
        }

        private void txtChannelRowName_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
                txtChannelRowName_LostFocus(sender, null);
            }
        }

        private void txtChannelRowName_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox channelTextBox = sender as TextBox;
            ChannelDTO channel = channelTextBox.DataContext as ChannelDTO;
            channel.Name = channelTextBox.Text;
        }
    }
}
