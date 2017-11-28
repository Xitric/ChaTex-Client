using IO.ChaTex;
using IO.ChaTex.Models;
using Microsoft.Rest;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ChaTex_Client
{

    public partial class CreateChat : Window
    {
        private readonly IUsers usersApi;
        private ObservableCollection<UserDTO> users;

        public CreateChat(IUsers usersApi)
        {
            this.usersApi = usersApi;

            InitializeComponent();
            populateUI();
        }

        private async void populateUI()
        {
            try
            {
                users = new ObservableCollection<UserDTO>(await usersApi.GetAllUsersAsync());
                lstBoxUsers.ItemsSource = users;
            }
            catch (HttpOperationException er)
            {
                //TODO: Exception handling
                throw er;
            }
        }

        private void lstBoxUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedUserIds = lstBoxUsers.SelectedItems.Cast<UserDTO>().Select(x => x.Id);

            if (users != null)
            {
                //Chat hasent been implemented yet, so thats why this is out commented
                //_chatApi.GetMessagesBetweenUser((int)user.Id);
                //ucChannelMessageView.SetChat((int)chat.Id);
            }
        }
    }
}
