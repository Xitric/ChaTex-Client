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
namespace ChaTex_Client {
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window {
        
        public Login() {
            InitializeComponent();
        }

        private void SignInBtn_Click(object sender, RoutedEventArgs e) {
            var usersapi = new UsersApi();
            try
            {
                var token = usersapi.Login(EmailText.Text);
                Configuration.ApiKey.Add("token", token);
            }
            catch (ApiException er)
            {
                if (er.ErrorCode == 404)
                    MessageBox.Show(er.Message, "Error logging in", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    throw er;
            }
            new Overview().Show();
            Close();
           
        }

        private void EmailText_TextChanged(object sender, TextChangedEventArgs e)
        {
                SignInBtn.IsEnabled = EmailText.Text.Length > 0;
        }
    }
}
