using System.Windows;
using System.Windows.Controls;
using ChaTex_Client.UserDialogs;
using IO.ChaTex;
using Microsoft.Rest;
using BusinessLayer;
using System.Threading.Tasks;
using IO.ChaTex.Models;

namespace ChaTex_Client
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private IUsers usersApi;
        private Overview mainWindow;

        public Login(IUsers users, Overview mainWindow)
        {
            this.usersApi = users;
            this.mainWindow = mainWindow;

            InitializeComponent();
            txtUserEmail.Text = Properties.Settings.Default.Username;
        }

        private async Task<string> loginAsync(string email, string password)
        {
            string token = null;

            try
            {
                token = await usersApi.LoginAsync(new UserCredentials()
                {
                    Email = email,
                    Password = password
                });
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
            }

            return token;
        }

        private void saveCredentials(string email, string token)
        {
            Properties.Settings.Default.Username = email;
            Properties.Settings.Default.Save();
            TokenHandler.Token = token;
        }

        private async void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            Task<string> loginTask = loginAsync(txtUserEmail.Text, txtUserPassword.Password);
            txtUserPassword.Clear();
            string token = await loginTask;

            if (token != null)
            {
                saveCredentials(txtUserEmail.Text, token);
                mainWindow.Show();
                Close();
            }
        }

        private void txtUserEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSignIn.IsEnabled = txtUserEmail.Text.Length > 0 && txtUserPassword.Password.Length > 0;
        }

        private void txtUserPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            btnSignIn.IsEnabled = txtUserEmail.Text.Length > 0 && txtUserPassword.Password.Length > 0;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
