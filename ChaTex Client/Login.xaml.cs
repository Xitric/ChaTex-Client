using System;
using System.Collections.Generic;
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
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;
using ChaTex_Client.UserDialogs;

namespace ChaTex_Client {
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window {
        
        public Login() {
            InitializeComponent();
            txtEmail.Text = Properties.Settings.Default.Username;
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e) {
            var usersapi = new UsersApi();
            try
            {
                var token = usersapi.Login(txtEmail.Text);
                Properties.Settings.Default.Username = txtEmail.Text;
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

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSignIn.IsEnabled = txtEmail.Text.Length > 0;
        }
    }
}
