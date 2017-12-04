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

namespace ChaTex_Client.UserControls
{
    /// <summary>
    /// Interaction logic for CreateGroup.xaml
    /// </summary>
    public partial class CreateGroup : UserControl
    {
        private readonly List<AddMemberViewModel> users;
        private readonly ObservableCollection<AddMemberViewModel> displayUsers;
        private readonly IUsers usersApi;
        private readonly IGroups groupsApi;

        private Task fetchInformationTask;

        public CreateGroup(IUsers usersApi, IGroups groupsApi)
        {
            this.usersApi = usersApi;
            this.groupsApi = groupsApi;

            InitializeComponent();

            users = new List<AddMemberViewModel>();
            displayUsers = new ObservableCollection<AddMemberViewModel>();
            icMembers.ItemsSource = displayUsers;
        }

        public void Reset()
        {
            fetchInformationTask = UpdateDisplay();
        }

        private async Task UpdateDisplay()
        {
            //Wait for current job to finish
            if (fetchInformationTask != null)
            {
                await fetchInformationTask;
            }

            IList<UserDTO> allUsers;
            users.Clear();
            displayUsers.Clear();

            try
            {
                allUsers = await usersApi.GetAllUsersAsync();
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                return;
            }

            users.AddRange(allUsers.Where(u => !u.Me).Select(u => new AddMemberViewModel(u)));
            users.Sort((first, second) =>
            {
                return string.Compare(first.Name, second.Name);
            });

            foreach (AddMemberViewModel user in users)
            {
                displayUsers.Add(user);
            }
        }

        private void txtSearchMembers_TextChanged(object sender, TextChangedEventArgs e)
        {
            displayUsers.Clear();

            foreach (AddMemberViewModel user in users)
            {
                bool match = txtSearchMembers.Text.Length == 0 ||
                    user.Name.IndexOf(txtSearchMembers.Text, StringComparison.InvariantCultureIgnoreCase) >= 0;

                if (match)
                {
                    displayUsers.Add(user);
                }
            }
        }

        private void bMemberRow_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Border memberRow = sender as Border;
            AddMemberViewModel memberViewModel = memberRow.DataContext as AddMemberViewModel;

            memberViewModel.IsSelected = !memberViewModel.IsSelected;
        }

        private void txtGroupName_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnCreateGroup.IsEnabled = txtGroupName.Text.Length > 0;
        }

        private async void btnCreateGroup_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GroupDTO group = null;

            try
            {
                group = await groupsApi.CreateGroupAsync(new CreateGroupDTO()
                {
                    AllowEmployeeSticky = true,
                    AllowEmployeeAcknowledgeable = true,
                    AllowEmployeeBookmark = true,
                    GroupName = txtGroupName.Text
                });
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                return;
            }

            List<int?> userIdList = users.Where(u => u.IsSelected).Select(u => (int?)u.Id).ToList();

            try
            {
                await groupsApi.AddUsersToGroupAsync(group.Id, userIdList);
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                return;
            }

            MessageBox.Show("The group has now been created!");
        }
    }
}
