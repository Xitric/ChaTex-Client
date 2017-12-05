using ChaTex_Client.UserDialogs;
using IO.ChaTex;
using IO.ChaTex.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ChaTex_Client.UserControls
{
    /// <summary>
    /// Interaction logic for CreateGroup.xaml
    /// </summary>
    public partial class CreateGroup : UserControl
    {
        private readonly IGroups groupsApi;
        private readonly IChannels channelsApi;
        private readonly GroupSettings ucGroupSettings;

        public event Action<GroupDTO> GroupCreated;

        public CreateGroup(IGroups groupsApi, IChannels channelsApi, GroupSettings ucGroupSettings)
        {
            this.groupsApi = groupsApi;
            this.channelsApi = channelsApi;

            InitializeComponent();

            this.ucGroupSettings = ucGroupSettings;
            ucGroupSettings.GroupValidStateChanged += ucGroupSettings_GroupValidStateChanged;
            DockPanel.SetDock(ucGroupSettings, Dock.Top);
            dpnlSettings.Children.Add(ucGroupSettings);
        }

        public void Reset()
        {
            svMasterScroll.ScrollToTop();
            ucGroupSettings.Reset();
        }

        private async void btnCreateGroup_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GroupDTO group = null;

            //Create group
            try
            {
                group = await groupsApi.CreateGroupAsync(new CreateGroupDTO()
                {
                    AllowEmployeeAcknowledgeable = ucGroupSettings.IsAcknowledgeableSelected(),
                    AllowEmployeeBookmark = ucGroupSettings.IsBookmarkSelected(),
                    AllowEmployeeSticky = ucGroupSettings.IsStickySelected(),
                    GroupName = ucGroupSettings.GetGroupName()
                });
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                return;
            }

            //Add users, roles, and channels
            List<string> channelNames = ucGroupSettings.GetChannels().Select(c => c.Name).ToList();

            try
            {
                await groupsApi.AddUsersToGroupAsync(group.Id, ucGroupSettings.GetSelectedUsers());
                await groupsApi.AddRolesToGroupAsync(group.Id, ucGroupSettings.GetSelectedRoles());

                foreach (string channelName in channelNames)
                {
                    await channelsApi.CreateChannelAsync(group.Id, channelName);
                }
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                return;
            }

            MessageBox.Show("The group has now been created!");
            GroupCreated?.Invoke(group);
        }

        private void ucGroupSettings_GroupValidStateChanged(bool valid)
        {
            btnCreateGroup.IsEnabled = valid;
        }
    }
}
