using ChaTex_Client.UserDialogs;
using ChaTex_Client.ViewModels;
using IO.ChaTex;
using IO.ChaTex.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChaTex_Client.UserControls
{
    /// <summary>
    /// Interaction logic for GroupView.xaml
    /// </summary>
    public partial class GroupView : UserControl
    {
        private readonly ObservableCollection<MemberViewModel> members;
        private readonly ObservableCollection<RoleDTO> roles;
        private readonly IGroups groupsApi;
        private readonly IUsers usersApi;

        private GroupDTO currentGroup;
        private Task fetchMembersTask;

        public GroupView(IGroups groupsApi, IUsers usersApi)
        {
            this.groupsApi = groupsApi;
            this.usersApi = usersApi;

            InitializeComponent();

            members = new ObservableCollection<MemberViewModel>();
            icMembers.ItemsSource = members;
            roles = new ObservableCollection<RoleDTO>();
            icRoles.ItemsSource = roles;
        }

        public void SetGroup(GroupDTO group)
        {
            currentGroup = group;
            fetchMembersTask = updateDisplay();
        }

        private async Task updateDisplay()
        {
            //Wait for current job to finish
            if (fetchMembersTask != null)
            {
                await fetchMembersTask;
            }

            Task membersUpdate = updateMembers();
            Task rolesUpdate = updateRoles();

            await membersUpdate;
            await rolesUpdate;
        }

        private async Task updateMembers()
        {
            members.Clear();

            IList<UserDTO> users = await groupsApi.GetAllGroupUsersAsync(currentGroup.Id);
            IList<UserDTO> admins = await groupsApi.GetAllGroupAdminsAsync(currentGroup.Id);
            List<MemberViewModel> memberList = new List<MemberViewModel>();

            //Construct list of member view models
            foreach (UserDTO user in users)
            {
                bool isAdmin = false;
                foreach (UserDTO admin in admins)
                {
                    if (admin.Id == user.Id)
                    {
                        isAdmin = true;
                        break;
                    }
                }

                MemberViewModel member = new MemberViewModel(user, isAdmin);
                memberList.Add(member);
            }

            //Sort members. Administrators first, the regular members. Members are sorted alphabetically by their names, beginning with their first names
            memberList.Sort((first, second) => {
                if (first.IsAdmin == second.IsAdmin)
                {
                    return string.Compare(first.Name, second.Name);
                }

                return first.IsAdmin ? -1 : 1;
            });

            //Add sorted members to the view
            foreach (MemberViewModel member in memberList)
            {
                members.Add(member);
            }
        }

        private async Task updateRoles()
        {
            roles.Clear();
            
            IList<RoleDTO> getRoles = await groupsApi.GetAllGroupRolesAsync(currentGroup.Id);

            foreach (RoleDTO role in getRoles)
            {
                roles.Add(role);
            }
        }

        private async void dpnlMemberRow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DockPanel memberRow = sender as DockPanel;
            MemberViewModel memberViewModel = memberRow.DataContext as MemberViewModel;

            memberRow.ContextMenu.DataContext = memberViewModel;

            if (e.ChangedButton == MouseButton.Left)
            {
                memberRow.ContextMenu.PlacementTarget = memberRow;
                memberRow.ContextMenu.IsOpen = true;
            }

            if (memberViewModel.Roles == null)
            {
                try
                {
                    memberViewModel.Roles = await usersApi.GetAllUserRolesAsync(memberViewModel.Id);
                }
                catch (HttpOperationException er)
                {
                    new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                    return;
                }
            }
        }
    }
}
