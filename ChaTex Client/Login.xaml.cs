using System.Windows;
using System.Windows.Controls;
using ChaTex_Client.UserDialogs;
using IO.ChaTex;
using Microsoft.Rest;
using BusinessLayer;
using System.Net;

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

        private async void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            btnSignIn.IsEnabled = false;

            try
            {
                var token = await usersApi.LoginAsync(txtUserEmail.Text);
                Properties.Settings.Default.Username = txtUserEmail.Text;
                Properties.Settings.Default.Save();
                TokenHandler.Token = token;

                mainWindow.Show();
                Close();
            }
            catch (HttpOperationException er)
            {
                if (er.Response.StatusCode == HttpStatusCode.BadRequest)
                {
                    new ErrorDialog("Error signing in", "Invalid E-mail").ShowDialog();
                }
                else
                {
                    new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                }
            }
            finally
            {
                btnSignIn.IsEnabled = true;
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
