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

namespace ChaTex_Client.UserDialogs
{
    /// <summary>
    /// Interaction logic for ErrorDialog.xaml
    /// </summary>
    public partial class ErrorDialog : Window
    {
        public string ErrorTitle { get; set; }
        public string ErrorMessage { get; set; }

        public ErrorDialog(string title, string message)
        {
            ErrorTitle = title;
            ErrorMessage = message;
            InitializeComponent();
            SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
