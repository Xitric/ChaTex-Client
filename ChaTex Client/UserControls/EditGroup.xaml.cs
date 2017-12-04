using ChaTex_Client.UserDialogs;
using IO.ChaTex;
using IO.ChaTex.Models;
using Microsoft.Rest;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private IList<UserDTO> originalUsers;
        private IList<RoleDTO> originalRoles;
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
            ucGroupSettings.Reset();

            try
            {
                originalUsers = await groupsApi.GetAllDirectGroupUsersAsync(group.Id);
                originalRoles = await groupsApi.GetAllGroupRolesAsync(group.Id);
                originalChannels = group.Channels;
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                return;
            }

            await ucGroupSettings.LoadFromGroup(group);
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UcGroupSettings_GroupValidStateChanged(bool obj)
        {
            
        }
    }
}
