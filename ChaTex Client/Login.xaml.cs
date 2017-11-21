using System.Windows;
using System.Windows.Controls;
using ChaTex_Client.UserDialogs;
using IO.Swagger.Api;
using IO.Swagger.Client;

namespace ChaTex_Client {
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window {
        
        public Login() {
            InitializeComponent();
            txtUserEmail.Text = Properties.Settings.Default.Username;
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e) {
            var usersapi = new UsersApi();
            try
            {
                var token = usersapi.Login(txtUserEmail.Text);
                Properties.Settings.Default.Username = txtUserEmail.Text;
                Properties.Settings.Default.Save();
                Configuration.ApiKey.Add("token", token);
            }
            catch (ApiException er)
            {
                new ExceptionDialog(er).ShowDialog();
                if (er.ErrorCode == 404)
                {
                    return;
                }
            }
            new Overview().Show();
            Close();
           
        }

        private void txtUserEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSignIn.IsEnabled = txtUserEmail.Text.Length > 0;
        }
    }
}
