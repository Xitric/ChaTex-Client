using IO.Swagger.Api;
using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace ChaTex_Client
{
  
    public partial class CreateChat : Window
    {
        ObservableCollection<UserDTO> users;
        UsersApi _userApi = null;

        public CreateChat()
        {
            InitializeComponent();
            _userApi = new UsersApi();
            populateUI();
        }

        private void populateUI()
        {
            users = new ObservableCollection<UserDTO>(_userApi.GetAllUsers());
            lstBoxUsers.ItemsSource = users;
        }

        private void lstBoxUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedUserIds = lstBoxUsers.SelectedItems.Cast<UserDTO>().Select(x => x.Id);

            if (users != null)
            {
                //_chatApi.GetMessagesBetweenUser((int)user.Id);
                //ucChannelMessageView.SetChat((int)chat.Id);
            }
        }
    }
}
