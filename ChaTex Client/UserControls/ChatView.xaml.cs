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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChaTex_Client.UserControls
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : UserControl
    {
        ObservableCollection<UserDTO> users;
        UsersApi usersApi = null;

        public static ChatView m_Instance = null;
       

        public static ChatView GetInstance()
        {
            if (m_Instance == null)
            {
                m_Instance = new ChatView();
            }
            m_Instance.populateUI();
            return m_Instance;
        }
        private ChatView()
        {
            InitializeComponent();

            usersApi = new UsersApi();
        }

        private void populateUI()
        {
            users = new ObservableCollection<UserDTO>(usersApi.GetAllUsers());
            lstBoxUsers.ItemsSource = users;
        }

        private void txtSearchUsers_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearchUsers.Text.Length > 0)
            {
                lstBoxUsers.ItemsSource = users.Where(x => x.FirstName.ToLower().Contains(txtSearchUsers.Text.ToLower()) || x.LastName.ToLower().Contains(txtSearchUsers.Text.ToLower()));
            }
            else
            {
                lstBoxUsers.ItemsSource = users;
            }
        }
    }
}
