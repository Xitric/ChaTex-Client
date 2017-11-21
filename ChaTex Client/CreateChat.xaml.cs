using IO.Swagger.Api;
using IO.Swagger.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ChaTex_Client
{
  
    public partial class CreateChat : Window
    {
        private readonly UsersApi userApi;
        private ObservableCollection<UserDTO> users;

        public CreateChat()
        {
            InitializeComponent();
            userApi = new UsersApi();
            populateUI();
        }

        private void populateUI()
        {
            users = new ObservableCollection<UserDTO>(userApi.GetAllUsers());
            lstBoxUsers.ItemsSource = users;
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
