using System.Windows;
using System.Windows.Controls;
using ChaTex_Client.UserDialogs;
using IO.ChaTex;
using Microsoft.Rest;
using BusinessLayer;
using System.Net;
using System.Threading.Tasks;

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

        private async Task<string> login(string email)
        {
            string token = null;

            try
            {
                token = await usersApi.LoginAsync(email);

                if (token == null)
                {
                    new ErrorDialog("Network error", "Unable to contact server.\nPlease inform your administrator.").ShowDialog();
                }
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
            btnSignIn.IsEnabled = false;
            string token = await login(txtUserEmail.Text);
            btnSignIn.IsEnabled = true;

            if (token != null)
            {
                saveCredentials(txtUserEmail.Text, token);
                mainWindow.Show();
                Close();
            }
        }

        private void txtUserEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSignIn.IsEnabled = txtUserEmail.Text.Length > 0;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
